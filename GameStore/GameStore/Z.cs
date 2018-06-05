using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace GameStore
{
    public static class Z
    {
        public static void Write(string str)
        {
            using (StreamWriter sw = new StreamWriter("D:\\Info.txt"))
            {
                sw.WriteLine(str);
            }

        }
    }
}