using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zadatak_1
{
    class Program
    {
        public static readonly string FileColors = @"../../Colors.txt";
        public static List<string> colors = new List<string>();
        public static Random random = new Random();
        public static string[] format;
        public static string[] orientation;
        static object l = new object();
        static EventWaitHandle waitHande = new AutoResetEvent(false);

        public static void ColorPrint()
        {

            colors.Add("red");
            colors.Add("blue");
            colors.Add("black");
            colors.Add("white");

            if (System.IO.File.Exists(FileColors))
            {
                StreamWriter writer = new StreamWriter(FileColors, false, Encoding.UTF8);

                foreach (string item in colors)
                {
                    writer.WriteLine(item);
                }
                writer.Close();


            }
            else
            {
                Console.WriteLine("The file does not exist or the path is incorrect.");
            }

        }


        public static void Printer(Thread t)
        {
            string orientation1 = "portrait";
            string orientation2 = "landscape";
            string[] orientation = { orientation1, orientation2 };

            string format1 = "A3";
            string format2 = "A4";
            string[] format = { format1, format2 };

            lock (l)
            {

                waitHande.Set();
                int a = random.Next(0, colors.Count);
                int c = random.Next(0, 2);
                int d = random.Next(0, 2);

                Thread.Sleep(100);
                waitHande.WaitOne();

                Console.WriteLine(t.Name + " sent the request to print the document format: " + format[c] + ", color: " + colors[a] + ", orientation: " + orientation[d]);
                Thread.Sleep(1000);
                Console.WriteLine(t.Name + " user can take an " + format[c] + " format document.\n");
            }


        }




        static void Main(string[] args)
        {
            ColorPrint();

            Thread[] thr = new Thread[10];

            for (int i = 0; i < 10; i++)
            {

                Thread t = new Thread(() => Printer(Thread.CurrentThread)) //creating threads
                {

                    Name = string.Format("Computer_{0} ", i + 1) //naming threads

                };

                thr[i] = t;

            }

            foreach (var i in thr) i.Start();
            Console.ReadLine();

        }

    }
}


