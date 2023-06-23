using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileLoadTest
{
    internal class Program
    {
        static void Main(string[] args)
        {

            int choice = int.Parse(args[0]);

            var startTime = DateTime.Now;

            var taskList = new List<Task>();   

            if (choice == 1)    //single threaded
            {
                LoadFile(false, taskList);
            }
            else if (choice == 2)   //multithreaded
            {
                LoadFile(true, taskList);
            }

           

            var endTime = DateTime.Now;
            var diff = endTime - startTime;

            Console.WriteLine("Elapsed Time:");
            Console.WriteLine($"hours: {diff.Hours}");
            Console.WriteLine($"Minutes: {diff.Minutes}");
            Console.WriteLine($"Seconds: {diff.Seconds}");
            Console.WriteLine($"Milliseconds: {diff.Milliseconds}");
            Console.WriteLine();
            Console.WriteLine("Hit enter to terminate.");
            Console.ReadLine();
            
        }

        static void LoadFile(bool isMultiThread, List<Task> taskList)
        {
            string fileName = @"d:\_work\testdata\testdata_tab.txt";
            string? data;
            DataModel theData;
            List<DataModel> theList = new();
            int count = 1;
            int groupCount = 1;

            var reader = new System.IO.StreamReader(fileName);

            ThreadPool.SetMaxThreads(6, 6);

            while (!reader.EndOfStream) {

                data = reader.ReadLine();

                theData = ReadData(data);
                theData.id = count;

                theList.Add(theData);

                if (theList.Count % 5000 == 0)
                {
                    if (!isMultiThread)
                    {
                        LoadRunner runner = new LoadRunner(theList.ToArray(), groupCount);
                        runner.Run();
                        
                    } else
                    {
                        LoadRunner runner = new LoadRunner(theList.ToArray(), groupCount);
                        ThreadPool.QueueUserWorkItem(delegate { runner.Run(); });

                        while(ThreadPool.PendingWorkItemCount > 1)
                        {
                            //Console.WriteLine($"Sleeping thread.  Pending work items = {ThreadPool.PendingWorkItemCount}");
                            Thread.Sleep(10);
                        }

                    }
                    theList.Clear();
                    groupCount++;
                }
                count++;
                
            }

        }

        static DataModel ReadData(string data)
        {
            return new DataModel(data);
        }
    }
}