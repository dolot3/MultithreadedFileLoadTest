namespace FileLoadTest
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string input;
            Console.WriteLine("Enter 1 for single threaded or 2 for multithreaded:");
            input = Console.ReadLine();
            Console.WriteLine();

            int choice = int.Parse(input);

            var startTime = DateTime.Now;

            if (choice == 1)
            {
                Thread.Sleep(1000);
            }
            else if (choice == 2)
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
    }
}