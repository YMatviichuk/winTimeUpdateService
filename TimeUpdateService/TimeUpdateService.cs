using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TimeUpdateService
{
    public partial class TimeUpdateService : ServiceBase
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetSystemTime(ref SYSTEMTIME lpSystemTime);

        public TimeUpdateService()
        {
            InitializeComponent();
        }

        void UpdateSystemTime()
        {
            var netTime = GetNISTDate();
            var time = new SYSTEMTIME()
            {
                wYear = (short)netTime.Year,
                wMonth = (short)netTime.Month,
                wDay = (short)netTime.Day,
                wHour = (short)netTime.Hour,
                wMinute = (short)netTime.Minute,
                wSecond = (short)netTime.Second,
                wMilliseconds = (short)netTime.Millisecond
            };
            SetSystemTime(ref time);
        }

        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            base.OnSessionChange(changeDescription);
            UpdateSystemTime();
        }

        protected override void OnStart(string[] args)
        {
            UpdateSystemTime();
        }

        protected override void OnStop()
        {
        }

        public static DateTime GetNISTDate()
        {
            var client = new TcpClient("time.nist.gov", 13);
            using (var streamReader = new StreamReader(client.GetStream()))
            {
                var response = streamReader.ReadToEnd();
                var utcDateTimeString = response.Substring(7, 17);
                return DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            public short wYear;
            public short wMonth;
            public short wDayOfWeek;
            public short wDay;
            public short wHour;
            public short wMinute;
            public short wSecond;
            public short wMilliseconds;
        }
    }
}
