using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ZKTecoFingerPrintScanner_Implementation.Helpers
{
    public class Utilities
    {
        public static void EnableControls(bool enableControl, params Control[] controls)
        {
            foreach (var control in controls)
            {
                control.Enabled = enableControl;
            }
        }

        public static Bitmap GetImage(byte[] buffer, int width, int height)
        {
            Bitmap output = new Bitmap(width, height);
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = output.LockBits(rect, ImageLockMode.ReadWrite, output.PixelFormat);
            IntPtr ptr = bmpData.Scan0;

            Marshal.Copy(buffer, 0, ptr, buffer.Length);
            output.UnlockBits(bmpData);

            return output;
        }


        public static void ShowStatusBar(string message, Controls.StatusBar statusBar, bool type)
        {
            statusBar.Visible = true;
            statusBar.Message = message;
            statusBar.StatusBarForeColor = Color.White;
            if (type)
                statusBar.StatusBarBackColor = Color.FromArgb(79, 208, 154);
            else
                statusBar.StatusBarBackColor = Color.FromArgb(230, 112, 134);

        }

        public static void PrintTxt(string content)
        {
            string fileName = "info.txt";
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filePath = Path.Combine(projectPath, fileName);

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



        public void saveDataLocal(string dkey)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection appSettings = config.AppSettings.Settings;

            if (appSettings["dkey"] != null)
            {
                appSettings["dkey"].Value = dkey;
            }
            else
            {
                appSettings.Add("dkey", dkey);
            }

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
        public void SaveDataLocal(string dkey)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["dkey"].Value = dkey;
            config.Save(ConfigurationSaveMode.Modified);
        }
        public string Dkey()
        {
            string valor = ConfigurationManager.AppSettings["dkey"] ?? null;
            return valor;
        }
    }
}
