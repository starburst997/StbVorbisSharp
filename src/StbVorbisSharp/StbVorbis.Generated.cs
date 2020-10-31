using System;
using System.Runtime.InteropServices;

namespace StbVorbisSharp
{
	unsafe partial class StbVorbis
	{
		public enum STBVorbisError
		{
			VORBIS__no_error,
			VORBIS_need_more_data = 1,
			VORBIS_invalid_api_mixing,
			VORBIS_outofmem,
			VORBIS_feature_not_supported,
			VORBIS_too_many_channels,
			VORBIS_file_open_failure,
			VORBIS_seek_without_length,
			VORBIS_unexpected_eof = 10,
			VORBIS_seek_invalid,
			VORBIS_invalid_setup = 20,
			VORBIS_invalid_stream,
			VORBIS_missing_capture_pattern = 30,
			VORBIS_invalid_stream_structure_version,
			VORBIS_continued_packet_flag_invalid,
			VORBIS_incorrect_stream_serial_number,
			VORBIS_invalid_first_page,
			VORBIS_bad_packet_type,
			VORBIS_cant_find_last_page,
			VORBIS_seek_failed,
			VORBIS_ogg_skeleton_not_supported
		}

		public const int VORBIS_packet_id = 1;
		public const int VORBIS_packet_comment = 3;
		public const int VORBIS_packet_setup = 5;

		public static uint[] _crc_table = new uint[256];

		public static byte[] ogg_page_header = { 0x4f, 0x67, 0x67, 0x53 };

		public static float[] inverse_db_table = { 1.0649863e-07f, 1.1341951e-07f, 1.2079015e-07f, 1.2863978e-07f, 1.3699951e-07f, 1.4590251e-07f, 1.5538408e-07f, 1.6548181e-07f, 1.7623575e-07f, 1.8768855e-07f, 1.9988561e-07f, 2.1287530e-07f, 2.2670913e-07f, 2.4144197e-07f, 2.5713223e-07f, 2.7384213e-07f, 2.9163793e-07f, 3.1059021e-07f, 3.3077411e-07f, 3.5226968e-07f, 3.7516214e-07f, 3.9954229e-07f, 4.2550680e-07f, 4.5315863e-07f, 4.8260743e-07f, 5.1396998e-07f, 5.4737065e-07f, 5.8294187e-07f, 6.2082472e-07f, 6.6116941e-07f, 7.0413592e-07f, 7.4989464e-07f, 7.9862701e-07f, 8.5052630e-07f, 9.0579828e-07f, 9.6466216e-07f, 1.0273513e-06f, 1.0941144e-06f, 1.1652161e-06f, 1.2409384e-06f, 1.3215816e-06f, 1.4074654e-06f, 1.4989305e-06f, 1.5963394e-06f, 1.7000785e-06f, 1.8105592e-06f, 1.9282195e-06f, 2.0535261e-06f, 2.1869758e-06f, 2.3290978e-06f, 2.4804557e-06f, 2.6416497e-06f, 2.8133190e-06f, 2.9961443e-06f, 3.1908506e-06f, 3.3982101e-06f, 3.6190449e-06f, 3.8542308e-06f, 4.1047004e-06f, 4.3714470e-06f, 4.6555282e-06f, 4.9580707e-06f, 5.2802740e-06f, 5.6234160e-06f, 5.9888572e-06f, 6.3780469e-06f, 6.7925283e-06f, 7.2339451e-06f, 7.7040476e-06f, 8.2047000e-06f, 8.7378876e-06f, 9.3057248e-06f, 9.9104632e-06f, 1.0554501e-05f, 1.1240392e-05f, 1.1970856e-05f, 1.2748789e-05f, 1.3577278e-05f, 1.4459606e-05f, 1.5399272e-05f, 1.6400004e-05f, 1.7465768e-05f, 1.8600792e-05f, 1.9809576e-05f, 2.1096914e-05f, 2.2467911e-05f, 2.3928002e-05f, 2.5482978e-05f, 2.7139006e-05f, 2.8902651e-05f, 3.0780908e-05f, 3.2781225e-05f, 3.4911534e-05f, 3.7180282e-05f, 3.9596466e-05f, 4.2169667e-05f, 4.4910090e-05f, 4.7828601e-05f, 5.0936773e-05f, 5.4246931e-05f, 5.7772202e-05f, 6.1526565e-05f, 6.5524908e-05f, 6.9783085e-05f, 7.4317983e-05f, 7.9147585e-05f, 8.4291040e-05f, 8.9768747e-05f, 9.5602426e-05f, 0.00010181521f, 0.00010843174f, 0.00011547824f, 0.00012298267f, 0.00013097477f, 0.00013948625f, 0.00014855085f, 0.00015820453f, 0.00016848555f, 0.00017943469f, 0.00019109536f, 0.00020351382f, 0.00021673929f, 0.00023082423f, 0.00024582449f, 0.00026179955f, 0.00027881276f, 0.00029693158f, 0.00031622787f, 0.00033677814f, 0.00035866388f, 0.00038197188f, 0.00040679456f, 0.00043323036f, 0.00046138411f, 0.00049136745f, 0.00052329927f, 0.00055730621f, 0.00059352311f, 0.00063209358f, 0.00067317058f, 0.00071691700f, 0.00076350630f, 0.00081312324f, 0.00086596457f, 0.00092223983f, 0.00098217216f, 0.0010459992f, 0.0011139742f, 0.0011863665f, 0.0012634633f, 0.0013455702f, 0.0014330129f, 0.0015261382f, 0.0016253153f, 0.0017309374f, 0.0018434235f, 0.0019632195f, 0.0020908006f, 0.0022266726f, 0.0023713743f, 0.0025254795f, 0.0026895994f, 0.0028643847f, 0.0030505286f, 0.0032487691f, 0.0034598925f, 0.0036847358f, 0.0039241906f, 0.0041792066f, 0.0044507950f, 0.0047400328f, 0.0050480668f, 0.0053761186f, 0.0057254891f, 0.0060975636f, 0.0064938176f, 0.0069158225f, 0.0073652516f, 0.0078438871f, 0.0083536271f, 0.0088964928f, 0.009474637f, 0.010090352f, 0.010746080f, 0.011444421f, 0.012188144f, 0.012980198f, 0.013823725f, 0.014722068f, 0.015678791f, 0.016697687f, 0.017782797f, 0.018938423f, 0.020169149f, 0.021479854f, 0.022875735f, 0.024362330f, 0.025945531f, 0.027631618f, 0.029427276f, 0.031339626f, 0.033376252f, 0.035545228f, 0.037855157f, 0.040315199f, 0.042935108f, 0.045725273f, 0.048696758f, 0.051861348f, 0.055231591f, 0.058820850f, 0.062643361f, 0.066714279f, 0.071049749f, 0.075666962f, 0.080584227f, 0.085821044f, 0.091398179f, 0.097337747f, 0.10366330f, 0.11039993f, 0.11757434f, 0.12521498f, 0.13335215f, 0.14201813f, 0.15124727f, 0.16107617f, 0.17154380f, 0.18269168f, 0.19456402f, 0.20720788f, 0.22067342f, 0.23501402f, 0.25028656f, 0.26655159f, 0.28387361f, 0.30232132f, 0.32196786f, 0.34289114f, 0.36517414f, 0.38890521f, 0.41417847f, 0.44109412f, 0.46975890f, 0.50028648f, 0.53279791f, 0.56742212f, 0.60429640f, 0.64356699f, 0.68538959f, 0.72993007f, 0.77736504f, 0.82788260f, 0.88168307f, 0.9389798f, 1.0f };

		public static int[,] channel_selector = { { 0, 0 }, { 1, 0 }, { 2, 4 } };

		[StructLayout(LayoutKind.Sequential)]
		public struct stb_vorbis_alloc
		{
			public sbyte* alloc_buffer;
			public int alloc_buffer_length_in_bytes;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct stb_vorbis_info
		{
			public uint sample_rate;
			public int channels;
			public uint setup_memory_required;
			public uint setup_temp_memory_required;
			public uint temp_memory_required;
			public int max_frame_size;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct Codebook
		{
			public int dimensions;
			public int entries;
			public byte* codeword_lengths;
			public float minimum_value;
			public float delta_value;
			public byte value_bits;
			public byte lookup_type;
			public byte sequence_p;
			public byte sparse;
			public uint lookup_values;
			public float* multiplicands;
			public uint* codewords;
			public fixed short fast_huffman[(1 << 10)];
			public uint* sorted_codewords;
			public int* sorted_values;
			public int sorted_entries;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct Floor0
		{
			public byte order;
			public ushort rate;
			public ushort bark_map_size;
			public byte amplitude_bits;
			public byte amplitude_offset;
			public byte number_of_books;
			public fixed byte book_list[16];
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct Floor1
		{
			public byte partitions;
			public fixed byte partition_class_list[32];
			public fixed byte class_dimensions[16];
			public fixed byte class_subclasses[16];
			public fixed byte class_masterbooks[16];
			public fixed short subclass_books[16 * 8];
			public fixed ushort Xlist[31 * 8 + 2];
			public fixed byte sorted_order[31 * 8 + 2];
			public fixed byte neighbors[(31 * 8 + 2) * 2];
			public byte floor1_multiplier;
			public byte rangebits;
			public int values;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct Floor
		{
			public Floor0 floor0;
			public Floor1 floor1;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MappingChannel
		{
			public byte magnitude;
			public byte angle;
			public byte mux;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct Mapping
		{
			public ushort coupling_steps;
			public MappingChannel* chan;
			public byte submaps;
			public fixed byte submap_floor[15];
			public fixed byte submap_residue[15];
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct Mode
		{
			public byte blockflag;
			public byte mapping;
			public ushort windowtype;
			public ushort transformtype;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct CRCscan
		{
			public uint goal_crc;
			public int bytes_left;
			public uint crc_so_far;
			public int bytes_done;
			public uint sample_loc;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct ProbedPage
		{
			public uint page_start;
			public uint page_end;
			public uint last_decoded_sample;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct stbv__floor_ordering
		{
			public ushort x;
			public ushort id;
		}

		public static int error(stb_vorbis f, int e)
		{
			f.error = (int)(e);
			if ((f.eof == 0) && (e != (int)STBVorbisError.VORBIS_need_more_data))
			{
				f.error = (int)(e);
			}

			return (int)(0);
		}

		public static void* make_block_array(void* mem, int count, int size)
		{
			int i = 0;
			void** p = (void**)(mem);
			sbyte* q = (sbyte*)(p + count);
			for (i = (int)(0); (i) < (count); ++i)
			{
				p[i] = q;
				q += size;
			}
			return p;
		}

		public static void* setup_malloc(stb_vorbis f, int sz)
		{
			sz = (int)((sz + 3) & ~3);
			f.setup_memory_required += (uint)(sz);
			if ((f.alloc.alloc_buffer) != null)
			{
				void* p = f.alloc.alloc_buffer + f.setup_offset;
				if ((f.setup_offset + sz) > (f.temp_offset))
					return null;
				f.setup_offset += (int)(sz);
				return p;
			}

			return (sz) != 0 ? CRuntime.malloc((ulong)(sz)) : null;
		}

		public static void setup_free(stb_vorbis f, void* p)
		{
			if ((f.alloc.alloc_buffer) != null)
				return;
			CRuntime.free(p);
		}

		public static void* setup_temp_malloc(stb_vorbis f, int sz)
		{
			sz = (int)((sz + 3) & ~3);
			if ((f.alloc.alloc_buffer) != null)
			{
				if ((f.temp_offset - sz) < (f.setup_offset))
					return null;
				f.temp_offset -= (int)(sz);
				return f.alloc.alloc_buffer + f.temp_offset;
			}

			return CRuntime.malloc((ulong)(sz));
		}

		public static void setup_temp_free(stb_vorbis f, void* p, int sz)
		{
			if ((f.alloc.alloc_buffer) != null)
			{
				f.temp_offset += (int)((sz + 3) & ~3);
				return;
			}

			CRuntime.free(p);
		}

		public static void crc32_init()
		{
			int i = 0;
			int j = 0;
			uint s = 0;
			for (i = (int)(0); (i) < (256); i++)
			{
				for (s = (uint)((uint)(i) << 24), j = (int)(0); (j) < (8); ++j)
				{
					s = (uint)((s << 1) ^ ((s) >= (1U << 31) ? 0x04c11db7 : 0));
				}
				StbVorbis._crc_table[i] = (uint)(s);
			}
		}

		public static uint crc32_update(uint crc, byte _byte_)
		{
			return (uint)((crc << 8) ^ StbVorbis._crc_table[_byte_ ^ (crc >> 24)]);
		}

		public static uint bit_reverse(uint n)
		{
			n = (uint)(((n & 0xAAAAAAAA) >> 1) | ((n & 0x55555555) << 1));
			n = (uint)(((n & 0xCCCCCCCC) >> 2) | ((n & 0x33333333) << 2));
			n = (uint)(((n & 0xF0F0F0F0) >> 4) | ((n & 0x0F0F0F0F) << 4));
			n = (uint)(((n & 0xFF00FF00) >> 8) | ((n & 0x00FF00FF) << 8));
			return (uint)((n >> 16) | (n << 16));
		}

		public static float square(float x)
		{
			return (float)(x * x);
		}

		public static int ilog(int n)
		{
			sbyte* log2_4 = stackalloc sbyte[16];
			log2_4[0] = (sbyte)(0);
			log2_4[1] = (sbyte)(1);
			log2_4[2] = (sbyte)(2);
			log2_4[3] = (sbyte)(2);
			log2_4[4] = (sbyte)(3);
			log2_4[5] = (sbyte)(3);
			log2_4[6] = (sbyte)(3);
			log2_4[7] = (sbyte)(3);
			log2_4[8] = (sbyte)(4);
			log2_4[9] = (sbyte)(4);
			log2_4[10] = (sbyte)(4);
			log2_4[11] = (sbyte)(4);
			log2_4[12] = (sbyte)(4);
			log2_4[13] = (sbyte)(4);
			log2_4[14] = (sbyte)(4);
			log2_4[15] = (sbyte)(4);

			if ((n) < (0))
				return (int)(0);
			if ((n) < (1 << 14))
				if ((n) < (1 << 4))
					return (int)(0 + log2_4[n]);
				else if ((n) < (1 << 9))
					return (int)(5 + log2_4[n >> 5]);
				else
					return (int)(10 + log2_4[n >> 10]);
			else if ((n) < (1 << 24))
				if ((n) < (1 << 19))
					return (int)(15 + log2_4[n >> 15]);
				else
					return (int)(20 + log2_4[n >> 20]);
			else if ((n) < (1 << 29))
				return (int)(25 + log2_4[n >> 25]);
			else
				return (int)(30 + log2_4[n >> 30]);
		}

		public static float float32_unpack(uint x)
		{
			uint mantissa = (uint)(x & 0x1fffff);
			uint sign = (uint)(x & 0x80000000);
			uint exp = (uint)((x & 0x7fe00000) >> 21);
			double res = (double)((sign) != 0 ? -(double)(mantissa) : (double)(mantissa));
			return (float)(CRuntime.ldexp((double)((float)(res)), (int)(exp - 788)));
		}

		public static void add_entry(Codebook* c, uint huff_code, int symbol, int count, int len, uint* values)
		{
			if (c->sparse == 0)
			{
				c->codewords[symbol] = (uint)(huff_code);
			}
			else
			{
				c->codewords[count] = (uint)(huff_code);
				c->codeword_lengths[count] = (byte)(len);
				values[count] = (uint)(symbol);
			}

		}

		public static int compute_codewords(Codebook* c, byte* len, int n, uint* values)
		{
			int i = 0;
			int k = 0;
			int m = (int)(0);
			uint* available = stackalloc uint[32];
			CRuntime.memset(available, 0, 32);
			for (k = (int)(0); (k) < (n); ++k)
			{
				if ((len[k]) < (255))
					break;
			}
			if ((k) == (n))
			{
				return (int)(1);
			}

			add_entry(c, (uint)(0), (int)(k), (int)(m++), (int)(len[k]), values);
			for (i = (int)(1); i <= len[k]; ++i)
			{
				available[i] = (uint)(1U << (32 - i));
			}
			for (i = (int)(k + 1); (i) < (n); ++i)
			{
				uint res = 0;
				int z = (int)(len[i]);
				int y = 0;
				if ((z) == (255))
					continue;
				while (((z) > (0)) && (available[z] == 0))
				{
					--z;
				}
				if ((z) == (0))
				{
					return (int)(0);
				}
				res = (uint)(available[z]);
				available[z] = (uint)(0);
				add_entry(c, (uint)(bit_reverse((uint)(res))), (int)(i), (int)(m++), (int)(len[i]), values);
				if (z != len[i])
				{
					for (y = (int)(len[i]); (y) > (z); --y)
					{
						available[y] = (uint)(res + (1 << (32 - y)));
					}
				}
			}
			return (int)(1);
		}

		public static void compute_accelerated_huffman(Codebook* c)
		{
			int i = 0;
			int len = 0;
			for (i = (int)(0); (i) < (1 << 10); ++i)
			{
				c->fast_huffman[i] = (short)(-1);
			}
			len = (int)((c->sparse) != 0 ? c->sorted_entries : c->entries);
			if ((len) > (32767))
				len = (int)(32767);
			for (i = (int)(0); (i) < (len); ++i)
			{
				if (c->codeword_lengths[i] <= 10)
				{
					uint z = (uint)((c->sparse) != 0 ? bit_reverse((uint)(c->sorted_codewords[i])) : c->codewords[i]);
					while ((z) < (1 << 10))
					{
						c->fast_huffman[z] = (short)(i);
						z += (uint)(1 << c->codeword_lengths[i]);
					}
				}
			}
		}

		public static int uint32_compare(void* p, void* q)
		{
			uint x = (uint)(*(uint*)(p));
			uint y = (uint)(*(uint*)(q));
			return (int)((x) < (y) ? -1 : ((x) > (y) ? 1 : 0));
		}

		public static int include_in_sort(Codebook* c, byte len)
		{
			if ((c->sparse) != 0)
			{
				return (int)(1);
			}

			if ((len) == (255))
				return (int)(0);
			if ((len) > (10))
				return (int)(1);
			return (int)(0);
		}

		public static void compute_sorted_huffman(Codebook* c, byte* lengths, uint* values)
		{
			int i = 0;
			int len = 0;
			if (c->sparse == 0)
			{
				int k = (int)(0);
				for (i = (int)(0); (i) < (c->entries); ++i)
				{
					if ((include_in_sort(c, (byte)(lengths[i]))) != 0)
						c->sorted_codewords[k++] = (uint)(bit_reverse((uint)(c->codewords[i])));
				}
			}
			else
			{
				for (i = (int)(0); (i) < (c->sorted_entries); ++i)
				{
					c->sorted_codewords[i] = (uint)(bit_reverse((uint)(c->codewords[i])));
				}
			}

			CRuntime.qsort(c->sorted_codewords, (ulong)(c->sorted_entries), (ulong)(sizeof(uint)), uint32_compare);
			c->sorted_codewords[c->sorted_entries] = (uint)(0xffffffff);
			len = (int)((c->sparse) != 0 ? c->sorted_entries : c->entries);
			for (i = (int)(0); (i) < (len); ++i)
			{
				int huff_len = (int)((c->sparse) != 0 ? lengths[values[i]] : lengths[i]);
				if ((include_in_sort(c, (byte)(huff_len))) != 0)
				{
					uint code = (uint)(bit_reverse((uint)(c->codewords[i])));
					int x = (int)(0);
					int n = (int)(c->sorted_entries);
					while ((n) > (1))
					{
						int m = (int)(x + (n >> 1));
						if (c->sorted_codewords[m] <= code)
						{
							x = (int)(m);
							n -= (int)(n >> 1);
						}
						else
						{
							n >>= 1;
						}
					}
					if ((c->sparse) != 0)
					{
						c->sorted_values[x] = (int)(values[i]);
						c->codeword_lengths[x] = (byte)(huff_len);
					}
					else
					{
						c->sorted_values[x] = (int)(i);
					}
				}
			}
		}

		public static int vorbis_validate(byte* data)
		{
			byte* vorbis = stackalloc byte[6];
			vorbis[0] = (byte)('v');
			vorbis[1] = (byte)('o');
			vorbis[2] = (byte)('r');
			vorbis[3] = (byte)('b');
			vorbis[4] = (byte)('i');
			vorbis[5] = (byte)('s');

			return (int)((CRuntime.memcmp(data, vorbis, (ulong)(6))) == (0) ? 1 : 0);
		}

		public static int lookup1_values(int entries, int dim)
		{
			int r = (int)(CRuntime.floor((double)(CRuntime.exp((double)((float)(CRuntime.log((double)((float)(entries)))) / dim)))));
			if ((int)(CRuntime.floor((double)(CRuntime.pow((double)((float)(r) + 1), (double)(dim))))) <= entries)
				++r;
			return (int)(r);
		}

		public static void compute_twiddle_factors(int n, float* A, float* B, float* C)
		{
			int n4 = (int)(n >> 2);
			int n8 = (int)(n >> 3);
			int k = 0;
			int k2 = 0;
			for (k = (int)(k2 = (int)(0)); (k) < (n4); ++k, k2 += (int)(2))
			{
				A[k2] = ((float)(CRuntime.cos((double)(4 * k * 3.14159265358979323846264f / n))));
				A[k2 + 1] = ((float)(-CRuntime.sin((double)(4 * k * 3.14159265358979323846264f / n))));
				B[k2] = (float)((float)(CRuntime.cos((double)((k2 + 1) * 3.14159265358979323846264f / n / 2))) * 0.5f);
				B[k2 + 1] = (float)((float)(CRuntime.sin((double)((k2 + 1) * 3.14159265358979323846264f / n / 2))) * 0.5f);
			}
			for (k = (int)(k2 = (int)(0)); (k) < (n8); ++k, k2 += (int)(2))
			{
				C[k2] = ((float)(CRuntime.cos((double)(2 * (k2 + 1) * 3.14159265358979323846264f / n))));
				C[k2 + 1] = ((float)(-CRuntime.sin((double)(2 * (k2 + 1) * 3.14159265358979323846264f / n))));
			}
		}

		public static void compute_window(int n, float* window)
		{
			int n2 = (int)(n >> 1);
			int i = 0;
			for (i = (int)(0); (i) < (n2); ++i)
			{
				window[i] = ((float)(CRuntime.sin((double)(0.5 * 3.14159265358979323846264f * square((float)(CRuntime.sin((double)((i - 0 + 0.5) / n2 * 0.5 * 3.14159265358979323846264f))))))));
			}
		}

		public static void compute_bitreverse(int n, ushort* rev)
		{
			int ld = (int)(ilog((int)(n)) - 1);
			int i = 0;
			int n8 = (int)(n >> 3);
			for (i = (int)(0); (i) < (n8); ++i)
			{
				rev[i] = (ushort)((bit_reverse((uint)(i)) >> (32 - ld + 3)) << 2);
			}
		}

		public static int init_blocksize(stb_vorbis f, int b, int n)
		{
			int n2 = (int)(n >> 1);
			int n4 = (int)(n >> 2);
			int n8 = (int)(n >> 3);
			f.A[b] = (float*)(setup_malloc(f, (int)(sizeof(float) * n2)));
			f.B[b] = (float*)(setup_malloc(f, (int)(sizeof(float) * n2)));
			f.C[b] = (float*)(setup_malloc(f, (int)(sizeof(float) * n4)));
			if (((f.A[b] == null) || (f.B[b] == null)) || (f.C[b] == null))
				return (int)(error(f, (int)(STBVorbisError.VORBIS_outofmem)));
			compute_twiddle_factors((int)(n), f.A[b], f.B[b], f.C[b]);
			f.window[b] = (float*)(setup_malloc(f, (int)(sizeof(float) * n2)));
			if (f.window[b] == null)
				return (int)(error(f, (int)(STBVorbisError.VORBIS_outofmem)));
			compute_window((int)(n), f.window[b]);
			f.bit_reverse[b] = (ushort*)(setup_malloc(f, (int)(sizeof(ushort) * n8)));
			if (f.bit_reverse[b] == null)
				return (int)(error(f, (int)(STBVorbisError.VORBIS_outofmem)));
			compute_bitreverse((int)(n), f.bit_reverse[b]);
			return (int)(1);
		}

		public static void neighbors(ushort* x, int n, int* plow, int* phigh)
		{
			int low = (int)(-1);
			int high = (int)(65536);
			int i = 0;
			for (i = (int)(0); (i) < (n); ++i)
			{
				if (((x[i]) > (low)) && ((x[i]) < (x[n])))
				{
					*plow = (int)(i);
					low = (int)(x[i]);
				}
				if (((x[i]) < (high)) && ((x[i]) > (x[n])))
				{
					*phigh = (int)(i);
					high = (int)(x[i]);
				}
			}
		}

		public static int point_compare(void* p, void* q)
		{
			stbv__floor_ordering* a = (stbv__floor_ordering*)(p);
			stbv__floor_ordering* b = (stbv__floor_ordering*)(q);
			return (int)((a->x) < (b->x) ? -1 : ((a->x) > (b->x) ? 1 : 0));
		}

		public static byte get8(stb_vorbis z)
		{
			if ((1) != 0)
			{
				if ((z.stream) >= (z.stream_end))
				{
					z.eof = (int)(1);
					return (byte)(0);
				}
				return (byte)(*z.stream++);
			}

		}

		public static uint get32(stb_vorbis f)
		{
			uint x = 0;
			x = (uint)(get8(f));
			x += (uint)(get8(f) << 8);
			x += (uint)(get8(f) << 16);
			x += (uint)((uint)(get8(f)) << 24);
			return (uint)(x);
		}

		public static int getn(stb_vorbis z, byte* data, int n)
		{
			if ((1) != 0)
			{
				if ((z.stream + n) > (z.stream_end))
				{
					z.eof = (int)(1);
					return (int)(0);
				}
				CRuntime.memcpy(data, z.stream, (ulong)(n));
				z.stream += n;
				return (int)(1);
			}

		}

		public static void skip(stb_vorbis z, int n)
		{
			if ((1) != 0)
			{
				z.stream += n;
				if ((z.stream) >= (z.stream_end))
					z.eof = (int)(1);
				return;
			}

		}

		public static int set_file_offset(stb_vorbis f, uint loc)
		{
			if ((f.push_mode) != 0)
				return (int)(0);
			f.eof = (int)(0);
			if ((1) != 0)
			{
				if (((f.stream_start + loc) >= (f.stream_end)) || ((f.stream_start + loc) < (f.stream_start)))
				{
					f.stream = f.stream_end;
					f.eof = (int)(1);
					return (int)(0);
				}
				else
				{
					f.stream = f.stream_start + loc;
					return (int)(1);
				}
			}

		}

		public static int capture_pattern(stb_vorbis f)
		{
			if (0x4f != get8(f))
				return (int)(0);
			if (0x67 != get8(f))
				return (int)(0);
			if (0x67 != get8(f))
				return (int)(0);
			if (0x53 != get8(f))
				return (int)(0);
			return (int)(1);
		}

		public static int start_page_no_capturepattern(stb_vorbis f)
		{
			uint loc0 = 0;
			uint loc1 = 0;
			uint n = 0;
			if (0 != get8(f))
				return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_stream_structure_version)));
			f.page_flag = (byte)(get8(f));
			loc0 = (uint)(get32(f));
			loc1 = (uint)(get32(f));
			get32(f);
			n = (uint)(get32(f));
			f.last_page = (int)(n);
			get32(f);
			f.segment_count = (int)(get8(f));
			if (getn(f, f.segments, (int)(f.segment_count)) == 0)
				return (int)(error(f, (int)(STBVorbisError.VORBIS_unexpected_eof)));
			f.end_seg_with_known_loc = (int)(-2);
			if ((loc0 != ~0U) || (loc1 != ~0U))
			{
				int i = 0;
				for (i = (int)(f.segment_count - 1); (i) >= (0); --i)
				{
					if ((f.segments[i]) < (255))
						break;
				}
				if ((i) >= (0))
				{
					f.end_seg_with_known_loc = (int)(i);
					f.known_loc_for_packet = (uint)(loc0);
				}
			}

			if ((f.first_decode) != 0)
			{
				int i = 0;
				int len = 0;
				ProbedPage p = new ProbedPage();
				len = (int)(0);
				for (i = (int)(0); (i) < (f.segment_count); ++i)
				{
					len += (int)(f.segments[i]);
				}
				len += (int)(27 + f.segment_count);
				p.page_start = (uint)(f.first_audio_page_offset);
				p.page_end = (uint)(p.page_start + len);
				p.last_decoded_sample = (uint)(loc0);
				f.p_first = (ProbedPage)(p);
			}

			f.next_seg = (int)(0);
			return (int)(1);
		}

		public static int start_page(stb_vorbis f)
		{
			if (capture_pattern(f) == 0)
				return (int)(error(f, (int)(STBVorbisError.VORBIS_missing_capture_pattern)));
			return (int)(start_page_no_capturepattern(f));
		}

		public static int start_packet(stb_vorbis f)
		{
			while ((f.next_seg) == (-1))
			{
				if (start_page(f) == 0)
					return (int)(0);
				if ((f.page_flag & 1) != 0)
					return (int)(error(f, (int)(STBVorbisError.VORBIS_continued_packet_flag_invalid)));
			}
			f.last_seg = (int)(0);
			f.valid_bits = (int)(0);
			f.packet_bytes = (int)(0);
			f.bytes_in_seg = (byte)(0);
			return (int)(1);
		}

		public static int maybe_start_packet(stb_vorbis f)
		{
			if ((f.next_seg) == (-1))
			{
				int x = (int)(get8(f));
				if ((f.eof) != 0)
					return (int)(0);
				if (0x4f != x)
					return (int)(error(f, (int)(STBVorbisError.VORBIS_missing_capture_pattern)));
				if (0x67 != get8(f))
					return (int)(error(f, (int)(STBVorbisError.VORBIS_missing_capture_pattern)));
				if (0x67 != get8(f))
					return (int)(error(f, (int)(STBVorbisError.VORBIS_missing_capture_pattern)));
				if (0x53 != get8(f))
					return (int)(error(f, (int)(STBVorbisError.VORBIS_missing_capture_pattern)));
				if (start_page_no_capturepattern(f) == 0)
					return (int)(0);
				if ((f.page_flag & 1) != 0)
				{
					f.last_seg = (int)(0);
					f.bytes_in_seg = (byte)(0);
					return (int)(error(f, (int)(STBVorbisError.VORBIS_continued_packet_flag_invalid)));
				}
			}

			return (int)(start_packet(f));
		}

		public static int next_segment(stb_vorbis f)
		{
			int len = 0;
			if ((f.last_seg) != 0)
				return (int)(0);
			if ((f.next_seg) == (-1))
			{
				f.last_seg_which = (int)(f.segment_count - 1);
				if (start_page(f) == 0)
				{
					f.last_seg = (int)(1);
					return (int)(0);
				}
				if ((f.page_flag & 1) == 0)
					return (int)(error(f, (int)(STBVorbisError.VORBIS_continued_packet_flag_invalid)));
			}

			len = (int)(f.segments[f.next_seg++]);
			if ((len) < (255))
			{
				f.last_seg = (int)(1);
				f.last_seg_which = (int)(f.next_seg - 1);
			}

			if ((f.next_seg) >= (f.segment_count))
				f.next_seg = (int)(-1);
			f.bytes_in_seg = (byte)(len);
			return (int)(len);
		}

		public static int get8_packet_raw(stb_vorbis f)
		{
			if (f.bytes_in_seg == 0)
			{
				if ((f.last_seg) != 0)
					return (int)(-1);
				else if (next_segment(f) == 0)
					return (int)(-1);
			}

			--f.bytes_in_seg;
			++f.packet_bytes;
			return (int)(get8(f));
		}

		public static int get8_packet(stb_vorbis f)
		{
			int x = (int)(get8_packet_raw(f));
			f.valid_bits = (int)(0);
			return (int)(x);
		}

		public static void flush_packet(stb_vorbis f)
		{
			while (get8_packet_raw(f) != (-1))
			{
			}
		}

		public static void prep_huffman(stb_vorbis f)
		{
			if (f.valid_bits <= 24)
			{
				if ((f.valid_bits) == (0))
					f.acc = (uint)(0);
				do
				{
					int z = 0;
					if (((f.last_seg) != 0) && (f.bytes_in_seg == 0))
						return;
					z = (int)(get8_packet_raw(f));
					if ((z) == (-1))
						return;
					f.acc += (uint)((uint)(z) << f.valid_bits);
					f.valid_bits += (int)(8);
				}
				while (f.valid_bits <= 24);
			}

		}

		public static int codebook_decode_scalar_raw(stb_vorbis f, Codebook* c)
		{
			int i = 0;
			prep_huffman(f);
			if (((c->codewords) == null) && ((c->sorted_codewords) == null))
				return (int)(-1);
			if (((c->entries) > (8) ? c->sorted_codewords != null : c->codewords == null))
			{
				uint code = (uint)(bit_reverse((uint)(f.acc)));
				int x = (int)(0);
				int n = (int)(c->sorted_entries);
				int len = 0;
				while ((n) > (1))
				{
					int m = (int)(x + (n >> 1));
					if (c->sorted_codewords[m] <= code)
					{
						x = (int)(m);
						n -= (int)(n >> 1);
					}
					else
					{
						n >>= 1;
					}
				}
				if (c->sparse == 0)
					x = (int)(c->sorted_values[x]);
				len = (int)(c->codeword_lengths[x]);
				if ((f.valid_bits) >= (len))
				{
					f.acc >>= len;
					f.valid_bits -= (int)(len);
					return (int)(x);
				}
				f.valid_bits = (int)(0);
				return (int)(-1);
			}

			for (i = (int)(0); (i) < (c->entries); ++i)
			{
				if ((c->codeword_lengths[i]) == (255))
					continue;
				if ((c->codewords[i]) == (f.acc & ((1 << c->codeword_lengths[i]) - 1)))
				{
					if ((f.valid_bits) >= (c->codeword_lengths[i]))
					{
						f.acc >>= c->codeword_lengths[i];
						f.valid_bits -= (int)(c->codeword_lengths[i]);
						return (int)(i);
					}
					f.valid_bits = (int)(0);
					return (int)(-1);
				}
			}
			error(f, (int)(STBVorbisError.VORBIS_invalid_stream));
			f.valid_bits = (int)(0);
			return (int)(-1);
		}

		public static int codebook_decode_scalar(stb_vorbis f, Codebook* c)
		{
			int i = 0;
			if ((f.valid_bits) < (10))
				prep_huffman(f);
			i = (int)(f.acc & ((1 << 10) - 1));
			i = (int)(c->fast_huffman[i]);
			if ((i) >= (0))
			{
				f.acc >>= c->codeword_lengths[i];
				f.valid_bits -= (int)(c->codeword_lengths[i]);
				if ((f.valid_bits) < (0))
				{
					f.valid_bits = (int)(0);
					return (int)(-1);
				}
				return (int)(i);
			}

			return (int)(codebook_decode_scalar_raw(f, c));
		}

		public static int codebook_decode_start(stb_vorbis f, Codebook* c)
		{
			int z = (int)(-1);
			if ((c->lookup_type) == (0))
				error(f, (int)(STBVorbisError.VORBIS_invalid_stream));
			else
			{
				z = (int)(codebook_decode_scalar(f, c));
				if ((z) < (0))
				{
					if (f.bytes_in_seg == 0)
						if ((f.last_seg) != 0)
							return (int)(z);
					error(f, (int)(STBVorbisError.VORBIS_invalid_stream));
				}
			}

			return (int)(z);
		}

		public static int codebook_decode(stb_vorbis f, Codebook* c, float* output, int len)
		{
			int i = 0;
			int z = (int)(codebook_decode_start(f, c));
			if ((z) < (0))
				return (int)(0);
			if ((len) > (c->dimensions))
				len = (int)(c->dimensions);
			z *= (int)(c->dimensions);
			if ((c->sequence_p) != 0)
			{
				float last = (float)(0);
				for (i = (int)(0); (i) < (len); ++i)
				{
					float val = (float)((c->multiplicands[z + i]) + last);
					output[i] += (float)(val);
					last = (float)(val + c->minimum_value);
				}
			}
			else
			{
				float last = (float)(0);
				for (i = (int)(0); (i) < (len); ++i)
				{
					output[i] += (float)((c->multiplicands[z + i]) + last);
				}
			}

			return (int)(1);
		}

		public static int codebook_decode_step(stb_vorbis f, Codebook* c, float* output, int len, int step)
		{
			int i = 0;
			int z = (int)(codebook_decode_start(f, c));
			float last = (float)(0);
			if ((z) < (0))
				return (int)(0);
			if ((len) > (c->dimensions))
				len = (int)(c->dimensions);
			z *= (int)(c->dimensions);
			for (i = (int)(0); (i) < (len); ++i)
			{
				float val = (float)((c->multiplicands[z + i]) + last);
				output[i * step] += (float)(val);
				if ((c->sequence_p) != 0)
					last = (float)(val);
			}
			return (int)(1);
		}

		public static int codebook_decode_deinterleave_repeat(stb_vorbis f, Codebook* c, float** outputs, int ch, int* c_inter_p, int* p_inter_p, int len, int total_decode)
		{
			int c_inter = (int)(*c_inter_p);
			int p_inter = (int)(*p_inter_p);
			int i = 0;
			int z = 0;
			int effective = (int)(c->dimensions);
			if ((c->lookup_type) == (0))
				return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_stream)));
			while ((total_decode) > (0))
			{
				float last = (float)(0);
				z = (int)(codebook_decode_scalar(f, c));
				if ((z) < (0))
				{
					if (f.bytes_in_seg == 0)
						if ((f.last_seg) != 0)
							return (int)(0);
					return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_stream)));
				}
				if ((c_inter + p_inter * ch + effective) > (len * ch))
				{
					effective = (int)(len * ch - (p_inter * ch - c_inter));
				}
				{
					z *= (int)(c->dimensions);
					if ((c->sequence_p) != 0)
					{
						for (i = (int)(0); (i) < (effective); ++i)
						{
							float val = (float)((c->multiplicands[z + i]) + last);
							if ((outputs[c_inter]) != null)
								outputs[c_inter][p_inter] += (float)(val);
							if ((++c_inter) == (ch))
							{
								c_inter = (int)(0);
								++p_inter;
							}
							last = (float)(val);
						}
					}
					else
					{
						for (i = (int)(0); (i) < (effective); ++i)
						{
							float val = (float)((c->multiplicands[z + i]) + last);
							if ((outputs[c_inter]) != null)
								outputs[c_inter][p_inter] += (float)(val);
							if ((++c_inter) == (ch))
							{
								c_inter = (int)(0);
								++p_inter;
							}
						}
					}
				}
				total_decode -= (int)(effective);
			}
			*c_inter_p = (int)(c_inter);
			*p_inter_p = (int)(p_inter);
			return (int)(1);
		}

		public static int predict_point(int x, int x0, int x1, int y0, int y1)
		{
			int dy = (int)(y1 - y0);
			int adx = (int)(x1 - x0);
			int err = (int)(CRuntime.abs((int)(dy)) * (x - x0));
			int off = (int)(err / adx);
			return (int)((dy) < (0) ? y0 - off : y0 + off);
		}

		public static void draw_line(float* output, int x0, int y0, int x1, int y1, int n)
		{
			int dy = (int)(y1 - y0);
			int adx = (int)(x1 - x0);
			int ady = (int)(CRuntime.abs((int)(dy)));
			int _base_ = 0;
			int x = (int)(x0);
			int y = (int)(y0);
			int err = (int)(0);
			int sy = 0;
			_base_ = (int)(dy / adx);
			if ((dy) < (0))
				sy = (int)(_base_ - 1);
			else
				sy = (int)(_base_ + 1);
			ady -= (int)(CRuntime.abs((int)(_base_)) * adx);
			if ((x1) > (n))
				x1 = (int)(n);
			if ((x) < (x1))
			{
				output[x] *= (float)(StbVorbis.inverse_db_table[y]);
				for (++x; (x) < (x1); ++x)
				{
					err += (int)(ady);
					if ((err) >= (adx))
					{
						err -= (int)(adx);
						y += (int)(sy);
					}
					else
						y += (int)(_base_);
					output[x] *= (float)(StbVorbis.inverse_db_table[y]);
				}
			}

		}

		public static int residue_decode(stb_vorbis f, Codebook* book, float* target, int offset, int n, int rtype)
		{
			int k = 0;
			if ((rtype) == (0))
			{
				int step = (int)(n / book->dimensions);
				for (k = (int)(0); (k) < (step); ++k)
				{
					if (codebook_decode_step(f, book, target + offset + k, (int)(n - offset - k), (int)(step)) == 0)
						return (int)(0);
				}
			}
			else
			{
				for (k = (int)(0); (k) < (n);)
				{
					if (codebook_decode(f, book, target + offset, (int)(n - k)) == 0)
						return (int)(0);
					k += (int)(book->dimensions);
					offset += (int)(book->dimensions);
				}
			}

			return (int)(1);
		}

		public static void decode_residue(stb_vorbis f, float** residue_buffers, int ch, int n, int rn, byte* do_not_decode)
		{
			int i = 0;
			int j = 0;
			int pass = 0;
			Residue r = f.residue_config[rn];
			int rtype = (int)(f.residue_types[rn]);
			int c2 = (int)(r.classbook);
			int classwords = (int)(f.codebooks[c2].dimensions);
			uint actual_size = (uint)((rtype) == (2) ? n * 2 : n);
			uint limit_r_begin = (uint)((r.begin) < (actual_size) ? r.begin : actual_size);
			uint limit_r_end = (uint)((r.end) < (actual_size) ? r.end : actual_size);
			int n_read = (int)(limit_r_end - limit_r_begin);
			int part_read = (int)(n_read / r.part_size);
			int temp_alloc_point = (int)((f).temp_offset);
			byte*** part_classdata = (byte***)(make_block_array((f.alloc.alloc_buffer != null ? setup_temp_malloc(f, (int)(f.channels * (sizeof(void*) + (part_read * sizeof(byte*))))) : CRuntime.malloc((ulong)(f.channels * (sizeof(void*) + (part_read * sizeof(byte*)))))), (int)(f.channels), (int)(part_read * sizeof(byte*))));
			for (i = (int)(0); (i) < (ch); ++i)
			{
				if (do_not_decode[i] == 0)
					CRuntime.memset(residue_buffers[i], (int)(0), (ulong)(sizeof(float) * n));
			}
			if (((rtype) == (2)) && (ch != 1))
			{
				for (j = (int)(0); (j) < (ch); ++j)
				{
					if (do_not_decode[j] == 0)
						break;
				}
				if ((j) == (ch))
					goto done;
				for (pass = (int)(0); (pass) < (8); ++pass)
				{
					int pcount = (int)(0);
					int class_set = (int)(0);
					if ((ch) == (2))
					{
						while ((pcount) < (part_read))
						{
							int z = (int)(r.begin + pcount * r.part_size);
							int c_inter = (int)(z & 1);
							int p_inter = (int)(z >> 1);
							if ((pass) == (0))
							{
								Codebook* c = f.codebooks + r.classbook;
								int q = 0;
								q = (int)(codebook_decode_scalar(f, c));
								if ((c->sparse) != 0)
									q = (int)(c->sorted_values[q]);
								if ((q) == (-1))
									goto done;
								part_classdata[0][class_set] = r.classdata[q];
							}
							for (i = (int)(0); ((i) < (classwords)) && ((pcount) < (part_read)); ++i, ++pcount)
							{
								int z2 = (int)(r.begin + pcount * r.part_size);
								int c = (int)(part_classdata[0][class_set][i]);
								int b = (int)(r.residue_books[c, pass]);
								if ((b) >= (0))
								{
									Codebook* book = f.codebooks + b;
									if (codebook_decode_deinterleave_repeat(f, book, (float**)(residue_buffers), (int)(ch), &c_inter, &p_inter, (int)(n), (int)(r.part_size)) == 0)
										goto done;
								}
								else
								{
									z2 += (int)(r.part_size);
									c_inter = (int)(z2 & 1);
									p_inter = (int)(z2 >> 1);
								}
							}
							++class_set;
						}
					}
					else if ((ch) == (1))
					{
						while ((pcount) < (part_read))
						{
							int z = (int)(r.begin + pcount * r.part_size);
							int c_inter = (int)(0);
							int p_inter = (int)(z);
							if ((pass) == (0))
							{
								Codebook* c = f.codebooks + r.classbook;
								int q = 0;
								q = (int)(codebook_decode_scalar(f, c));
								if ((c->sparse) != 0)
									q = (int)(c->sorted_values[q]);
								if ((q) == (-1))
									goto done;
								part_classdata[0][class_set] = r.classdata[q];
							}
							for (i = (int)(0); ((i) < (classwords)) && ((pcount) < (part_read)); ++i, ++pcount)
							{
								int z2 = (int)(r.begin + pcount * r.part_size);
								int c = (int)(part_classdata[0][class_set][i]);
								int b = (int)(r.residue_books[c, pass]);
								if ((b) >= (0))
								{
									Codebook* book = f.codebooks + b;
									if (codebook_decode_deinterleave_repeat(f, book, (float**)(residue_buffers), (int)(ch), &c_inter, &p_inter, (int)(n), (int)(r.part_size)) == 0)
										goto done;
								}
								else
								{
									z2 += (int)(r.part_size);
									c_inter = (int)(0);
									p_inter = (int)(z2);
								}
							}
							++class_set;
						}
					}
					else
					{
						while ((pcount) < (part_read))
						{
							int z = (int)(r.begin + pcount * r.part_size);
							int c_inter = (int)(z % ch);
							int p_inter = (int)(z / ch);
							if ((pass) == (0))
							{
								Codebook* c = f.codebooks + r.classbook;
								int q = 0;
								q = (int)(codebook_decode_scalar(f, c));
								if ((c->sparse) != 0)
									q = (int)(c->sorted_values[q]);
								if ((q) == (-1))
									goto done;
								part_classdata[0][class_set] = r.classdata[q];
							}
							for (i = (int)(0); ((i) < (classwords)) && ((pcount) < (part_read)); ++i, ++pcount)
							{
								int z2 = (int)(r.begin + pcount * r.part_size);
								int c = (int)(part_classdata[0][class_set][i]);
								int b = (int)(r.residue_books[c, pass]);
								if ((b) >= (0))
								{
									Codebook* book = f.codebooks + b;
									if (codebook_decode_deinterleave_repeat(f, book, (float**)(residue_buffers), (int)(ch), &c_inter, &p_inter, (int)(n), (int)(r.part_size)) == 0)
										goto done;
								}
								else
								{
									z2 += (int)(r.part_size);
									c_inter = (int)(z2 % ch);
									p_inter = (int)(z2 / ch);
								}
							}
							++class_set;
						}
					}
				}
				goto done;
			}

			for (pass = (int)(0); (pass) < (8); ++pass)
			{
				int pcount = (int)(0);
				int class_set = (int)(0);
				while ((pcount) < (part_read))
				{
					if ((pass) == (0))
					{
						for (j = (int)(0); (j) < (ch); ++j)
						{
							if (do_not_decode[j] == 0)
							{
								Codebook* c = f.codebooks + r.classbook;
								int temp = 0;
								temp = (int)(codebook_decode_scalar(f, c));
								if ((c->sparse) != 0)
									temp = (int)(c->sorted_values[temp]);
								if ((temp) == (-1))
									goto done;
								part_classdata[j][class_set] = r.classdata[temp];
							}
						}
					}
					for (i = (int)(0); ((i) < (classwords)) && ((pcount) < (part_read)); ++i, ++pcount)
					{
						for (j = (int)(0); (j) < (ch); ++j)
						{
							if (do_not_decode[j] == 0)
							{
								int c = (int)(part_classdata[j][class_set][i]);
								int b = (int)(r.residue_books[c, pass]);
								if ((b) >= (0))
								{
									float* target = residue_buffers[j];
									int offset = (int)(r.begin + pcount * r.part_size);
									int n2 = (int)(r.part_size);
									Codebook* book = f.codebooks + b;
									if (residue_decode(f, book, target, (int)(offset), (int)(n2), (int)(rtype)) == 0)
										goto done;
								}
							}
						}
					}
					++class_set;
				}
			}
		done:
			;

			CRuntime.free(part_classdata);
			(f).temp_offset = (int)(temp_alloc_point);
		}

		public static void imdct_step3_iter0_loop(int n, float* e, int i_off, int k_off, float* A)
		{
			float* ee0 = e + i_off;
			float* ee2 = ee0 + k_off;
			int i = 0;
			for (i = (int)(n >> 2); (i) > (0); --i)
			{
				float k00_20 = 0;
				float k01_21 = 0;
				k00_20 = (float)(ee0[0] - ee2[0]);
				k01_21 = (float)(ee0[-1] - ee2[-1]);
				ee0[0] += (float)(ee2[0]);
				ee0[-1] += (float)(ee2[-1]);
				ee2[0] = (float)(k00_20 * A[0] - k01_21 * A[1]);
				ee2[-1] = (float)(k01_21 * A[0] + k00_20 * A[1]);
				A += 8;
				k00_20 = (float)(ee0[-2] - ee2[-2]);
				k01_21 = (float)(ee0[-3] - ee2[-3]);
				ee0[-2] += (float)(ee2[-2]);
				ee0[-3] += (float)(ee2[-3]);
				ee2[-2] = (float)(k00_20 * A[0] - k01_21 * A[1]);
				ee2[-3] = (float)(k01_21 * A[0] + k00_20 * A[1]);
				A += 8;
				k00_20 = (float)(ee0[-4] - ee2[-4]);
				k01_21 = (float)(ee0[-5] - ee2[-5]);
				ee0[-4] += (float)(ee2[-4]);
				ee0[-5] += (float)(ee2[-5]);
				ee2[-4] = (float)(k00_20 * A[0] - k01_21 * A[1]);
				ee2[-5] = (float)(k01_21 * A[0] + k00_20 * A[1]);
				A += 8;
				k00_20 = (float)(ee0[-6] - ee2[-6]);
				k01_21 = (float)(ee0[-7] - ee2[-7]);
				ee0[-6] += (float)(ee2[-6]);
				ee0[-7] += (float)(ee2[-7]);
				ee2[-6] = (float)(k00_20 * A[0] - k01_21 * A[1]);
				ee2[-7] = (float)(k01_21 * A[0] + k00_20 * A[1]);
				A += 8;
				ee0 -= 8;
				ee2 -= 8;
			}
		}

		public static void imdct_step3_inner_r_loop(int lim, float* e, int d0, int k_off, float* A, int k1)
		{
			int i = 0;
			float k00_20 = 0;
			float k01_21 = 0;
			float* e0 = e + d0;
			float* e2 = e0 + k_off;
			for (i = (int)(lim >> 2); (i) > (0); --i)
			{
				k00_20 = (float)(e0[-0] - e2[-0]);
				k01_21 = (float)(e0[-1] - e2[-1]);
				e0[-0] += (float)(e2[-0]);
				e0[-1] += (float)(e2[-1]);
				e2[-0] = (float)((k00_20) * A[0] - (k01_21) * A[1]);
				e2[-1] = (float)((k01_21) * A[0] + (k00_20) * A[1]);
				A += k1;
				k00_20 = (float)(e0[-2] - e2[-2]);
				k01_21 = (float)(e0[-3] - e2[-3]);
				e0[-2] += (float)(e2[-2]);
				e0[-3] += (float)(e2[-3]);
				e2[-2] = (float)((k00_20) * A[0] - (k01_21) * A[1]);
				e2[-3] = (float)((k01_21) * A[0] + (k00_20) * A[1]);
				A += k1;
				k00_20 = (float)(e0[-4] - e2[-4]);
				k01_21 = (float)(e0[-5] - e2[-5]);
				e0[-4] += (float)(e2[-4]);
				e0[-5] += (float)(e2[-5]);
				e2[-4] = (float)((k00_20) * A[0] - (k01_21) * A[1]);
				e2[-5] = (float)((k01_21) * A[0] + (k00_20) * A[1]);
				A += k1;
				k00_20 = (float)(e0[-6] - e2[-6]);
				k01_21 = (float)(e0[-7] - e2[-7]);
				e0[-6] += (float)(e2[-6]);
				e0[-7] += (float)(e2[-7]);
				e2[-6] = (float)((k00_20) * A[0] - (k01_21) * A[1]);
				e2[-7] = (float)((k01_21) * A[0] + (k00_20) * A[1]);
				e0 -= 8;
				e2 -= 8;
				A += k1;
			}
		}

		public static void imdct_step3_inner_s_loop(int n, float* e, int i_off, int k_off, float* A, int a_off, int k0)
		{
			int i = 0;
			float A0 = (float)(A[0]);
			float A1 = (float)(A[0 + 1]);
			float A2 = (float)(A[0 + a_off]);
			float A3 = (float)(A[0 + a_off + 1]);
			float A4 = (float)(A[0 + a_off * 2 + 0]);
			float A5 = (float)(A[0 + a_off * 2 + 1]);
			float A6 = (float)(A[0 + a_off * 3 + 0]);
			float A7 = (float)(A[0 + a_off * 3 + 1]);
			float k00 = 0;
			float k11 = 0;
			float* ee0 = e + i_off;
			float* ee2 = ee0 + k_off;
			for (i = (int)(n); (i) > (0); --i)
			{
				k00 = (float)(ee0[0] - ee2[0]);
				k11 = (float)(ee0[-1] - ee2[-1]);
				ee0[0] = (float)(ee0[0] + ee2[0]);
				ee0[-1] = (float)(ee0[-1] + ee2[-1]);
				ee2[0] = (float)((k00) * A0 - (k11) * A1);
				ee2[-1] = (float)((k11) * A0 + (k00) * A1);
				k00 = (float)(ee0[-2] - ee2[-2]);
				k11 = (float)(ee0[-3] - ee2[-3]);
				ee0[-2] = (float)(ee0[-2] + ee2[-2]);
				ee0[-3] = (float)(ee0[-3] + ee2[-3]);
				ee2[-2] = (float)((k00) * A2 - (k11) * A3);
				ee2[-3] = (float)((k11) * A2 + (k00) * A3);
				k00 = (float)(ee0[-4] - ee2[-4]);
				k11 = (float)(ee0[-5] - ee2[-5]);
				ee0[-4] = (float)(ee0[-4] + ee2[-4]);
				ee0[-5] = (float)(ee0[-5] + ee2[-5]);
				ee2[-4] = (float)((k00) * A4 - (k11) * A5);
				ee2[-5] = (float)((k11) * A4 + (k00) * A5);
				k00 = (float)(ee0[-6] - ee2[-6]);
				k11 = (float)(ee0[-7] - ee2[-7]);
				ee0[-6] = (float)(ee0[-6] + ee2[-6]);
				ee0[-7] = (float)(ee0[-7] + ee2[-7]);
				ee2[-6] = (float)((k00) * A6 - (k11) * A7);
				ee2[-7] = (float)((k11) * A6 + (k00) * A7);
				ee0 -= k0;
				ee2 -= k0;
			}
		}

		public static void iter_54(float* z)
		{
			float k00 = 0;
			float k11 = 0;
			float k22 = 0;
			float k33 = 0;
			float y0 = 0;
			float y1 = 0;
			float y2 = 0;
			float y3 = 0;
			k00 = (float)(z[0] - z[-4]);
			y0 = (float)(z[0] + z[-4]);
			y2 = (float)(z[-2] + z[-6]);
			k22 = (float)(z[-2] - z[-6]);
			z[-0] = (float)(y0 + y2);
			z[-2] = (float)(y0 - y2);
			k33 = (float)(z[-3] - z[-7]);
			z[-4] = (float)(k00 + k33);
			z[-6] = (float)(k00 - k33);
			k11 = (float)(z[-1] - z[-5]);
			y1 = (float)(z[-1] + z[-5]);
			y3 = (float)(z[-3] + z[-7]);
			z[-1] = (float)(y1 + y3);
			z[-3] = (float)(y1 - y3);
			z[-5] = (float)(k11 - k22);
			z[-7] = (float)(k11 + k22);
		}

		public static void imdct_step3_inner_s_loop_ld654(int n, float* e, int i_off, float* A, int base_n)
		{
			int a_off = (int)(base_n >> 3);
			float A2 = (float)(A[0 + a_off]);
			float* z = e + i_off;
			float* _base_ = z - 16 * n;
			while ((z) > (_base_))
			{
				float k00 = 0;
				float k11 = 0;
				k00 = (float)(z[-0] - z[-8]);
				k11 = (float)(z[-1] - z[-9]);
				z[-0] = (float)(z[-0] + z[-8]);
				z[-1] = (float)(z[-1] + z[-9]);
				z[-8] = (float)(k00);
				z[-9] = (float)(k11);
				k00 = (float)(z[-2] - z[-10]);
				k11 = (float)(z[-3] - z[-11]);
				z[-2] = (float)(z[-2] + z[-10]);
				z[-3] = (float)(z[-3] + z[-11]);
				z[-10] = (float)((k00 + k11) * A2);
				z[-11] = (float)((k11 - k00) * A2);
				k00 = (float)(z[-12] - z[-4]);
				k11 = (float)(z[-5] - z[-13]);
				z[-4] = (float)(z[-4] + z[-12]);
				z[-5] = (float)(z[-5] + z[-13]);
				z[-12] = (float)(k11);
				z[-13] = (float)(k00);
				k00 = (float)(z[-14] - z[-6]);
				k11 = (float)(z[-7] - z[-15]);
				z[-6] = (float)(z[-6] + z[-14]);
				z[-7] = (float)(z[-7] + z[-15]);
				z[-14] = (float)((k00 + k11) * A2);
				z[-15] = (float)((k00 - k11) * A2);
				iter_54(z);
				iter_54(z - 8);
				z -= 16;
			}
		}

		public static void inverse_mdct(float* buffer, int n, stb_vorbis f, int blocktype)
		{
			int n2 = (int)(n >> 1);
			int n4 = (int)(n >> 2);
			int n8 = (int)(n >> 3);
			int l = 0;
			int ld = 0;
			int save_point = (int)((f).temp_offset);
			float* buf2 = (float*)(f.alloc.alloc_buffer != null ? setup_temp_malloc(f, (int)(n2 * sizeof(float))) : CRuntime.malloc((ulong)(n2 * sizeof(float))));
			float* u = null;
			float* v = null;
			float* A = f.A[blocktype];
			{
				float* d;
				float* e;
				float* AA;
				float* e_stop;
				d = &buf2[n2 - 2];
				AA = A;
				e = &buffer[0];
				e_stop = &buffer[n2];
				while (e != e_stop)
				{
					d[1] = (float)(e[0] * AA[0] - e[2] * AA[1]);
					d[0] = (float)(e[0] * AA[1] + e[2] * AA[0]);
					d -= 2;
					AA += 2;
					e += 4;
				}
				e = &buffer[n2 - 3];
				while ((d) >= (buf2))
				{
					d[1] = (float)(-e[2] * AA[0] - -e[0] * AA[1]);
					d[0] = (float)(-e[2] * AA[1] + -e[0] * AA[0]);
					d -= 2;
					AA += 2;
					e -= 4;
				}
			}

			u = buffer;
			v = buf2;
			{
				float* AA = &A[n2 - 8];
				float* d0;
				float* d1;
				float* e0;
				float* e1;
				e0 = &v[n4];
				e1 = &v[0];
				d0 = &u[n4];
				d1 = &u[0];
				while ((AA) >= (A))
				{
					float v40_20 = 0;
					float v41_21 = 0;
					v41_21 = (float)(e0[1] - e1[1]);
					v40_20 = (float)(e0[0] - e1[0]);
					d0[1] = (float)(e0[1] + e1[1]);
					d0[0] = (float)(e0[0] + e1[0]);
					d1[1] = (float)(v41_21 * AA[4] - v40_20 * AA[5]);
					d1[0] = (float)(v40_20 * AA[4] + v41_21 * AA[5]);
					v41_21 = (float)(e0[3] - e1[3]);
					v40_20 = (float)(e0[2] - e1[2]);
					d0[3] = (float)(e0[3] + e1[3]);
					d0[2] = (float)(e0[2] + e1[2]);
					d1[3] = (float)(v41_21 * AA[0] - v40_20 * AA[1]);
					d1[2] = (float)(v40_20 * AA[0] + v41_21 * AA[1]);
					AA -= 8;
					d0 += 4;
					d1 += 4;
					e0 += 4;
					e1 += 4;
				}
			}

			ld = (int)(ilog((int)(n)) - 1);
			imdct_step3_iter0_loop((int)(n >> 4), u, (int)(n2 - 1 - n4 * 0), (int)(-(n >> 3)), A);
			imdct_step3_iter0_loop((int)(n >> 4), u, (int)(n2 - 1 - n4 * 1), (int)(-(n >> 3)), A);
			imdct_step3_inner_r_loop((int)(n >> 5), u, (int)(n2 - 1 - n8 * 0), (int)(-(n >> 4)), A, (int)(16));
			imdct_step3_inner_r_loop((int)(n >> 5), u, (int)(n2 - 1 - n8 * 1), (int)(-(n >> 4)), A, (int)(16));
			imdct_step3_inner_r_loop((int)(n >> 5), u, (int)(n2 - 1 - n8 * 2), (int)(-(n >> 4)), A, (int)(16));
			imdct_step3_inner_r_loop((int)(n >> 5), u, (int)(n2 - 1 - n8 * 3), (int)(-(n >> 4)), A, (int)(16));
			l = (int)(2);
			for (; (l) < ((ld - 3) >> 1); ++l)
			{
				int k0 = (int)(n >> (l + 2));
				int k0_2 = (int)(k0 >> 1);
				int lim = (int)(1 << (l + 1));
				int i = 0;
				for (i = (int)(0); (i) < (lim); ++i)
				{
					imdct_step3_inner_r_loop((int)(n >> (l + 4)), u, (int)(n2 - 1 - k0 * i), (int)(-k0_2), A, (int)(1 << (l + 3)));
				}
			}
			for (; (l) < (ld - 6); ++l)
			{
				int k0 = (int)(n >> (l + 2));
				int k1 = (int)(1 << (l + 3));
				int k0_2 = (int)(k0 >> 1);
				int rlim = (int)(n >> (l + 6));
				int r = 0;
				int lim = (int)(1 << (l + 1));
				int i_off = 0;
				float* A0 = A;
				i_off = (int)(n2 - 1);
				for (r = (int)(rlim); (r) > (0); --r)
				{
					imdct_step3_inner_s_loop((int)(lim), u, (int)(i_off), (int)(-k0_2), A0, (int)(k1), (int)(k0));
					A0 += k1 * 4;
					i_off -= (int)(8);
				}
			}
			imdct_step3_inner_s_loop_ld654((int)(n >> 5), u, (int)(n2 - 1), A, (int)(n));
			{
				ushort* bitrev = f.bit_reverse[blocktype];
				float* d0 = &v[n4 - 4];
				float* d1 = &v[n2 - 4];
				while ((d0) >= (v))
				{
					int k4 = 0;
					k4 = (int)(bitrev[0]);
					d1[3] = (float)(u[k4 + 0]);
					d1[2] = (float)(u[k4 + 1]);
					d0[3] = (float)(u[k4 + 2]);
					d0[2] = (float)(u[k4 + 3]);
					k4 = (int)(bitrev[1]);
					d1[1] = (float)(u[k4 + 0]);
					d1[0] = (float)(u[k4 + 1]);
					d0[1] = (float)(u[k4 + 2]);
					d0[0] = (float)(u[k4 + 3]);
					d0 -= 4;
					d1 -= 4;
					bitrev += 2;
				}
			}

			{
				float* C = f.C[blocktype];
				float* d;
				float* e;
				d = v;
				e = v + n2 - 4;
				while ((d) < (e))
				{
					float a02 = 0;
					float a11 = 0;
					float b0 = 0;
					float b1 = 0;
					float b2 = 0;
					float b3 = 0;
					a02 = (float)(d[0] - e[2]);
					a11 = (float)(d[1] + e[3]);
					b0 = (float)(C[1] * a02 + C[0] * a11);
					b1 = (float)(C[1] * a11 - C[0] * a02);
					b2 = (float)(d[0] + e[2]);
					b3 = (float)(d[1] - e[3]);
					d[0] = (float)(b2 + b0);
					d[1] = (float)(b3 + b1);
					e[2] = (float)(b2 - b0);
					e[3] = (float)(b1 - b3);
					a02 = (float)(d[2] - e[0]);
					a11 = (float)(d[3] + e[1]);
					b0 = (float)(C[3] * a02 + C[2] * a11);
					b1 = (float)(C[3] * a11 - C[2] * a02);
					b2 = (float)(d[2] + e[0]);
					b3 = (float)(d[3] - e[1]);
					d[2] = (float)(b2 + b0);
					d[3] = (float)(b3 + b1);
					e[0] = (float)(b2 - b0);
					e[1] = (float)(b1 - b3);
					C += 4;
					d += 4;
					e -= 4;
				}
			}

			{
				float* d0;
				float* d1;
				float* d2;
				float* d3;
				float* B = f.B[blocktype] + n2 - 8;
				float* e = buf2 + n2 - 8;
				d0 = &buffer[0];
				d1 = &buffer[n2 - 4];
				d2 = &buffer[n2];
				d3 = &buffer[n - 4];
				while ((e) >= (v))
				{
					float p0 = 0;
					float p1 = 0;
					float p2 = 0;
					float p3 = 0;
					p3 = (float)(e[6] * B[7] - e[7] * B[6]);
					p2 = (float)(-e[6] * B[6] - e[7] * B[7]);
					d0[0] = (float)(p3);
					d1[3] = (float)(-p3);
					d2[0] = (float)(p2);
					d3[3] = (float)(p2);
					p1 = (float)(e[4] * B[5] - e[5] * B[4]);
					p0 = (float)(-e[4] * B[4] - e[5] * B[5]);
					d0[1] = (float)(p1);
					d1[2] = (float)(-p1);
					d2[1] = (float)(p0);
					d3[2] = (float)(p0);
					p3 = (float)(e[2] * B[3] - e[3] * B[2]);
					p2 = (float)(-e[2] * B[2] - e[3] * B[3]);
					d0[2] = (float)(p3);
					d1[1] = (float)(-p3);
					d2[2] = (float)(p2);
					d3[1] = (float)(p2);
					p1 = (float)(e[0] * B[1] - e[1] * B[0]);
					p0 = (float)(-e[0] * B[0] - e[1] * B[1]);
					d0[3] = (float)(p1);
					d1[0] = (float)(-p1);
					d2[3] = (float)(p0);
					d3[0] = (float)(p0);
					B -= 8;
					e -= 8;
					d0 += 4;
					d2 += 4;
					d1 -= 4;
					d3 -= 4;
				}
			}

			CRuntime.free(buf2);
			(f).temp_offset = (int)(save_point);
		}

		public static float* get_window(stb_vorbis f, int len)
		{
			len <<= 1;
			if ((len) == (f.blocksize_0))
				return f.window[0];
			if ((len) == (f.blocksize_1))
				return f.window[1];
			return null;
		}

		public static int do_floor(stb_vorbis f, Mapping* map, int i, int n, float* target, short* finalY, byte* step2_flag)
		{
			int n2 = (int)(n >> 1);
			int s = (int)(map->chan[i].mux);
			int floor = 0;
			floor = (int)(map->submap_floor[s]);
			if ((f.floor_types[floor]) == (0))
			{
				return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_stream)));
			}
			else
			{
				Floor1* g = &f.floor_config[floor].floor1;
				int j = 0;
				int q = 0;
				int lx = (int)(0);
				int ly = (int)(finalY[0] * g->floor1_multiplier);
				for (q = (int)(1); (q) < (g->values); ++q)
				{
					j = (int)(g->sorted_order[q]);
					if ((finalY[j]) >= (0))
					{
						int hy = (int)(finalY[j] * g->floor1_multiplier);
						int hx = (int)(g->Xlist[j]);
						if (lx != hx)
							draw_line(target, (int)(lx), (int)(ly), (int)(hx), (int)(hy), (int)(n2));
						lx = (int)(hx);
						ly = (int)(hy);
					}
				}
				if ((lx) < (n2))
				{
					for (j = (int)(lx); (j) < (n2); ++j)
					{
						target[j] *= (float)(StbVorbis.inverse_db_table[ly]);
					}
				}
			}

			return (int)(1);
		}

		public static int vorbis_decode_initial(stb_vorbis f, int* p_left_start, int* p_left_end, int* p_right_start, int* p_right_end, int* mode)
		{
			Mode* m;
			int i = 0;
			int n = 0;
			int prev = 0;
			int next = 0;
			int window_center = 0;
			f.channel_buffer_start = (int)(f.channel_buffer_end = (int)(0));
		retry:
			;
			if ((f.eof) != 0)
				return (int)(0);
			if (maybe_start_packet(f) == 0)
				return (int)(0);
			if (get_bits(f, (int)(1)) != 0)
			{
				if (((f).push_mode) != 0)
					return (int)(error(f, (int)(STBVorbisError.VORBIS_bad_packet_type)));
				while ((-1) != get8_packet(f))
				{
				}
				goto retry;
			}

			i = (int)(get_bits(f, (int)(ilog((int)(f.mode_count - 1)))));
			if ((i) == (-1))
				return (int)(0);
			if ((i) >= (f.mode_count))
				return (int)(0);
			*mode = (int)(i);
			m = (Mode*)f.mode_config + i;
			if ((m->blockflag) != 0)
			{
				n = (int)(f.blocksize_1);
				prev = (int)(get_bits(f, (int)(1)));
				next = (int)(get_bits(f, (int)(1)));
			}
			else
			{
				prev = (int)(next = (int)(0));
				n = (int)(f.blocksize_0);
			}

			window_center = (int)(n >> 1);
			if (((m->blockflag) != 0) && (prev == 0))
			{
				*p_left_start = (int)((n - f.blocksize_0) >> 2);
				*p_left_end = (int)((n + f.blocksize_0) >> 2);
			}
			else
			{
				*p_left_start = (int)(0);
				*p_left_end = (int)(window_center);
			}

			if (((m->blockflag) != 0) && (next == 0))
			{
				*p_right_start = (int)((n * 3 - f.blocksize_0) >> 2);
				*p_right_end = (int)((n * 3 + f.blocksize_0) >> 2);
			}
			else
			{
				*p_right_start = (int)(window_center);
				*p_right_end = (int)(n);
			}

			return (int)(1);
		}

		public static int vorbis_decode_packet_rest(stb_vorbis f, int* len, Mode* mode, int left_start, int left_end, int right_start, int right_end, int* p_left)
		{
			Mapping* map;
			int i = 0;
			int j = 0;
			int k = 0;
			int n = 0;
			int n2 = 0;
			int* zero_channel = stackalloc int[256];
			int* really_zero_channel = stackalloc int[256];
			n = (int)(f.blocksize[mode->blockflag]);
			map = &f.mapping[mode->mapping];
			n2 = (int)(n >> 1);
			for (i = (int)(0); (i) < (f.channels); ++i)
			{
				int s = (int)(map->chan[i].mux);
				int floor = 0;
				zero_channel[i] = (int)(0);
				floor = (int)(map->submap_floor[s]);
				if ((f.floor_types[floor]) == (0))
				{
					return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_stream)));
				}
				else
				{
					Floor1* g = &f.floor_config[floor].floor1;
					if ((get_bits(f, (int)(1))) != 0)
					{
						short* finalY;
						byte* step2_flag = stackalloc byte[256];
						int* range_list = stackalloc int[4];
						range_list[0] = (int)(256);
						range_list[1] = (int)(128);
						range_list[2] = (int)(86);
						range_list[3] = (int)(64);
						int range = (int)(range_list[g->floor1_multiplier - 1]);
						int offset = (int)(2);
						finalY = f.finalY[i];
						finalY[0] = (short)(get_bits(f, (int)(ilog((int)(range)) - 1)));
						finalY[1] = (short)(get_bits(f, (int)(ilog((int)(range)) - 1)));
						for (j = (int)(0); (j) < (g->partitions); ++j)
						{
							int pclass = (int)(g->partition_class_list[j]);
							int cdim = (int)(g->class_dimensions[pclass]);
							int cbits = (int)(g->class_subclasses[pclass]);
							int csub = (int)((1 << cbits) - 1);
							int cval = (int)(0);
							if ((cbits) != 0)
							{
								Codebook* c = f.codebooks + g->class_masterbooks[pclass];
								cval = (int)(codebook_decode_scalar(f, c));
								if ((c->sparse) != 0)
									cval = (int)(c->sorted_values[cval]);
							}
							for (k = (int)(0); (k) < (cdim); ++k)
							{
								int book = (int)(g->subclass_books[pclass * 8 + (cval & csub)]);
								cval = (int)(cval >> cbits);
								if ((book) >= (0))
								{
									int temp = 0;
									Codebook* c = f.codebooks + book;
									temp = (int)(codebook_decode_scalar(f, c));
									if ((c->sparse) != 0)
										temp = (int)(c->sorted_values[temp]);
									finalY[offset++] = (short)(temp);
								}
								else
									finalY[offset++] = (short)(0);
							}
						}
						if ((f.valid_bits) == (-1))
						{
							zero_channel[i] = (int)(1);
							goto error;
						}
						step2_flag[0] = (byte)(step2_flag[1] = (byte)(1));
						for (j = (int)(2); (j) < (g->values); ++j)
						{
							int low = 0;
							int high = 0;
							int pred = 0;
							int highroom = 0;
							int lowroom = 0;
							int room = 0;
							int val = 0;
							low = (int)(g->neighbors[j * 2 + 0]);
							high = (int)(g->neighbors[j * 2 + 1]);
							pred = (int)(predict_point((int)(g->Xlist[j]), (int)(g->Xlist[low]), (int)(g->Xlist[high]), (int)(finalY[low]), (int)(finalY[high])));
							val = (int)(finalY[j]);
							highroom = (int)(range - pred);
							lowroom = (int)(pred);
							if ((highroom) < (lowroom))
								room = (int)(highroom * 2);
							else
								room = (int)(lowroom * 2);
							if ((val) != 0)
							{
								step2_flag[low] = (byte)(step2_flag[high] = (byte)(1));
								step2_flag[j] = (byte)(1);
								if ((val) >= (room))
									if ((highroom) > (lowroom))
										finalY[j] = (short)(val - lowroom + pred);
									else
										finalY[j] = (short)(pred - val + highroom - 1);
								else if ((val & 1) != 0)
									finalY[j] = (short)(pred - ((val + 1) >> 1));
								else
									finalY[j] = (short)(pred + (val >> 1));
							}
							else
							{
								step2_flag[j] = (byte)(0);
								finalY[j] = (short)(pred);
							}
						}
						for (j = (int)(0); (j) < (g->values); ++j)
						{
							if (step2_flag[j] == 0)
								finalY[j] = (short)(-1);
						}
					}
					else
					{
						zero_channel[i] = (int)(1);
					}
				error:
					;
				}
			}
			CRuntime.memcpy(really_zero_channel, zero_channel, (ulong)(sizeof(int) * f.channels));
			for (i = (int)(0); (i) < (map->coupling_steps); ++i)
			{
				if ((zero_channel[map->chan[i].magnitude] == 0) || (zero_channel[map->chan[i].angle] == 0))
				{
					zero_channel[map->chan[i].magnitude] = (int)(zero_channel[map->chan[i].angle] = (int)(0));
				}
			}
			for (i = (int)(0); (i) < (map->submaps); ++i)
			{
				float** residue_buffers = stackalloc float*[16];
				int r = 0;
				byte* do_not_decode = stackalloc byte[256];
				int ch = (int)(0);
				for (j = (int)(0); (j) < (f.channels); ++j)
				{
					if ((map->chan[j].mux) == (i))
					{
						if ((zero_channel[j]) != 0)
						{
							do_not_decode[ch] = (byte)(1);
							residue_buffers[ch] = null;
						}
						else
						{
							do_not_decode[ch] = (byte)(0);
							residue_buffers[ch] = f.channel_buffers[j];
						}
						++ch;
					}
				}
				r = (int)(map->submap_residue[i]);
				decode_residue(f, residue_buffers, (int)(ch), (int)(n2), (int)(r), do_not_decode);
			}
			for (i = (int)(map->coupling_steps - 1); (i) >= (0); --i)
			{
				int n3 = (int)(n >> 1);
				float* m = f.channel_buffers[map->chan[i].magnitude];
				float* a = f.channel_buffers[map->chan[i].angle];
				for (j = (int)(0); (j) < (n3); ++j)
				{
					float a2 = 0;
					float m2 = 0;
					if ((m[j]) > (0))
						if ((a[j]) > (0))
						{
							m2 = (float)(m[j]);
							a2 = (float)(m[j] - a[j]);
						}
						else
						{
							a2 = (float)(m[j]);
							m2 = (float)(m[j] + a[j]);
						}
					else if ((a[j]) > (0))
					{
						m2 = (float)(m[j]);
						a2 = (float)(m[j] + a[j]);
					}
					else
					{
						a2 = (float)(m[j]);
						m2 = (float)(m[j] - a[j]);
					}
					m[j] = (float)(m2);
					a[j] = (float)(a2);
				}
			}
			for (i = (int)(0); (i) < (f.channels); ++i)
			{
				if ((really_zero_channel[i]) != 0)
				{
					CRuntime.memset(f.channel_buffers[i], (int)(0), (ulong)(sizeof(float) * n2));
				}
				else
				{
					do_floor(f, map, (int)(i), (int)(n), f.channel_buffers[i], f.finalY[i], null);
				}
			}
			for (i = (int)(0); (i) < (f.channels); ++i)
			{
				inverse_mdct(f.channel_buffers[i], (int)(n), f, (int)(mode->blockflag));
			}
			flush_packet(f);
			if ((f.first_decode) != 0)
			{
				f.current_loc = (uint)(-n2);
				f.discard_samples_deferred = (int)(n - right_end);
				f.current_loc_valid = (int)(1);
				f.first_decode = (byte)(0);
			}
			else if ((f.discard_samples_deferred) != 0)
			{
				if ((f.discard_samples_deferred) >= (right_start - left_start))
				{
					f.discard_samples_deferred -= (int)(right_start - left_start);
					left_start = (int)(right_start);
					*p_left = (int)(left_start);
				}
				else
				{
					left_start += (int)(f.discard_samples_deferred);
					*p_left = (int)(left_start);
					f.discard_samples_deferred = (int)(0);
				}
			}
			else if (((f.previous_length) == (0)) && ((f.current_loc_valid) != 0))
			{
			}

			if ((f.last_seg_which) == (f.end_seg_with_known_loc))
			{
				if (((f.current_loc_valid) != 0) && ((f.page_flag & 4) != 0))
				{
					uint current_end = (uint)(f.known_loc_for_packet);
					if ((current_end) < (f.current_loc + (right_end - left_start)))
					{
						if ((current_end) < (f.current_loc))
						{
							*len = (int)(0);
						}
						else
						{
							*len = (int)(current_end - f.current_loc);
						}
						*len += (int)(left_start);
						if ((*len) > (right_end))
							*len = (int)(right_end);
						f.current_loc += (uint)(*len);
						return (int)(1);
					}
				}
				f.current_loc = (uint)(f.known_loc_for_packet - (n2 - left_start));
				f.current_loc_valid = (int)(1);
			}

			if ((f.current_loc_valid) != 0)
				f.current_loc += (uint)(right_start - left_start);
			*len = (int)(right_end);
			return (int)(1);
		}

		public static int vorbis_decode_packet(stb_vorbis f, int* len, int* p_left, int* p_right)
		{
			int mode = 0;
			int left_end = 0;
			int right_end = 0;
			if (vorbis_decode_initial(f, p_left, &left_end, p_right, &right_end, &mode) == 0)
				return (int)(0);
			return (int)(vorbis_decode_packet_rest(f, len, (Mode*)f.mode_config + mode, (int)(*p_left), (int)(left_end), (int)(*p_right), (int)(right_end), p_left));
		}

		public static int vorbis_finish_frame(stb_vorbis f, int len, int left, int right)
		{
			int prev = 0;
			int i = 0;
			int j = 0;
			if ((f.previous_length) != 0)
			{
				int i2 = 0;
				int j2 = 0;
				int n = (int)(f.previous_length);
				float* w = get_window(f, (int)(n));
				for (i2 = (int)(0); (i2) < (f.channels); ++i2)
				{
					for (j2 = (int)(0); (j2) < (n); ++j2)
					{
						f.channel_buffers[i2][left + j2] = (float)(f.channel_buffers[i2][left + j2] * w[j2] + f.previous_window[i2][j2] * w[n - 1 - j2]);
					}
				}
			}

			prev = (int)(f.previous_length);
			f.previous_length = (int)(len - right);
			for (i = (int)(0); (i) < (f.channels); ++i)
			{
				for (j = (int)(0); (right + j) < (len); ++j)
				{
					f.previous_window[i][j] = (float)(f.channel_buffers[i][right + j]);
				}
			}
			if (prev == 0)
				return (int)(0);
			if ((len) < (right))
				right = (int)(len);
			f.samples_output += (uint)(right - left);
			return (int)(right - left);
		}

		public static int vorbis_pump_first_frame(stb_vorbis f)
		{
			int len = 0;
			int right = 0;
			int left = 0;
			int res = 0;
			res = (int)(vorbis_decode_packet(f, &len, &left, &right));
			if ((res) != 0)
				vorbis_finish_frame(f, (int)(len), (int)(left), (int)(right));
			return (int)(res);
		}

		public static int is_whole_packet_present(stb_vorbis f, int end_page)
		{
			int s = (int)(f.next_seg);
			int first = (int)(1);
			byte* p = f.stream;
			if (s != -1)
			{
				for (; (s) < (f.segment_count); ++s)
				{
					p += f.segments[s];
					if ((f.segments[s]) < (255))
						break;
				}
				if ((end_page) != 0)
					if ((s) < (f.segment_count - 1))
						return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_stream)));
				if ((s) == (f.segment_count))
					s = (int)(-1);
				if ((p) > (f.stream_end))
					return (int)(error(f, (int)(STBVorbisError.VORBIS_need_more_data)));
				first = (int)(0);
			}

			for (; (s) == (-1);)
			{
				byte* q;
				int n = 0;
				if ((p + 26) >= (f.stream_end))
					return (int)(error(f, (int)(STBVorbisError.VORBIS_need_more_data)));
				if ((CRuntime.memcmp(p, StbVorbis.ogg_page_header, (ulong)(4))) != 0)
					return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_stream)));
				if (p[4] != 0)
					return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_stream)));
				if ((first) != 0)
				{
					if ((f.previous_length) != 0)
						if ((p[5] & 1) != 0)
							return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_stream)));
				}
				else
				{
					if ((p[5] & 1) == 0)
						return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_stream)));
				}
				n = (int)(p[26]);
				q = p + 27;
				p = q + n;
				if ((p) > (f.stream_end))
					return (int)(error(f, (int)(STBVorbisError.VORBIS_need_more_data)));
				for (s = (int)(0); (s) < (n); ++s)
				{
					p += q[s];
					if ((q[s]) < (255))
						break;
				}
				if ((end_page) != 0)
					if ((s) < (n - 1))
						return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_stream)));
				if ((s) == (n))
					s = (int)(-1);
				if ((p) > (f.stream_end))
					return (int)(error(f, (int)(STBVorbisError.VORBIS_need_more_data)));
				first = (int)(0);
			}

			return (int)(1);
		}

		public static int start_decoder(stb_vorbis f)
		{
			byte* header = stackalloc byte[6];
			byte x = 0;
			byte y = 0;
			int len = 0;
			int i = 0;
			int j = 0;
			int k = 0;
			int max_submaps = (int)(0);
			int longest_floorlist = (int)(0);
			if (start_page(f) == 0)
				return (int)(0);
			if ((f.page_flag & 2) == 0)
				return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_first_page)));
			if ((f.page_flag & 4) != 0)
				return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_first_page)));
			if ((f.page_flag & 1) != 0)
				return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_first_page)));
			if (f.segment_count != 1)
				return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_first_page)));
			if (f.segments[0] != 30)
			{
				if (((((((((((f.segments[0]) == (64)) && ((getn(f, header, (int)(6))) != 0)) && ((header[0]) == ('f'))) && ((header[1]) == ('i'))) && ((header[2]) == ('s'))) && ((header[3]) == ('h'))) && ((header[4]) == ('e'))) && ((header[5]) == ('a'))) && ((get8(f)) == ('d'))) && ((get8(f)) == ('\0')))
					return (int)(error(f, (int)(STBVorbisError.VORBIS_ogg_skeleton_not_supported)));
				else
					return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_first_page)));
			}

			if (get8(f) != VORBIS_packet_id)
				return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_first_page)));
			if (getn(f, header, (int)(6)) == 0)
				return (int)(error(f, (int)(STBVorbisError.VORBIS_unexpected_eof)));
			if (vorbis_validate(header) == 0)
				return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_first_page)));
			if (get32(f) != 0)
				return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_first_page)));
			f.channels = (int)(get8(f));
			if (f.channels == 0)
				return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_first_page)));
			if ((f.channels) > (16))
				return (int)(error(f, (int)(STBVorbisError.VORBIS_too_many_channels)));
			f.sample_rate = (uint)(get32(f));
			if (f.sample_rate == 0)
				return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_first_page)));
			get32(f);
			get32(f);
			get32(f);
			x = (byte)(get8(f));
			{
				int log0 = 0;
				int log1 = 0;
				log0 = (int)(x & 15);
				log1 = (int)(x >> 4);
				f.blocksize_0 = (int)(1 << log0);
				f.blocksize_1 = (int)(1 << log1);
				if (((log0) < (6)) || ((log0) > (13)))
					return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
				if (((log1) < (6)) || ((log1) > (13)))
					return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
				if ((log0) > (log1))
					return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
			}

			x = (byte)(get8(f));
			if ((x & 1) == 0)
				return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_first_page)));
			if (start_page(f) == 0)
				return (int)(0);
			if (start_packet(f) == 0)
				return (int)(0);
			do
			{
				len = (int)(next_segment(f));
				skip(f, (int)(len));
				f.bytes_in_seg = (byte)(0);
			}
			while ((len) != 0);
			if (start_packet(f) == 0)
				return (int)(0);
			if (((f).push_mode) != 0)
			{
				if (is_whole_packet_present(f, (int)(1)) == 0)
				{
					if ((f.error) == (int)(STBVorbisError.VORBIS_invalid_stream))
						f.error = (int)(STBVorbisError.VORBIS_invalid_setup);
					return (int)(0);
				}
			}

			crc32_init();
			if (get8_packet(f) != VORBIS_packet_setup)
				return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
			for (i = (int)(0); (i) < (6); ++i)
			{
				header[i] = (byte)(get8_packet(f));
			}
			if (vorbis_validate(header) == 0)
				return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
			f.codebook_count = (int)(get_bits(f, (int)(8)) + 1);
			f.codebooks = (Codebook*)(setup_malloc(f, (int)(sizeof(Codebook) * f.codebook_count)));
			if ((f.codebooks) == null)
				return (int)(error(f, (int)(STBVorbisError.VORBIS_outofmem)));
			CRuntime.memset(f.codebooks, (int)(0), (ulong)(sizeof(Codebook) * f.codebook_count));
			for (i = (int)(0); (i) < (f.codebook_count); ++i)
			{
				uint* values;
				int ordered = 0;
				int sorted_count = 0;
				int total = (int)(0);
				byte* lengths;
				Codebook* c = f.codebooks + i;
				x = (byte)(get_bits(f, (int)(8)));
				if (x != 0x42)
					return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
				x = (byte)(get_bits(f, (int)(8)));
				if (x != 0x43)
					return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
				x = (byte)(get_bits(f, (int)(8)));
				if (x != 0x56)
					return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
				x = (byte)(get_bits(f, (int)(8)));
				c->dimensions = (int)((get_bits(f, (int)(8)) << 8) + x);
				x = (byte)(get_bits(f, (int)(8)));
				y = (byte)(get_bits(f, (int)(8)));
				c->entries = (int)((get_bits(f, (int)(8)) << 16) + (y << 8) + x);
				ordered = (int)(get_bits(f, (int)(1)));
				c->sparse = (byte)((ordered) != 0 ? 0 : get_bits(f, (int)(1)));
				if (((c->dimensions) == (0)) && (c->entries != 0))
					return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
				if ((c->sparse) != 0)
					lengths = (byte*)(setup_temp_malloc(f, (int)(c->entries)));
				else
					lengths = c->codeword_lengths = (byte*)(setup_malloc(f, (int)(c->entries)));
				if (lengths == null)
					return (int)(error(f, (int)(STBVorbisError.VORBIS_outofmem)));
				if ((ordered) != 0)
				{
					int current_entry = (int)(0);
					int current_length = (int)(get_bits(f, (int)(5)) + 1);
					while ((current_entry) < (c->entries))
					{
						int limit = (int)(c->entries - current_entry);
						int n = (int)(get_bits(f, (int)(ilog((int)(limit)))));
						if ((current_entry + n) > (c->entries))
						{
							return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
						}
						CRuntime.memset(lengths + current_entry, (int)(current_length), (ulong)(n));
						current_entry += (int)(n);
						++current_length;
					}
				}
				else
				{
					for (j = (int)(0); (j) < (c->entries); ++j)
					{
						int present = (int)((c->sparse) != 0 ? get_bits(f, (int)(1)) : 1);
						if ((present) != 0)
						{
							lengths[j] = (byte)(get_bits(f, (int)(5)) + 1);
							++total;
							if ((lengths[j]) == (32))
								return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
						}
						else
						{
							lengths[j] = (byte)(255);
						}
					}
				}
				if (((c->sparse) != 0) && ((total) >= (c->entries >> 2)))
				{
					if ((c->entries) > ((int)(f.setup_temp_memory_required)))
						f.setup_temp_memory_required = (uint)(c->entries);
					c->codeword_lengths = (byte*)(setup_malloc(f, (int)(c->entries)));
					if ((c->codeword_lengths) == null)
						return (int)(error(f, (int)(STBVorbisError.VORBIS_outofmem)));
					CRuntime.memcpy(c->codeword_lengths, lengths, (ulong)(c->entries));
					setup_temp_free(f, lengths, (int)(c->entries));
					lengths = c->codeword_lengths;
					c->sparse = (byte)(0);
				}
				if ((c->sparse) != 0)
				{
					sorted_count = (int)(total);
				}
				else
				{
					sorted_count = (int)(0);
					for (j = (int)(0); (j) < (c->entries); ++j)
					{
						if (((lengths[j]) > (10)) && (lengths[j] != 255))
							++sorted_count;
					}
				}
				c->sorted_entries = (int)(sorted_count);
				values = null;
				if (c->sparse == 0)
				{
					c->codewords = (uint*)(setup_malloc(f, (int)(sizeof(uint) * c->entries)));
					if (c->codewords == null)
						return (int)(error(f, (int)(STBVorbisError.VORBIS_outofmem)));
				}
				else
				{
					uint size = 0;
					if ((c->sorted_entries) != 0)
					{
						c->codeword_lengths = (byte*)(setup_malloc(f, (int)(c->sorted_entries)));
						if (c->codeword_lengths == null)
							return (int)(error(f, (int)(STBVorbisError.VORBIS_outofmem)));
						c->codewords = (uint*)(setup_temp_malloc(f, (int)(sizeof(uint) * c->sorted_entries)));
						if (c->codewords == null)
							return (int)(error(f, (int)(STBVorbisError.VORBIS_outofmem)));
						values = (uint*)(setup_temp_malloc(f, (int)(sizeof(uint) * c->sorted_entries)));
						if (values == null)
							return (int)(error(f, (int)(STBVorbisError.VORBIS_outofmem)));
					}
					size = (uint)(c->entries + (sizeof(uint) + sizeof(uint)) * c->sorted_entries);
					if ((size) > (f.setup_temp_memory_required))
						f.setup_temp_memory_required = (uint)(size);
				}
				if (compute_codewords(c, lengths, (int)(c->entries), values) == 0)
				{
					if ((c->sparse) != 0)
						setup_temp_free(f, values, (int)(0));
					return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
				}
				if ((c->sorted_entries) != 0)
				{
					c->sorted_codewords = (uint*)(setup_malloc(f, (int)(sizeof(uint) * (c->sorted_entries + 1))));
					if ((c->sorted_codewords) == null)
						return (int)(error(f, (int)(STBVorbisError.VORBIS_outofmem)));
					c->sorted_values = (int*)(setup_malloc(f, (int)(sizeof(int) * (c->sorted_entries + 1))));
					if ((c->sorted_values) == null)
						return (int)(error(f, (int)(STBVorbisError.VORBIS_outofmem)));
					++c->sorted_values;
					c->sorted_values[-1] = (int)(-1);
					compute_sorted_huffman(c, lengths, values);
				}
				if ((c->sparse) != 0)
				{
					setup_temp_free(f, values, (int)(sizeof(uint) * c->sorted_entries));
					setup_temp_free(f, c->codewords, (int)(sizeof(uint) * c->sorted_entries));
					setup_temp_free(f, lengths, (int)(c->entries));
					c->codewords = null;
				}
				compute_accelerated_huffman(c);
				c->lookup_type = (byte)(get_bits(f, (int)(4)));
				if ((c->lookup_type) > (2))
					return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
				if ((c->lookup_type) > (0))
				{
					ushort* mults;
					c->minimum_value = (float)(float32_unpack((uint)(get_bits(f, (int)(32)))));
					c->delta_value = (float)(float32_unpack((uint)(get_bits(f, (int)(32)))));
					c->value_bits = (byte)(get_bits(f, (int)(4)) + 1);
					c->sequence_p = (byte)(get_bits(f, (int)(1)));
					if ((c->lookup_type) == (1))
					{
						c->lookup_values = (uint)(lookup1_values((int)(c->entries), (int)(c->dimensions)));
					}
					else
					{
						c->lookup_values = (uint)(c->entries * c->dimensions);
					}
					if ((c->lookup_values) == (0))
						return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
					mults = (ushort*)(setup_temp_malloc(f, (int)(sizeof(ushort) * c->lookup_values)));
					if ((mults) == null)
						return (int)(error(f, (int)(STBVorbisError.VORBIS_outofmem)));
					for (j = (int)(0); (j) < ((int)(c->lookup_values)); ++j)
					{
						int q = (int)(get_bits(f, (int)(c->value_bits)));
						if ((q) == (-1))
						{
							setup_temp_free(f, mults, (int)(sizeof(ushort) * c->lookup_values));
							return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
						}
						mults[j] = (ushort)(q);
					}
					if ((c->lookup_type) == (1))
					{
						int len2 = 0;
						int sparse = (int)(c->sparse);
						float last = (float)(0);
						if ((sparse) != 0)
						{
							if ((c->sorted_entries) == (0))
								goto skip;
							c->multiplicands = (float*)(setup_malloc(f, (int)(sizeof(float) * c->sorted_entries * c->dimensions)));
						}
						else
							c->multiplicands = (float*)(setup_malloc(f, (int)(sizeof(float) * c->entries * c->dimensions)));
						if ((c->multiplicands) == null)
						{
							setup_temp_free(f, mults, (int)(sizeof(ushort) * c->lookup_values));
							return (int)(error(f, (int)(STBVorbisError.VORBIS_outofmem)));
						}
						len2 = (int)((sparse) != 0 ? c->sorted_entries : c->entries);
						for (j = (int)(0); (j) < (len2); ++j)
						{
							uint z = (uint)((sparse) != 0 ? c->sorted_values[j] : j);
							uint div = (uint)(1);
							for (k = (int)(0); (k) < (c->dimensions); ++k)
							{
								int off = (int)((z / div) % c->lookup_values);
								float val = (float)(mults[off]);
								val = (float)(mults[off] * c->delta_value + c->minimum_value + last);
								c->multiplicands[j * c->dimensions + k] = (float)(val);
								if ((c->sequence_p) != 0)
									last = (float)(val);
								if ((k + 1) < (c->dimensions))
								{
									if ((div) > (uint.MaxValue / c->lookup_values))
									{
										setup_temp_free(f, mults, (int)(sizeof(ushort) * c->lookup_values));
										return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
									}
									div *= (uint)(c->lookup_values);
								}
							}
						}
						c->lookup_type = (byte)(2);
					}
					else
					{
						float last = (float)(0);
						c->multiplicands = (float*)(setup_malloc(f, (int)(sizeof(float) * c->lookup_values)));
						if ((c->multiplicands) == null)
						{
							setup_temp_free(f, mults, (int)(sizeof(ushort) * c->lookup_values));
							return (int)(error(f, (int)(STBVorbisError.VORBIS_outofmem)));
						}
						for (j = (int)(0); (j) < ((int)(c->lookup_values)); ++j)
						{
							float val = (float)(mults[j] * c->delta_value + c->minimum_value + last);
							c->multiplicands[j] = (float)(val);
							if ((c->sequence_p) != 0)
								last = (float)(val);
						}
					}
				skip:
					;
					setup_temp_free(f, mults, (int)(sizeof(ushort) * c->lookup_values));
				}
			}
			x = (byte)(get_bits(f, (int)(6)) + 1);
			for (i = (int)(0); (i) < (x); ++i)
			{
				uint z = (uint)(get_bits(f, (int)(16)));
				if (z != 0)
					return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
			}
			f.floor_count = (int)(get_bits(f, (int)(6)) + 1);
			f.floor_config = (Floor*)(setup_malloc(f, (int)(f.floor_count * sizeof(Floor))));
			if ((f.floor_config) == null)
				return (int)(error(f, (int)(STBVorbisError.VORBIS_outofmem)));
			for (i = (int)(0); (i) < (f.floor_count); ++i)
			{
				f.floor_types[i] = (ushort)(get_bits(f, (int)(16)));
				if ((f.floor_types[i]) > (1))
					return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
				if ((f.floor_types[i]) == (0))
				{
					Floor0* g = &f.floor_config[i].floor0;
					g->order = (byte)(get_bits(f, (int)(8)));
					g->rate = (ushort)(get_bits(f, (int)(16)));
					g->bark_map_size = (ushort)(get_bits(f, (int)(16)));
					g->amplitude_bits = (byte)(get_bits(f, (int)(6)));
					g->amplitude_offset = (byte)(get_bits(f, (int)(8)));
					g->number_of_books = (byte)(get_bits(f, (int)(4)) + 1);
					for (j = (int)(0); (j) < (g->number_of_books); ++j)
					{
						g->book_list[j] = (byte)(get_bits(f, (int)(8)));
					}
					return (int)(error(f, (int)(STBVorbisError.VORBIS_feature_not_supported)));
				}
				else
				{
					stbv__floor_ordering* p = stackalloc stbv__floor_ordering[31 * 8 + 2];
					Floor1* g = &f.floor_config[i].floor1;
					int max_class = (int)(-1);
					g->partitions = (byte)(get_bits(f, (int)(5)));
					for (j = (int)(0); (j) < (g->partitions); ++j)
					{
						g->partition_class_list[j] = (byte)(get_bits(f, (int)(4)));
						if ((g->partition_class_list[j]) > (max_class))
							max_class = (int)(g->partition_class_list[j]);
					}
					for (j = (int)(0); j <= max_class; ++j)
					{
						g->class_dimensions[j] = (byte)(get_bits(f, (int)(3)) + 1);
						g->class_subclasses[j] = (byte)(get_bits(f, (int)(2)));
						if ((g->class_subclasses[j]) != 0)
						{
							g->class_masterbooks[j] = (byte)(get_bits(f, (int)(8)));
							if ((g->class_masterbooks[j]) >= (f.codebook_count))
								return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
						}
						for (k = (int)(0); (k) < (1 << g->class_subclasses[j]); ++k)
						{
							g->subclass_books[j * 8 + k] = (short)(get_bits(f, (int)(8)) - 1);
							if ((g->subclass_books[j * 8 + k]) >= (f.codebook_count))
								return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
						}
					}
					g->floor1_multiplier = (byte)(get_bits(f, (int)(2)) + 1);
					g->rangebits = (byte)(get_bits(f, (int)(4)));
					g->Xlist[0] = (ushort)(0);
					g->Xlist[1] = (ushort)(1 << g->rangebits);
					g->values = (int)(2);
					for (j = (int)(0); (j) < (g->partitions); ++j)
					{
						int c = (int)(g->partition_class_list[j]);
						for (k = (int)(0); (k) < (g->class_dimensions[c]); ++k)
						{
							g->Xlist[g->values] = (ushort)(get_bits(f, (int)(g->rangebits)));
							++g->values;
						}
					}
					for (j = (int)(0); (j) < (g->values); ++j)
					{
						p[j].x = (ushort)(g->Xlist[j]);
						p[j].id = (ushort)(j);
					}
					CRuntime.qsort(p, (ulong)(g->values), (ulong)(sizeof(stbv__floor_ordering)), point_compare);
					for (j = (int)(0); (j) < (g->values); ++j)
					{
						g->sorted_order[j] = ((byte)(p[j].id));
					}
					for (j = (int)(2); (j) < (g->values); ++j)
					{
						int low = 0;
						int hi = 0;
						neighbors(g->Xlist, (int)(j), &low, &hi);
						g->neighbors[j * 2 + 0] = (byte)(low);
						g->neighbors[j * 2 + 1] = (byte)(hi);
					}
					if ((g->values) > (longest_floorlist))
						longest_floorlist = (int)(g->values);
				}
			}
			f.residue_count = (int)(get_bits(f, (int)(6)) + 1);
			f.residue_config = new Residue[f.residue_count];
			for (i = 0; i < f.residue_config.Length; ++i)
			{
				f.residue_config[i] = new Residue();
			};
			if ((f.residue_config) == null)
				return (int)(error(f, (int)(STBVorbisError.VORBIS_outofmem)));
			for (i = (int)(0); (i) < (f.residue_count); ++i)
			{
				byte* residue_cascade = stackalloc byte[64];
				Residue r = f.residue_config[i];
				f.residue_types[i] = (ushort)(get_bits(f, (int)(16)));
				if ((f.residue_types[i]) > (2))
					return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
				r.begin = (uint)(get_bits(f, (int)(24)));
				r.end = (uint)(get_bits(f, (int)(24)));
				if ((r.end) < (r.begin))
					return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
				r.part_size = (uint)(get_bits(f, (int)(24)) + 1);
				r.classifications = (byte)(get_bits(f, (int)(6)) + 1);
				r.classbook = (byte)(get_bits(f, (int)(8)));
				if ((r.classbook) >= (f.codebook_count))
					return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
				for (j = (int)(0); (j) < (r.classifications); ++j)
				{
					byte high_bits = (byte)(0);
					byte low_bits = (byte)(get_bits(f, (int)(3)));
					if ((get_bits(f, (int)(1))) != 0)
						high_bits = (byte)(get_bits(f, (int)(5)));
					residue_cascade[j] = (byte)(high_bits * 8 + low_bits);
				}
				r.residue_books = new short[r.classifications, 8];
				if ((r.residue_books) == null)
					return (int)(error(f, (int)(STBVorbisError.VORBIS_outofmem)));
				for (j = (int)(0); (j) < (r.classifications); ++j)
				{
					for (k = (int)(0); (k) < (8); ++k)
					{
						if ((residue_cascade[j] & (1 << k)) != 0)
						{
							r.residue_books[j, k] = (short)(get_bits(f, (int)(8)));
							if ((r.residue_books[j, k]) >= (f.codebook_count))
								return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
						}
						else
						{
							r.residue_books[j, k] = (short)(-1);
						}
					}
				}
				r.classdata = (byte**)(setup_malloc(f, (int)(sizeof(byte*) * f.codebooks[r.classbook].entries)));
				if (r.classdata == null)
					return (int)(error(f, (int)(STBVorbisError.VORBIS_outofmem)));
				CRuntime.memset(r.classdata, (int)(0), (ulong)(sizeof(byte*) * f.codebooks[r.classbook].entries));
				for (j = (int)(0); (j) < (f.codebooks[r.classbook].entries); ++j)
				{
					int classwords = (int)(f.codebooks[r.classbook].dimensions);
					int temp = (int)(j);
					r.classdata[j] = (byte*)(setup_malloc(f, (int)(sizeof(byte) * classwords)));
					if ((r.classdata[j]) == null)
						return (int)(error(f, (int)(STBVorbisError.VORBIS_outofmem)));
					for (k = (int)(classwords - 1); (k) >= (0); --k)
					{
						r.classdata[j][k] = (byte)(temp % r.classifications);
						temp /= (int)(r.classifications);
					}
				}
			}
			f.mapping_count = (int)(get_bits(f, (int)(6)) + 1);
			f.mapping = (Mapping*)(setup_malloc(f, (int)(f.mapping_count * sizeof(Mapping))));
			if ((f.mapping) == null)
				return (int)(error(f, (int)(STBVorbisError.VORBIS_outofmem)));
			CRuntime.memset(f.mapping, (int)(0), (ulong)(f.mapping_count * sizeof(Mapping)));
			for (i = (int)(0); (i) < (f.mapping_count); ++i)
			{
				Mapping* m = f.mapping + i;
				int mapping_type = (int)(get_bits(f, (int)(16)));
				if (mapping_type != 0)
					return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
				m->chan = (MappingChannel*)(setup_malloc(f, (int)(f.channels * sizeof(MappingChannel))));
				if ((m->chan) == null)
					return (int)(error(f, (int)(STBVorbisError.VORBIS_outofmem)));
				if ((get_bits(f, (int)(1))) != 0)
					m->submaps = (byte)(get_bits(f, (int)(4)) + 1);
				else
					m->submaps = (byte)(1);
				if ((m->submaps) > (max_submaps))
					max_submaps = (int)(m->submaps);
				if ((get_bits(f, (int)(1))) != 0)
				{
					m->coupling_steps = (ushort)(get_bits(f, (int)(8)) + 1);
					for (k = (int)(0); (k) < (m->coupling_steps); ++k)
					{
						m->chan[k].magnitude = (byte)(get_bits(f, (int)(ilog((int)(f.channels - 1)))));
						m->chan[k].angle = (byte)(get_bits(f, (int)(ilog((int)(f.channels - 1)))));
						if ((m->chan[k].magnitude) >= (f.channels))
							return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
						if ((m->chan[k].angle) >= (f.channels))
							return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
						if ((m->chan[k].magnitude) == (m->chan[k].angle))
							return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
					}
				}
				else
					m->coupling_steps = (ushort)(0);
				if ((get_bits(f, (int)(2))) != 0)
					return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
				if ((m->submaps) > (1))
				{
					for (j = (int)(0); (j) < (f.channels); ++j)
					{
						m->chan[j].mux = (byte)(get_bits(f, (int)(4)));
						if ((m->chan[j].mux) >= (m->submaps))
							return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
					}
				}
				else
					for (j = (int)(0); (j) < (f.channels); ++j)
					{
						m->chan[j].mux = (byte)(0);
					}
				for (j = (int)(0); (j) < (m->submaps); ++j)
				{
					get_bits(f, (int)(8));
					m->submap_floor[j] = (byte)(get_bits(f, (int)(8)));
					m->submap_residue[j] = (byte)(get_bits(f, (int)(8)));
					if ((m->submap_floor[j]) >= (f.floor_count))
						return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
					if ((m->submap_residue[j]) >= (f.residue_count))
						return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
				}
			}
			f.mode_count = (int)(get_bits(f, (int)(6)) + 1);
			for (i = (int)(0); (i) < (f.mode_count); ++i)
			{
				Mode* m = (Mode*)f.mode_config + i;
				m->blockflag = (byte)(get_bits(f, (int)(1)));
				m->windowtype = (ushort)(get_bits(f, (int)(16)));
				m->transformtype = (ushort)(get_bits(f, (int)(16)));
				m->mapping = (byte)(get_bits(f, (int)(8)));
				if (m->windowtype != 0)
					return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
				if (m->transformtype != 0)
					return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
				if ((m->mapping) >= (f.mapping_count))
					return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_setup)));
			}
			flush_packet(f);
			f.previous_length = (int)(0);
			for (i = (int)(0); (i) < (f.channels); ++i)
			{
				f.channel_buffers[i] = (float*)(setup_malloc(f, (int)(sizeof(float) * f.blocksize_1)));
				f.previous_window[i] = (float*)(setup_malloc(f, (int)(sizeof(float) * f.blocksize_1 / 2)));
				f.finalY[i] = (short*)(setup_malloc(f, (int)(sizeof(short) * longest_floorlist)));
				if ((((f.channel_buffers[i]) == null) || ((f.previous_window[i]) == null)) || ((f.finalY[i]) == null))
					return (int)(error(f, (int)(STBVorbisError.VORBIS_outofmem)));
				CRuntime.memset(f.channel_buffers[i], (int)(0), (ulong)(sizeof(float) * f.blocksize_1));
			}
			if (init_blocksize(f, (int)(0), (int)(f.blocksize_0)) == 0)
				return (int)(0);
			if (init_blocksize(f, (int)(1), (int)(f.blocksize_1)) == 0)
				return (int)(0);
			f.blocksize[0] = (int)(f.blocksize_0);
			f.blocksize[1] = (int)(f.blocksize_1);
			{
				uint imdct_mem = (uint)(f.blocksize_1 * sizeof(float) >> 1);
				uint classify_mem = 0;
				int i2 = 0;
				int max_part_read = (int)(0);
				for (i2 = (int)(0); (i2) < (f.residue_count); ++i2)
				{
					Residue r = f.residue_config[i2];
					uint actual_size = (uint)(f.blocksize_1 / 2);
					uint limit_r_begin = (uint)((r.begin) < (actual_size) ? r.begin : actual_size);
					uint limit_r_end = (uint)((r.end) < (actual_size) ? r.end : actual_size);
					int n_read = (int)(limit_r_end - limit_r_begin);
					int part_read = (int)(n_read / r.part_size);
					if ((part_read) > (max_part_read))
						max_part_read = (int)(part_read);
				}
				classify_mem = (uint)(f.channels * (sizeof(void*) + max_part_read * sizeof(byte)));
				f.temp_memory_required = (uint)(classify_mem);
				if ((imdct_mem) > (f.temp_memory_required))
					f.temp_memory_required = (uint)(imdct_mem);
			}

			f.first_decode = (byte)(1);
			f.first_audio_page_offset = (uint)(stb_vorbis_get_file_offset(f));
			return (int)(1);
		}

		public static void vorbis_deinit(stb_vorbis p)
		{
			int i = 0;
			int j = 0;
			if ((p.residue_config) != null)
			{
				for (i = (int)(0); (i) < (p.residue_count); ++i)
				{
					Residue r = p.residue_config[i];
					if ((r.classdata) != null)
					{
						for (j = (int)(0); (j) < (p.codebooks[r.classbook].entries); ++j)
						{
							setup_free(p, r.classdata[j]);
						}
						setup_free(p, r.classdata);
					}
				}
			}

			if ((p.codebooks) != null)
			{
				for (i = (int)(0); (i) < (p.codebook_count); ++i)
				{
					Codebook* c = p.codebooks + i;
					setup_free(p, c->codeword_lengths);
					setup_free(p, c->multiplicands);
					setup_free(p, c->codewords);
					setup_free(p, c->sorted_codewords);
					if (c->sorted_values != null)
					{
						setup_free(p, c->sorted_values - 1);
					}
				}
				setup_free(p, p.codebooks);
			}

			setup_free(p, p.floor_config);
			if ((p.mapping) != null)
			{
				for (i = (int)(0); (i) < (p.mapping_count); ++i)
				{
					setup_free(p, p.mapping[i].chan);
				}
				setup_free(p, p.mapping);
			}

			for (i = (int)(0); ((i) < (p.channels)) && ((i) < (16)); ++i)
			{
				setup_free(p, p.channel_buffers[i]);
				setup_free(p, p.previous_window[i]);
				setup_free(p, p.finalY[i]);
			}
			for (i = (int)(0); (i) < (2); ++i)
			{
				setup_free(p, p.A[i]);
				setup_free(p, p.B[i]);
				setup_free(p, p.C[i]);
				setup_free(p, p.window[i]);
				setup_free(p, p.bit_reverse[i]);
			}
		}

		public static void stb_vorbis_close(stb_vorbis p)
		{
			if ((p) == null)
				return;
			vorbis_deinit(p);
		}

		public static void vorbis_init(stb_vorbis p, stb_vorbis_alloc* z)
		{
			if ((z) != null)
			{
				p.alloc = (stb_vorbis_alloc)(*z);
				p.alloc.alloc_buffer_length_in_bytes = (int)((p.alloc.alloc_buffer_length_in_bytes + 3) & ~3);
				p.temp_offset = (int)(p.alloc.alloc_buffer_length_in_bytes);
			}

			p.eof = (int)(0);
			p.error = (int)(STBVorbisError.VORBIS__no_error);
			p.stream = null;
			p.codebooks = null;
			p.page_crc_tests = (int)(-1);
		}

		public static int stb_vorbis_get_sample_offset(stb_vorbis f)
		{
			if ((f.current_loc_valid) != 0)
				return (int)(f.current_loc);
			else
				return (int)(-1);
		}

		public static stb_vorbis_info stb_vorbis_get_info(stb_vorbis f)
		{
			stb_vorbis_info d = new stb_vorbis_info();
			d.channels = (int)(f.channels);
			d.sample_rate = (uint)(f.sample_rate);
			d.setup_memory_required = (uint)(f.setup_memory_required);
			d.setup_temp_memory_required = (uint)(f.setup_temp_memory_required);
			d.temp_memory_required = (uint)(f.temp_memory_required);
			d.max_frame_size = (int)(f.blocksize_1 >> 1);
			return (stb_vorbis_info)(d);
		}

		public static int stb_vorbis_get_error(stb_vorbis f)
		{
			int e = (int)(f.error);
			f.error = (int)(STBVorbisError.VORBIS__no_error);
			return (int)(e);
		}

		public static stb_vorbis vorbis_alloc(stb_vorbis f)
		{
			return f;
			
			// TODO: Below didn't make sense in C#
			//stb_vorbis p = new stb_vorbis();
			//return p;
		}

		public static void stb_vorbis_flush_pushdata(stb_vorbis f)
		{
			f.previous_length = (int)(0);
			f.page_crc_tests = (int)(0);
			f.discard_samples_deferred = (int)(0);
			f.current_loc_valid = (int)(0);
			f.first_decode = (byte)(0);
			f.samples_output = (uint)(0);
			f.channel_buffer_start = (int)(0);
			f.channel_buffer_end = (int)(0);
		}

		public static int vorbis_search_for_page_pushdata(stb_vorbis f, byte* data, int data_len)
		{
			int i = 0;
			int n = 0;
			for (i = (int)(0); (i) < (f.page_crc_tests); ++i)
			{
				f.scan[i].bytes_done = (int)(0);
			}
			if ((f.page_crc_tests) < (4))
			{
				if ((data_len) < (4))
					return (int)(0);
				data_len -= (int)(3);
				for (i = (int)(0); (i) < (data_len); ++i)
				{
					if ((data[i]) == (0x4f))
					{
						if ((0) == (CRuntime.memcmp(data + i, StbVorbis.ogg_page_header, (ulong)(4))))
						{
							int j = 0;
							int len = 0;
							uint crc = 0;
							if (((i + 26) >= (data_len)) || ((i + 27 + data[i + 26]) >= (data_len)))
							{
								data_len = (int)(i);
								break;
							}
							len = (int)(27 + data[i + 26]);
							for (j = (int)(0); (j) < (data[i + 26]); ++j)
							{
								len += (int)(data[i + 27 + j]);
							}
							crc = (uint)(0);
							for (j = (int)(0); (j) < (22); ++j)
							{
								crc = (uint)(crc32_update((uint)(crc), (byte)(data[i + j])));
							}
							for (; (j) < (26); ++j)
							{
								crc = (uint)(crc32_update((uint)(crc), (byte)(0)));
							}
							n = (int)(f.page_crc_tests++);
							f.scan[n].bytes_left = (int)(len - j);
							f.scan[n].crc_so_far = (uint)(crc);
							f.scan[n].goal_crc = (uint)(data[i + 22] + (data[i + 23] << 8) + (data[i + 24] << 16) + (data[i + 25] << 24));
							if ((data[i + 27 + data[i + 26] - 1]) == (255))
								f.scan[n].sample_loc = uint.MaxValue;
							else
								f.scan[n].sample_loc = (uint)(data[i + 6] + (data[i + 7] << 8) + (data[i + 8] << 16) + (data[i + 9] << 24));
							f.scan[n].bytes_done = (int)(i + j);
							if ((f.page_crc_tests) == (4))
								break;
						}
					}
				}
			}

			for (i = (int)(0); (i) < (f.page_crc_tests);)
			{
				uint crc = 0;
				int j = 0;
				int n2 = (int)(f.scan[i].bytes_done);
				int m = (int)(f.scan[i].bytes_left);
				if ((m) > (data_len - n2))
					m = (int)(data_len - n2);
				crc = (uint)(f.scan[i].crc_so_far);
				for (j = (int)(0); (j) < (m); ++j)
				{
					crc = (uint)(crc32_update((uint)(crc), (byte)(data[n2 + j])));
				}
				f.scan[i].bytes_left -= (int)(m);
				f.scan[i].crc_so_far = (uint)(crc);
				if ((f.scan[i].bytes_left) == (0))
				{
					if ((f.scan[i].crc_so_far) == (f.scan[i].goal_crc))
					{
						data_len = (int)(n2 + m);
						f.page_crc_tests = (int)(-1);
						f.previous_length = (int)(0);
						f.next_seg = (int)(-1);
						f.current_loc = (uint)(f.scan[i].sample_loc);
						f.current_loc_valid = f.current_loc != ~0U ? 1 : 0;
						return (int)(data_len);
					}
					f.scan[i] = (CRCscan)(f.scan[--f.page_crc_tests]);
				}
				else
				{
					++i;
				}
			}
			return (int)(data_len);
		}

		public static int stb_vorbis_decode_frame_pushdata(stb_vorbis f, byte* data, int data_len, int* channels, ref float*[] output, int* samples)
		{
			int i = 0;
			int len = 0;
			int right = 0;
			int left = 0;
			if (((f).push_mode) == 0)
				return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_api_mixing)));
			if ((f.page_crc_tests) >= (0))
			{
				*samples = (int)(0);
				return (int)(vorbis_search_for_page_pushdata(f, data, (int)(data_len)));
			}

			f.stream = data;
			f.stream_end = data + data_len;
			f.error = (int)(STBVorbisError.VORBIS__no_error);
			if (is_whole_packet_present(f, (int)(0)) == 0)
			{
				*samples = (int)(0);
				return (int)(0);
			}

			if (vorbis_decode_packet(f, &len, &left, &right) == 0)
			{
				int error = (int)(f.error);
				if ((error) == (int)(STBVorbisError.VORBIS_bad_packet_type))
				{
					f.error = (int)(STBVorbisError.VORBIS__no_error);
					while (get8_packet(f) != (-1))
					{
						if ((f.eof) != 0)
							break;
					}
					*samples = (int)(0);
					return (int)(f.stream - data);
				}
				if ((error) == (int)(STBVorbisError.VORBIS_continued_packet_flag_invalid))
				{
					if ((f.previous_length) == (0))
					{
						f.error = (int)(STBVorbisError.VORBIS__no_error);
						while (get8_packet(f) != (-1))
						{
							if ((f.eof) != 0)
								break;
						}
						*samples = (int)(0);
						return (int)(f.stream - data);
					}
				}
				stb_vorbis_flush_pushdata(f);
				f.error = (int)(error);
				*samples = (int)(0);
				return (int)(1);
			}

			len = (int)(vorbis_finish_frame(f, (int)(len), (int)(left), (int)(right)));
			for (i = (int)(0); (i) < (f.channels); ++i)
			{
				f.outputs[i] = f.channel_buffers[i] + left;
			}
			if ((channels) != null)
				*channels = (int)(f.channels);
			*samples = (int)(len);
			output = f.outputs;
			return (int)(f.stream - data);
		}

		public static stb_vorbis stb_vorbis_open_pushdata(byte* data, int data_len, int* data_used, int* error, stb_vorbis_alloc* alloc)
		{
			stb_vorbis f;
			stb_vorbis p = new stb_vorbis();
			vorbis_init(p, alloc);
			p.stream = data;
			p.stream_end = data + data_len;
			p.push_mode = (byte)(1);
			if (start_decoder(p) == 0)
			{
				if ((p.eof) != 0)
					*error = (int)(STBVorbisError.VORBIS_need_more_data);
				else
					*error = (int)(p.error);
				return null;
			}

			f = vorbis_alloc(p);
			if ((f) != null)
			{
				f = (stb_vorbis)(p);
				*data_used = ((int)(f.stream - data));
				*error = (int)(0);
				return f;
			}
			else
			{
				vorbis_deinit(p);
				return null;
			}

		}

		public static uint stb_vorbis_get_file_offset(stb_vorbis f)
		{
			if ((f.push_mode) != 0)
				return (uint)(0);
			if ((1) != 0)
				return (uint)(f.stream - f.stream_start);
		}

		public static uint vorbis_find_page(stb_vorbis f, uint* end, uint* last)
		{
			for (; ; )
			{
				int n = 0;
				if ((f.eof) != 0)
					return (uint)(0);
				n = (int)(get8(f));
				if ((n) == (0x4f))
				{
					uint retry_loc = (uint)(stb_vorbis_get_file_offset(f));
					int i = 0;
					if ((retry_loc - 25) > (f.stream_len))
						return (uint)(0);
					for (i = (int)(1); (i) < (4); ++i)
					{
						if (get8(f) != StbVorbis.ogg_page_header[i])
							break;
					}
					if ((f.eof) != 0)
						return (uint)(0);
					if ((i) == (4))
					{
						byte* header = stackalloc byte[27];
						uint i2 = 0;
						uint crc = 0;
						uint goal = 0;
						uint len = 0;
						for (i2 = (uint)(0); (i2) < (4); ++i2)
						{
							header[i2] = (byte)(StbVorbis.ogg_page_header[i2]);
						}
						for (; (i2) < (27); ++i2)
						{
							header[i2] = (byte)(get8(f));
						}
						if ((f.eof) != 0)
							return (uint)(0);
						if (header[4] != 0)
							goto invalid;
						goal = (uint)(header[22] + (header[23] << 8) + (header[24] << 16) + (header[25] << 24));
						for (i2 = (uint)(22); (i2) < (26); ++i2)
						{
							header[i2] = (byte)(0);
						}
						crc = (uint)(0);
						for (i2 = (uint)(0); (i2) < (27); ++i2)
						{
							crc = (uint)(crc32_update((uint)(crc), (byte)(header[i2])));
						}
						len = (uint)(0);
						for (i2 = (uint)(0); (i2) < (header[26]); ++i2)
						{
							int s = (int)(get8(f));
							crc = (uint)(crc32_update((uint)(crc), (byte)(s)));
							len += (uint)(s);
						}
						if (((len) != 0) && ((f.eof) != 0))
							return (uint)(0);
						for (i2 = (uint)(0); (i2) < (len); ++i2)
						{
							crc = (uint)(crc32_update((uint)(crc), (byte)(get8(f))));
						}
						if ((crc) == (goal))
						{
							if ((end) != null)
								*end = (uint)(stb_vorbis_get_file_offset(f));
							if ((last) != null)
							{
								if ((header[5] & 0x04) != 0)
									*last = (uint)(1);
								else
									*last = (uint)(0);
							}
							set_file_offset(f, (uint)(retry_loc - 1));
							return (uint)(1);
						}
					}
				invalid:
					;
					set_file_offset(f, (uint)(retry_loc));
				}
			}
		}

		public static int get_seek_page_info(stb_vorbis f, ProbedPage* z)
		{
			byte* header = stackalloc byte[27];
			byte* lacing = stackalloc byte[255];
			int i = 0;
			int len = 0;
			z->page_start = (uint)(stb_vorbis_get_file_offset(f));
			getn(f, header, (int)(27));
			if ((((header[0] != 'O') || (header[1] != 'g')) || (header[2] != 'g')) || (header[3] != 'S'))
				return (int)(0);
			getn(f, lacing, (int)(header[26]));
			len = (int)(0);
			for (i = (int)(0); (i) < (header[26]); ++i)
			{
				len += (int)(lacing[i]);
			}
			z->page_end = (uint)(z->page_start + 27 + header[26] + len);
			z->last_decoded_sample = (uint)(header[6] + (header[7] << 8) + (header[8] << 16) + (header[9] << 24));
			set_file_offset(f, (uint)(z->page_start));
			return (int)(1);
		}

		public static int go_to_page_before(stb_vorbis f, uint limit_offset)
		{
			uint previous_safe = 0;
			uint end = 0;
			if (((limit_offset) >= (65536)) && ((limit_offset - 65536) >= (f.first_audio_page_offset)))
				previous_safe = (uint)(limit_offset - 65536);
			else
				previous_safe = (uint)(f.first_audio_page_offset);
			set_file_offset(f, (uint)(previous_safe));
			while ((vorbis_find_page(f, &end, null)) != 0)
			{
				if (((end) >= (limit_offset)) && ((stb_vorbis_get_file_offset(f)) < (limit_offset)))
					return (int)(1);
				set_file_offset(f, (uint)(end));
			}
			return (int)(0);
		}

		public static int seek_to_sample_coarse(stb_vorbis f, uint sample_number)
		{
			ProbedPage left = new ProbedPage();
			ProbedPage right = new ProbedPage();
			ProbedPage mid = new ProbedPage();
			int i = 0;
			int start_seg_with_known_loc = 0;
			int end_pos = 0;
			int page_start = 0;
			uint delta = 0;
			uint stream_length = 0;
			uint padding = 0;
			double offset = 0;
			double bytes_per_sample = 0;
			int probe = (int)(0);
			stream_length = (uint)(stb_vorbis_stream_length_in_samples(f));
			if ((stream_length) == (0))
				return (int)(error(f, (int)(STBVorbisError.VORBIS_seek_without_length)));
			if ((sample_number) > (stream_length))
				return (int)(error(f, (int)(STBVorbisError.VORBIS_seek_invalid)));
			padding = (uint)((f.blocksize_1 - f.blocksize_0) >> 2);
			if ((sample_number) < (padding))
				sample_number = (uint)(0);
			else
				sample_number -= (uint)(padding);
			left = (ProbedPage)(f.p_first);
			while ((left.last_decoded_sample) == (~0U))
			{
				set_file_offset(f, (uint)(left.page_end));
				if (get_seek_page_info(f, &left) == 0)
					goto error;
			}
			right = (ProbedPage)(f.p_last);
			if (sample_number <= left.last_decoded_sample)
			{
				if ((stb_vorbis_seek_start(f)) != 0)
					return (int)(1);
				return (int)(0);
			}

			while (left.page_end != right.page_start)
			{
				delta = (uint)(right.page_start - left.page_end);
				if (delta <= 65536)
				{
					set_file_offset(f, (uint)(left.page_end));
				}
				else
				{
					if ((probe) < (2))
					{
						if ((probe) == (0))
						{
							double data_bytes = (double)(right.page_end - left.page_start);
							bytes_per_sample = (double)(data_bytes / right.last_decoded_sample);
							offset = (double)(left.page_start + bytes_per_sample * (sample_number - left.last_decoded_sample));
						}
						else
						{
							double error = (double)(((double)(sample_number) - mid.last_decoded_sample) * bytes_per_sample);
							if (((error) >= (0)) && ((error) < (8000)))
								error = (double)(8000);
							if (((error) < (0)) && ((error) > (-8000)))
								error = (double)(-8000);
							offset += (double)(error * 2);
						}
						if ((offset) < (left.page_end))
							offset = (double)(left.page_end);
						if ((offset) > (right.page_start - 65536))
							offset = (double)(right.page_start - 65536);
						set_file_offset(f, (uint)(offset));
					}
					else
					{
						set_file_offset(f, (uint)(left.page_end + (delta / 2) - 32768));
					}
					if (vorbis_find_page(f, null, null) == 0)
						goto error;
				}
				for (; ; )
				{
					if (get_seek_page_info(f, &mid) == 0)
						goto error;
					if (mid.last_decoded_sample != ~0U)
						break;
					set_file_offset(f, (uint)(mid.page_end));
				}
				if ((mid.page_start) == (right.page_start))
					break;
				if ((sample_number) < (mid.last_decoded_sample))
					right = (ProbedPage)(mid);
				else
					left = (ProbedPage)(mid);
				++probe;
			}
			page_start = (int)(left.page_start);
			set_file_offset(f, (uint)(page_start));
			if (start_page(f) == 0)
				return (int)(error(f, (int)(STBVorbisError.VORBIS_seek_failed)));
			end_pos = (int)(f.end_seg_with_known_loc);
			for (; ; )
			{
				for (i = (int)(end_pos); (i) > (0); --i)
				{
					if (f.segments[i - 1] != 255)
						break;
				}
				start_seg_with_known_loc = (int)(i);
				if (((start_seg_with_known_loc) > (0)) || ((f.page_flag & 1) == 0))
					break;
				if (go_to_page_before(f, (uint)(page_start)) == 0)
					goto error;
				page_start = (int)(stb_vorbis_get_file_offset(f));
				if (start_page(f) == 0)
					goto error;
				end_pos = (int)(f.segment_count - 1);
			}
			f.current_loc_valid = (int)(0);
			f.last_seg = (int)(0);
			f.valid_bits = (int)(0);
			f.packet_bytes = (int)(0);
			f.bytes_in_seg = (byte)(0);
			f.previous_length = (int)(0);
			f.next_seg = (int)(start_seg_with_known_loc);
			for (i = (int)(0); (i) < (start_seg_with_known_loc); i++)
			{
				skip(f, (int)(f.segments[i]));
			}
			if (vorbis_pump_first_frame(f) == 0)
				return (int)(0);
			if ((f.current_loc) > (sample_number))
				return (int)(error(f, (int)(STBVorbisError.VORBIS_seek_failed)));
			return (int)(1);
		error:
			;
			stb_vorbis_seek_start(f);
			return (int)(error(f, (int)(STBVorbisError.VORBIS_seek_failed)));
		}

		public static int peek_decode_initial(stb_vorbis f, int* p_left_start, int* p_left_end, int* p_right_start, int* p_right_end, int* mode)
		{
			int bits_read = 0;
			int bytes_read = 0;
			if (vorbis_decode_initial(f, p_left_start, p_left_end, p_right_start, p_right_end, mode) == 0)
				return (int)(0);
			bits_read = (int)(1 + ilog((int)(f.mode_count - 1)));
			if ((f.mode_config[*mode].blockflag) != 0)
				bits_read += (int)(2);
			bytes_read = (int)((bits_read + 7) / 8);
			f.bytes_in_seg += (byte)(bytes_read);
			f.packet_bytes -= (int)(bytes_read);
			skip(f, (int)(-bytes_read));
			if ((f.next_seg) == (-1))
				f.next_seg = (int)(f.segment_count - 1);
			else
				f.next_seg--;
			f.valid_bits = (int)(0);
			return (int)(1);
		}

		public static int stb_vorbis_seek_frame(stb_vorbis f, uint sample_number)
		{
			uint max_frame_samples = 0;
			if (((f).push_mode) != 0)
				return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_api_mixing)));
			if (seek_to_sample_coarse(f, (uint)(sample_number)) == 0)
				return (int)(0);
			max_frame_samples = (uint)((f.blocksize_1 * 3 - f.blocksize_0) >> 2);
			while ((f.current_loc) < (sample_number))
			{
				int left_start = 0;
				int left_end = 0;
				int right_start = 0;
				int right_end = 0;
				int mode = 0;
				int frame_samples = 0;
				if (peek_decode_initial(f, &left_start, &left_end, &right_start, &right_end, &mode) == 0)
					return (int)(error(f, (int)(STBVorbisError.VORBIS_seek_failed)));
				frame_samples = (int)(right_start - left_start);
				if ((f.current_loc + frame_samples) > (sample_number))
				{
					return (int)(1);
				}
				else if ((f.current_loc + frame_samples + max_frame_samples) > (sample_number))
				{
					vorbis_pump_first_frame(f);
				}
				else
				{
					f.current_loc += (uint)(frame_samples);
					f.previous_length = (int)(0);
					maybe_start_packet(f);
					flush_packet(f);
				}
			}
			return (int)(1);
		}

		public static int stb_vorbis_seek(stb_vorbis f, uint sample_number)
		{
			if (stb_vorbis_seek_frame(f, (uint)(sample_number)) == 0)
				return (int)(0);
			if (sample_number != f.current_loc)
			{
				int n = 0;
				uint frame_start = (uint)(f.current_loc);
				float*[] output = null;
				stb_vorbis_get_frame_float(f, &n, ref output);
				f.channel_buffer_start += (int)(sample_number - frame_start);
			}

			return (int)(1);
		}

		public static int stb_vorbis_seek_start(stb_vorbis f)
		{
			if (((f).push_mode) != 0)
			{
				return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_api_mixing)));
			}

			set_file_offset(f, (uint)(f.first_audio_page_offset));
			f.previous_length = (int)(0);
			f.first_decode = (byte)(1);
			f.next_seg = (int)(-1);
			return (int)(vorbis_pump_first_frame(f));
		}

		public static uint stb_vorbis_stream_length_in_samples(stb_vorbis f)
		{
			uint restore_offset = 0;
			uint previous_safe = 0;
			uint end = 0;
			uint last_page_loc = 0;
			if (((f).push_mode) != 0)
				return (uint)(error(f, (int)(STBVorbisError.VORBIS_invalid_api_mixing)));
			if (f.total_samples == 0)
			{
				uint last = 0;
				uint lo = 0;
				uint hi = 0;
				sbyte* header = stackalloc sbyte[6];
				restore_offset = (uint)(stb_vorbis_get_file_offset(f));
				if (((f.stream_len) >= (65536)) && ((f.stream_len - 65536) >= (f.first_audio_page_offset)))
					previous_safe = (uint)(f.stream_len - 65536);
				else
					previous_safe = (uint)(f.first_audio_page_offset);
				set_file_offset(f, (uint)(previous_safe));
				if (vorbis_find_page(f, &end, &last) == 0)
				{
					f.error = (int)(STBVorbisError.VORBIS_cant_find_last_page);
					f.total_samples = (uint)(0xffffffff);
					goto done;
				}
				last_page_loc = (uint)(stb_vorbis_get_file_offset(f));
				while (last == 0)
				{
					set_file_offset(f, (uint)(end));
					if (vorbis_find_page(f, &end, &last) == 0)
					{
						break;
					}
					previous_safe = (uint)(last_page_loc + 1);
					last_page_loc = (uint)(stb_vorbis_get_file_offset(f));
				}
				set_file_offset(f, (uint)(last_page_loc));
				getn(f, (byte*)(header), (int)(6));
				lo = (uint)(get32(f));
				hi = (uint)(get32(f));
				if (((lo) == (0xffffffff)) && ((hi) == (0xffffffff)))
				{
					f.error = (int)(STBVorbisError.VORBIS_cant_find_last_page);
					f.total_samples = (uint)(0xffffffff);
					goto done;
				}
				if ((hi) != 0)
					lo = (uint)(0xfffffffe);
				f.total_samples = (uint)(lo);
				f.p_last.page_start = (uint)(last_page_loc);
				f.p_last.page_end = (uint)(end);
				f.p_last.last_decoded_sample = (uint)(lo);
			done:
				;
				set_file_offset(f, (uint)(restore_offset));
			}

			return (uint)((f.total_samples) == (0xffffffff) ? 0 : f.total_samples);
		}

		public static float stb_vorbis_stream_length_in_seconds(stb_vorbis f)
		{
			return (float)(stb_vorbis_stream_length_in_samples(f) / (float)(f.sample_rate));
		}

		public static int stb_vorbis_get_frame_float(stb_vorbis f, int* channels, ref float*[] output)
		{
			int len = 0;
			int right = 0;
			int left = 0;
			int i = 0;
			if (((f).push_mode) != 0)
				return (int)(error(f, (int)(STBVorbisError.VORBIS_invalid_api_mixing)));
			if (vorbis_decode_packet(f, &len, &left, &right) == 0)
			{
				f.channel_buffer_start = (int)(f.channel_buffer_end = (int)(0));
				return (int)(0);
			}

			len = (int)(vorbis_finish_frame(f, (int)(len), (int)(left), (int)(right)));
			for (i = (int)(0); (i) < (f.channels); ++i)
			{
				f.outputs[i] = f.channel_buffers[i] + left;
			}
			f.channel_buffer_start = (int)(left);
			f.channel_buffer_end = (int)(left + len);
			if ((channels) != null)
				*channels = (int)(f.channels);
			if ((output) != null)
				output = f.outputs;
			return (int)(len);
		}

		public static stb_vorbis stb_vorbis_open_memory(byte* data, int len, int* error, stb_vorbis_alloc* alloc)
		{
			stb_vorbis f;
			stb_vorbis p = new stb_vorbis();
			if ((data) == null)
				return null;
			vorbis_init(p, alloc);
			p.stream = data;
			p.stream_end = data + len;
			p.stream_start = p.stream;
			p.stream_len = (uint)(len);
			p.push_mode = (byte)(0);
			if ((start_decoder(p)) != 0)
			{
				f = vorbis_alloc(p);
				if ((f) != null)
				{
					f = (stb_vorbis)(p);
					vorbis_pump_first_frame(f);
					if ((error) != null)
						*error = (int)(STBVorbisError.VORBIS__no_error);
					return f;
				}
			}

			if ((error) != null)
				*error = (int)(p.error);
			vorbis_deinit(p);
			return null;
		}

		public static void copy_samples(short* dest, float* src, int len)
		{
			int i = 0;
			for (i = (int)(0); (i) < (len); ++i)
			{
				int v = ((int)((src[i]) * (1 << (15))));
				if (((uint)(v + 32768)) > (65535))
					v = (int)((v) < (0) ? -32768 : 32767);
				dest[i] = (short)(v);
			}
		}

		public static void compute_samples(int mask, short* output, int num_c, float*[] data, int d_offset, int len)
		{
			float* buffer = stackalloc float[32];
			int i = 0;
			int j = 0;
			int o = 0;
			int n = (int)(32);
			for (o = (int)(0); (o) < (len); o += (int)(32))
			{
				CRuntime.memset(buffer, (int)(0), (ulong)(sizeof(float) * 32));
				if ((o + n) > (len))
					n = (int)(len - o);
				for (j = (int)(0); (j) < (num_c); ++j)
				{
					if ((channel_position[num_c, j] & mask) != 0)
					{
						for (i = (int)(0); (i) < (n); ++i)
						{
							buffer[i] += (float)(data[j][d_offset + o + i]);
						}
					}
				}
				for (i = (int)(0); (i) < (n); ++i)
				{
					int v = ((int)((buffer[i]) * (1 << (15))));
					if (((uint)(v + 32768)) > (65535))
						v = (int)((v) < (0) ? -32768 : 32767);
					output[o + i] = (short)(v);
				}
			}
		}

		public static void compute_stereo_samples(short* output, int num_c, float*[] data, int d_offset, int len)
		{
			float* buffer = stackalloc float[32];
			int i = 0;
			int j = 0;
			int o = 0;
			int n = (int)(32 >> 1);
			for (o = (int)(0); (o) < (len); o += (int)(32 >> 1))
			{
				int o2 = (int)(o << 1);
				CRuntime.memset(buffer, (int)(0), (ulong)(sizeof(float) * 32));
				if ((o + n) > (len))
					n = (int)(len - o);
				for (j = (int)(0); (j) < (num_c); ++j)
				{
					int m = (int)(channel_position[num_c, j] & (2 | 4));
					if ((m) == (2 | 4))
					{
						for (i = (int)(0); (i) < (n); ++i)
						{
							buffer[i * 2 + 0] += (float)(data[j][d_offset + o + i]);
							buffer[i * 2 + 1] += (float)(data[j][d_offset + o + i]);
						}
					}
					else if ((m) == (2))
					{
						for (i = (int)(0); (i) < (n); ++i)
						{
							buffer[i * 2 + 0] += (float)(data[j][d_offset + o + i]);
						}
					}
					else if ((m) == (4))
					{
						for (i = (int)(0); (i) < (n); ++i)
						{
							buffer[i * 2 + 1] += (float)(data[j][d_offset + o + i]);
						}
					}
				}
				for (i = (int)(0); (i) < (n << 1); ++i)
				{
					int v = ((int)((buffer[i]) * (1 << (15))));
					if (((uint)(v + 32768)) > (65535))
						v = (int)((v) < (0) ? -32768 : 32767);
					output[o2 + i] = (short)(v);
				}
			}
		}

		public static void convert_samples_short(int buf_c, short** buffer, int b_offset, int data_c, float*[] data, int d_offset, int samples)
		{
			int i = 0;
			if (((buf_c != data_c) && (buf_c <= 2)) && (data_c <= 6))
			{
				for (i = (int)(0); (i) < (buf_c); ++i)
				{
					compute_samples((int)(StbVorbis.channel_selector[buf_c, i]), buffer[i] + b_offset, (int)(data_c), data, (int)(d_offset), (int)(samples));
				}
			}
			else
			{
				int limit = (int)((buf_c) < (data_c) ? buf_c : data_c);
				for (i = (int)(0); (i) < (limit); ++i)
				{
					copy_samples(buffer[i] + b_offset, data[i] + d_offset, (int)(samples));
				}
				for (; (i) < (buf_c); ++i)
				{
					CRuntime.memset(buffer[i] + b_offset, (int)(0), (ulong)(sizeof(short) * samples));
				}
			}

		}

		public static int stb_vorbis_get_frame_short(stb_vorbis f, int num_c, short** buffer, int num_samples)
		{
			float*[] output = null;
			int len = (int)(stb_vorbis_get_frame_float(f, null, ref output));
			if ((len) > (num_samples))
				len = (int)(num_samples);
			if ((len) != 0)
				convert_samples_short((int)(num_c), buffer, (int)(0), (int)(f.channels), output, (int)(0), (int)(len));
			return (int)(len);
		}

		public static void convert_channels_short_interleaved(int buf_c, short* buffer, int data_c, float*[] data, int d_offset, int len)
		{
			int i = 0;
			if (((buf_c != data_c) && (buf_c <= 2)) && (data_c <= 6))
			{
				for (i = (int)(0); (i) < (buf_c); ++i)
				{
					compute_stereo_samples(buffer, (int)(data_c), data, (int)(d_offset), (int)(len));
				}
			}
			else
			{
				int limit = (int)((buf_c) < (data_c) ? buf_c : data_c);
				int j = 0;
				for (j = (int)(0); (j) < (len); ++j)
				{
					for (i = (int)(0); (i) < (limit); ++i)
					{
						float f = (float)(data[i][d_offset + j]);
						int v = ((int)((f) * (1 << (15))));
						if (((uint)(v + 32768)) > (65535))
							v = (int)((v) < (0) ? -32768 : 32767);
						*buffer++ = (short)(v);
					}
					for (; (i) < (buf_c); ++i)
					{
						*buffer++ = (short)(0);
					}
				}
			}

		}

		public static int stb_vorbis_get_frame_short_interleaved(stb_vorbis f, int num_c, short* buffer, int num_shorts)
		{
			float*[] output = null;
			int len = 0;
			if ((num_c) == (1))
				return (int)(stb_vorbis_get_frame_short(f, (int)(num_c), &buffer, (int)(num_shorts)));
			len = (int)(stb_vorbis_get_frame_float(f, null, ref output));
			if ((len) != 0)
			{
				if ((len * num_c) > (num_shorts))
					len = (int)(num_shorts / num_c);
				convert_channels_short_interleaved((int)(num_c), buffer, (int)(f.channels), output, (int)(0), (int)(len));
			}

			return (int)(len);
		}

		public static int stb_vorbis_get_samples_short_interleaved(stb_vorbis f, int channels, short* buffer, int num_shorts)
		{
			float*[] outputs = null;
			int len = (int)(num_shorts / channels);
			int n = (int)(0);
			int z = (int)(f.channels);
			if ((z) > (channels))
				z = (int)(channels);
			while ((n) < (len))
			{
				int k = (int)(f.channel_buffer_end - f.channel_buffer_start);
				if ((n + k) >= (len))
					k = (int)(len - n);
				if ((k) != 0)
					convert_channels_short_interleaved((int)(channels), buffer, (int)(f.channels), f.channel_buffers, (int)(f.channel_buffer_start), (int)(k));
				buffer += k * channels;
				n += (int)(k);
				f.channel_buffer_start += (int)(k);
				if ((n) == (len))
					break;
				if (stb_vorbis_get_frame_float(f, null, ref outputs) == 0)
					break;
			}
			return (int)(n);
		}

		public static int stb_vorbis_get_samples_short(stb_vorbis f, int channels, short** buffer, int len)
		{
			float*[] outputs = null;
			int n = (int)(0);
			int z = (int)(f.channels);
			if ((z) > (channels))
				z = (int)(channels);
			while ((n) < (len))
			{
				int k = (int)(f.channel_buffer_end - f.channel_buffer_start);
				if ((n + k) >= (len))
					k = (int)(len - n);
				if ((k) != 0)
					convert_samples_short((int)(channels), buffer, (int)(n), (int)(f.channels), f.channel_buffers, (int)(f.channel_buffer_start), (int)(k));
				n += (int)(k);
				f.channel_buffer_start += (int)(k);
				if ((n) == (len))
					break;
				if (stb_vorbis_get_frame_float(f, null, ref outputs) == 0)
					break;
			}
			return (int)(n);
		}

		public static int stb_vorbis_decode_memory(byte* mem, int len, int* channels, int* sample_rate, ref short* output)
		{
			int data_len = 0;
			int offset = 0;
			int total = 0;
			int limit = 0;
			int error = 0;
			short* data;
			stb_vorbis v = stb_vorbis_open_memory(mem, (int)(len), &error, null);
			if ((v) == null)
				return (int)(-1);
			limit = (int)(v.channels * 4096);
			*channels = (int)(v.channels);
			if ((sample_rate) != null)
				*sample_rate = (int)(v.sample_rate);
			offset = (int)(data_len = (int)(0));
			total = (int)(limit);
			data = (short*)(CRuntime.malloc((ulong)(total * sizeof(short))));
			if ((data) == null)
			{
				stb_vorbis_close(v);
				return (int)(-2);
			}

			for (; ; )
			{
				int n = (int)(stb_vorbis_get_frame_short_interleaved(v, (int)(v.channels), data + offset, (int)(total - offset)));
				if ((n) == (0))
					break;
				data_len += (int)(n);
				offset += (int)(n * v.channels);
				if ((offset + limit) > (total))
				{
					short* data2;
					total *= (int)(2);
					data2 = (short*)(CRuntime.realloc(data, (ulong)(total * sizeof(short))));
					if ((data2) == null)
					{
						CRuntime.free(data);
						stb_vorbis_close(v);
						return (int)(-2);
					}
					data = data2;
				}
			}
			output = data;
			stb_vorbis_close(v);
			return (int)(data_len);
		}

		public static int stb_vorbis_get_samples_float_interleaved(stb_vorbis f, int channels, float* buffer, int num_floats)
		{
			float*[] outputs = null;
			int len = (int)(num_floats / channels);
			int n = (int)(0);
			int z = (int)(f.channels);
			if ((z) > (channels))
				z = (int)(channels);
			while ((n) < (len))
			{
				int i = 0;
				int j = 0;
				int k = (int)(f.channel_buffer_end - f.channel_buffer_start);
				if ((n + k) >= (len))
					k = (int)(len - n);
				for (j = (int)(0); (j) < (k); ++j)
				{
					for (i = (int)(0); (i) < (z); ++i)
					{
						*buffer++ = (float)(f.channel_buffers[i][f.channel_buffer_start + j]);
					}
					for (; (i) < (channels); ++i)
					{
						*buffer++ = (float)(0);
					}
				}
				n += (int)(k);
				f.channel_buffer_start += (int)(k);
				if ((n) == (len))
					break;
				if (stb_vorbis_get_frame_float(f, null, ref outputs) == 0)
					break;
			}
			return (int)(n);
		}

		public static int stb_vorbis_get_samples_float(stb_vorbis f, int channels, float** buffer, int num_samples)
		{
			float*[] outputs = null;
			int n = (int)(0);
			int z = (int)(f.channels);
			if ((z) > (channels))
				z = (int)(channels);
			while ((n) < (num_samples))
			{
				int i = 0;
				int k = (int)(f.channel_buffer_end - f.channel_buffer_start);
				if ((n + k) >= (num_samples))
					k = (int)(num_samples - n);
				if ((k) != 0)
				{
					for (i = (int)(0); (i) < (z); ++i)
					{
						CRuntime.memcpy(buffer[i] + n, f.channel_buffers[i] + f.channel_buffer_start, (ulong)(sizeof(float) * k));
					}
					for (; (i) < (channels); ++i)
					{
						CRuntime.memset(buffer[i] + n, (int)(0), (ulong)(sizeof(float) * k));
					}
				}
				n += (int)(k);
				f.channel_buffer_start += (int)(k);
				if ((n) == (num_samples))
					break;
				if (stb_vorbis_get_frame_float(f, null, ref outputs) == 0)
					break;
			}
			return (int)(n);
		}
	}
}
