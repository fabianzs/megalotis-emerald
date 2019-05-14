using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Webapp
{
    public class SpreadSheet
    {
            public string Range { get; set; }
            public string MajorDimension { get; set; }
            public string[][] Values { get; set; }
    }
}
