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
        public static List<string> computers = new List<string>();
        public static Random random = new Random();
        static object l1 = new object();
        static object l2 = new object();
        static AutoResetEvent event1 = new AutoResetEvent(true);
        static AutoResetEvent event2 = new AutoResetEvent(true);

        /// <summary>
        /// A method that writes data to a file
        /// </summary>
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


        /// <summary>
        /// A4 printer method
        /// </summary>
        public static void Printer1()
        {

            lock (l1)
            {
                Thread.Sleep(1000);
                Console.WriteLine(Thread.CurrentThread.Name + " user can take an A4 format document.\n");

            }

        }

        /// <summary>
        /// A3 printer method
        /// </summary>
        public static void Printer2()
        {

            lock (l2)
            {

                Thread.Sleep(1000);
                Console.WriteLine(Thread.CurrentThread.Name + " user can take an A3 format document.\n");
            }

        }
        /// <summary>
        /// Printer method
        /// </summary>
        public static void Printer()
        {
            while (computers.Count < 10)
            {
                string format1 = "A3";
                string format2 = "A4";
                string[] format = { format1, format2 };
                int c = random.Next(0, 2);
                string orientation1 = "portrait";
                string orientation2 = "landscape";
                string[] orientation = { orientation1, orientation2 };
                int a = random.Next(0, colors.Count);
                int d = random.Next(0, 2);

                Console.WriteLine(Thread.CurrentThread.Name + " sent the request to print the document format: " + format[c] + ", color: " + colors[a] + ", orientation: " + orientation[d]);
                Thread.Sleep(100);

                //If the format is A3 we call the Printer1 method
                if (format[c] == format1) 
                {
                    event1.WaitOne();
                    if (computers.Count == 10)
                    {
                        return;

                    }
                    Printer1();
                    event1.Set();
                }
                //If the format is A4, we call the Printer2 method
                else
                {
                    event2.WaitOne();
                    if (computers.Count == 10)
                    {
                        return;
                    }
                    Printer2();
                    event2.Set();
                }
                
                if (!computers.Contains(Thread.CurrentThread.Name))
                {
                    computers.Add(Thread.CurrentThread.Name);
                }
            }
        }



        static void Main(string[] args)
        {
            Thread color = new Thread(new ThreadStart(ColorPrint));
            color.Start();
            color.Join();


            for (int i = 0; i < 10; i++)
            {
                Thread thread = new Thread(Printer) //Creating threads
                {

                    Name = String.Format("Computer_{0}", i + 1) //Naming threads
                };
                thread.Start(); //Starting threads

            }

            Console.ReadLine();
        }

    }
}



