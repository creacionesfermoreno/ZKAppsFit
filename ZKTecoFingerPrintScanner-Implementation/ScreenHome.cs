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
        public PictureBox PicImagePersonal_ { get; set; }

        private DataSocioAll dataSocioAll;
        private STGlobal stGlobal;

        private int SelectedIndexPersonal = 0;

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
            PicImagePersonal_ = pbAPHuella;
            lblIntents_ = lblIntents;

            dataSocioAll = DataSocioAll.Instance;
            stGlobal = STGlobal.Instance;

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



        //*************************************************** OnEventPersonal ***************************************************
        public void TOneControl(HorarioFijo hfijo, bool clear = false)
        {
            lblT1M1.Text = clear ? "0:00" : hfijo.FechaHoraIngresoTexto ?? "0:00";
            lblT1M2.Text = clear ? "0:00" : hfijo.FechaHoraRefrigerioSalidaTexto ?? "0:00";
            lblT1M3.Text = clear ? "0:00" : hfijo.FechaHoraRefrigerioRetornoTexto ?? "0:00";
            lblT1M4.Text = clear ? "0:00" : hfijo.FechaHoraSalidaTexto ?? "0:00";

        }
        public void TTwoControl(HorarioFijo hfijo, bool clear = false)
        {
            lblT2M1.Text = clear ? "0:00" : hfijo.FechaHoraIngreso_TurnoTardeTexto ?? "0:00";
            lblT2M2.Text = clear ? "0:00" : hfijo.FechaHoraRefrigerioSalida_TurnoTardeTexto ?? "0:00";
            lblT2M3.Text = clear ? "0:00" : hfijo.FechaHoraRefrigerioRetorno_TurnoTardeTexto ?? "0:00";
            lblT2M4.Text = clear ? "0:00" : hfijo.FechaHoraSalida_TurnoTardeTexto ?? "0:00";
        }
        public void InfoPersonal(SocioModel socio, bool clear = false)
        {
            lblPName.Text = clear ? "NOMBRE, APELLIDOS" : $"{socio.Nombre} {socio.Apellidos}".ToUpper();
            lblPCargo.Text = clear ? "CARGO" : $"{socio.Descripcion}".ToUpper();
            lblPPhone.Text = clear ? "CELULAR" : $"{socio.Celular}".ToUpper();

            if (clear == false)
            {
                if (string.IsNullOrEmpty(socio.ImagenUrl) == false && validateHttps(socio.ImagenUrl) == true)
                {
                    using (WebClient webClient = new WebClient())
                    {
                        byte[] imageData = webClient.DownloadData(socio.ImagenUrl);
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            pboxPImage.Image = Image.FromStream(ms);
                        }
                    }
                }
            }
            else
            {
                pboxPImage.Image = BIOCHECK.Properties.Resources.user2;
            }
        }

        //Render Horario profesional
        private void RenderLViewProfesionales()
        {
            try
            {
                HorarioSigleton hinstance = HorarioSigleton.Instance;
                var HorariosProfesionales = hinstance.HorarioProfesional;

                if (HorariosProfesionales != null && HorariosProfesionales.Count > 0)
                {
                    lboxProfesionales.Items.Clear();
                    lboxProfesionales.Items.Add(string.Format("{0,-150} {1,-20} {2,-20}", "CLASE", "INGRESO", "SALIDA"));
                    lboxProfesionales.Items.Add(new string('-', 300));

                    foreach (HorarioProfesional p in HorariosProfesionales)
                    {
                        String clase = $"{p.DesSala} - {p.Disciplina} {p.HoraInicioTexto} - {p.HoraFinTexto} AFORO {p.CantidadAsistencias} DE {p.CapacidadPermitida}";
                        string claseAlineada = clase.PadRight(120);
                        string ingresoAlineado = string.IsNullOrEmpty(p.FechaHoraIngresoTxt) ? "--:--" : p.FechaHoraIngresoTxt.PadRight(20);
                        string salidaAlineada = string.IsNullOrEmpty(p.FechaHoraSalidaTxt) ? "--:--" : p.FechaHoraSalidaTxt.PadRight(20);
                        lboxProfesionales.Items.Add($"{claseAlineada} {ingresoAlineado} {salidaAlineada}");
                    }
                }
                else
                {
                    lboxProfesionales.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                managementZk.createFileLog("ScreenHome", ex);
            }
        }

        //Render Horario fijo
        public void renderHorarioFijo(HorarioFijo hfijo)
        {
            if (hfijo.tipoTurno == 1)
            {
                // T1
                pTurno1.Visible = true;
                pTurno2.Visible = false;
                TOneControl(hfijo);
            }
            else if (hfijo.tipoTurno == 2)
            {
                // T2
                pTurno2.Visible = true;
                pTurno1.Visible = false;
                TTwoControl(hfijo);
            }
            else if (hfijo.tipoTurno == 3)
            {
                //T1 and T2
                pTurno1.Visible = true;
                pTurno2.Visible = true;
                TOneControl(hfijo);
                TTwoControl(hfijo);
            }
            else
            {
                Console.WriteLine("");
            }
        }


        //mark fijo
        public async Task MarckPersonalFijoAsync(int operation)
        {
            try
            {
                AppsFitService api = new AppsFitService();
                HorarioSigleton inst = HorarioSigleton.Instance;
                if (inst.SelectedPersonal)
                {
                    var body = new
                    {
                        CodigoUnidadNegocio = DataSession.Unidad,
                        CodigoSede = DataSession.Sede,
                        NumeroDocumento = inst.Personal.DNI,
                        OperacionMarcacion = operation
                    };
                    var resp = await api.MarcarPersonalFijo(body);

                    if (resp.Success)
                    {
                        await managementZk.DataHorariosPersonalFijoReload();
                        renderHorarioFijo(inst.HorarioFijo);
                        StatusMessageD($"ASISTENCIA REGISTRADA", true, false);
                        inst.SelectedPersonal = false;
                    }
                    else
                    {
                        StatusMessageD($"{resp.Message1}", false, false);
                    }
                }
                else
                {
                    StatusMessageD($"", false, true);
                    MessageBox.Show("PARA MARCAR DEBE BUSCAR UN PERSONAL VÁLIDO");
                }
            }
            catch (Exception ex)
            {

                managementZk.createFileLog("ScreenHome", ex);
            }
        }


        public async Task MarkPersonalProfesional(int type)
        {
            try
            {
                AppsFitService api = new AppsFitService();
                HorarioSigleton hsgt = HorarioSigleton.Instance;
                var Horario = hsgt.HorarioProfesional[SelectedIndexPersonal];
                var body = new
                {
                    CodigoUnidadNegocio = DataSession.Unidad,
                    CodigoSede = DataSession.Sede,
                    CodigoHorarioClasesConfiguracion = Horario.CodigoHorarioClasesConfiguracion,
                    CodigoHorarioClasesTiempoReal = Horario.CodigoHorarioClasesTiempoReal,
                    CodigoProfesional = Horario.CodigoProfesional,
                    CodigoPersonalAsistencia = Horario.CodigoPersonalAsistencia,
                    DiaNumero = Horario.DiaNumero,
                    TipoAsistencia = type
                };
                var resp = await api.MarcarPersonalProfesional(body);
                if (resp.Success)
                {
                    await managementZk.DataHorariosProfesionalReload();
                    RenderLViewProfesionales();
                    StatusMessageD($"ASISTENCIA REGISTRADA", true, false);
                    hsgt.SelectedPersonal = false;
                }
                else { StatusMessageD($"{resp.Message1}", false, false); }
            }
            catch (Exception ex)
            {
                managementZk.createFileLog("ScreenHome", ex);
            }
        }

        //*************************************************** *** ***************************************************


        public void OnEventPersonal(string type)
        {
            try
            {
                HorarioSigleton hinstance = HorarioSigleton.Instance;

                switch (type)
                {
                    case "LHF":
                        HorarioFijo hfijo = hinstance.HorarioFijo;
                        renderHorarioFijo(hfijo);
                        InfoPersonal(hinstance.Personal);
                        StatusMessage(hinstance.Message, true);
                        StatusMessageD($"", false, true);

                        break;
                    case "LHF-F":
                        TOneControl(new HorarioFijo(), true);
                        TTwoControl(new HorarioFijo(), true);
                        InfoPersonal(new SocioModel(), true);
                        StatusMessage("NO SE ENCONTRO PERSONAL", false);
                        StatusMessageD($"", false, true);
                        break;

                    //profesionales
                    case "LHPROF":
                        RenderLViewProfesionales();
                        InfoPersonal(hinstance.Personal);
                        StatusMessage(hinstance.Message, true);
                        StatusMessageD($"", false, true);
                        break;

                    case "LHPROF-F":
                        lboxProfesionales.Items.Clear();
                        StatusMessage("NO SE ENCONTRO PERSONAL", false);
                        StatusMessageD($"", false, true);
                        break;

                    default:
                        Console.WriteLine("");
                        break;
                }
            }
            catch (Exception ex)
            {

                managementZk.createFileLog("ScreenHome", ex);
            }
        }

        public void OnEventGeneral(string message, dynamic message2)
        {

            try
            {
                switch (message)
                {
                    case "MA-T":
                        if (dataSocioAll.Membresias != null && dataSocioAll.Membresias.Count > 0)
                        {
                            var membresia = dataSocioAll.Membresias[0];
                            txtMPromo.Text = membresia.NombrePaquete.ToString();
                            txtMFecha.Text = membresia.FCrecionText.ToString();
                            txtMFInicio.Text = membresia.DesFechaInicio.ToString();
                            txtMFin.Text = membresia.DesFechaFin.ToString();
                            txtMPrecio.Text = membresia.Costo.ToString();
                            txtMAcuenta.Text = membresia.MontoTotal.ToString();
                            txtMDebe.Text = membresia.Debe.ToString();
                            txtMFrezing.Text = membresia.CantidadFreezing.ToString();
                            txtMFrezingTom.Text = membresia.CantidadFreezingTomados.ToString();
                            txtMFrezingActual.Text = membresia.CantidadAsistencia.ToString();
                            txtMContrato.Text = membresia.NroContrato.ToString();
                            txtMResponsable.Text = membresia.AsesorComercial.ToString();
                            txtMSede.Text = membresia.CodigoSede.ToString();

                            btnMarkAsistence.Visible = true;
                            lblPlan.Text = membresia.Descripcion.ToUpper();
                            MessageStatusMembresia(membresia.ObtenerTiempoVencimiento, membresia.Estado);

                            dataSocioAll.MembresiasSelected = membresia;
                            string deudaMem = dataSocioAll.MembresiasSelected.Debe > 0 ? $"DEBE {dataSocioAll.MembresiasSelected.Debe} EN MEMBRESIA" : "";
                            StlyDeudaM(deudaMem, !string.IsNullOrEmpty(deudaMem));
                            if (stGlobal.CheackAutomatic)
                            {
                                btnMarkAsistence.PerformClick();
                            }
                            _ = Task.Run(() => LoadDataToGrids());

                            StatusMessage(dataSocioAll.MessageGenericD, true);
                            SocioInfoMatch(true);
                        }

                        break;
                    case "MA-F":
                        SocioInfoMatch(false);
                        StatusMessage($"No se encontro socio {DateTime.Now}", false);
                        MessageStatusMembresia("ESTADO DE MEMBRESIA", 0, true);
                        StatusMessageD("", false, true);
                        lblPlan.Text = "";
                        clearMembresiaText();
                        listBox1.Items.Clear();
                        listBox2.Items.Clear();
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



        //clear txt membresia
        public void clearMembresiaText()
        {
            txtMPromo.Text = "";
            txtMFecha.Text = "";
            txtMFInicio.Text = "";
            txtMFin.Text = "";
            txtMPrecio.Text = "";
            txtMAcuenta.Text = "";
            txtMDebe.Text = "";
            txtMFrezing.Text = "";
            txtMFrezingTom.Text = "";
            txtMFrezingActual.Text = "";
            txtMContrato.Text = "";
            txtMResponsable.Text = "";
            txtMSede.Text = "";

        }

        private void LoadDataToGrids()
        {

            try
            {
                if (dataSocioAll.Asistences != null && dataSocioAll.Asistences.Count > 0)
                {
                    listBox1.Items.Clear();
                    listBox1.Items.Add(string.Format("{0,-25} {1,-25} {2,-25} {3,-30}", "Fecha de Creación", "Hora", "Día de la Semana", "Usuario de Creación"));
                    listBox1.Items.Add(new string('-', 150));

                    foreach (Asistence a in dataSocioAll.Asistences)
                    {
                        listBox1.Items.Add(string.Format("{0,-25} {1,-25} {2,-25} {3,-30}", a.FCreacionText, a.HourText, a.DiaSemana, a.UsuarioCreacion));
                    }

                }

                if (dataSocioAll.Incidencias != null && dataSocioAll.Incidencias.Count > 0)
                {
                    listBox2.Items.Clear();
                    listBox2.Items.Add(string.Format("{0,-30} {1,-30} {2,-60}", "Fecha de Creación", "Usuario de Creación", "Ocurrencia"));
                    listBox2.Items.Add(new string('-', 100));
                    foreach (Incidencia c in dataSocioAll.Incidencias)
                    {
                        listBox2.Items.Add(string.Format("{0,-30} {1,-30} {2,-60}", c.FechaCreacion, c.UsuarioCreacion, c.Ocurrencia));
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
                stGlobal.KeyEmpresa = DataSession.DKey;
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
                managementZk.EventPersonal += OnEventPersonal;
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
                HorarioSigleton hinstnc = HorarioSigleton.Instance;

                if (TabControl.SelectedTab == tabPage1)
                {
                    managementZk.SetMatchSearch(true);
                    managementZk.SetIsRegister(false);
                    stGlobal.TypeMatch = 1;
                    clearMA();
                    lblDev.Text = stGlobal.TypeMatch.ToString();
                }
                if (TabControl.SelectedTab == tabPage2)
                {
                    clearTab2();
                    managementZk.SetMatchSearch(false);
                }

                if (TabControl.SelectedTab == tabPage3)
                {
                    managementZk.SetIsRegister(false);
                    managementZk.SetMatchSearch(false);
                    getConfiguration();
                }
                if (TabControl.SelectedTab == tabPage4)
                {
                    managementZk.SetMatchSearch(true);
                    managementZk.SetIsRegister(false);
                    rbTMFijo.Checked = true;
                    stGlobal.TypeMatch = 2;
                    lblDev.Text = stGlobal.TypeMatch.ToString();
                    hinstnc.SelectedPersonal = false;
                    TOneControl(new HorarioFijo(), true);
                    TTwoControl(new HorarioFijo(), true);
                    StatusMessageD("", false, true);
                    InfoPersonal(new SocioModel(),true);
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
            managementZk.EventPersonal -= OnEventPersonal;
        }


        public void getConfiguration()
        {
            try
            {
                if (!string.IsNullOrEmpty(DataSession.DKey) && !string.IsNullOrEmpty(DataSession.Name))
                {
                    txtCbusiness.Text = DataSession.Name.ToString();
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
                managementZk.EventPersonal -= OnEventPersonal;
            }
            catch (Exception ex)
            {
                managementZk.createFileLog("ScreeHome", ex);
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
                    StatusMessageD("", false, true);

                    if (dataSocioAll.Membresias != null && dataSocioAll.Membresias.Count == 0)
                    {
                        StatusMessageD($"DEBE CONTAR CON UNA MEMBRESIA", false);

                        return;
                    }
                    else
                    {

                        if (dataSocioAll.MembresiasSelected != null)
                        {
                            string message = ValidateMembresia(dataSocioAll.MembresiasSelected);

                            if (!string.IsNullOrEmpty(message))
                            {
                                StatusMessageD($"{message}", false);
                                return;
                            }
                            else
                            {
                                if (Convert.ToInt32(dataSocioAll.MembresiasSelected.Debe) == 0)
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
                                    managementZk.createFile(res.Message1 + "" + res.Success);
                                    if (res.Success)
                                    {
                                        // Update List
                                        await managementZk.ReloadDataAsistence();
                                        _ = Task.Run(() => LoadDataToGrids());
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
                if (membresia.Estado == 2)
                {
                    return "😨 SU MEMBRESÍA FINALIZÓ 👎";
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
        //******************************************************-search by register-****************************************
        private void rbTMFijo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTMFijo.Checked)
            {
                stGlobal.TypeMatch = 2;
                panelPFijoContent.Visible = true;
                panelPProfeContent.Visible = false;
                pbAPHuella.Image = BIOCHECK.Properties.Resources.huell;
                HorarioSigleton hsigleton = HorarioSigleton.Instance;
                hsigleton.SelectedPersonal = false;
                TOneControl(new HorarioFijo(), true);
                TTwoControl(new HorarioFijo(), true);
                StatusMessageD("", false,true);
                InfoPersonal(new SocioModel(), true);

            }
        }

        private void rbTMEvent_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTMEvent.Checked)
            {
                stGlobal.TypeMatch = 3;
                panelPFijoContent.Visible = false;
                panelPProfeContent.Visible = true;
                pbAPHuella.Image = BIOCHECK.Properties.Resources.huell;
                lboxProfesionales.Items.Clear();
                HorarioSigleton hsigleton = HorarioSigleton.Instance;
                hsigleton.SelectedPersonal= false;
                StatusMessageD("", false, true);
                InfoPersonal(new SocioModel(), true);
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

        private void dgvIncidencias_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }



        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (listBox1.SelectedIndex != -1 && listBox1.SelectedIndex > 1)
            //{
            //    int selectedIndex = listBox1.SelectedIndex;

            //}
        }

        private void lboxProfesionales_SelectedIndexChanged(object sender, EventArgs e)
        {
            HorarioSigleton inst = HorarioSigleton.Instance;
            if (lboxProfesionales.SelectedIndex != -1 && lboxProfesionales.SelectedIndex > 1)
            {
                int selectedIndex = lboxProfesionales.SelectedIndex;
                SelectedIndexPersonal = selectedIndex - 2;
                inst.SelectedPersonal = true;
            }
            else
            {
                inst.SelectedPersonal = false;
                SelectedIndexPersonal = 0;
            }
        }

        private async void btnMIngresoProf_Click(object sender, EventArgs e)
        {
            HorarioSigleton hsgt = HorarioSigleton.Instance;

            if (hsgt.SelectedPersonal)
            {
                await MarkPersonalProfesional(1);
            }
            else
            {
                StatusMessageD($"", false, true);
                MessageBox.Show("DEBE SELECIONAR UNA CLASE");
            }
        }

        private async void btnMSalidaProf_Click(object sender, EventArgs e)
        {
            HorarioSigleton hsgt = HorarioSigleton.Instance;

            if (hsgt.SelectedPersonal)
            {
                await MarkPersonalProfesional(2);
            }
            else
            {
                StatusMessageD($"", false, true);
                MessageBox.Show("DEBE SELECIONAR UNA CLASE");
            }
        }

        private void btnMarcarEntradaT1_Click(object sender, EventArgs e)
        {
            _ = MarckPersonalFijoAsync(1);
        }

        private void btnMarcarIBT1_Click(object sender, EventArgs e)
        {
            _ = MarckPersonalFijoAsync(2);
        }

        private void btnMarcarFBT1_Click(object sender, EventArgs e)
        {
            _ = MarckPersonalFijoAsync(3);
        }
        private void btnMarcarSalidaT1_Click(object sender, EventArgs e)
        {
            _ = MarckPersonalFijoAsync(4);
        }
        private void btnMarcarEntradaT2_Click(object sender, EventArgs e)
        {
            _ = MarckPersonalFijoAsync(5);
        }

        private void btnMarcarIBT2_Click(object sender, EventArgs e)
        {
            _ = MarckPersonalFijoAsync(6);
        }

        private void btnMarcarFBT2_Click(object sender, EventArgs e)
        {
            _ = MarckPersonalFijoAsync(7);
        }

        private void btnMarcarSalidaT2_Click(object sender, EventArgs e)
        {
            _ = MarckPersonalFijoAsync(8);
        }

       
    }
}
