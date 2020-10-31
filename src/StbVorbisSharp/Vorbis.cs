using System;
using System.Runtime.InteropServices;
using static StbVorbisSharp.StbVorbis;

namespace StbVorbisSharp
{
	public unsafe class Vorbis : IDisposable
	{
		private stb_vorbis_alloc* _vorbisAlloc;
		private IntPtr _vorbisAllocPtr;
		
		private stb_vorbis _vorbis;
		private stb_vorbis_info _vorbisInfo;
		private float _lengthInSeconds;
		private int _length;
		private int _decoded;
		private int _current;

		private byte* _data;
		private IntPtr _dataPtr;
		private bool _alloc;

		public stb_vorbis StbVorbis => _vorbis;
		public stb_vorbis_info StbVorbisInfo => _vorbisInfo;
		public int SampleRate => (int)_vorbisInfo.sample_rate;
		public int Channels => _vorbisInfo.channels;
		public float LengthInSeconds => _lengthInSeconds;
		public float Length => _length;
		public int Decoded => _decoded;
		public int Position => _current;

		public Vorbis(byte[] data)
		{
			//Alloc();
			Load(data);
		}

		public Vorbis(byte* data, int length)
		{
			//Alloc();
			Load(data, length);
		}

		// TODO: Does this mean we can re-use this class without any new alloc when loading a new file? Meaning we could recycle it?
		private void Alloc()
		{
			_vorbisAllocPtr = Marshal.AllocHGlobal(sizeof(stb_vorbis_alloc) * 1);
			_vorbisAlloc = (stb_vorbis_alloc*) _vorbisAllocPtr.ToPointer();
		}

		public void Load(byte[] data)
		{
			Clear();
			
			// Previously was using a fixed pointer outside a fixed statement, which is probably a bad idea (but was working!)
			_dataPtr = Marshal.AllocHGlobal(data.Length * sizeof(byte));
			_data = (byte*) _dataPtr.ToPointer();
			_alloc = true;

			var fileCopy = _data;
			for (int i = 0; i < data.Length; i++)
				*fileCopy++ = data[i];
			
			Load(_data, data.Length, false);
		}

		public void Load(byte* data, int length, bool checkClear = true)
		{
			if (checkClear) Clear();

			var vorbis = stb_vorbis_open_memory(data, length, null, null); //_vorbisAlloc);
			
			_vorbis = vorbis;
			_vorbisInfo = stb_vorbis_get_info(vorbis);
			_lengthInSeconds = stb_vorbis_stream_length_in_seconds(_vorbis);
			_length = (int) stb_vorbis_stream_length_in_samples(_vorbis);

			Restart();
		}
		
		public void Seek(int samples)
		{
			stb_vorbis_seek(_vorbis, (uint) samples);

			_current = samples;
		}
		
		public void Clear()
		{
			if (!_alloc) return;
			
			_alloc = false;
			Marshal.FreeHGlobal(_dataPtr);
		}
		
		public void Dispose()
		{
			Clear();
			
			Marshal.FreeHGlobal(_vorbisAllocPtr);
			_vorbisAlloc = null;
		}

		public void Restart()
		{
			stb_vorbis_seek_start(_vorbis);
		}

		public int Decode(float[] samples, int offset, int count)
		{
			fixed (float* ptr = samples)
			{
				var decoded = stb_vorbis_get_samples_float_interleaved(_vorbis, _vorbisInfo.channels, ptr + offset, count);
				_current += decoded;
				_decoded = decoded;

				return decoded * _vorbisInfo.channels;
			}
		}

		public static Vorbis FromMemory(byte[] data)
		{
			return new Vorbis(data);
		}
		
		public static Vorbis FromPointer(byte* data, int length)
		{
			return new Vorbis(data, length);
		}
	}
}