using System;
using System.Runtime.InteropServices;
using System.Security;

namespace NSTsukikage.WinMM.WaveIO
{
    [SuppressUnmanagedCodeSecurity]
    public static class Win32
    {
        public const int MMSYSERR_NOERROR = 0;
        public const int CALLBACK_WINDOW = 65536;
        public const int CALLBACK_THREAD = 131072;
        public const int CALLBACK_FUNCTION = 196608;
        public const int MM_WOM_DONE = 957;
        public const int MM_WIM_DATA = 960;

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern uint waveOutGetNumDevs();

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int waveOutGetDevCaps(
          uint uDeviceID,
          out Win32.WaveOutCaps pwoc,
          int cbwoc);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int waveOutOpen(
          out IntPtr phwo,
          uint uDeviceID,
          ref Win32.WaveFormatEx pwfx,
          IntPtr dwCallback,
          IntPtr dwCallbackInstance,
          int fdwOpen);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int waveOutPrepareHeader(IntPtr hwo, ref Win32.WaveHeader pwh, int cbwh);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int waveOutPrepareHeader(IntPtr hwo, IntPtr pwh, int cbwh);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int waveOutWrite(IntPtr hwo, ref Win32.WaveHeader pwh, int cbwh);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int waveOutWrite(IntPtr hwo, IntPtr pwh, int cbwh);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int waveOutUnprepareHeader(IntPtr hwo, ref Win32.WaveHeader pwh, int cbwh);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int waveOutUnprepareHeader(IntPtr hwo, IntPtr pwh, int cbwh);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int waveOutReset(IntPtr hwo);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int waveOutClose(IntPtr hwo);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern uint waveInGetNumDevs();

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int waveInGetDevCaps(uint uDeviceID, out Win32.WaveInCaps pwic, int cbwic);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int waveInOpen(
          out IntPtr phwi,
          uint uDeviceID,
          ref Win32.WaveFormatEx pwfx,
          IntPtr dwCallback,
          IntPtr dwCallbackInstance,
          int fdwOpen);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int waveInPrepareHeader(IntPtr hwi, ref Win32.WaveHeader pwh, int cbwh);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int waveInPrepareHeader(IntPtr hwi, IntPtr pwh, int cbwh);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int waveInAddBuffer(IntPtr hwi, ref Win32.WaveHeader pwh, int cbwh);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int waveInAddBuffer(IntPtr hwi, IntPtr pwh, int cbwh);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int waveInUnprepareHeader(IntPtr hwi, ref Win32.WaveHeader pwh, int cbwh);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int waveInUnprepareHeader(IntPtr hwi, IntPtr pwh, int cbwh);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int waveInStart(IntPtr hwi);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int waveInReset(IntPtr hwi);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int waveInClose(IntPtr hwi);

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct WaveFormatEx
        {
            public short wFormatTag;
            public short nChannels;
            public int nSamplesPerSec;
            public int nAvgBytesPerSec;
            public short nBlockAlign;
            public short wBitsPerSample;
            public short cbSize;

            public WaveFormatEx(int SamplesPerSec, int BitsPerSample, int Channels)
            {
                this.wFormatTag = (short)Win32.WaveFormatEx.WAVE_FORMAT_PCM;
                this.nSamplesPerSec = SamplesPerSec;
                this.nChannels = (short)Channels;
                this.wBitsPerSample = (short)BitsPerSample;
                this.nBlockAlign = (short)(Channels * BitsPerSample >> 3);
                this.nAvgBytesPerSec = SamplesPerSec * nBlockAlign;
                this.cbSize = 0;
            }

            public static int WAVE_FORMAT_PCM
            {
                get
                {
                    return 1;
                }
            }

            public static int SizeOfWaveFormatEx
            {
                get
                {
                    return Marshal.SizeOf(typeof(Win32.WaveFormatEx));
                }
            }
        }

        public struct WaveHeader
        {
            public IntPtr lpData;
            public uint dwBufferLength;
            public uint dwBytesRecorded;
            public IntPtr dwUser;
            public uint dwFlags;
            public uint dwLoops;
            public IntPtr lpNext;
            public IntPtr reserved;

            public static int SizeOfWaveHeader
            {
                get
                {
                    return Marshal.SizeOf(typeof(Win32.WaveHeader));
                }
            }

            public static Win32.WaveHeader FromIntPtr(IntPtr p)
            {
                return (Win32.WaveHeader)Marshal.PtrToStructure(p, typeof(Win32.WaveHeader));
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct WaveOutCaps
        {
            public ushort wMid;
            public ushort wPid;
            public uint vDriverVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szPname;
            public uint dwFormats;
            public ushort wChannels;
            public ushort wReserved1;
            public uint dwSupport;

            public static int SizeOfWaveOutCaps
            {
                get
                {
                    return Marshal.SizeOf(typeof(Win32.WaveOutCaps));
                }
            }
        }

        public struct WaveInCaps
        {
            public ushort wMid;
            public ushort wPid;
            public uint vDriverVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szPname;
            public uint dwFormats;
            public ushort wChannels;
            public ushort wReserved1;

            public static int SizeOfWaveInCaps
            {
                get
                {
                    return Marshal.SizeOf(typeof(Win32.WaveInCaps));
                }
            }
        }
    }
}
