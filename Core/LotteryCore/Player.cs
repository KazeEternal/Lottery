using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class Player
    {
        public DateTime LastWin { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public List<Item> WonItems { get; set; } = new List<Item>();
    }
}
