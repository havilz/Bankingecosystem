using System;
using System.Windows;
// using BankingEcosystem.Atm.AppLayer; // if needed

namespace TestConsole
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Starting TestConsole...");
            try
            {
                var app = new Application();
                Console.WriteLine("Application instance created successfully.");
                
                // Keep it simple first.
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CRASH: {ex}");
            }
            Console.WriteLine("Done.");
        }
    }
}
