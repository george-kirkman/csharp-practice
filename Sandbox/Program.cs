using System;
using System.Globalization;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine(CultureInfo.CurrentCulture.DateTimeFormat.ToString());
            DateTime DoB = DateTime.Parse("26/06/1986", new CultureInfo("en-GB"));
            Console.WriteLine(DoB);
        }
    }
}