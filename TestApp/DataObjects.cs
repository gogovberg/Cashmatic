using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    public class MoneyLevels
    {
        public Dictionary<int, int> Coins { set; get; }
        public Dictionary<int, int> Notes { set; get; }
        public Dictionary<int, int> Stacker { set; get; }
    }
}
