using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZKTecoFingerPrintScanner_Implementation.Helpers;
using ZKTecoFingerPrintScanner_Implementation.Services;

namespace ZKTecoFingerPrintScanner_Implementation
{
    public partial class LoadingForm : Form
    {
        public LoadingForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "";
            this.BackColor = Color.White;
           // this.Opacity = 0;
        }

        private async void LoadingForm_Load(object sender, EventArgs e)
        {
            DataManager ma = new DataManager();
           
            string key = ma.ReadData();
            if (!string.IsNullOrEmpty(key))
            {
                AppsFitService serv = new AppsFitService();
                var isValid = await serv.VerifyDkey(new { DefaultKeyEmpresa = key });
                if (isValid.Success)
                {
                    
                    ScreenHome home = new ScreenHome();
                    DataSession.DKey = key;
                    DataSession.Unidad = isValid.Data.unidad;
                    home.Show();
                    this.Hide();
                }
                else
                {
                    
                    Login login = new Login();
                    login.Show();
                    this.Hide();
                }
            }
            else
            {
                
                Login login = new Login();
                login.Show();
                this.Hide();
            }
        }
    }
}
