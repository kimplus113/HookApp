using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HookApp
{
    class Program
    {
        static void Main(string[] args)
        {
            KeyBoardHook keyBoardHook = new KeyBoardHook();
            keyBoardHook.Install();
            LoadFile load = new LoadFile();
            List<Dictionary> dic = new List<Dictionary>();
            dic = load.ReadFile();
            Application.Run();
        }
    }
}
