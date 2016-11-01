using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 1; i <= 20; i++)
            {
                double  j = i / 10.0;
                double m = F_Function.F_dist(j, 1, 4);
                Console.WriteLine(m);
            }
            Console.WriteLine("取小数点后c位（c = 4）");
            for (int i = 1; i <= 20; i++)
            {
                int c = 4;
                double j = i / 10.0;
                double m = F_Function.F_dist(j, 1, 4, c);//结果取小数点后x位
                Console.WriteLine(m);
            }
        }
    }
}
