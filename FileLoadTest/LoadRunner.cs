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

        public LoadRunner(DataModel[] theData, int group) 
        {
            data = theData;
            groupNumber = group;
        }

        public void Run()
        {
            //Console.WriteLine($"Starting group {groupNumber}");

            using (LoadContext context = new()) {
                context.AddRange(data);
                context.SaveChanges();
            }

        }
    }
}
