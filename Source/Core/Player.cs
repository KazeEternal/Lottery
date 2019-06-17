using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class Player
    {
        public DateTime LastWin { get; set; }
        public String FirstName { get; set; } = String.Empty;
        public string MiddleName { get; set; } = String.Empty;
        public String LastName { get; set; } = String.Empty;
        public String FullName { get { return String.Format("{0} {1} {2}", FirstName, MiddleName, LastName).Replace("  ", " "); } }
        public List<Item> WonItems { get; set; } = new List<Item>();
        public List<Question> Answers { get; private set; } = new List<Question>();
    }
}
