namespace AssetStudio.hpf
{
	// GameFramework.Utility.Verifier
	using System;
	using System.IO;

	public static class Verifier
	{
		private sealed class Crc32
		{
			private const int TableLength = 256;

			private const uint DefaultPolynomial = 3988292384u;

			private const uint DefaultSeed = uint.MaxValue;

			private readonly uint m_Seed;

			private readonly uint[] m_Table;

			private uint m_Hash;

			public Crc32()
				: this(3988292384u, uint.MaxValue)
			{
			}

			public Crc32(uint polynomial, uint seed)
			{
				m_Seed = seed;
				m_Table = InitializeTable(polynomial);
				m_Hash = seed;
			}

			public void Initialize()
			{
				m_Hash = m_Seed;
			}

			public void HashCore(byte[] bytes, int offset, int length)
			{
				m_Hash = CalculateHash(m_Table, m_Hash, bytes, offset, length);
			}

			public uint HashFinal()
			{
				return ~m_Hash;
			}

			private static uint CalculateHash(uint[] table, uint value, byte[] bytes, int offset, int length)
			{
				int last = offset + length;
				for (int i = offset; i < last; i++)
				{
					value = ((value >> 8) ^ table[bytes[i] ^ (value & 0xFF)]);
				}
				return value;
			}

			private static uint[] InitializeTable(uint polynomial)
			{
				uint[] table = new uint[256];
				for (int i = 0; i < 256; i++)
				{
					uint entry = (uint)i;
					for (int j = 0; j < 8; j++)
					{
						entry = (((entry & 1) != 1) ? (entry >> 1) : ((entry >> 1) ^ polynomial));
					}
					table[i] = entry;
				}
				return table;
			}
		}

		private const int CachedBytesLength = 4096;

		private static readonly byte[] s_CachedBytes = new byte[4096];

		private static readonly Crc32 s_Algorithm = new Crc32();

		public static DateTime DateTimeNow = DateTime.Now;

		private static readonly uint[] crcTable = new uint[256]
		{
		0u,
		1996959894u,
		3993919788u,
		2567524794u,
		124634137u,
		1886057615u,
		3915621685u,
		2657392035u,
		249268274u,
		2044508324u,
		3772115230u,
		2547177864u,
		162941995u,
		2125561021u,
		3887607047u,
		2428444049u,
		498536548u,
		1789927666u,
		4089016648u,
		2227061214u,
		450548861u,
		1843258603u,
		4107580753u,
		2211677639u,
		325883990u,
		1684777152u,
		4251122042u,
		2321926636u,
		335633487u,
		1661365465u,
		4195302755u,
		2366115317u,
		997073096u,
		1281953886u,
		3579855332u,
		2724688242u,
		1006888145u,
		1258607687u,
		3524101629u,
		2768942443u,
		901097722u,
		1119000684u,
		3686517206u,
		2898065728u,
		853044451u,
		1172266101u,
		3705015759u,
		2882616665u,
		651767980u,
		1373503546u,
		3369554304u,
		3218104598u,
		565507253u,
		1454621731u,
		3485111705u,
		3099436303u,
		671266974u,
		1594198024u,
		3322730930u,
		2970347812u,
		795835527u,
		1483230225u,
		3244367275u,
		3060149565u,
		1994146192u,
		31158534u,
		2563907772u,
		4023717930u,
		1907459465u,
		112637215u,
		2680153253u,
		3904427059u,
		2013776290u,
		251722036u,
		2517215374u,
		3775830040u,
		2137656763u,
		141376813u,
		2439277719u,
		3865271297u,
		1802195444u,
		476864866u,
		2238001368u,
		4066508878u,
		1812370925u,
		453092731u,
		2181625025u,
		4111451223u,
		1706088902u,
		314042704u,
		2344532202u,
		4240017532u,
		1658658271u,
		366619977u,
		2362670323u,
		4224994405u,
		1303535960u,
		984961486u,
		2747007092u,
		3569037538u,
		1256170817u,
		1037604311u,
		2765210733u,
		3554079995u,
		1131014506u,
		879679996u,
		2909243462u,
		3663771856u,
		1141124467u,
		855842277u,
		2852801631u,
		3708648649u,
		1342533948u,
		654459306u,
		3188396048u,
		3373015174u,
		1466479909u,
		544179635u,
		3110523913u,
		3462522015u,
		1591671054u,
		702138776u,
		2966460450u,
		3352799412u,
		1504918807u,
		783551873u,
		3082640443u,
		3233442989u,
		3988292384u,
		2596254646u,
		62317068u,
		1957810842u,
		3939845945u,
		2647816111u,
		81470997u,
		1943803523u,
		3814918930u,
		2489596804u,
		225274430u,
		2053790376u,
		3826175755u,
		2466906013u,
		167816743u,
		2097651377u,
		4027552580u,
		2265490386u,
		503444072u,
		1762050814u,
		4150417245u,
		2154129355u,
		426522225u,
		1852507879u,
		4275313526u,
		2312317920u,
		282753626u,
		1742555852u,
		4189708143u,
		2394877945u,
		397917763u,
		1622183637u,
		3604390888u,
		2714866558u,
		953729732u,
		1340076626u,
		3518719985u,
		2797360999u,
		1068828381u,
		1219638859u,
		3624741850u,
		2936675148u,
		906185462u,
		1090812512u,
		3747672003u,
		2825379669u,
		829329135u,
		1181335161u,
		3412177804u,
		3160834842u,
		628085408u,
		1382605366u,
		3423369109u,
		3138078467u,
		570562233u,
		1426400815u,
		3317316542u,
		2998733608u,
		733239954u,
		1555261956u,
		3268935591u,
		3050360625u,
		752459403u,
		1541320221u,
		2607071920u,
		3965973030u,
		1969922972u,
		40735498u,
		2617837225u,
		3943577151u,
		1913087877u,
		83908371u,
		2512341634u,
		3803740692u,
		2075208622u,
		213261112u,
		2463272603u,
		3855990285u,
		2094854071u,
		198958881u,
		2262029012u,
		4057260610u,
		1759359992u,
		534414190u,
		2176718541u,
		4139329115u,
		1873836001u,
		414664567u,
		2282248934u,
		4279200368u,
		1711684554u,
		285281116u,
		2405801727u,
		4167216745u,
		1634467795u,
		376229701u,
		2685067896u,
		3608007406u,
		1308918612u,
		956543938u,
		2808555105u,
		3495958263u,
		1231636301u,
		1047427035u,
		2932959818u,
		3654703836u,
		1088359270u,
		936918000u,
		2847714899u,
		3736837829u,
		1202900863u,
		817233897u,
		3183342108u,
		3401237130u,
		1404277552u,
		615818150u,
		3134207493u,
		3453421203u,
		1423857449u,
		601450431u,
		3009837614u,
		3294710456u,
		1567103746u,
		711928724u,
		3020668471u,
		3272380065u,
		1510334235u,
		755167117u
		};

		public static int GetCrc32(byte[] bytes)
		{
			if (bytes == null)
			{
				Warning("Bytes is invalid.");
			}
			return GetCrc32(bytes, 0, bytes.Length);
		}

		public static int GetCrc32(byte[] bytes, int offset, int length)
		{
			if (bytes == null)
			{
				Warning("Bytes is invalid.");
			}
			if (offset < 0 || length < 0 || offset + length > bytes.Length)
			{
				Warning("Offset or length is invalid.");
			}
			s_Algorithm.HashCore(bytes, offset, length);
			int result = (int)s_Algorithm.HashFinal();
			s_Algorithm.Initialize();
			return result;
		}

		public static int GetCrc32(Stream stream)
		{
			if (stream == null)
			{
				Warning("Stream is invalid.");
                return 0;
            }
			while (true)
			{
				int bytesRead = stream.Read(s_CachedBytes, 0, 4096);
				if (bytesRead > 0)
				{
					s_Algorithm.HashCore(s_CachedBytes, 0, bytesRead);
					continue;
				}
				break;
			}
			int result = (int)s_Algorithm.HashFinal();
			s_Algorithm.Initialize();
			Array.Clear(s_CachedBytes, 0, 4096);
			return result;
		}

		public static byte[] GetCrc32Bytes(int crc32)
		{
			return new byte[4]
			{
			(byte)((crc32 >> 24) & 0xFF),
			(byte)((crc32 >> 16) & 0xFF),
			(byte)((crc32 >> 8) & 0xFF),
			(byte)(crc32 & 0xFF)
			};
		}

		public static void GetCrc32Bytes(int crc32, byte[] bytes)
		{
			GetCrc32Bytes(crc32, bytes, 0);
		}

		public static void GetCrc32Bytes(int crc32, byte[] bytes, int offset)
		{
			if (bytes == null)
			{
				Warning("Result is invalid.");
				return;
			}
			if (offset < 0 || offset + 4 > bytes.Length)
			{
				Warning("Offset or length is invalid.");
				return;
			}
			bytes[offset] = (byte)((crc32 >> 24) & 0xFF);
			bytes[offset + 1] = (byte)((crc32 >> 16) & 0xFF);
			bytes[offset + 2] = (byte)((crc32 >> 8) & 0xFF);
			bytes[offset + 3] = (byte)(crc32 & 0xFF);
		}

		internal static int GetCrc32(Stream stream, byte[] code, int length)
		{
			if (stream == null)
			{
				Warning("Stream is invalid.");
                return 0;
			}
			if (code == null)
			{
				Warning("Code is invalid.");
                return 0;
			}
			int codeLength = code.Length;
			if (codeLength <= 0)
			{
				Warning("Code length is invalid.");
                return 0;
            }
			int bytesLength = (int)stream.Length;
			if (length < 0 || length > bytesLength)
			{
				length = bytesLength;
			}
			int codeIndex = 0;
			while (true)
			{
				int bytesRead = stream.Read(s_CachedBytes, 0, 4096);
				if (bytesRead <= 0)
				{
					break;
				}
				if (length > 0)
				{
					for (int i = 0; i < bytesRead && i < length; i++)
					{
						s_CachedBytes[i] ^= code[codeIndex++];
						codeIndex %= codeLength;
					}
					length -= bytesRead;
				}
				s_Algorithm.HashCore(s_CachedBytes, 0, bytesRead);
			}
			int result = (int)s_Algorithm.HashFinal();
			s_Algorithm.Initialize();
			Array.Clear(s_CachedBytes, 0, 4096);
			return result;
		}

		public static uint GetCrc32Unit(string msg)
		{
			if (string.IsNullOrEmpty(msg))
			{
				return 0u;
			}
			int iCount = msg.Length;
			uint crc = uint.MaxValue;
			for (int i = 0; i < iCount; i++)
			{
				crc = (((crc >> 8) & 0xFFFFFF) ^ crcTable[(crc ^ (byte)msg[i]) & 0xFF]);
			}
			return (uint)((int)crc ^ -1);
		}

		public static uint GetCrc32UnitBytes(byte[] bytes, int offset, int size)
		{
			if (bytes == null || size == 0)
			{
				return 0u;
			}
			uint crc = uint.MaxValue;
			for (int i = 0; i < size; i++)
			{
				crc = (((crc >> 8) & 0xFFFFFF) ^ crcTable[(crc ^ bytes[offset + i]) & 0xFF]);
			}
			return (uint)((int)crc ^ -1);
		}

        public static void Warning(string value)
        {
            Console.WriteLine(value);
        }
	}

}