namespace AssetStudio.hpf
{
	// GameFramework.Resource.ResourceManager.AssetInfoManager
	using System;
	using System.Collections.Generic;
	using System.Runtime.InteropServices;
	using System.Text;

    [StructLayout(LayoutKind.Explicit, Size = 12)]
    public struct BaseAssetInfo
    {
        [FieldOffset(0)]
        public uint asset_crc;

        [FieldOffset(4)]
        public int ab_index;

        [FieldOffset(8)]
        public int asset_beDependedTag;
    }

    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct BaseBundleInfo
    {
        [FieldOffset(0)]
        public uint ab_crc;

        [FieldOffset(4)]
        public int dep_arr_addr;
    }

    public class ConflicBaseAssetInfo
    {
        public int ab_index;

        public int asset_beDepended;

        public string asset_name;
    }


	public class AssetInfoManager
	{
		public static int AssetCount;

		public static int AssetBundleCount;

		private static int fileLength = 0;

		private static char[] PackageListHeader;

		private static byte PackageListVersion;

		private static byte[] encryptBytes;

		private static byte[] applicableGameVersionBytes;

		private static int InternalResourceVersion;

		private static int assert_Blob;

		private static int arr_Blob;

		private static int ab_Blob;

		private static int conflic_Blob;

		private unsafe static byte* m_pData;

		private unsafe static BaseAssetInfo* p_assetinfo;

		private unsafe static BaseBundleInfo* p_bundleinfo;

		private static int m_Position = 0;

		private static Dictionary<uint, int> crcIndexer;

		private static Dictionary<string, ConflicBaseAssetInfo> conflic_asset_abindex;

		
		private static int Position
		{
			get
			{
				return m_Position;
			}
			set
			{
				m_Position = value;
			}
		}

        public unsafe static void Setup(byte[] bytes)
		{
			fileLength = bytes.Length;
			IntPtr p = Marshal.AllocHGlobal(bytes.Length);
			Marshal.Copy(bytes, 0, p, bytes.Length);
			m_pData = (byte*)(void*)p;
			m_Position = 0;
			PackageListHeader = read_chars(3);
			PackageListVersion = read_byte();
			encryptBytes = read_bytes(4);
			applicableGameVersionBytes = read_bytes(read_byte());
			InternalResourceVersion = read_int();
			AssetCount = read_int();
			AssetBundleCount = read_int();
			assert_Blob = read_int();
			arr_Blob = read_int();
			ab_Blob = read_int();
			conflic_Blob = read_int();
			p_assetinfo = (BaseAssetInfo*)(m_pData + assert_Blob);
			crcIndexer = new Dictionary<uint, int>(AssetCount);
			List<uint> conflic_asset = new List<uint>();
			for (int i = 0; i < AssetCount; i++)
			{
				BaseAssetInfo info = p_assetinfo[i];
				if (crcIndexer.ContainsKey(info.asset_crc))
				{
					if (!conflic_asset.Contains(info.asset_crc))
					{
						conflic_asset.Add(info.asset_crc);
					}
				}
				else
				{
					crcIndexer.Add(info.asset_crc, i);
				}
			}
			foreach (uint crc in conflic_asset)
			{
				crcIndexer.Remove(crc);
			}
			p_bundleinfo = (BaseBundleInfo*)(m_pData + ab_Blob);
			if (conflic_asset_abindex != null)
			{
				conflic_asset_abindex.Clear();
				conflic_asset_abindex = null;
			}
		}

		private unsafe static byte read_byte()
		{
			byte ret = m_pData[m_Position];
			m_Position++;
			return ret;
		}

		private unsafe static short read_short()
		{
			short ret = *(short*)(m_pData + m_Position);
			m_Position += 2;
			return ret;
		}

		private unsafe static ushort read_ushort()
		{
			ushort ret = *(ushort*)(m_pData + m_Position);
			m_Position += 2;
			return ret;
		}

		private unsafe static int read_int()
		{
			int ret = *(int*)(m_pData + m_Position);
			m_Position += 4;
			return ret;
		}

		private unsafe static uint read_uint()
		{
			uint ret = *(uint*)(m_pData + m_Position);
			m_Position += 4;
			return ret;
		}

		private unsafe static char[] read_chars(int count)
		{
			byte* p = m_pData + m_Position;
			char[] ret = new char[count];
			for (int i = 0; i < count; i++)
			{
				ret[i] = (char)p[i];
			}
			m_Position += count;
			return ret;
		}

		private unsafe static byte[] read_bytes(int count)
		{
			byte* p = m_pData + m_Position;
			byte[] ret = new byte[count];
			for (int i = 0; i < count; i++)
			{
				ret[i] = p[i];
			}
			m_Position += count;
			return ret;
		}

		private unsafe static short[] read_shorts(int count)
		{
			short* p = (short*)(m_pData + m_Position);
			short[] ret = new short[count];
			for (int i = 0; i < count; i++)
			{
				ret[i] = p[i];
			}
			m_Position += count * 2;
			return ret;
		}

		private unsafe static ushort[] read_ushorts(int count)
		{
			ushort* p = (ushort*)(m_pData + m_Position);
			ushort[] ret = new ushort[count];
			for (int i = 0; i < count; i++)
			{
				ret[i] = p[i];
			}
			m_Position += count * 2;
			return ret;
		}

		private unsafe static int[] read_ints(int count)
		{
			int* p = (int*)(m_pData + m_Position);
			int[] ret = new int[count];
			for (int i = 0; i < count; i++)
			{
				ret[i] = p[i];
			}
			m_Position += count * 4;
			return ret;
		}

		private unsafe static uint[] read_uints(int count)
		{
			uint* p = (uint*)(m_pData + m_Position);
			uint[] ret = new uint[count];
			for (int i = 0; i < count; i++)
			{
				ret[i] = p[i];
			}
			m_Position += count * 4;
			return ret;
		}

		private unsafe static string read_str()
		{
			byte strlen = read_byte();
			string ret = new string((sbyte*)(m_pData + m_Position), 0, strlen, Encoding.UTF8);
			m_Position += strlen;
			return ret;
		}

		public unsafe static List<uint> GetAll_AB_crc()
        {
			var list = new List<uint>();
			foreach(var item in crcIndexer) {
				var crc=item.Key;
				var index = item.Value;

                BaseAssetInfo assetInfo = p_assetinfo[index];
                BaseBundleInfo bundleInfo = p_bundleinfo[assetInfo.ab_index];
				///bedependedTag = assetinfo.asset_beDependedTag;
                list.Add(bundleInfo.ab_crc);
            }
			return list;
		}

		public unsafe static int GetAssetBundleIndex(string asset, out int bedependedTag)
		{
			uint asset_crc = Verifier.GetCrc32Unit(asset);
			int index = -1;
			if (crcIndexer.TryGetValue(asset_crc, out index))
			{
				BaseAssetInfo assetinfo = p_assetinfo[index];
				bedependedTag = assetinfo.asset_beDependedTag;
				return assetinfo.ab_index;
			}
			if (conflic_asset_abindex == null)
			{
				conflic_asset_abindex = new Dictionary<string, ConflicBaseAssetInfo>();
				Position = conflic_Blob;
				int conflic_count = read_int();
				for (int i = 0; i < conflic_count; i++)
				{
					ConflicBaseAssetInfo conflicBaseAssetInfo = new ConflicBaseAssetInfo();
					conflicBaseAssetInfo.ab_index = read_int();
					conflicBaseAssetInfo.asset_beDepended = read_int();
					conflicBaseAssetInfo.asset_name = read_str();
					conflic_asset_abindex.Add(conflicBaseAssetInfo.asset_name, conflicBaseAssetInfo);
				}
			}
			ConflicBaseAssetInfo conflicInfo;
			if (!conflic_asset_abindex.TryGetValue(asset, out conflicInfo))
			{
				bedependedTag = 0;
				return -1;
			}
			bedependedTag = conflicInfo.asset_beDepended;
			return conflicInfo.ab_index;
		}

		public static bool HasAsset(string asset)
		{
			int tag;
			return GetAssetBundleIndex(asset, out tag) >= 0;
		}

		public unsafe static uint GetAssetBundle(int bundle_index)
		{
			if (bundle_index < 0 || bundle_index >= AssetBundleCount)
			{
				return 0u;
			}
			BaseBundleInfo bundleinfo = p_bundleinfo[bundle_index];
			return bundleinfo.ab_crc;
		}

		public unsafe static uint* GetDependences(int bundle_index, out int count)
		{
			count = 0;
			uint* ret = (uint*)(void*)IntPtr.Zero;
			if (bundle_index < 0 || bundle_index >= AssetBundleCount)
			{
				return ret;
			}
			BaseBundleInfo bundleinfo = p_bundleinfo[bundle_index];
			int dep_arr_addr = bundleinfo.dep_arr_addr;
			if (dep_arr_addr < 0)
			{
				return ret;
			}
			Position = arr_Blob + dep_arr_addr;
			count = read_short();
			return (uint*)(m_pData + Position);
		}

		public unsafe static uint[] GetDependences(int bundle_index)
		{
			uint[] ret = null;
			if (bundle_index < 0 || bundle_index >= AssetBundleCount)
			{
				return ret;
			}
			BaseBundleInfo bundleinfo = p_bundleinfo[bundle_index];
			int dep_arr_addr = bundleinfo.dep_arr_addr;
			if (dep_arr_addr < 0)
			{
				return ret;
			}
			Position = arr_Blob + dep_arr_addr;
			int length = read_short();
			return read_uints(length);
		}

		public unsafe static void Destroy()
		{
			Marshal.FreeHGlobal((IntPtr)(void*)m_pData);
			m_pData = null;
			p_assetinfo = null;
			p_bundleinfo = null;
		}
    }

}