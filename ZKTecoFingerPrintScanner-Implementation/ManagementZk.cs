using libzkfpcsharp;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using ZKTecoFingerPrintScanner_Implementation.Helpers;
using ZKTecoFingerPrintScanner_Implementation.Models;
using ZKTecoFingerPrintScanner_Implementation.Services;
using StatusBar = ZKTecoFingerPrintScanner_Implementation.Controls.StatusBar;
using static MaterialSkin.Controls.MaterialForm;
using System.Linq;
using System.Net;
using System.Data;

namespace ZKTecoFingerPrintScanner_Implementation
{
    public class ManagementZk
    {
        private IntPtr deviceHandle;
        private bool isInitialized;
        public string name_sn = "";

        //varialbles
        const int REGISTER_FINGER_COUNT = 3;
        IntPtr FormHandle = IntPtr.Zero;

        zkfp fpInstance = new zkfp();

        bool bIsTimeToDie = false;
        bool IsRegister = false;
        bool bIdentify = true;
        bool MatchSearch = false;
        byte[] FPBuffer;
        int RegisterCount = 0;

        byte[][] RegTmps = new byte[REGISTER_FINGER_COUNT][];

        byte[] RegTmp = new byte[2048];
        byte[] CapTmp = new byte[2048];
        int cbCapTmp = 2048;
        int regTempLen = 0;
        int iFid = 1;

        const int MESSAGE_CAPTURED_OK = 0x0400 + 6;

        private int mfpWidth = 0;
        private int mfpHeight = 0;
        public string nameserie;


        Thread captureThread = null;
        public delegate void FingerprintCapturedEventHandler(Bitmap bmp, string message, string message2);
        public event FingerprintCapturedEventHandler FingerprintCaptured;

        public delegate void EventHandleGeneral(string type, dynamic value);
        public event EventHandleGeneral EventGeneral;

        public delegate void AlertaEventHandler(string mensaje);
        public event AlertaEventHandler AlertaEvent;

        private ScreenHome myForm;

        public ManagementZk(ScreenHome screenHome)
        {
            myForm = screenHome;

        }

        public void SetIsRegister(bool value)
        {
            IsRegister = value;
        }
        public void SetMatchSearch(bool value)
        {
            MatchSearch = value;
        }

        //Initialize
        public bool Initialize()
        {
            int callBackCode = fpInstance.Initialize();
            if (zkfp.ZKFP_ERR_OK == callBackCode)
            {
                int nCount = fpInstance.GetDeviceCount();
                if (nCount > 0)
                {
                    for (int index = 1; index <= nCount; index++)
                    {

                    }
                    isInitialized = true;
                    ConnectDevice(0);
                }
            }
            else
            {
                isInitialized = false;
            }
            return isInitialized;
        }
        //connect device
        public void ConnectDevice(int deviceIndex)
        {
            int openDeviceCallBackCode = fpInstance.OpenDevice(deviceIndex);

            if (zkfp.ZKFP_ERR_OK != openDeviceCallBackCode)
            {
                // No se puede conectar con el dispositivo
                return;
            }

            RegisterCount = 0;
            regTempLen = 0;
            iFid = 1;

            for (int i = 0; i < REGISTER_FINGER_COUNT; i++)
            {
                RegTmps[i] = new byte[2048];
            }

            byte[] paramValue = new byte[4];


            // Recuperar el ancho de la imagen de la huella digital
            int size = 4;
            fpInstance.GetParameters(1, paramValue, ref size);
            zkfp2.ByteArray2Int(paramValue, ref mfpWidth);

            // Recuperar la altura de la imagen de la huella digital
            size = 4;
            fpInstance.GetParameters(2, paramValue, ref size);
            zkfp2.ByteArray2Int(paramValue, ref mfpHeight);

            FPBuffer = new byte[mfpWidth * mfpHeight];

            // Cree un hilo para recuperar cualquier huella digital nueva y manejar los eventos del dispositivo
            captureThread = new Thread(new ThreadStart(DoCapture));
            captureThread.IsBackground = true;
            captureThread.Start();
            bIsTimeToDie = false;
            nameserie = fpInstance.devSn;
            MessageDispositive($"Conectado : {nameserie}", true);

        }




        //disconnect
        public void Disconnect()
        {
            if (isInitialized)
            {
                isInitialized = false;
                bIsTimeToDie = true;
                RegisterCount = 0;
                Thread.Sleep(1000);
                int result = fpInstance.CloseDevice();

                captureThread.Abort();
                if (result == zkfp.ZKFP_ERR_OK)
                {
                    //  MessageDispositive("Desconectado", false);

                    Thread.Sleep(1000);
                    result = fpInstance.Finalize();

                    if (result == zkfp.ZKFP_ERR_OK)
                    {
                        regTempLen = 0;
                        IsRegister = false;
                        bIdentify = true;
                        // MessageDispositive(MessageManager.msg_FP_Disconnected, false);
                    }
                    else
                    {
                        // MessageDispositive(MessageManager.msg_FP_FailedToReleaseResources, false);
                    }

                }
                else
                {
                    string message = FingerPrintDeviceUtilities.DisplayDeviceErrorByCode(result);
                    // MessageDispositive(message, false);
                }
            }
        }
        public void MessageDispositive(string message, bool status)
        {
            //Panel panel = myForm.Controls["PanelHeader"] as Panel;
            //Label lblSerie = panel.Controls["lblSerie"] as Label;
            //lblSerie.ForeColor = Color.White;

            myForm.lblSerie_.ForeColor = Color.White;
            myForm.lblSerie_.Text = message;
            //lblSerie.Text = message;
            if (status)
            {
                myForm.lblSerie_.ForeColor = Color.White;
                myForm.lblSerie_.BackColor = Color.FromArgb(79, 208, 154);
            }
            else
            {
                myForm.lblSerie_.ForeColor = Color.White;
                myForm.lblSerie_.BackColor = Color.FromArgb(230, 112, 134);
            }
        }



        //capture
        private void DoCapture()
        {
            try
            {
                while (!bIsTimeToDie)
                {
                    cbCapTmp = 2048;
                    int ret = fpInstance.AcquireFingerprint(FPBuffer, CapTmp, ref cbCapTmp);

                    if (ret == zkfp.ZKFP_ERR_OK)
                    {
                        HandleBio();
                    }
                    Thread.Sleep(100);
                }

            }
            catch { }
        }

        public string messageInfo = "";
        public string messageSuccess = "";
        public async Task HandleBio()
        {
            DisplayFingerPrintImage();
            try
            {
                if (IsRegister)
                {
                    //  StatusMessage("", false, true);

                    int ret = zkfp.ZKFP_ERR_OK;
                    int fid = 0, score = 0;
                    ret = fpInstance.Identify(CapTmp, ref fid, ref score);
                    if (zkfp.ZKFP_ERR_OK == ret)
                    {
                        int deleteCode = fpInstance.DelRegTemplate(fid);
                        if (deleteCode != zkfp.ZKFP_ERR_OK)
                        {
                            // StatusMessage($"remove {RegisterCount}", false);
                            return;
                        }
                    }

                    if (RegisterCount > 0 && fpInstance.Match(CapTmp, RegTmps[RegisterCount - 1]) <= 0)
                    {
                        // StatusMessage($"Por favor presione el mismo dedo {RegisterCount}", false);
                        return;
                    }

                    Array.Copy(CapTmp, RegTmps[RegisterCount], cbCapTmp);
                    messageSuccess = "";
                    RegisterCount++;


                    if (RegisterCount >= REGISTER_FINGER_COUNT)
                    {
                        RegisterCount = 0;
                        CompleteRegistration();
                    }
                    else
                    {

                        Label lblIntents = SetControlValue("tabPage2", "lblIntents");
                        int remainingCont = REGISTER_FINGER_COUNT - RegisterCount;
                        string a = REGISTER_FINGER_COUNT > 1 ? "veces" : "vez";
                        lblIntents.Text = $"{remainingCont} {a} más";
                    }

                }

                else
                {

                    if (MatchSearch)
                    {
                        List<SocioModel> sociosList = SocioData.socios;
                        bool match = false;

                        foreach (SocioModel socio in sociosList)
                        {
                            var storedTemplateBytes = zkfp.Base64String2Blob(socio.Huella.ToString());
                            int ret = fpInstance.Match(CapTmp, storedTemplateBytes);
                            if (ret > 0)
                            {
                                match = true;
                                socio.MessageExtra = $"puntuación de éxito : {ret} -  {socio.Nombre} {socio.CodigoSocio}";
                                await DataMembresia(socio);
                                return;
                            }
                        }
                        if (!match)
                        {
                            EventGeneral.Invoke("MA-F", true);
                            return;
                        }
                    }

                }
            }
            catch (Exception ex)
            {


            }
        }

        //gestion membresia
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(5);

        public async Task DataMembresia(SocioModel socio)
        {
            DataStatic.Socio = socio;
            AppsFitService serv = new AppsFitService();

            try
            {
                await semaphore.WaitAsync();
                var resp = await serv.MembresiasList(new
                {
                    CodigoUnidadNegocio = socio.CodigoUnidadNegocio,
                    CodigoSede = socio.CodigoSede,
                    Socio = socio.CodigoSocio
                });

                if (resp.Success)
                {
                    var respHistorial = await serv.AsistencesList(new
                    {
                        CodigoUnidadNegocio = socio.CodigoUnidadNegocio,
                        CodigoSede = socio.CodigoSede,
                        Membresia = resp.Data[0].CodigoMenbresia
                    });
                    var HPC = await serv.HistorialPC(new
                    {
                        CodigoUnidadNegocio = socio.CodigoUnidadNegocio,
                        CodigoSede = socio.CodigoSede,
                        Membresia = resp.Data[0].CodigoMenbresia
                    });

                    DataStatic.Membresias = resp.Data;
                    DataStatic.Asistences = respHistorial.Data;
                    DataStatic.Pagos = HPC.Data.Pagos;
                    DataStatic.Cuotas = HPC.Data.Cuotas;
                    DataStatic.Incidencias = HPC.Data.Incidencias;
                    EventGeneral.Invoke("MA-T", 1);
                }
            }
            finally
            {
                semaphore.Release();
            }
        }

        public void TextStatusMembresia(int status, string message)
        {
            Label lblMessageMem = SetControlValueT("tabPage1", "lblMessageMem");
            if (status == 1)
            {
                lblMessageMem.BackColor = Color.Green;
            }
            if (status == 2)
            {
                lblMessageMem.BackColor = Color.Gray;
            }
            lblMessageMem.Text = message ?? "";
        }

        private void CompleteRegistration()
        {
            Label lblIntents = SetControlValue("tabPage2", "lblIntents");
            lblIntents.Text = "Completado";

            int ret = GenerateRegisteredFingerPrint();
            if (ret == zkfp.ZKFP_ERR_OK)
            {
                ret = AddTemplateToMemory();
                if (ret == zkfp.ZKFP_ERR_OK)
                {
                    string fingerPrintTemplate = string.Empty;
                    zkfp.Blob2Base64String(RegTmp, regTempLen, ref fingerPrintTemplate);
                    // createFile(fingerPrintTemplate);

                    // Register huella
                    StatusMessage("Huella registrada correctamente", true);
                    registerHuella(fingerPrintTemplate);
                    ClearDeviceUser();
                }
                else
                {
                    StatusMessage("Error al agregar la plantilla de usuarios", false);
                }
            }
            else
            {
                StatusMessage($"No se puede inscribir al usuario actual. Código de error: {ret}", false);
            }

            IsRegister = false;
        }

        private void StatusMessage(string message, bool success, bool empy = false)
        {
            StatusBar lblMessage = myForm.Controls["lblMessage"] as StatusBar;

            lblMessage.Message = message;
            lblMessage.StatusBarForeColor = Color.White;

            if (success)
            {
                lblMessage.StatusBarBackColor = Color.FromArgb(79, 208, 154);
            }
            else
            {
                lblMessage.StatusBarBackColor = Color.FromArgb(230, 112, 134);
            }
            if (empy)
            {
                lblMessage.StatusBarBackColor = Color.FromArgb(250, 250, 250);
            }
        }

        public void ClearDeviceUser()
        {
            try
            {
                int deleteCode = fpInstance.DelRegTemplate(iFid);
                if (deleteCode != zkfp.ZKFP_ERR_OK)
                {

                }
                iFid = 1;
            }
            catch { }

        }

        public void ResetListenRegister()
        {
            ClearImage();
            IsRegister = true;
            RegisterCount = 0;
            regTempLen = 0;
        }

        public void MatchBio()
        {
            MatchSearch = true;
        }

        private int GenerateRegisteredFingerPrint()
        {
            return fpInstance.GenerateRegTemplate(RegTmps[0], RegTmps[1], RegTmps[2], RegTmp, ref regTempLen);
        }
        private int AddTemplateToMemory()
        {

            return fpInstance.AddRegTemplate(iFid, RegTmp);
        }



        private void DisplayFingerPrintImage()
        {
            MemoryStream ms = new MemoryStream();
            BitmapFormat.GetBitmap(FPBuffer, mfpWidth, mfpHeight, ref ms);
            Bitmap bmp = new Bitmap(ms);
            // PictureBox pict = SetControlValue("tabPage2", "PicRegister");
            // pict.Image = bmp;

            // FingerprintCaptured.Invoke(bmp, messageInfo, messageSuccess);
        }

        public dynamic SetControlValue(string tabPageName, string controlName)
        {
            dynamic control1 = null;
            TabControl tabControl = myForm.Controls["TabControl"] as TabControl;
            if (tabControl != null)
            {
                TabPage tabPage = tabControl.Controls[tabPageName] as TabPage;
                if (tabPage != null)
                {
                    Control control = tabPage.Controls[controlName];
                    if (control != null)
                    {
                        if (control is PictureBox pictureBox)
                        {
                            control1 = pictureBox;
                        }
                        if (control is Label label)
                        {
                            control1 = label;
                        }
                        if (control is StatusBar statusBar)
                        {
                            control1 = statusBar;
                        }
                    }
                }
            }
            return control1;
        }

        public dynamic SetControlValueT(string tabPageName, string controlName)
        {
            dynamic control1 = null;
            TabControl tabControl = myForm.Controls["TabControl"] as TabControl;
            if (tabControl != null)
            {
                TabPage tabPage = tabControl.Controls[tabPageName] as TabPage;
                if (tabPage != null)
                {
                    Control control = tabPage.Controls.Find(controlName, true).FirstOrDefault() as Control;
                    if (control != null)
                    {
                        if (control is PictureBox pictureBox)
                        {
                            control1 = pictureBox;
                        }
                        if (control is Label label)
                        {
                            control1 = label;
                        }
                        if (control is StatusBar statusBar)
                        {
                            control1 = statusBar;
                        }
                        if (control is DataGridView dataGridView)
                        {
                            control1 = dataGridView;
                        }
                    }
                }
            }
            return control1;
        }


        private void ClearImage()
        {
            //  PictureBox pict = SetControlValue("tabPage2", "PicRegister");
            // pict.Image = null;
        }

        public async void registerHuella(string huella)
        {
            var data = new { DefaultKeyEmpresa = DataSession.DKey, Socio = DataSession.Filtre, Huella = huella };
            AppsFitService service = new AppsFitService();
            var resp = await service.RegHuellaAPI(data);
            if (resp.Success)
            {

            }
            else
            {

            }

        }

        public void createFile(string content)
        {
            string fileName = "debugDev.txt";
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filePath = System.IO.Path.Combine(projectPath, fileName);

            try
            {
                if (File.Exists(filePath))
                {

                    using (StreamWriter sw = File.AppendText(filePath))
                    {
                        sw.WriteLine();
                        sw.WriteLine(content);
                    }
                }
                else
                {

                    using (StreamWriter sw = File.CreateText(filePath))
                    {
                        sw.WriteLine(content);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Se produjo un error: " + ex.Message);
            }
        }


    }
}
