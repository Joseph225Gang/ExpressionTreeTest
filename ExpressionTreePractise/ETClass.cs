using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTreePractise
{
    internal class ETClass
    {
        public bool ETProperty { get; set; }
        public string ETMethod(int number, string text) => number + text;
    }
}
