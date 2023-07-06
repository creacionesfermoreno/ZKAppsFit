using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZKTecoFingerPrintScanner_Implementation.Helpers;
using ZKTecoFingerPrintScanner_Implementation.Models;
using ZKTecoFingerPrintScanner_Implementation.Services;

namespace ZKTecoFingerPrintScanner_Implementation
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            PLoading.ForeColor = Color.FromArgb(255, 0, 0);
            PLoading.BackColor = Color.FromArgb(150, 0, 0);
            btnPase.Visible = false;
        }

        private void BtnCloseLogin_Click(object sender, EventArgs e)
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

        private async void button1_Click(object sender, EventArgs e)
        {
            if (txtDkey.Text.Length == 0)
            {
                txtDkey.Select();
                return;
            }

            PLoading.Visible = true;
            AppsFitService serv = new AppsFitService();

            ResponseModel resp = await serv.VerifyDkey(new { DefaultKeyEmpresa = txtDkey.Text });
            if (resp.Success)
            {
                DataItem item = (DataItem)resp.Data;
                txtName.Text = item.name;
                txtUneg.Text = item.unidad.ToString();
                txtSede.Text = item.sede.ToString();

                using (WebClient webClient = new WebClient())
                {
                    byte[] imageData = webClient.DownloadData(item.image);
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        PicLogo.Image = Image.FromStream(ms);
                    }
                }
                SbMessage.Message = resp.Message1;
                btnPase.Visible = true;


                DataManager dataManager = new DataManager();
                string dataToSave = txtDkey.Text;
                dataManager.SaveData(dataToSave);
            }
            else
            {
                SbMessage.Message = resp.Message1;
            }
            PLoading.Visible = false;

        }

        private void btnPase_Click(object sender, EventArgs e)
        {
            this.Hide();
            ScreenHome home = new ScreenHome();
            home.Show();
            DataSession.DKey = txtDkey.Text;
        }

        private  void Login_Load(object sender, EventArgs e)
        {
          

        }
    }
}
