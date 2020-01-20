using System;
using System.Runtime.InteropServices;
using System.Security;

namespace NSTsukikage.WinMM.WaveIO
{
    [SuppressUnmanagedCodeSecurity]
    internal class WaveBuffer : IDisposable
    {
        public IntPtr pHeader;
        public byte[] Data;
        private GCHandle dataHandle;
        private GCHandle bufferHandle;

        public WaveBuffer(uint dwSize)
        {
            this.Data = new byte[(int)dwSize];
            this.dataHandle = GCHandle.Alloc(Data, GCHandleType.Pinned);
            this.bufferHandle = GCHandle.Alloc(this);
            Win32.WaveHeader waveHeader = new Win32.WaveHeader
            {
                lpData = this.dataHandle.AddrOfPinnedObject(),
                dwBufferLength = (uint)this.Data.Length,
                dwUser = GCHandle.ToIntPtr(this.bufferHandle)
            };
            this.pHeader = Marshal.AllocHGlobal(Win32.WaveHeader.SizeOfWaveHeader);
            Marshal.StructureToPtr(waveHeader, this.pHeader, true);
        }

        public static WaveBuffer FromWaveHeader(Win32.WaveHeader header)
        {
            return (WaveBuffer)GCHandle.FromIntPtr(header.dwUser).Target;
        }

        public void Dispose()
        {
            if (this.pHeader == IntPtr.Zero)
                return;
            this.bufferHandle.Free();
            this.dataHandle.Free();
            Marshal.FreeHGlobal(this.pHeader);
            this.pHeader = IntPtr.Zero;
            GC.SuppressFinalize(this);
        }
    }
}
