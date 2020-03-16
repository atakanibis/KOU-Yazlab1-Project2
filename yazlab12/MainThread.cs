using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace WindowsFormsApp1
{
    class MainThread
    {
        int capacity = 0;
        int max_capacity = 10000;
        int request_time = 500;
        int response_time = 200;
        List<SubThread> SubThreads = new List<SubThread>();
        Thread SubThreadCreator;

        public void SubThreadControl()
        {
            foreach (var subthread in SubThreads)
            {
                if (subthread.Capacity == 0 && SubThreads.Count > 2)
                {
                    SubThreads.Remove(subthread);
                }
                else if (subthread.Capacity >= subthread.Max_capacity * 7 / 10)
                {
                    subthread.Capacity = subthread.Capacity / 2;
                    SubThread subthread3 = new SubThread();
                    subthread3.Capacity = subthread.Capacity;
                    SubThreads.Add(subthread3);
                }
            }
        }
        public void RequestFunction()
        {
            while (true)
            {
                Random rand = new Random();
                capacity += rand.Next(1, 100);
                if (capacity >= max_capacity)
                    capacity = max_capacity;
                Thread.Sleep(request_time);
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

        public MainThread()
        {
            Thread thread1 = new Thread(new ThreadStart(RequestFunction));
            thread1.Start();
            Thread thread2 = new Thread(new ThreadStart(ResponseFunction));
            thread2.Start();

            SubThread subthread1 = new SubThread();
            SubThread subthread2 = new SubThread();
            SubThreads.Add(subthread1);
            SubThreads.Add(subthread2);

            SubThreadCreator = new Thread(new ThreadStart(SubThreadControl));
            SubThreadCreator.Start();
        }
    }
}
