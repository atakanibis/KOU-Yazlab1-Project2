using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
namespace yazlab12
{
    class MainThread
    {
        int capacity = 0;
        int max_capacity = 10000;
        int request_time = 500;
        int response_time = 200;
        List<SubThread> SubThreads = new List<SubThread>();

        public int Capacity { get => capacity; set => capacity = value; }

        public void SubThreadControl()
        {
            while (true)
            {
                for (int i = 0; i < SubThreads.Count; i++)
                {
                    if (SubThreads[i].Capacity == 0 && SubThreads.Count > 2)
                    {
                        SubThreads.Remove(SubThreads[i]);
                    }
                    else if (SubThreads[i].Capacity >= SubThreads[i].Max_capacity * 7 / 10)
                    {
                        SubThreads[i].Capacity /= 2;
                        SubThread subthread3 = new SubThread
                        {
                            Capacity = SubThreads[i].Capacity
                        };
                        SubThreads.Add(subthread3);
                    }
                }
                Thread.Sleep(10);
            }
        }
        public void RequestFunction()
        {
            while (true)
            {
                Random rand = new Random();
                capacity += rand.Next(500, 1000);
                if (capacity >= max_capacity)
                    capacity = max_capacity;
                Thread.Sleep(request_time);
            }
        }
        
        public void SubRequestFunction()
        {
            int temp;
            while (true)
            {
                foreach (var subthread in SubThreads.ToArray())
                {
                    Random rand = new Random();
                    temp = rand.Next(200, 500);
                    if (temp <= capacity)
                    {
                        subthread.Capacity += temp;
                        capacity -= temp;
                    }
                    else
                    {
                        subthread.Capacity += capacity;
                        capacity = 0;
                    }
                    if (capacity < 0) capacity = 0;
                    if (subthread.Capacity >= subthread.Max_capacity)
                        subthread.Capacity = subthread.Max_capacity;
                }
                Thread.Sleep(SubThreads[0].Request_time);
            }
        }
        public void ResponseFunction()
        {
            while (true)
            {
                Random rand = new Random();
                capacity -= rand.Next(1, 50);
                if (capacity < 0)
                    capacity = 0;
                Thread.Sleep(response_time);
            }
        }
        void Interface()
        {
            while (true)
            {
                Thread.Sleep(100);
                Console.Clear();
                String Title = "=Thread Information Display=";
                Console.SetCursorPosition((Console.WindowWidth - Title.Length) / 2, 0);
                Console.Write(Title);
                Console.WriteLine();
                Console.Write("Main Thread : ");
                int percentage = (int)((double)(capacity * 100) / max_capacity);
                int space = 100 - percentage;
                for (int i = 0; i < percentage; i++)
                    Console.Write("█");


                for (int i = 0; i < space; i++)
                    Console.Write(" ");

                Console.Write("%" + percentage);
                Console.WriteLine();
                int threadIndex = 1;
                for (int j = 0; j < SubThreads.Count; j++)
                {
                    SubThread subthread = SubThreads[j];
                    Console.Write("Sub Thread " + threadIndex + " : ");
                    percentage = (int)((double)(subthread.Capacity * 100) / subthread.Max_capacity);
                    space = 100 - percentage;
                    for (int i = 0; i < percentage; i++)
                        Console.Write("-");
                    for (int i = 0; i < space; i++)
                        Console.Write(" ");

                    Console.Write("%" + (int)percentage);
                    Console.WriteLine();
                    threadIndex++;
                }
                Console.WriteLine("------------------------\nThread Count: " + (SubThreads.Count + 1));
            }
        }
        public MainThread()
        {
            Thread RequestThread = new Thread(new ThreadStart(RequestFunction));
            RequestThread.Start();
            Thread ResponseThread = new Thread(new ThreadStart(ResponseFunction));
            ResponseThread.Start();

            SubThread subthread1 = new SubThread();
            SubThread subthread2 = new SubThread();
            SubThreads.Add(subthread1);
            SubThreads.Add(subthread2);

            Thread SubRequestThread = new Thread(new ThreadStart(SubRequestFunction));
            SubRequestThread.Start();

            Thread SubThreadCreator = new Thread(new ThreadStart(SubThreadControl));
            SubThreadCreator.Start();

            Thread PrinterThread = new Thread(new ThreadStart(Interface));
            PrinterThread.Start();
        }
    }
}