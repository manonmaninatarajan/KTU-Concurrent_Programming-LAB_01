using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace LAB_01_CONCURRENT
{
     class Program
    {
        const string cfd = "mano_data1.txt";//data file
        //mano_data1.txt-All data matched to the selected criteria
        //mano_data2.txt-parts of data matched the filter criteria
        //mano_data3.txt-no data matches to the selected criteria
        const string cfr = "Result_data.txt";//result file
        const int numThreads = 2;//number of worker threads


        static void Main(string[] args)
        {
            List<mydata> listdata = new List<mydata>();
            if (File.Exists(cfr)) File.Delete(cfr);//if result exit delete it
                       
            Read(cfd, listdata); //1. Main thread Reading data from the data file to the list
            
            dataMonitor dataMonitor = new dataMonitor(listdata.Count / 2);//initializing data monitor and its size should be only 1/2 of the original data length
            resultMonitor resultMonitor = new resultMonitor(listdata.Count);//initializing result monitor.
                      
            Thread[] workerThreads = new Thread[numThreads];//new worker thread with length of numworkers
            for (int i = 0; i < numThreads; i++)//2.Main thread spawning selected amount of worker thread
            {
                workerThreads[i] = new Thread(()=>WorkerThread(dataMonitor, resultMonitor));
                workerThreads[i].Start();//thread starts.
                
            }
            for (int i = 0; i < listdata.Count; i++)//3.Main thread adding item to data monitor
            {
                dataMonitor.Adding_item(listdata[i]);

            }
            dataMonitor.iscomplete(); 
            for(int i = 0; i < numThreads; i++) //4. Main thread waits for all worker threads to complete its work
            {               
                workerThreads[i].Join();
            }
            resultMonitor.iscompleted();//4.worker thread waits untill all data is processed
            Console.WriteLine("Writting result to the text file after completion");         
            Print(cfr, resultMonitor.GetResults());//5.Main thread writes all data to the result file.
        }

        public static void Read(string filePath, List<mydata> listdata)//Method to read data from the data file.
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
               string line;
                 while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(';');
                        string text = parts[0];
                        int number = int.Parse(parts[1]);
                        double value = double.Parse(parts[2]);
                        mydata data = new mydata(text, number, value);
                        listdata.Add(data);
                    }
            }
            
        }

        public static void Print(string filePath, mydata[] listdata) //Method used to write all data to the result fiel
        {
          using (StreamWriter writer = new StreamWriter(filePath))
          {
                string line;
                string l = "----------------------------------------------------------------";
                line = string.Format("| {0,-20} | {1,-8} | {2,-8} | {3,-9} |", "Text", "Number", "Value", "Computed result");
                writer.WriteLine(l);
                writer.WriteLine(line);
                writer.WriteLine(l);
                Console.WriteLine(l);
                Console.WriteLine(line);
                Console.WriteLine(l);
                foreach (mydata data in listdata)
                {                   
               writer.WriteLine(data.ToString()+data.ComputeResult());
               Console.WriteLine(data.ToString()+data.ComputeResult());
             }
                writer.WriteLine(l);
                Console.WriteLine(l);
            }
           
        }


       public static void WorkerThread(dataMonitor dataMonitor, resultMonitor resultMonitor)
        {
            while (true)
            {
                mydata item = dataMonitor.remove(); //1.worker thread removes data from data monitor
                if (item == null) break;
                Thread.Sleep(500);
                if (item.FitsCriterion())//3.if filter is true worker thread insert value to the result data
                {
                    resultMonitor.Insert(item); 
                    
                }
            }
        }
    }
}
