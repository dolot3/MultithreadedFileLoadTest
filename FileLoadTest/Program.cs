using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace FileLoadTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CountdownEvent? theEvent = null;

            //fetch the flags for multithreaded processing and logging
            bool isMultiThread = bool.Parse(args[0]);
            bool logProcess = bool.Parse(args[1]);

            //setup countdown event if running multithreaded.
            if (isMultiThread) { theEvent = new CountdownEvent(1); }

            //Start the timer.
            var startTime = DateTime.Now;
                        
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

        static void LoadFile(bool isMultiThread, bool logProcess, CountdownEvent? theEvent)
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

                    PerformDataInsert(theList, groupCount, theEvent, isMultiThread, logProcess);

                    theList.Clear();
                    groupCount++;
                }
                count++;
                
            }

            //load any remaining records that didn't get included in the last batch of 5000
            if (theList.Count > 0)
                PerformDataInsert(theList, groupCount, theEvent, isMultiThread, logProcess);

        }

        static void PerformDataInsert(List<DataModel> theList, int groupCount, CountdownEvent? theEvent, bool isMultiThread, bool logProcess)
        {

            if (!isMultiThread)
            {
                //create new load runner and load this batch of data.
                LoadRunner runner = new LoadRunner(theList.ToArray(), groupCount, theEvent);
                runner.Run(logProcess);

            }
            else
            {
                //create new load running and queue the load process.
                LoadRunner runner = new LoadRunner(theList.ToArray(), groupCount, theEvent);
                ThreadPool.QueueUserWorkItem(delegate { runner.Run(logProcess); });

                //Since the countdown event must be initialized with at least 1, don't add another count until past the first grouping
                if (groupCount > 1) { theEvent.AddCount(1); }

                //Prevent the number of pending work items from growing too large and consuming too much memory.
                while (ThreadPool.PendingWorkItemCount > 0)
                {
                    if (logProcess)
                        Console.WriteLine($"Sleeping thread.  Pending work items = {ThreadPool.PendingWorkItemCount}");

                    Thread.Sleep(10);
                }

            }

        }

        static DataModel ReadData(string data)
        {
            return new DataModel(data);
        }
    }
}