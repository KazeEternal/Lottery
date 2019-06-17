using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console.Settings
{
    public class CommandLineAttribute : Attribute
    {
        public string Flag { get; private set; }
        public string Help { get; private set; }
        public bool IsRequired { get; private set; }
        public CommandLineAttribute(string flag, string help, bool isRequired = false)
        {
            Flag = flag;
            Help = help;
            IsRequired = isRequired;
        }
    }
}
