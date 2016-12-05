using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace SayNo
{
    class Program
    {
        private const string _soundFile = "petergriff.mp3";
        private const int _timerCount = 300;
        private static WindowsMediaPlayer _myplayer;

        // import the function in your class
        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        static void Main(string[] args)
        {

            try
            {
                _myplayer = new WindowsMediaPlayer();
                _myplayer.URL = _soundFile;

                int processId = GetAppPIDFromUser();
                bool playSuccessSound = GetPlaySuccessSoundFromUser();

                int timesFired = 0;

                while (true)
                {

                    Console.WriteLine("\nTimes Fired: {0} ", timesFired);

                    for (int a = _timerCount; a >= 0; a--)
                    {
                        Console.SetCursorPosition(0, 2);
                        Console.Write("Sending keystrokes in: {0} ", a);
                        System.Threading.Thread.Sleep(1000);
                    }

                    SendKeystrokes(processId, playSuccessSound);

                    timesFired++;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Doh", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }


        private static int GetAppPIDFromUser()
        {
            Console.Write("Please enter the PID for Matt's command line process (cmd.exe): ");
            int processId = Convert.ToInt32(Console.ReadLine());

            if (processId < 0)
                throw new Exception("Process Id is required or invalid!");

            return processId;
        }

        private static bool GetPlaySuccessSoundFromUser()
        {
            Console.Write("Would you like to be alerted on success (y/n): ");
            System.ConsoleKeyInfo userAnswer = Console.ReadKey();

            if (userAnswer.Key == ConsoleKey.Y)
                return true;
            else
                return false;
        }

        private static void SendKeystrokes(int processId, bool playSuccessSound)
        {
            Process process = GetWindowsProcess(processId);
            int pauseAmount = 200;

            IntPtr h = process.MainWindowHandle;
            SetForegroundWindow(h);

            // Pause a half second between keypresses
            SendKeys.SendWait("n");
            Thread.Sleep(pauseAmount);

            SendKeys.SendWait("{ENTER}");
            Thread.Sleep(pauseAmount);

            SendKeys.SendWait("n");
            Thread.Sleep(pauseAmount);

            SendKeys.SendWait("{ENTER}");
            Thread.Sleep(pauseAmount);

            if (playSuccessSound)
                _myplayer.controls.play();

        }


        // Simply retrieve the process of Matt's command line 
        private static Process GetWindowsProcess(int processId)
        {
            Process process = Process.GetProcessById(processId);

            if (process != null)
            {
                return process;
            }
            else
            {
                return null;
            }
        }

    }

}




