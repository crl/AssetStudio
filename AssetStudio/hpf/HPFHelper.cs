using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AssetStudio.hpf
{
    public class HPFHelper
    {
        private static List<string> hpfs = new List<string>();

        public static void RunFolder(string folder)
        {
            var files = Directory.GetFiles(folder, "*.hpf", SearchOption.AllDirectories).ToList();

            if (files.Count == 0)
            {
                return;
            }
            PlatformMsgManager.CloseAllHpf();

            var savePrefix = Path.Combine(folder, "exports");

            if (Directory.Exists(savePrefix)==false)
            {
                Directory.CreateDirectory(savePrefix);
            }
            Progress.Reset();
            int i = 0;
        
            foreach (var file in files)
            {
                Single(file, savePrefix);
                Progress.Report(++i, files.Count);
            }

            Logger.Info("Ready to go");
        }

        public static void RunFile(string file, string savePrefix)
        {
            if (File.Exists(file)==false)
            {
                return;
            }

            Progress.Reset();
            Single(file, savePrefix);
            Logger.Info("Ready to go");
        }

        public static void Single(string file, string savePrefix)
        {
            var item = file.Replace("\\", "/");
            var name = Path.GetFileNameWithoutExtension(item);
            var savePath = Path.Combine(savePrefix, name).Replace("\\", "/");
            if (Directory.Exists(savePath) == false)
            {
                Directory.CreateDirectory(savePath);
            }

            var sb = new StringBuilder();
            var md5 = new MD5CryptoServiceProvider();
            using (var io = File.OpenRead(item))
            {
                var bytes = md5.ComputeHash(io);

                for (int j = 0, len = bytes.Length; j < len; j++)
                {
                    sb.Append(bytes[j].ToString("x2"));
                }
            }

            var result = PlatformMsgManager.ExportHpfFiles(item, 0, savePath, callback, false);
            Logger.Info(string.Format("Export {0} to {1} md5:{2}", item, savePath, sb.ToString()));
        }

        private static void callback(int a, int b, string msg)
        {
            if (a == b)
            {
                Progress.Report(a, b);
            }
           
        }

        private static EndianBinaryReader getReader(int index)
        {
            EndianBinaryReader reader = null;

            reader = new EndianBinaryReader(File.OpenRead(hpfs[index]));
            return reader;
        }
    }
}