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
        public static List<Thread> threads = new List<Thread>();
        public static Random random = new Random();
        public static string[] format;
        public static string[] orientation;
        static object l1 = new object();
        static object l2 = new object();
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



        public static void Printer1()
        {

            string orientation1 = "portrait";
            string orientation2 = "landscape";
            string[] orientation = { orientation1, orientation2 };

            
            int a = random.Next(0, colors.Count);
            int c = random.Next(0, 2);
            int d = random.Next(0, 2);

            Thread.Sleep(100);


            Console.WriteLine(Thread.CurrentThread.Name + " sent the request to print the document format: A4" + ", color: " + colors[a] + ", orientation: " + orientation[d]);

            lock (l2)
            {
               
                Thread.Sleep(1000);
                Console.WriteLine(Thread.CurrentThread.Name + " user can take an A4 format document.\n");

            }
            


        }



        public static void Printer2()
        {
            string orientation1 = "portrait";
            string orientation2 = "landscape";
            string[] orientation = { orientation1, orientation2 };

            
            int a = random.Next(0, colors.Count);
            int c = random.Next(0, 2);
            int d = random.Next(0, 2);

            Thread.Sleep(100);
            Console.WriteLine(Thread.CurrentThread.Name + " sent the request to print the document format: A3" + ", color: " + colors[a] + ", orientation: " + orientation[d]);

            lock (l2)
            {
               
                Thread.Sleep(1000);
                Console.WriteLine(Thread.CurrentThread.Name + " user can take an A3 format document.\n");
                
            }
            
        }

        public static void Printer()
        {
            string format1 = "A3";
            string format2 = "A4";
            string[] format = { format1, format2 };
            int c = random.Next(0, 2);

            if (format[c] == "A3")
            {
                
                Printer1();
            }
           
            else if (format[c] == "A4")
            {
               
                Printer2();
            }
            
            }        
                       


        static void Main(string[] args)
        {
            Thread color = new Thread(new ThreadStart(ColorPrint));
            color.Start();
            color.Join();

            
            for (int i = 0; i < 10; i++)
            {
                Thread t = new Thread(new ThreadStart(Printer))
                {
                    Name = string.Format("Computer_{0} ", i + 1)
                };
                threads.Add(t);

            }
            
            for (int i = 0; i < threads.Count; i++)
            {
                threads[i].Start();
            }
            Console.ReadLine();
        }
    }
}



