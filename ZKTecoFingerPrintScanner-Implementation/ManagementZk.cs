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
        public bool isInitialized;
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
        STGlobal stGlobal = STGlobal.Instance;
        DataSocioAll dataSocioAll = DataSocioAll.Instance;
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
            MessageDispositive("Dispositivo conectado", true);

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
                    MessageDispositive("Dispositivo desconectado", false);

                    Thread.Sleep(1000);
                    result = fpInstance.Finalize();

                    if (result == zkfp.ZKFP_ERR_OK)
                    {
                        regTempLen = 0;
                        IsRegister = false;
                        bIdentify = true;
                        MessageDispositive("Dispositivo desconectado", false);
                    }
                    else
                    {
                        MessageDispositive("Error en dispositivo", false);
                    }
                }
                else
                {
                    string message = FingerPrintDeviceUtilities.DisplayDeviceErrorByCode(result);
                    MessageDispositive(message, false);
                }
            }
        }


        public void MessageDispositive(string message, bool status)
        {
            myForm.lblSerie_.Text = message;
            if (status)
            {
                myForm.lblSerie_.ForeColor = Color.Green;
            }
            else
            {
                myForm.lblSerie_.ForeColor = Color.Red;
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

                        Label lblIntents = myForm.lblIntents_;
                        int remainingCont = REGISTER_FINGER_COUNT - RegisterCount;
                        string a = REGISTER_FINGER_COUNT > 1 ? "veces" : "vez";
                        lblIntents.Text = $"{remainingCont} {a} más";
                    }

                }

                else
                {

                    if (MatchSearch)
                    {
                        HuellaData huellaData = HuellaData.Instance;
                        bool match = false;
                        foreach (SocioModel socio in huellaData.Socios)
                        {
                            var storedTemplateBytes = zkfp.Base64String2Blob(socio.Huella.ToString());
                            int ret = fpInstance.Match(CapTmp, storedTemplateBytes);

                            if (ret > stGlobal.Precision)
                            {
                                match = true;
                                string fullName = socio.Nombre + " " + socio.Apellidos;
                                dataSocioAll.MessageGenericD = $"puntuación de éxito : {ret}/100 - ({socio.CodigoSocio}) {fullName.ToUpper()}";
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
            dataSocioAll.Socio = socio;
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

                    dataSocioAll.Membresias = resp.Data;
                    dataSocioAll.Asistences = respHistorial.Data;
                    dataSocioAll.Pagos = HPC.Data.Pagos;
                    dataSocioAll.Cuotas = HPC.Data.Cuotas;
                    dataSocioAll.Incidencias = HPC.Data.Incidencias;

                    EventGeneral.Invoke("MA-T", 1);
                }
                else
                {
                    EventGeneral.Invoke("MA-F", 1);
                }
            }
            finally
            {
                semaphore.Release();
            }
        }


        private void CompleteRegistration()
        {
            Label lblIntents = myForm.lblIntents_;
            lblIntents.Text = "Completado";

            int ret = GenerateRegisteredFingerPrint();
            if (ret == zkfp.ZKFP_ERR_OK)
            {
                ret = AddTemplateToMemory();
                if (ret == zkfp.ZKFP_ERR_OK)
                {
                    string fingerPrintTemplate = string.Empty;
                    zkfp.Blob2Base64String(RegTmp, regTempLen, ref fingerPrintTemplate);
                    // Register huella
                    registerHuella(fingerPrintTemplate);
                }
                else { }
            }
            else
            {
                // StatusMessage($"No se puede inscribir al usuario actual. Código de error: {ret}", false);
            }

            IsRegister = false;
        }


        public async void registerHuella(string huella)
        {
            try
            {

                AppsFitService service = new AppsFitService();
                ResponseModel resp;
                switch (stGlobal.TypeRegister)
                {
                    case 1:
                        var data = new { DefaultKeyEmpresa = DataSession.DKey, Socio = DataSession.Code, Huella = huella };
                        resp = await service.RegHuellaAPI(data);
                        break;
                    case 2:
                        var dat = new { Dni = stGlobal.SearchRegister, Huella = huella };
                        resp = await service.RegHuellaAPIFijo(dat);
                        break;
                    default:
                        var da = new { Dni = stGlobal.SearchRegister, Huella = huella };
                        resp = await service.RegHuellaAPIFEvent(da);
                        break;
                }
                if (resp.Success)
                {
                    EventGeneral.Invoke("REG", true);
                }
                else
                {
                    DataStatic.MessageGeneric = resp.Message1;
                    EventGeneral.Invoke("REG-FAIL", true);
                }
            }
            catch (Exception ex)
            {
                createFile(ex.ToString());
            }
            ClearDeviceUser();
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

            PictureBox pasistence = myForm.picHuellaMA_;
            PictureBox pregister = myForm.PicRegister_;
            pasistence.Image = bmp;
            pregister.Image = bmp;
        }


        private void ClearImage()
        {

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

        public void createFileLog(string page, Exception err)
        {

            string content = $"File: {page} ({DateTime.Now})\nError: {err.Message}\nMethod: {err.TargetSite}\nLinea: {err.StackTrace}";

            string fileName = "log.txt";
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



        public void EliminarArchivoLog()
        {
            try
            {
                string fileName = "log.txt";
                string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
                string filePath = Path.Combine(projectPath, fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);

                }
            }
            catch (Exception ex)
            {
            }
        }


        public string ShowLogs()
        {
            string message = "";
            try
            {
                string fileName = "log.txt";
                string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
                string filePath = System.IO.Path.Combine(projectPath, fileName);

                if (File.Exists(filePath))
                {
                    string logContent = File.ReadAllText(filePath);
                    message = logContent;
                }
            }
            catch (Exception ex)
            {
                message = "";
            }
            return message;
        }
    }

}
