using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class FileHelper
    {
        #region 文件操作
        public static FileInfo[] GetFiles(string directoryPath)
        {
            if (!IsExistDirectory(directoryPath))
            {
                throw new DirectoryNotFoundException();
            }
            var root = new DirectoryInfo(directoryPath);
            return root.GetFiles();
        }
        public static bool IsExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }
        public static string ReadFile(string Path)
        {
            string s;
            if (!File.Exists(Path))
                s = "不存在相应的目录";
            else
            {
                var f2 = new StreamReader(Path, Encoding.Default);
                s = f2.ReadToEnd();
                f2.Close();
                f2.Dispose();
            }
            return s;
        }
        public static void FileMove(string OrignFile, string NewFile)
        {
            File.Move(OrignFile, NewFile);
        }
        public static void CreateDir(string dir)
        {
            if (dir.Length == 0)
                return;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
        
        public static string ProExt(string ext)
        {
            string[] badext = { "exe", "msi", "bat", "com", "sys", "aspx", "asax", "ashx" };
            if (string.IsNullOrEmpty(ext)) 
                return "";
            if (badext.Contains(ext)) 
                throw new Exception("危险文件");
            if (ext.First() == '.') 
                return ext;
            return "." + ext;
        }

        public static string GetRandom(int intStart, int intEnd)
        {
            Random rd = new Random();
            return rd.Next(intStart, intEnd).ToString();  //生成随机数
        }
        #endregion
    }
}
