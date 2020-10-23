using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookApp
{
   
    class Dictionary
    {
        public String key;
        public String value;
        public Dictionary(String key, String value) {
            this.key = key;
            this.value = value;
        }
        public Dictionary() { }

        public string Value { get => Value1; set => this.Value1 = value; }
        public string Value1 { get => value; set => this.value = value; }
    }
}
