using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileLoadTest
{
    internal class LoadRunner
    {
        DataModel[] data;
        int groupNumber;
        CountdownEvent? theEvent;

        public LoadRunner(DataModel[] theData, int group, CountdownEvent? countdownEvent = null) 
        {
            data = theData;
            groupNumber = group;
            theEvent = countdownEvent;
        }

        public void Run(bool logProcess)
        {
            if (logProcess) 
                Console.WriteLine($"Starting group {groupNumber}");

            using (LoadContext context = new()) {
                context.AddRange(data);
                context.SaveChanges();
            }

            //If running multithreaded, then signal that this batch is complete.
            if (theEvent != null) theEvent.Signal();

        }
    }
}
