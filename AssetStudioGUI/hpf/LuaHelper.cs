using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AssetStudio.hpf
{
    public class LuaHelper
    {
        private static List<string> luas = new List<string>();
        private static int Total = 0;
        public static void RunFolder(string folder)
        {
            var files = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories).ToList();

            if (files.Count == 0)
            {
                return;
            }

            foreach (var file in files)
            {
                luas.Add(file);
            }
            Total = luas.Count;
            Progress.Reset();
            start();
        }

        private static void start()
        {
            if (luas.Count == 0)
            {
                Progress.Reset();
                Logger.Info("Ready!!!");
                return;
            }
            var file = luas[0];
            luas.RemoveAt(0);

            Progress.Report(Total-luas.Count, Total);
            Single(file);
        }

        private static void Single(string file)
        {
            var app = AppDomain.CurrentDomain.BaseDirectory;

            var prefix = Path.GetDirectoryName(file);

            var fileName=Path.GetFileNameWithoutExtension(file);
            var to=Path.Combine(prefix, fileName);

            var luadec = "luadec.exe";
            var exe = Path.Combine(app, $"libs/5.3/bin/{luadec}");
            var c = string.Format("/c {0} {1} > {2}_d.lua", luadec, file, to);

            var root = Path.GetDirectoryName(exe);
            System.Diagnostics.ProcessStartInfo p = new System.Diagnostics.ProcessStartInfo("cmd.exe", c);
            p.WorkingDirectory = root;
            p.RedirectStandardOutput = true;
            p.RedirectStandardError = true;
            p.CreateNoWindow = true;
            System.Diagnostics.Process pro = new System.Diagnostics.Process();
            pro.StartInfo = p;
            pro.Start();

            string log = pro.StandardOutput.ReadToEnd();
            if (!string.IsNullOrEmpty(log))
            {
                Console.WriteLine(log);
            }
            string error = pro.StandardError.ReadToEnd();
            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine(error);
            }

            pro.Close();
            start();
        }
    }
}
