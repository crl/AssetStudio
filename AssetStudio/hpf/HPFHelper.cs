using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AssetStudio.hpf
{
    public class HPFHelper
    {
        private static List<string> hpfs = new List<string>();

        public static void Run(string folder)
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
                var item = file.Replace("\\", "/");
                var name = Path.GetFileNameWithoutExtension(item);
                var savePath = Path.Combine(savePrefix, name).Replace("\\","/");
                if (Directory.Exists(savePath)==false)
                {
                    Directory.CreateDirectory(savePath);
                }

                var result =PlatformMsgManager.ExportHpfFiles(item, 0, savePath, callback);
                Console.WriteLine("export {0} to {1} ={2}", item, savePath, result);

                Progress.Report(i++, files.Count);
            }
        }

        private static void callback(int a, int b, string msg)
        {
            if (a == b)
            {
                Console.WriteLine("export error:{0}", msg);
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