using System;
using System.IO;

namespace MyApp.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            var instance = new Program();
            instance.Run(args, System.Console.Out);
        }

        public void Run(string[] args, TextWriter output)
        {
            var service = new StringService();

            foreach(var item in args)
            {
                var result = service.Reverse(item);
                output.WriteLine(result);
            }
        }
    }
}
