using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZKTecoFingerPrintScanner_Implementation.Controls;
using ZKTecoFingerPrintScanner_Implementation.Helpers;
using ZKTecoFingerPrintScanner_Implementation.Models;
using ZKTecoFingerPrintScanner_Implementation.Services;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ZKTecoFingerPrintScanner_Implementation
{
    public partial class ScreenHome : Form
    {
        private ManagementZk managementZk;

        private const int timeoutInSeconds = 10;
        public Label dev { get; set; }

        public Label lblSerie_ { get; set; }
        public Label lblIntents_ { get; set; }
        // public StatusBar lblMessage_ { get; set; }
        public PictureBox picHuellaMA_ { get; set; }
        public PictureBox PicRegister_ { get; set; }

        STGlobal stGlobal = STGlobal.Instance;
        DataSocioAll dataSocioAll = DataSocioAll.Instance;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]

        private static extern IntPtr CreateRoundRectRgn(
            int nLeft,
            int nTop,
            int nRight,
            int nButtom,
            int nWidthEllipse,
            int nHeightEllipse
        );

        public ScreenHome()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            lblSerie_ = lblSerie;
            picHuellaMA_ = picHuellaMA;
            PicRegister_ = PicRegister;
            lblIntents_ = lblIntents;

            managementZk = new ManagementZk(this);
            // managementZk.FingerprintCaptured += OnFingerprintCaptured;
            //managementZk.EventGeneral += OnEventGeneral;
            //AdjustFormSize(70);

            // CenterControl(panel1, lblMessageMem);
            btnMarkAsistence.Region = Region.FromHrgn(CreateRoundRectRgn
                (0, 0, btnMarkAsistence.Width, btnMarkAsistence.Height, 30, 30));

            panelMA.Region = Region.FromHrgn(CreateRoundRectRgn
                (0, 0, panelMA.Width, panelMA.Height, 30, 30));

            panelDeviseConnect.Region = Region.FromHrgn(CreateRoundRectRgn
                (0, 0, panelDeviseConnect.Width, panelDeviseConnect.Height, 20, 20));

            panelIntentos.Region = Region.FromHrgn(CreateRoundRectRgn
                (0, 0, panelIntentos.Width, panelIntentos.Height, 20, 20));

            BtnConnectionNew.Region = Region.FromHrgn(CreateRoundRectRgn
                (0, 0, BtnConnectionNew.Width, BtnConnectionNew.Height, 20, 20));

            BtnDisconnectionNew.Region = Region.FromHrgn(CreateRoundRectRgn
                (0, 0, BtnDisconnectionNew.Width, BtnDisconnectionNew.Height, 20, 20));
            //Control asistencia
            panelCAUser.Region = Region.FromHrgn(CreateRoundRectRgn
                (0, 0, panelCAUser.Width, panelCAUser.Height, 20, 20));

            panelAPHuella.Region = Region.FromHrgn(CreateRoundRectRgn
                (0, 0, panelAPHuella.Width, panelAPHuella.Height, 20, 20));

            //TabControl.TabPages[0].Text = "Asistencia cliente";
            //TabControl.TabPages[1].Text = "Configuracion";
            //TabControl.TabPages[2].Text = "Asistencia personal";
            //TabControl.TabPages[3].Text = "Registro";
            
        }


        //center control
        public void CenterControl(Control parent, Control child)
        {
            int x = 0;
            x = (parent.Width / 2) - (child.Width / 2);
            child.Location = new System.Drawing.Point(x, child.Location.Y);
        }


        private void AdjustFormSize(int percentage)
        {
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            int width = (int)(bounds.Width * (percentage / 100.0));
            int height = (int)(bounds.Height * (percentage / 100.0));
            int x = (bounds.Width - width) / 2;
            int y = (bounds.Height - height) / 2;

            Size = new Size(width, height);
            Location = new Point(x, y);
        }



        public void OnEventGeneral(string message, dynamic message2)
        {

            try
            {
                switch (message)
                {
                    case "MA-T":

                        if (dataSocioAll.Membresias == null || !dataSocioAll.Membresias.Any())
                        {
                            dgvMembresias.ClearSelection();
                            dgvMembresias.Rows.Clear();
                        }
                        else
                        {
                            dgvMembresias.Rows.Clear();
                            foreach (Membresia m in dataSocioAll.Membresias)
                            {
                                dgvMembresias.Rows.Add(
                                    m.NombrePaquete,
                                    m.FCrecionText,
                                    m.DesFechaInicio,
                                    m.DesFechaFin,
                                    m.Costo,
                                    m.MontoTotal,
                                    m.Debe,
                                    m.CantidadFreezing,
                                    m.CantidadFreezingTomados,
                                    m.CantidadAsistencia,
                                    m.NroContrato,
                                    m.AsesorComercial,
                                    m.CodigoSede);
                            }

                            if (dataSocioAll.Membresias.Count > 0)
                            {
                                lblPlan.Text = dataSocioAll.Membresias[0].Descripcion.ToUpper();
                                MessageStatusMembresia(dataSocioAll.Membresias[0].ObtenerTiempoVencimiento, dataSocioAll.Membresias[0].Estado);

                                dgvMembresias.Rows[0].Selected = true;
                                dataSocioAll.MembresiasSelected = dataSocioAll.Membresias[0];
                                string deudaMem = dataSocioAll.MembresiasSelected.Debe > 0 ? $"DEBE {dataSocioAll.MembresiasSelected.Debe} EN MEMBRESIA" : "";
                                StlyDeudaM(deudaMem, !string.IsNullOrEmpty(deudaMem));
                                if (stGlobal.CheackAutomatic)
                                {
                                    btnMarkAsistence.PerformClick();
                                }
                                _ = Task.Run(() => LoadDataToGrids());
                            }
                            StatusMessage(dataSocioAll.MessageGenericD, true);
                            SocioInfoMatch(true);
                        }

                        break;
                    case "MA-F":
                        dgvMembresias.Rows.Clear();
                        dgvAsistences.Rows.Clear();
                        dgvHpago.Rows.Clear();
                        dgvHcuotas.Rows.Clear();
                        SocioInfoMatch(false);
                        StatusMessage($"No se encontro socio {DateTime.Now}", false);
                        MessageStatusMembresia("ESTADO DE MEMBRESIA", 0, true);
                        StatusMessageD("", false, true);
                        lblPlan.Text = "";

                        break;
                    case "REG":
                        StatusMessage("Registro de huella exitosa", true);
                        _ = LoadFingers(false);
                        break;
                    case "REG-FAIL":
                        StatusMessage(DataStatic.MessageGeneric, false);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                managementZk.createFileLog("ScreeHome", ex);
            }

        }

        private void LoadDataToGrids()
        {

            try
            {

                dgvHcuotas.ClearSelection();
                dgvHcuotas.Rows.Clear();

                dgvHpago.ClearSelection();
                dgvHpago.Rows.Clear();

                dgvAsistences.ClearSelection();
                dgvAsistences.Rows.Clear();

                dgvIncidencias.ClearSelection();
                dgvIncidencias.Rows.Clear();


                if (dataSocioAll.Asistences.Count > 0)
                {
                    foreach (Asistence a in dataSocioAll.Asistences)
                    {
                        dgvAsistences.Rows.Add(a.FCreacionText, a.HourText, a.DiaSemana, a.UsuarioCreacion);
                    }
                }


                if (dataSocioAll.Pagos.Count > 0)
                {
                    foreach (Pago p in dataSocioAll.Pagos)
                    {
                        dgvHpago.Rows.Add(p.desFechaPago, p.Monto, p.NroComprobante, p.DesFormaPago, p.UsuarioCreacion);
                    }
                }

                if (dataSocioAll.Cuotas.Count > 0)
                {
                    foreach (Cuota c in dataSocioAll.Cuotas)
                    {
                        dgvHcuotas.Rows.Add(c.Fecha, c.Monto, c.UsuarioCreacion);
                    }
                }

                if (dataSocioAll.Incidencias.Count > 0)
                {
                    foreach (Incidencia c in dataSocioAll.Incidencias)
                    {
                        dgvIncidencias.Rows.Add(c.FechaCreacion, c.UsuarioCreacion, c.Ocurrencia);
                    }
                }

            }
            catch (Exception ex)
            {
                managementZk.createFileLog("ScreeHome", ex);
            }
        }


        //set socio match
        private void SocioInfoMatch(bool success)
        {
            try
            {
                if (success)
                {
                    var socio = dataSocioAll.Socio;
                    lblFullName_.Text = $"{socio.Nombre}, {socio.Apellidos}".ToUpper();
                    string deudaStr = socio.DeudaSuplemento > 0 ? $"DEBE {socio.DeudaSuplemento} EN PRODUCTOS" : "";
                    StlyDeuda(deudaStr, deudaStr.Length > 0 ? true : false);
                    if (string.IsNullOrEmpty(socio.ImagenUrl) == false && validateHttps(socio.ImagenUrl) == true)
                    {
                        using (WebClient webClient = new WebClient())
                        {
                            byte[] imageData = webClient.DownloadData(socio.ImagenUrl);
                            using (MemoryStream ms = new MemoryStream(imageData))
                            {
                                PicMaUser.Image = Image.FromStream(ms);
                            }
                        }
                    }
                }
                else
                {
                    lblFullName_.Text = "NOMBRES, APELLIDOS COMPLETOS";
                    StlyDeuda();
                    PicMaUser.Image = BIOCHECK.Properties.Resources.user2;
                }
            }
            catch (Exception ex)
            {
                managementZk.createFileLog("ScreeHome", ex);
            }
        }

        private void MessageStatusMembresia(string message, int status, bool clear = false)
        {
            try
            {
                string finish = "😨 SU MEMBRESÍA FINALIZÓ 👎";
                lblMessageMem.Text = status == 2 ? finish : message;
                lblMessageMem.ForeColor = Color.White;
                lblMessageMem.BackColor = clear ? Color.FromArgb(37, 47, 59) : (status == 1 ? Color.Green : Color.Gray);
            }
            catch (Exception ex)
            {
                managementZk.createFileLog("ScreeHome", ex);
            }
        }

        private void StlyDeuda(string message = "", bool deuda = false)
        {
            try
            {
                lblDeudaProductos.Text = string.IsNullOrEmpty(message) ? "DEUDA PRODUCTOS" : message;
                lblDeudaProductos.ForeColor = Color.White;
                lblDeudaProductos.BackColor = deuda ? Color.FromArgb(192, 0, 0) : Color.Gray;
            }
            catch (Exception ex)
            {
                managementZk.createFileLog("ScreeHome", ex);
            }
        }
        private void StlyDeudaM(string message = "", bool deuda = false)
        {
            try
            {
                lblDeudaMembresia.Text = string.IsNullOrEmpty(message) ? "DEUDA MEMBRESIA" : message;
                lblDeudaMembresia.ForeColor = Color.White;
                lblDeudaMembresia.BackColor = deuda ? Color.FromArgb(192, 0, 0) : Color.Gray;
            }
            catch (Exception ex)
            {
                managementZk.createFileLog("ScreeHome", ex);
            }
        }

        MaterialSkinManager skinManager = MaterialSkinManager.Instance;

        private bool isFirstLoad = true;
        private async void ScreenHome_Load(object sender, EventArgs e)
        {

            LoadingForm loading = new LoadingForm();
            loading.Opacity = 0;
            loading.Visible = false;
            loading.Close();

            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            Color customColor = Color.FromArgb(0, 80, 200);

            skinManager.ColorScheme = new ColorScheme(
                Primary.Blue900,
                Primary.Blue600,
                Primary.Blue600,
                Accent.Amber700,
                TextShade.WHITE
            );

            if (isFirstLoad)
            {
                if (TabControl.SelectedTab == tabPage1)
                {
                    //handle match
                    managementZk.SetMatchSearch(true);
                    _ = LoadFingers(true);
                }
                isFirstLoad = false;
            }
            //date format 
            DateFormat();

            //loadData 
            loadInitialData();

        }

        private void loadInitialData()
        {
            try
            {
                lblGym.Text = DataSession.Name;
                lblRubro.Text = DataSession.Rubro;
                if (!string.IsNullOrEmpty(DataSession.Logo))
                {
                    using (WebClient webClient = new WebClient())
                    {
                        byte[] imageData = webClient.DownloadData(DataSession.Logo);
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            picGymLogo.Image = Image.FromStream(ms);
                        }
                    }
                }

                DataManagerD dpre = new DataManagerD();
                if (string.IsNullOrEmpty(dpre.ReadData()))
                {
                    tbPrecicion.Value = 60;
                    lblPrecicion.Text = $"60%";
                    stGlobal.Precision = 60;
                }
                else
                {
                    var pre = dpre.ReadData();
                    tbPrecicion.Value = Convert.ToInt32(pre);
                    lblPrecicion.Text = $"{pre}%";
                    stGlobal.Precision = Convert.ToInt32(pre);
                }
                rbTSocio.Checked = true;
                stGlobal.TypeRegister = 1;
                stGlobal.TypeMatch = 1;
            }
            catch (Exception ex)
            {
                managementZk.createFileLog("ScreeHome", ex);
            }
        }

        public async Task LoadFingers(bool init = true)
        {
            AppsFitService api = new AppsFitService();
            try
            {
                HuellaData huellaData = HuellaData.Instance;

                if (init)
                {
                    var response = await api.FingerPrintsList(new { DefaultKeyEmpresa = DataSession.DKey });
                    if (response.Success)
                    {
                        huellaData.SetSocios(response.Data);
                        lblCount.Text = $"CANTIDAD REGISTROS : {huellaData.Socios.Count}";
                    }

                    var responseF = await api.FingerPrintsListFijo(new { DefaultKeyEmpresa = DataSession.DKey });
                    if (responseF.Success)
                    {
                        huellaData.SetFijos(responseF.Data);
                        lblCountFijo.Text = $"REGISTROS PERSONAL : {huellaData.Fijos.Count}";
                    }

                    var responseEvent = await api.FingerPrintsListEvent(new { DefaultKeyEmpresa = DataSession.DKey });
                    if (responseEvent.Success)
                    {
                        huellaData.SetProfesionales(responseEvent.Data);
                        lblCountEvent.Text = $"REGISTROS PROFESIONALES : {huellaData.Profesionales.Count}";
                    }
                }
                else
                {
                    switch (stGlobal.TypeRegister)
                    {
                        case 2:
                            var responseF = await api.FingerPrintsListFijo(new { DefaultKeyEmpresa = DataSession.DKey });
                            if (responseF.Success)
                            {
                                huellaData.SetFijos(responseF.Data);
                                lblCountFijo.Text = $"REGISTROS PERSONAL : {huellaData.Fijos.Count}";
                            }
                            break;
                        case 3:
                            var responseEvent = await api.FingerPrintsListEvent(new { DefaultKeyEmpresa = DataSession.DKey });
                            if (responseEvent.Success)
                            {
                                huellaData.SetProfesionales(responseEvent.Data);
                                lblCountEvent.Text = $"REGISTROS PROFESIONALES : {huellaData.Profesionales.Count}";
                            }
                            break;
                        default:
                            var response = await api.FingerPrintsList(new { DefaultKeyEmpresa = DataSession.DKey });
                            if (response.Success)
                            {
                                huellaData.SetSocios(response.Data);
                                lblCount.Text = $"CANTIDAD REGISTROS : {huellaData.Socios.Count}";
                            }
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                managementZk.createFileLog("ScreeHome", ex);
            }

        }


        private void BtnConnection_Click(object sender, EventArgs e)
        {
            try
            {
                managementZk.Initialize();
                managementZk.EventGeneral += OnEventGeneral;
            }
            catch (Exception ex)
            {
                managementZk.createFileLog("ScreeHome", ex);
            }
        }


        private void StatusMessage(string message, bool success)
        {

            try
            {
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
            }
            catch (Exception ex)
            {
                managementZk.createFileLog("ScreeHome", ex);
            }
        }

        private void StatusMessageD(string message, bool success, bool reset = false)
        {

            try
            {
                statusBar1.Message = message;
                statusBar1.StatusBarForeColor = Color.White;

                if (reset)
                {
                    statusBar1.StatusBarBackColor = Color.White;
                }
                else
                {
                    if (success)
                    {

                        statusBar1.StatusBarBackColor = Color.FromArgb(79, 208, 154);
                    }
                    else
                    {
                        statusBar1.StatusBarBackColor = Color.FromArgb(230, 112, 134);
                    }
                }
            }
            catch (Exception ex)
            {
                managementZk.createFileLog("ScreeHome", ex);
            }
        }

        public bool validateHttps(string url)
        {
            try
            {
                bool valid = false;
                string patron = @"https:\/\/\S+";
                bool contieneHTTPS = Regex.IsMatch(url, patron);

                if (contieneHTTPS)
                {
                    valid = true;
                }
                else
                {
                    valid = false;
                }
                return valid;
            }
            catch (Exception ex)
            {
                managementZk.createFileLog("ScreeHome", ex);
                return false;
            }
        }

        private async void BtnSearch_Click(object sender, EventArgs e)
        {
            AppsFitService api = new AppsFitService();
            try
            {
                if (string.IsNullOrEmpty(TxtSearch.Text))
                {
                    StatusMessage("Ingrese Número de documento", false);
                    TxtSearch.Focus();
                }
                else
                {
                    //socio
                    if (stGlobal.TypeRegister == 1) //socio
                    {
                        SearchData(1);
                    }

                    //pf
                    if (stGlobal.TypeRegister == 2)
                    {
                        SearchData(2);
                    }
                    //event
                    if (stGlobal.TypeRegister == 3)
                    {
                        SearchData(3);
                    }
                }
            }
            catch (Exception ex)
            {
                managementZk.createFileLog("ScreeHome", ex);
            }
        }

        //clear register huella
        public void ClearRegister()
        {
            try
            {
                lblMessage.Message = "";
                lblIntents.Text = "3 veces más";
                PicRegister.Image = null;
                lblMessage.StatusBarBackColor = Color.FromArgb(250, 250, 250);
            }
            catch (Exception ex)
            {
                managementZk.createFileLog("ScreeHome", ex);
            }
        }

        public void clearTab2()
        {
            try
            {
                lblMessage.Message = "";
                lblIntents.Text = "";
                PicRegister.Image = BIOCHECK.Properties.Resources.huell;
                ImageUser.Image = BIOCHECK.Properties.Resources.user2;
                TxtSearch.Text = "";
                TxtCode.Text = "";
                TxtName.Text = "";
                TxtSurname.Text = "";
                TxtNro.Text = "";
                lblMessage.StatusBarBackColor = Color.FromArgb(250, 250, 250);
                StatusMessageD("", false, true);

            }
            catch (Exception ex) { managementZk.createFileLog("ScreeHome", ex); }
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (TabControl.SelectedTab == tabPage1)
                {
                    managementZk.SetMatchSearch(true);
                    managementZk.SetIsRegister(false);
                    clearMA();
                }
                if (TabControl.SelectedTab == tabPage2)
                {
                    clearTab2();
                    managementZk.SetMatchSearch(false);
                    dgvHcuotas.Rows.Clear();
                    dgvHpago.Rows.Clear();
                    dgvMembresias.Rows.Clear();
                    dgvAsistences.Rows.Clear();
                }

                if (TabControl.SelectedTab == tabPage3)
                {
                    managementZk.SetIsRegister(false);
                    managementZk.SetMatchSearch(false);
                    getConfiguration();
                }
            }
            catch (Exception ex)
            {
                managementZk.createFileLog("ScreeHome", ex);
            }
        }

        private void BtnDisconnection_Click(object sender, EventArgs e)
        {
            managementZk.Disconnect();
            managementZk.EventGeneral -= OnEventGeneral;
        }

      
        public void getConfiguration()
        {
            try
            {
                if (!string.IsNullOrEmpty(DataSession.DKey) && !string.IsNullOrEmpty(DataSession.Name))
                {
                    txtCbusiness.Text = DataSession.Unidad.ToString();
                    txtCsede.Text = DataSession.Sede.ToString();
                    txtCng.Text = DataSession.Unidad.ToString();
                    txtCKey.Text = DataSession.DKey.ToString();

                    if (!string.IsNullOrEmpty(DataSession.Logo))
                    {
                        using (WebClient webClient = new WebClient())
                        {
                            byte[] imageData = webClient.DownloadData(DataSession.Logo);
                            using (MemoryStream ms = new MemoryStream(imageData))
                            {
                                PicCLogo.Image = Image.FromStream(ms);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                managementZk.createFileLog("ScreeHome", ex);
            }

        }

        private void PicCloseHome_Click(object sender, EventArgs e)
        {
            try
            {
                List<Form> formsToClose = new List<Form>();
                foreach (Form form in Application.OpenForms)
                {
                    formsToClose.Add(form);
                }
                foreach (Form form in formsToClose)
                {
                    form.Close();
                    form.Dispose();
                }
                managementZk.EventGeneral -= OnEventGeneral;
            }
            catch (Exception ex)
            {
                managementZk.createFileLog("ScreeHome", ex);
            }
        }



        private async void dgridMembresias_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                try
                {
                    dgvMembresias.ClearSelection();
                    dgvMembresias.Rows[e.RowIndex].Selected = true;

                    var membresias = dataSocioAll.Membresias[e.RowIndex];
                    int CodigoMembresia = membresias.CodigoMenbresia;
                    int CodigoSede = membresias.CodigoSede;
                    int CodigoUnidadNegocio = DataSession.Unidad;

                    AppsFitService serv = new AppsFitService();


                    var commonParameters = new
                    {
                        CodigoUnidadNegocio = CodigoUnidadNegocio,
                        CodigoSede = CodigoSede,
                        Membresia = CodigoMembresia
                    };

                    var respHistorial = await serv.AsistencesList(commonParameters);
                    var HPC = await serv.HistorialPC(commonParameters);

                    if (respHistorial.Success)
                    {
                        dataSocioAll.Asistences = respHistorial.Data;
                    }

                    dataSocioAll.Pagos = HPC.Data.Pagos;
                    dataSocioAll.Cuotas = HPC.Data.Cuotas;
                    dataSocioAll.Incidencias = HPC.Data.Incidencias;

                    MessageStatusMembresia(membresias.ObtenerTiempoVencimiento, membresias.Estado);
                    lblPlan.Text = membresias.Descripcion.ToUpper();

                    string deudaMem = Convert.ToInt32(membresias.Debe) > 0 ? $"DEBE {membresias.Debe} EN MEMBRESIA" : "";
                    StlyDeudaM(deudaMem, Convert.ToInt32(membresias.Debe) > 0 ? true : false);

                    DataStatic.MembresiasSelected = membresias;
                    if (membresias.Estado == 1)
                    {
                        btnMarkAsistence.Visible = true;
                    }
                    else
                    {
                        btnMarkAsistence.Visible = false;
                    }

                    _ = Task.Run(() => LoadDataToGrids());

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error : {ex.Message}\t Metodo: {ex.TargetSite},\t Linea:{ex.StackTrace}");
                }
            }
        }




        //clears--------------------
        private void clearMA()
        {
            try
            {
                lblFullName_.Text = "NOMBRES, APELLIDOS COMPLETOS";
                PicMaUser.Image = null;
                lblPlan.Text = "NOMBRE DEL PLAN";
                lblMessageMem.Text = "ESTADO MEMBRESIA";
                lblMessage.Message = "";
                picHuellaMA.Image = BIOCHECK.Properties.Resources.huell;
                PicMaUser.Image = BIOCHECK.Properties.Resources.user2;

                dgvAsistences.Rows.Clear();
                dgvHcuotas.Rows.Clear();
                dgvHpago.Rows.Clear();
                dgvMembresias.Rows.Clear();
                dgvIncidencias.Rows.Clear();

                StatusMessageD("", false, true);
            }
            catch (Exception ex)
            {
                managementZk.createFileLog("ScreeHome", ex);
            }
        }



        private void timerNow_Tick(object sender, EventArgs e)
        {
            DateTime horaActual = DateTime.Now;
            string horaFormateada = horaActual.ToString("HH:mm:ss");
            lblHour.Text = horaFormateada;
        }
        private void DateFormat()
        {
            DateTime dnow = DateTime.Now;
            string dnowFormat = dnow.ToString("dddd d 'de' MMMM 'de' yyyy");
            lblDate.Text = dnowFormat;
        }


        private async void btnMarkAsistence_Click(object sender, EventArgs e)
        {

            if (managementZk.isInitialized)
            {
                try
                {
                    if (dataSocioAll.Membresias.Count == 0)
                    {
                        StatusMessageD($"DEBE CONTAR CON UNA MEMBRESIA", false);

                        return;
                    }
                    else
                    {
                        string message = ValidateMembresia(dataSocioAll.MembresiasSelected);

                        if (!string.IsNullOrEmpty(message))
                        {
                            StatusMessageD($"{message}", false);
                            return;
                        }
                        else
                        {
                            if (dataSocioAll.MembresiasSelected.Debe == 0)
                            {
                                //? Success, validate and mark attendance
                                AppsFitService serv = new AppsFitService();

                                var data = new
                                {
                                    CodigoUnidadNegocio = DataSession.Unidad,
                                    Sede = DataSession.Sede,
                                    Socio = dataSocioAll.MembresiasSelected.CodigoSocio,
                                    Membresia = dataSocioAll.MembresiasSelected.CodigoMenbresia
                                };

                                var res = await serv.MarkAsistence(data);
                                StatusMessageD($"{res.Message1}", res.Success);
                                if (res.Success)
                                {
                                    // Update List
                                    DataGridViewCellEventArgs cellEventArgs = new DataGridViewCellEventArgs(0, 0);
                                    dgridMembresias_CellClick(dgvMembresias, cellEventArgs);
                                }
                            }
                            else
                            {
                                //Debe
                                StatusMessageD("", false, true);
                                MessageBox.Show($"TIENES UNA DEUDA DE {dataSocioAll.MembresiasSelected.Debe} !");
                                return;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    managementZk.createFileLog("ScreeHome", ex);
                }
            }
            else
            {
                StatusMessageD($"DEBE CONECTAR EL DISPOSITIVO", false);
                return;
            }
        }




        // Function to validate the membership and return a message
        private string ValidateMembresia(Membresia membresia)
        {
            try
            {
                if (membresia.ObtenerDisponibilidadHorarioPaquete <= 0)
                {
                    return "ESTA MEMBRESIA NO TIENE ACCESO PARA ESTA SEDE.";
                }

                if (membresia.flagPaqueteSedePermiso <= 0)
                {
                    return "HORARIO NO DISPONIBLE";
                }

                if (membresia.NroIngreso < membresia.NroIngresoActual)
                {
                    return "NRO ASISTENCIAS LLEGO A SU LIMITE, REVISA EL NRO DE SESIONES DE LA MEMBRESIA";
                }
            }
            catch (Exception ex)
            {
                managementZk.createFileLog("ScreeHome", ex);
                return string.Empty;
            }
            return string.Empty;

        }

        private void ckAuto_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            stGlobal.CheackAutomatic = checkBox.Checked;
        }

       

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment deployment = ApplicationDeployment.CurrentDeployment;

                try
                {
                    UpdateCheckInfo updateCheckInfo = deployment.CheckForDetailedUpdate();
                    if (updateCheckInfo.UpdateAvailable)
                    {
                        deployment.Update();
                        MessageBox.Show("Se ha instalado una nueva versión de la aplicación. Reinicia la aplicación para aplicar los cambios.");
                    }
                    else
                    {
                        MessageBox.Show("No hay actualizaciones disponibles.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al verificar actualizaciones: " + ex.Message);
                }
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label25_Click(object sender, EventArgs e)
        {

        }


        private void rbPersonal_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTPF.Checked)
            {
                stGlobal.TypeRegister = 2;
            }
        }

        private void rbTPE_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTPE.Checked)
            {
                stGlobal.TypeRegister = 3;
            }
        }
        private void rbTSocio_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTSocio.Checked)
            {
                stGlobal.TypeRegister = 1;
            }
        }



        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            lblPrecicion.Text = $"{tbPrecicion.Value}%";
        }

        private void tbPrecicion_ValueChanged(object sender, EventArgs e)
        {
            int pre = tbPrecicion.Value;

            stGlobal.Precision = pre;
            DataManagerD dm = new DataManagerD();
            dm.SaveData(pre.ToString());
        }




        //******************************************************+search by register+****************************************
        private async void SearchData(int type)
        {
            try
            {
                AppsFitService api = new AppsFitService();
                var body = new
                {
                    DefaultKeyEmpresa = DataSession.DKey,
                    Filtre = TxtSearch.Text,
                };

                ResponseModel response;
                switch (type)
                {
                    case 1:
                        response = await api.SearchSocio(body);
                        break;
                    case 2:
                        response = await api.SearchPF(TxtSearch.Text);
                        break;
                    default:
                        response = await api.SearchPEvent(TxtSearch.Text);
                        break;
                }

                if (response.Success)
                {
                    DataItem item = response.Data;
                    TxtCode.Text = Convert.ToString(item.code);
                    TxtName.Text = Convert.ToString(item.name);
                    TxtSurname.Text = Convert.ToString(item.surnames);
                    TxtNro.Text = Convert.ToString(item.nro_document);

                    if (!string.IsNullOrEmpty(item.image) && validateHttps(item.image))
                    {
                        using (WebClient webClient = new WebClient())
                        {
                            byte[] imageData = webClient.DownloadData(item.image);
                            using (MemoryStream ms = new MemoryStream(imageData))
                            {
                                ImageUser.Image = Image.FromStream(ms);
                            }
                        }
                    }

                    managementZk.ResetListenRegister();
                    ClearRegister();
                    DataSession.Filtre = TxtSearch.Text;
                    DataSession.Code = item.code;
                    stGlobal.SearchRegister = TxtSearch.Text;
                }
                else
                {
                    DataSession.Filtre = "";
                    stGlobal.SearchRegister = "";
                    StatusMessage($"{response.Message1} - {DateTime.Now}", false);
                    TxtCode.Text = "";
                    TxtName.Text = "";
                    TxtSurname.Text = "";
                    TxtNro.Text = "";
                    ImageUser.Image = BIOCHECK.Properties.Resources.user1;
                }

            }
            catch (Exception ex)
            {
                managementZk.createFileLog("ScreeHome", ex);
            }
        }

        private void rbTMFijo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTMFijo.Checked)
            {
                stGlobal.TypeMatch = 2;
            }
        }

        private void rbTMEvent_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTMEvent.Checked)
            {
                stGlobal.TypeMatch = 3;
            }
        }

        private void btnShowLogs_Click(object sender, EventArgs e)
        {
            var logs = managementZk.ShowLogs();
            txtLogs.Text = logs.ToString();
        }

        private void btnDeleteLogs_Click(object sender, EventArgs e)
        {
            
            managementZk.EliminarArchivoLog();
        }

        private void btnDevLog_Click(object sender, EventArgs e)
        {
            try
            {
                throw new Exception("Esto es una excepción intencional para pruebas.");
            }
            catch (Exception ex)
            {
                managementZk.createFileLog("ScreeHome", ex);
            }
        }

        //******************************************************-search by register-****************************************


    }
}
