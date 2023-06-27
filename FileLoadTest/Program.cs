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
            bool isMultiThread = bool.Parse(args[0]);
            bool logProcess = bool.Parse(args[1]);

            var startTime = DateTime.Now;

            CountdownEvent theEvent = new CountdownEvent(1);
                        
            LoadFile(isMultiThread, logProcess, theEvent);

            if (isMultiThread) { theEvent.Wait(); }

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

        static void LoadFile(bool isMultiThread, bool logProcess, CountdownEvent theEvent)
        {
            string fileName = @"d:\_work\testdata\testdata_tab.txt";
            string? data;
            DataModel theData;
            List<DataModel> theList = new();
            int count = 1;
            int groupCount = 1;

            var reader = new System.IO.StreamReader(fileName);

            while (!reader.EndOfStream) {

                data = reader.ReadLine();

                theData = ReadData(data);
                theData.id = count;

                theList.Add(theData);

                if (theList.Count % 5000 == 0)
                {
                    if (!isMultiThread)
                    {
                        LoadRunner runner = new LoadRunner(theList.ToArray(), groupCount, null);
                        runner.Run(logProcess);
                        
                    } else
                    {

                        LoadRunner runner = new LoadRunner(theList.ToArray(), groupCount, theEvent);

                        ThreadPool.QueueUserWorkItem(delegate { runner.Run(logProcess); });
                        if (groupCount > 1) { theEvent.AddCount(1); }

                        //Prevent the number of pending work items from growing too large and consuming too much memory.
                        while (ThreadPool.PendingWorkItemCount > 0)
                        {
                            if (logProcess)
                                Console.WriteLine($"Sleeping thread.  Pending work items = {ThreadPool.PendingWorkItemCount}");

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