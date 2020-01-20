using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Windows.Forms;

namespace NSTsukikage.WinMM.WaveIO
{
    [SuppressUnmanagedCodeSecurity]
    public sealed class MessageThread : IDisposable, IMessageFilter
    {
        private Thread thread;
        public int Win32ThreadID;
        public Dictionary<int, MessageThread.CallbackDelegate> MessageHandlers;

        public MessageThread()
        {
            this.MessageHandlers = new Dictionary<int, MessageThread.CallbackDelegate>();
            using (ManualResetEvent initialized = new ManualResetEvent(false))
            {
                this.thread = new Thread(() =>
               {
                   Application.AddMessageFilter(this);
                   this.Win32ThreadID = MessageThread.GetCurrentThreadId();
                   initialized.Set();
                   Application.Run();
               });
                this.thread.Start();
                initialized.WaitOne();
            }
        }

        public void PostMessage(int msg, IntPtr wParam, IntPtr lParam)
        {
            MessageThread.PostThreadMessage(this.Win32ThreadID, msg, wParam, lParam);
        }

        public void Dispose()
        {
            if (this.thread == null)
                return;
            this.PostMessage(18, IntPtr.Zero, IntPtr.Zero);
            this.thread.Join();
            this.Win32ThreadID = 0;
            this.thread = null;
        }

        bool IMessageFilter.PreFilterMessage(ref Message m)
        {
            if (this.MessageHandlers.TryGetValue(m.Msg, out CallbackDelegate callbackDelegate))
                callbackDelegate(m);
            return false;
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PostThreadMessage(
          int idThread,
          int msg,
          IntPtr wParam,
          IntPtr lParam);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int GetCurrentThreadId();

        public delegate void CallbackDelegate(Message m);
    }
}
