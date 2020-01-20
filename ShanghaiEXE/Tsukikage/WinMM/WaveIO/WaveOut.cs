using System;
using System.Security;
using System.Threading;

namespace NSTsukikage.WinMM.WaveIO
{
    [SuppressUnmanagedCodeSecurity]
    public class WaveOut : IDisposable
    {
        private IntPtr deviceHandle = IntPtr.Zero;
        private int enqueuedBufferSize = 0;
        private readonly MessageThread eventHandler;
        public const int WaveMapper = -1;

        public IntPtr Handle
        {
            get
            {
                return this.deviceHandle;
            }
        }

        public int EnqueuedBufferSize
        {
            get
            {
                return this.enqueuedBufferSize;
            }
        }

        public event WaveOut.WaveOutDoneHandler OnDone;

        public WaveOut(int deviceId, int samplesPerSec, int bitsPerSample, int channels)
        {
            Win32.WaveFormatEx pwfx = new Win32.WaveFormatEx(samplesPerSec, bitsPerSample, channels);
            this.eventHandler = new MessageThread();
            IntPtr dwCallback = new IntPtr(this.eventHandler.Win32ThreadID);
            int num = Win32.waveOutOpen(out this.deviceHandle, (uint)deviceId, ref pwfx, dwCallback, IntPtr.Zero, 131072);
            if ((uint)num > 0U)
            {
                this.eventHandler.Dispose();
                throw new Exception(string.Format("The device could not be opened ({0})", num));
            }
            this.eventHandler.MessageHandlers[957] = m =>
           {
               WaveBuffer waveBuffer = WaveBuffer.FromWaveHeader(Win32.WaveHeader.FromIntPtr(m.LParam));
               Win32.waveOutUnprepareHeader(this.deviceHandle, waveBuffer.pHeader, Win32.WaveHeader.SizeOfWaveHeader);
               Interlocked.Add(ref this.enqueuedBufferSize, -waveBuffer.Data.Length);
               waveBuffer.Dispose();
               if (this.OnDone == null)
                   return;
               this.OnDone();
           };
        }

        private void EnsureOpened()
        {
            if (this.deviceHandle == IntPtr.Zero)
                throw new InvalidOperationException("Device not opening!");
        }

        public void Write(byte[] waveform)
        {
            this.EnsureOpened();
            WaveBuffer waveBuffer = new WaveBuffer((uint)waveform.Length);
            Array.Copy(waveform, waveBuffer.Data, waveform.Length);
            Interlocked.Add(ref this.enqueuedBufferSize, waveform.Length);
            Win32.waveOutPrepareHeader(this.deviceHandle, waveBuffer.pHeader, Win32.WaveHeader.SizeOfWaveHeader);
            Win32.waveOutWrite(this.deviceHandle, waveBuffer.pHeader, Win32.WaveHeader.SizeOfWaveHeader);
        }

        public void Stop()
        {
            this.EnsureOpened();
            Win32.waveOutReset(this.deviceHandle);
            while ((uint)this.enqueuedBufferSize > 0U)
                Thread.Sleep(0);
        }

        public void Close()
        {
            if (!(this.deviceHandle != IntPtr.Zero))
                return;
            this.Stop();
            Win32.waveOutClose(this.deviceHandle);
            this.deviceHandle = IntPtr.Zero;
            this.eventHandler.Dispose();
            GC.SuppressFinalize(this);
        }

        void IDisposable.Dispose()
        {
            this.Close();
        }

        public static string[] GetDeviceNames()
        {
            uint numDevs = Win32.waveOutGetNumDevs();
            string[] strArray = new string[(int)numDevs];
            for (uint uDeviceID = 0; uDeviceID < numDevs; ++uDeviceID)
            {
                Win32.WaveOutCaps pwoc = new Win32.WaveOutCaps();
                Win32.waveOutGetDevCaps(uDeviceID, out pwoc, Win32.WaveOutCaps.SizeOfWaveOutCaps);
                strArray[(int)uDeviceID] = pwoc.szPname;
            }
            return strArray;
        }

        public delegate void WaveOutDoneHandler();
    }
}
