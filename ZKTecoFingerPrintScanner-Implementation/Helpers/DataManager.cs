using System;
using System.IO;

namespace ZKTecoFingerPrintScanner_Implementation.Helpers
{
    public class DataManager
    {
        private string filePath;

        public DataManager()
        {
            string fileName = "datas.txt";
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filePath = System.IO.Path.Combine(projectPath, fileName);
            this.filePath = filePath;
        }

        public void SaveData(string data)
        {
            File.WriteAllText(filePath, data);
        }

        public string ReadData()
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }

            return string.Empty;
        }
    }


    public class DataManagerD
    {
        private string filePath;

        public DataManagerD()
        {
            string fileName = "pre.txt";
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filePath = System.IO.Path.Combine(projectPath, fileName);
            this.filePath = filePath;
        }

        public void SaveData(string data)
        {
            File.WriteAllText(filePath, data);
        }

        public string ReadData()
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }

            return string.Empty;
        }
    }

}
