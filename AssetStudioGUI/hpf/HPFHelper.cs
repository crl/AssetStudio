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
        private static int Total = 0;
        public static void RunFolder(string folder)
        {
            var files = Directory.GetFiles(folder, "*.hpf", SearchOption.AllDirectories).ToList();

            if (files.Count == 0)
            {
                return;
            }
            PlatformMsgManager.CloseAllHpf();
            hpfs.Clear();

           

            foreach (var file in files)
            {
                hpfs.Add(file);
            }
            Total = hpfs.Count;

            start();
        }

        private static void start()
        {
            Progress.Reset();
            if (hpfs.Count == 0)
            {
                Logger.Info("Ready!!!");
                return;
            }
            var file = hpfs[0];
            hpfs.RemoveAt(0);
            Single(file);
        }

        public static void RunFile(string file)
        {
            if (File.Exists(file)==false)
            {
                return;
            }

            Progress.Reset();
            hpfs.Clear();
            hpfs.Add(file);
            Total = hpfs.Count;
            start();
        }

        private static void Single(string file)
        {
            var item = file.Replace("\\", "/");
            var name = Path.GetFileNameWithoutExtension(item);

            var folder = Path.GetDirectoryName(item);
            var savePath = Path.Combine(folder, "exports", name).Replace("\\", "/");

            if (Directory.Exists(savePath) == false)
            {
                Directory.CreateDirectory(savePath);
            }

            var sb = new StringBuilder();
            /*var md5 = new MD5CryptoServiceProvider();
            using (var io = File.OpenRead(item))
            {
                var bytes = md5.ComputeHash(io);

                for (int j = 0, len = bytes.Length; j < len; j++)
                {
                    sb.Append(bytes[j].ToString("x2"));
                }
            }*/
            Logger.Info(string.Format("Export {0}  md5:{1}", item, sb.ToString()));
            var result = PlatformMsgManager.ExportHpfFiles(item, 0, savePath, callback, false);
        }

        private static void callback(int a, int b, string msg)
        {
            if (a == b)
            {
                start();
                return;
            }
            Progress.Report(b,a);
        }

        private static EndianBinaryReader getReader(int index)
        {
            EndianBinaryReader reader = null;

            reader = new EndianBinaryReader(File.OpenRead(hpfs[index]));
            return reader;
        }
    }
}