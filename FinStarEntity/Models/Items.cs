using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinStarEntity.Models
{
    public class Items
    {
        public Items(int iD, string value, int number)
        {
            ID = iD;
            Value = value;
            Number = number;
        }

        private Items() { }

        public int ID { get; set; }
        public string Value { get; set; }
        public int Number { get; set; }
    }
}
