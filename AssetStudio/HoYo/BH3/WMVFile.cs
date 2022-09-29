using System.Collections.Generic;

namespace AssetStudio
{
    public class WMVFile
    {
        public Dictionary<long, StreamFile[]> Bundles = new Dictionary<long, StreamFile[]>();
        public WMVFile(FileReader reader, long[] BundlePos=null)
        {
            if (BundlePos!=null && BundlePos.Length != 0)
            {
                foreach (var pos in BundlePos)
                {
                    reader.Position = pos;
                    var bundle = new BundleFile(reader);
                    //Bundles.Add(pos, bundle.FileList);
                }
            }
            else
            {
                while (reader.Position != reader.BaseStream.Length)
                {
                    var pos = reader.Position;
                    var bundle = new BundleFile(reader);
                    //Bundles.Add(pos, bundle.FileList);
                }
            }
        }
    }
}
