using System;
using System.Windows.Forms;
using AssetStudio;
using Object = AssetStudio.Object;

namespace AssetStudioGUI
{
    internal class AssetItem : ListViewItem
    {
        public Object Asset;
        public string ab_hash;
        public SerializedFile SourceFile;
        public string Container = string.Empty;
        public string TypeString;
        public long m_PathID;
        public long FullSize;
        public ClassIDType Type;
        public string InfoText;
        public string UniqueID;
        public GameObjectTreeNode TreeNode;

        public AssetItem(Object asset)
        {
            Asset = asset;
            SourceFile = asset.assetsFile;
            Type = asset.type;
            TypeString = Type.ToString();
            m_PathID = asset.m_PathID;
            FullSize = asset.byteSize;
        }

        private string GetFullSize()
        {
            float size = FullSize;
            if (size > 1024)
            {
                size /= 1024;
                if (size > 512)
                {
                    size /= 1024;
                    return Math.Round(size,2)+ "m";
                }

                return Math.Round(size,2) + "k";
            }

            return FullSize.ToString();
        }

        public void SetSubItems()
        {
            SubItems.AddRange(new[]
            {
                Container, //Container
                TypeString, //Type
                m_PathID.ToString(), //PathID
                ab_hash,
                GetFullSize(), //Size
            });
        }
    }
}
