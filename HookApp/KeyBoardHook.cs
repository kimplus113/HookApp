using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace HookApp
    {

    class KeyBoardHook
    {

        #region Win32 API Functions and Constants

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
                KeyboardHookDelegate lpfn, IntPtr hMod, int dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private const int WH_KEYBOARD_LL = 13;
      
        private const int WM_KEYDOWN = 0x0100;
       
        #endregion

        private KeyboardHookDelegate _hookProc;
        private IntPtr _hookHandle = IntPtr.Zero;

        public delegate IntPtr KeyboardHookDelegate(int nCode, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        public struct KeyboardHookStruct
        {
            public int VirtualKeyCode;
            public int ScanCode;
            public int Flags;
            public int Time;
            public int ExtraInfo;
        }
      
      

        // destructor
        ~KeyBoardHook()
        {
            Uninstall();
        }

        public void Install()
        {
            _hookProc = KeyboardHookProc;
            _hookHandle = SetupHook(_hookProc);

            if (_hookHandle == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }
        private IntPtr SetupHook(KeyboardHookDelegate hookProc)
        {
            IntPtr hInstance = Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]);

            return SetWindowsHookEx(WH_KEYBOARD_LL, hookProc, hInstance, 0);
        }


        public static String word = "";


        public LoadFile load = new LoadFile();
        public List<Dictionary> dic = new List<Dictionary>();
        public KeyBoardHook()
        {
            dic = load.ReadFile();
        }
        private IntPtr KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                KeyboardHookStruct kbStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));

                if (wParam == (IntPtr)WM_KEYDOWN)
                {

                    Keys k = (Keys)kbStruct.VirtualKeyCode;
                    String key = k.ToString();
                    if (key.Length == 1)
                    {
                        word += key;
                    }

                    if (k == Keys.Space || k == Keys.Enter)
                    {     
                        String temp = word;
                        word  = findStr(temp);
                        Replace(temp, word);
                        word = "";
                    }
                    if (k == Keys.Escape) // exit program
                        Application.Exit();
                      
                }
            }

            return CallNextHookEx(_hookHandle, nCode, wParam, lParam);
        }
       

        public String findStr(String str)
        {
            String Str = str.ToLower();
            String result = "";
            for (int i = 0; i < dic.Count; i++)
            {
                if (String.Compare(Str, dic[i].key) == 0) result = dic[i].value;
            }
            return result;
        }
        public void Replace(String temp, String word)
        {
            if (word.Length > 0)
            {
                int index = temp.Length;
                while (index > 0)
                {
                    SendKeys.Send("{BACKSPACE}");
                    index--;
                }
                SendKeys.Send(word);
            }
        }
        public void Uninstall()
        {
            UnhookWindowsHookEx(_hookHandle);
        }
    }
       
}