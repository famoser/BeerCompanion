using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Famoser.BeerCompanion.View.Models
{
    public class Color
    {
        public Color(string value, string name)
        {
            ColorValue = value;
            ColorName = name;
        }

        public string ColorValue { get; set; }
        public string ColorName { get; set; }
    }
}
