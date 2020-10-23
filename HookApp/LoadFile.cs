using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
namespace HookApp
{
    class LoadFile
    {
        public List<Dictionary> dictionary = new List<Dictionary>();
        public List<Dictionary> ReadFile() { 
            try
            {
                using (StreamReader sr = new StreamReader("HookApp.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        dictionary.Add(new Dictionary(splitStr(line).key, splitStr(line).value));
                    }
                    return dictionary;
                }
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dictionary;
        }
        private Dictionary splitStr (String str)
        {
            Dictionary dic = new Dictionary();
            String[] split = str.Split(':');
            dic.key=split[0].Trim();
            dic.value = split[1].Trim();
            return dic;
        }
    }
}