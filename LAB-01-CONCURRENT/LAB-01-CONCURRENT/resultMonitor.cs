using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace LAB_01_CONCURRENT
{
    class resultMonitor
    {
        private mydata[] results;
        private int count = 0;
        private int curr_index = 0;
        private readonly object _locker = new object();
        private bool done = false;//bool method to check wheather the adding item process completesd or not 
        public resultMonitor(int Size)
        {
            results = new mydata[Size];
            
        }
        public void Sort()
        {
            bool flag = true;
            while (flag)
            {
                flag = false;
                for (int i = 0; i < count - 1; i++) // Loop through results
                {
                    mydata a = results[i];
                    mydata b = results[i + 1];
                    if (a.CompareTo(b) > 0) // Change to > for ascending order
                    {
                        results[i] = b; // Swap elements
                        results[i + 1] = a;
                        flag = true; // Set flag to true to indicate a swap occurred
                    }
                } // for
            } // while
        } // Sort method

        public void Insert(mydata result)
        {
            lock (_locker)
            {
                results[count++] = result;
                    Sort();
            }
           
        }       

        public mydata[] GetResults()
        {
            if (done) return results;
            else
                return null;
           

        }
        public void iscompleted() //checking the Adding process is completed or not
        {
            lock (_locker)
            {
                done = true; // if completed
                Monitor.PulseAll(_locker);
            }
        }

    }
}
