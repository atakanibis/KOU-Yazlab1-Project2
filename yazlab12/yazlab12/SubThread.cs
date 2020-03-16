using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace yazlab12
{
    class SubThread
    {

        int capacity = 0;
        int max_capacity = 5000;
        int request_time = 500;
        int response_time = 300;
        public int Capacity { get => capacity; set => capacity = value; }
        public int Max_capacity { get => max_capacity; set => max_capacity = value; }
        public int Request_time { get => request_time; set => request_time = value; }

        public void ResponseFunction()
        {
            while (true)
            {
                Random rand = new Random();
                capacity -= rand.Next(1, 50);
                if (capacity <= 0)
                    capacity = 0;
                Thread.Sleep(response_time);
            }
        }
        public SubThread()
        {
            Thread ResponseThread = new Thread(new ThreadStart(ResponseFunction));
            ResponseThread.Start();
        }
    }
}