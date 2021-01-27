using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AssetStudio.hpf
{
    public class HPFHelper
    {
        private static List<string> hpfs = new List<string>();
        public static List<EndianBinaryReader> Run(string folder)
        {
            var result=new List<EndianBinaryReader>();
            var files = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories).ToList();

            var filePath = Path.Combine(folder, "version");
            if (File.Exists(filePath) == false)
            {
                return result;
            }

            var bytes = File.ReadAllBytes(filePath);
            if (bytes.Length == 0)
            {
                return result;
            }
            AssetInfoManager.Setup(bytes);

            PlatformMsgManager.CloseAllHpf();

            for (int i = 0; i < 30; i++)
            {
                filePath = Path.Combine(folder, $"GameData{i}.hpf");
                if (File.Exists(filePath) == false)
                {
                    break;
                }
                hpfs.Add(filePath);
                var index=PlatformMsgManager.OpenHpf(filePath, 0, 0, 0);
                Console.WriteLine($"{filePath}: {index}");
            }
            //PlatformMsgManager.CloseAllHpf();


            var list=AssetInfoManager.GetAll_AB_crc();

            foreach(var item in list)
            {
                long i=PlatformMsgManager.GetFileOffsetInHpf(item.ToString(), 0);

                int index = (int)(i >> 32);
                ulong offset = (ulong)(i & 0x00000000FFFFFFFFL);

                if(index>-1 && index < hpfs.Count)
                {
                    var reader = getReader(index);
                    reader.Position =(long)offset;

                    result.Add(reader);


                    //if(result.Count>100)break;
                    
                }
            }

            return result;
        }

        private static EndianBinaryReader getReader(int index)
        {
            EndianBinaryReader reader = null;

            reader = new EndianBinaryReader(File.OpenRead(hpfs[index]));
            return reader;
        }
    }
}