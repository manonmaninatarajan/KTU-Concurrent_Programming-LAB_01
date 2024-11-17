using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LAB_01_CONCURRENT
{
    class dataMonitor
    {
        private int count = 0;//initializing count
        private mydata[] up_data;//creating an array to carry data from mydata class
        private int curr_index = 0;// current index
        private int last_index = 0;//last index
        private bool done = false;//bool method to check wheather the adding item process completesd or not
        private static readonly object _locker = new object();//locker object

        public dataMonitor(int size)//constructor with parameter-size
        {
            up_data = new mydata[size];
        }

        public void Adding_item(mydata data) //Adding item to data monitor mydata[]
        {
            lock (_locker) 
            {
                while (count >= up_data.Length) // Check if the mydata array is full or not
                {
                    Console.WriteLine("Data monitor is full..waiting for data monitor to get empty...");
                    Monitor.Wait(_locker); //block the current thread untill free space
                }
                if (curr_index == up_data.Length)
                {
                    curr_index = 0;                   
                    Console.WriteLine("Index is at the end,so it will add value to the index 0.");
                }
                up_data[curr_index] = data; // Insert data to the inde               
                count++; // Increment count            
                curr_index++; // Moving to the next index              
                Console.WriteLine(data.ToString()+"added to the data monitor");
                Monitor.PulseAll(_locker); 

            }
        
        }

        public mydata remove() // Removing item from the data monitor
        {
            lock (_locker)
            {
                while (count == 0 && !done) 
                {
                    Console.WriteLine("Data monitor is empty, waiting for it to have some data");
                    Monitor.Wait(_locker); // block the current thread untill specific condition met
                }
                if (count == 0 && done)
                {
                    Console.WriteLine("no data fount");
                    return null; // Indicate that processing is complete
                }

                mydata data = up_data[last_index]; 
                last_index++; 
                count--;
                Console.WriteLine(data.ToString()+"removed from the data monitor");
              
                if (last_index == up_data.Length )
                {
                    last_index = 0; // if it equals it will reset to index 0       
                    Console.WriteLine("Index is at the end,so it will remove value to the index 0.");
                }
                Monitor.PulseAll(_locker); 
                return data; 
                
            }
           
        }

        public void iscomplete() //checking the Adding process is completed or not
        {
            lock (_locker)
            {
                done = true; // if completed
                Monitor.PulseAll(_locker); 
            }
        }
    }
}
