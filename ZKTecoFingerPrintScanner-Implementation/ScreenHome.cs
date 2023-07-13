﻿using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        public StatusBar lblMessage_ { get; set; }
        public PictureBox picHuellaMA_ { get; set; }
        public PictureBox PicRegister_ { get; set; }

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
            // AdjustFormSize(80);
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

        private void button3_Click(object sender, EventArgs e)
        {

            foreach (Asistence a in DataStatic.Asistences)
            {
                dgvAsistences.Rows.Add(a.FCreacionText, a.HourText, a.DiaSemana, a.UsuarioCreacion);
            }


            foreach (Pago p in DataStatic.Pagos)
            {
                dgvHpago.Rows.Add(p.Estado, p.desFechaPago, p.Monto, p.NroComprobante, p.DesFormaPago, p.UsuarioCreacion);
            }

        }

        public void OnEventGeneral(string message, dynamic message2)
        {

            try
            {
                switch (message)
                {
                    case "MA-T":
                        dgvMembresias.Rows.Clear();

                        StatusMessage(DataStatic.Socio.MessageExtra, true);
                        SocioInfoMatch(true);
                        if (DataStatic.Membresias.Count > 0)
                        {
                            lblPlan.Text = DataStatic.Membresias[0].Descripcion.ToUpper();
                            MessageStatusMembresia(DataStatic.Membresias[0].ObtenerTiempoVencimiento, DataStatic.Membresias[0].Estado);


                            

                        }
                        foreach (Membresia m in DataStatic.Membresias)
                        {
                            dgvMembresias.Rows.Add(
                              m.colorEstado,
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
                        if (DataStatic.Membresias.Count > 0)
                        {
                            dgvMembresias.Rows[0].Selected = true;
                            DataStatic.MembresiasSelected = DataStatic.Membresias[0];
                            if (CheckBoxValue.IsChecked)
                            {
                                btnMarkAsistence.PerformClick();
                            }
                        }

                        Thread dataLoadThread = new Thread(LoadDataToGrids);
                        dataLoadThread.Start();

                        break;
                    case "MA-F":
                        dgvMembresias.Rows.Clear();
                        dgvAsistences.Rows.Clear();
                        dgvHpago.Rows.Clear();
                        dgvHcuotas.Rows.Clear();
                        SocioInfoMatch(false);
                        StatusMessage("No se encontro socio", false);
                        MessageStatusMembresia("", 0, true);
                        lblPlan.Text = "";
                        break;
                    default:
                        break;
                }
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        private void LoadDataToGrids()
        {
            dgvHcuotas.Rows.Clear();
            dgvHpago.Rows.Clear();
            dgvAsistences.Rows.Clear();
            dgvIncidencias.Rows.Clear();
            try
            {
                foreach (Asistence a in DataStatic.Asistences)
                {
                    dgvAsistences.Rows.Add(a.FCreacionText, a.HourText, a.DiaSemana, a.UsuarioCreacion);
                }

                foreach (Pago p in DataStatic.Pagos)
                {
                    dgvHpago.Rows.Add(p.desFechaPago, p.Monto, p.NroComprobante, p.DesFormaPago, p.UsuarioCreacion);
                }

                foreach (Cuota c in DataStatic.Cuotas)
                {
                    dgvHcuotas.Rows.Add(c.Fecha, c.Monto, c.UsuarioCreacion);
                }

                foreach (Incidencia c in DataStatic.Incidencias)
                {
                    dgvIncidencias.Rows.Add(c.FechaCreacion, c.UsuarioCreacion, c.Ocurrencia);
                }

            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción en el hilo secundario
                managementZk.createFile(ex.Message.ToString());
            }
        }

        //set socio match
        private void SocioInfoMatch(bool success)
        {
            if (success)
            {
                var socio = DataStatic.Socio;
                lblFullName_.Text = $"{socio.Nombre}, {socio.Apellidos}".ToUpper();
                if (!string.IsNullOrEmpty(socio.ImagenUrl))
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
                PicMaUser.Image = null;
            }
        }

        private void MessageStatusMembresia(string message, int status, bool clear = false)
        {
            string finish = "😨 SU MEMBRESÍA FINALIZÓ 👎";
            lblMessageMem.Text = status == 2 ? finish : message;
            lblMessageMem.ForeColor = Color.White;
            lblMessageMem.BackColor = clear ? Color.White : (status == 1 ? Color.Green : Color.Gray);
        }

        MaterialSkinManager skinManager = MaterialSkinManager.Instance;

        private bool isFirstLoad = true;
        private void ScreenHome_Load(object sender, EventArgs e)
        {

            LoadingForm loading = new LoadingForm();
            loading.Opacity = 0;
            loading.Visible = false;
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
                    _ = LoadFingers();
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
        }

        public async Task LoadFingers()
        {
            AppsFitService serv = null;
            var task = Task.Run(async () =>
            {
                serv = new AppsFitService();
                var response = await serv.FingerPrintsList(new { DefaultKeyEmpresa = DataSession.DKey });
                return response;
            });
            try
            {
                var result = await task;

                if (result.Success)
                {
                    SocioData.SetListaUsers(result.Data);
                    lbldev.Text = $"Total de Huellas cargadas:{SocioData.socios.Count}";
                }
                else
                {
                }
            }
            catch (Exception ex)
            {

            }
        }


        private void BtnConnection_Click(object sender, EventArgs e)
        {
            managementZk.Initialize();

            //connect and suscribe event
            managementZk.EventGeneral += OnEventGeneral;
        }


        private void StatusMessage(string message, bool success)
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

        private async void BtnSearch_Click(object sender, EventArgs e)
        {
            AppsFitService api = new AppsFitService();
            if (TxtSearch.Text == "")
            {
                StatusMessage("Ingrese Número de documento", false);
                TxtSearch.Focus();
            }
            else
            {
                var body = new
                {
                    DefaultKeyEmpresa = DataSession.DKey,
                    Filtre = TxtSearch.Text,
                };
                try
                {
                    var response = await api.SearchSocio(body);
                    if (response.Success == true)
                    {
                        DataItem item = response.Data as DataItem;
                        TxtCode.Text = Convert.ToString(item.code);
                        TxtName.Text = Convert.ToString(item.name);
                        TxtSurname.Text = Convert.ToString(item.surnames);
                        TxtNro.Text = Convert.ToString(item.nro_document);

                        using (WebClient webClient = new WebClient())
                        {
                            byte[] imageData = webClient.DownloadData(item.image);
                            using (MemoryStream ms = new MemoryStream(imageData))
                            {
                                ImageUser.Image = Image.FromStream(ms);
                            }
                        }
                        managementZk.ResetListenRegister();
                        ClearRegister();
                        DataSession.Filtre = TxtSearch.Text;
                    }
                    else
                    {
                        DataSession.Filtre = "";
                        StatusMessage($"{response.Message1}", false);
                        TxtCode.Text = "";
                        TxtName.Text = "";
                        TxtSurname.Text = "";
                        TxtNro.Text = "";
                        ImageUser.Image = Properties.Resources.user1;
                    }

                }
                catch (HttpRequestException ex)
                {

                }
            }

        }

        //clear register huella
        public void ClearRegister()
        {
            lblMessage.Message = "";
            lblIntents.Text = "3 veces más";
            PicRegister.Image = null;
            lblMessage.StatusBarBackColor = Color.FromArgb(250, 250, 250);
        }

        public void clearTab2()
        {
            lblMessage.Message = "";
            lblIntents.Text = "";
            PicRegister.Image = null;
            TxtSearch.Text = "";
            TxtCode.Text = "";
            TxtName.Text = "";
            TxtSurname.Text = "";
            TxtNro.Text = "";
            lblMessage.StatusBarBackColor = Color.FromArgb(250, 250, 250);
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TabControl.SelectedTab == tabPage1)
            {
                managementZk.SetMatchSearch(true);
                managementZk.SetIsRegister(false);
                managementZk.EventGeneral += OnEventGeneral;
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
                managementZk.EventGeneral -= OnEventGeneral;

            }

            if (TabControl.SelectedTab == tabPage3)
            {
                managementZk.SetIsRegister(false);
                managementZk.SetMatchSearch(false);
                getConfiguration();
                managementZk.EventGeneral -= OnEventGeneral;
            }
        }

        private void BtnDisconnection_Click(object sender, EventArgs e)
        {
            managementZk.Disconnect();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataManager dataManager = new DataManager();
            string retrievedData = dataManager.ReadData();

        }

        private void TabSelector_Click(object sender, EventArgs e)
        {

        }
        public void getConfiguration()
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

        private void PicCloseHome_Click(object sender, EventArgs e)
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
        }



        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            dgvHcuotas.Visible = true;
            dgvHpago.Visible = false;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            dgvHcuotas.Visible = false;
            dgvHpago.Visible = true;
        }

        private async void dgridMembresias_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                dgvMembresias.ClearSelection();
                dgvMembresias.Rows[e.RowIndex].Selected = true;

                var membresias = DataStatic.Membresias[e.RowIndex];
                int CodigoMembresia = membresias.CodigoMenbresia;
                int CodigoSede = membresias.CodigoSede;
                int CodigoUnidadNegocio = DataSession.Unidad;

                AppsFitService serv = new AppsFitService();

                var respHistorial = await serv.AsistencesList(new
                {
                    CodigoUnidadNegocio = CodigoUnidadNegocio,
                    CodigoSede = CodigoSede,
                    Membresia = CodigoMembresia
                });
                var HPC = await serv.HistorialPC(new
                {
                    CodigoUnidadNegocio = CodigoUnidadNegocio,
                    CodigoSede = CodigoSede,
                    Membresia = CodigoMembresia
                });
                DataStatic.Asistences = respHistorial.Data;
                DataStatic.Pagos = HPC.Data.Pagos;
                DataStatic.Cuotas = HPC.Data.Cuotas;
                DataStatic.Incidencias = HPC.Data.Incidencias;
                MessageStatusMembresia(membresias.ObtenerTiempoVencimiento, membresias.Estado);
                lblPlan.Text = membresias.Descripcion.ToUpper();

                DataStatic.MembresiasSelected = membresias;
                if (membresias.Estado == 1)
                {
                    btnMarkAsistence.Visible = true;
                }
                else
                {
                    btnMarkAsistence.Visible = false;
                }

                Thread dataLoadThread = new Thread(LoadDataToGrids);
                dataLoadThread.Start();




            }
        }

        private void dgvMembresias_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        //clears--------------------
        private void clearMA()
        {
            lblFullName_.Text = "NOMBRES, APELLIDOS COMPLETOS";
            PicMaUser.Image = null;
            lblPlan.Text = "NOMBRE DEL PLAN";
            lblMessageMem.Text = "ESTADO MEMBRESIA";
            lblMessage.Message = "";
            picHuellaMA.Image = Properties.Resources.huell;
            PicMaUser.Image = Properties.Resources.user2;

            dgvAsistences.Rows.Clear();
            dgvHcuotas.Rows.Clear();
            dgvHpago.Rows.Clear();
            dgvMembresias.Rows.Clear();
            dgvIncidencias.Rows.Clear();
        }

        private void tableLayoutPanel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timerNow_Tick(object sender, EventArgs e)
        {
            DateTime horaActual = DateTime.Now;

            // Formatear la hora actual según tus preferencias
            string horaFormateada = horaActual.ToString("HH:mm:ss");

            // Actualizar el texto del Label con la hora formateada
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
            string message;
            //validate flag sede permission
            if (DataStatic.MembresiasSelected.ObtenerDisponibilidadHorarioPaquete > 0)
            {
                //validate horario
                if (DataStatic.MembresiasSelected.flagPaqueteSedePermiso > 0)
                {
                    if (DataStatic.MembresiasSelected.NroIngreso < DataStatic.MembresiasSelected.NroIngresoActual)
                    {
                        message = "NRO ASISTENCIAS LLEGO A SU LIMITE, REVISA EL NRO DE SESIONES DE LA MEMBRESIA";
                    }
                    else
                    {
                        //?sucess validate 
                        AppsFitService serv = new AppsFitService();

                        var data = new
                        {
                            CodigoUnidadNegocio = DataSession.Unidad,
                            Sede = DataSession.Sede,
                            Socio = DataStatic.MembresiasSelected.CodigoSocio,
                            Membresia = DataStatic.MembresiasSelected.CodigoMenbresia
                        };

                        var res = await serv.MarkAsistence(data);
                        if (res.Success)
                        {
                            message = res.Message1;
                            //update List
                            DataGridViewCellEventArgs cellEventArgs = new DataGridViewCellEventArgs(0, 0);
                            dgridMembresias_CellClick(dgvMembresias, cellEventArgs);
                        }
                        else { message = res.Message1; };
                        button1.Text = $"{DataStatic.MembresiasSelected.CodigoMenbresia}-{DataStatic.MembresiasSelected.CodigoSocio}";
                    }
                }
                else { message = "HORARIO NO DISPONIBLE"; }

            }
            else { message = "ESTA MEMBRESIA NO TIENE ACCESO PARA ESTA SEDE."; }

            MessageBox.Show(message);

        }

        private void button1_Click_1(object sender, EventArgs e)
        {


        }

        private void ckAuto_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            CheckBoxValue.IsChecked = checkBox.Checked;

            bool isChecked = CheckBoxValue.IsChecked;
            lbldev.Text = isChecked.ToString();
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void TxtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
