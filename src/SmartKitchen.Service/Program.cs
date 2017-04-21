using System;
using System.ServiceModel;

namespace HSR.CloudSolutions.SmartKitchen.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof (SmartKitchenService)))
            {
                try
                {
                    host.Open();
                    Console.WriteLine("SmartKitchenService is running...");

                    Console.WriteLine();
                    Console.WriteLine("Press ENTER to stop the service!");
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An exception occurred while running the SmartKitchenService...");
                    Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        var innerException = ex;
                        while (innerException.InnerException != null)
                        {
                            innerException = ex.InnerException;
                        }
                        Console.WriteLine($"Innerst Exception: {innerException.GetType().Name} => {innerException.Message}");
                    }
                    Console.WriteLine();
                    Console.WriteLine("Press ENTER to kill the service!");
                    Console.ReadLine();
                }
            }
        }
    }
}
