using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace FileLoadTest
{
    internal class Program
    {
        static void Main(string[] args)
        {

            int choice = int.Parse(args[0]);

            var startTime = DateTime.Now;

            if (choice == 1)    //single threaded
            {
                LoadFile();
            }
            else if (choice == 2)   //multithreaded
            {
                Thread.Sleep(2000);
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

        static void LoadFile()
        {
            string fileName = @"d:\_work\testdata\testdata_tab.txt";
            string? data;
            DataModel theData;
            List<DataModel> theList = new();

            var reader = new System.IO.StreamReader(fileName);

            while (!reader.EndOfStream) {
                data = reader.ReadLine();
                theData = ReadData(data);
                theList.Add(theData);
                if (theList.Count % 5000 == 0)
                {
                    LoadData(theList.ToArray());
                    theList.Clear();
                }
            }


        }

        static void LoadData(DataModel[] data)
        {

            LoadContext context = new();
            context.AddRange(data);

            context.SaveChanges();

            Console.WriteLine("Load Data");
        }

        static DataModel ReadData(string data)
        {
            return new DataModel(data);
        }
    }
}