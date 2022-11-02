using System.Windows.Forms;
using AssetStudio;
using System;
using Object = AssetStudio.Object;

namespace AssetStudioGUI
{
    internal class AssetItem : ListViewItem
    {
        public Object Asset;
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
                    return string.Format("{0}m~({1})", Math.Round(size, 2), FullSize);
                }
                else
                {
                    return string.Format("{0}k~({1})", Math.Round(size, 2),FullSize);
                }
            }
            return $"{FullSize}";
        }

        public void SetSubItems()
        {
            SubItems.AddRange(new[]
            {
                Container, //Container
                TypeString, //Type
                m_PathID.ToString(), //PathID
                GetFullSize(), //Size
            });
        }
    }
}
