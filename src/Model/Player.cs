using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nancy.Simple.Model
{
    public class Player
    {
        public int id { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public string version { get; set; }
        public int stack { get; set; }
        public int bet { get; set; }
        public Card[] hole_cards { get; set; }
    }

}
