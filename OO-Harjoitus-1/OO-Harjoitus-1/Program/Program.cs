using System;
using OO_Harjoitus_1.ProgramManager;
using OO_Harjoitus_1.Repos;

namespace OO_Harjoitus_1
{
    class Program
    {
        static void Main(string[] args)
        {
            InvoiceRepo invoiceRepo = new InvoiceRepo();
            ProductRepo productRepo = new ProductRepo(invoiceRepo.GetInvoices());

            // -----------------------------------------------------
            //              MAIN SCREEN CONFIGURATION
            // -----------------------------------------------------

            while (true)
            {
                ColorRepo.ChangeTextColor("\n--- Rakennus OY Billing System ---", ConsoleColor.Magenta);

                // Display the open invoices by default every time we loop back to the main screen
                DisplayOpenInvoices(invoiceRepo);

                Console.WriteLine("\nAvailable options:\n");
                Console.WriteLine("1. View All Invoices");
                Console.WriteLine("2. Add a New Invoice");
                Console.WriteLine("3. Delete an Invoice"); // New option added here.
                Console.WriteLine("4. View All Product Details");
                Console.WriteLine("5. View Invoices by Product");
                Console.WriteLine("6. Exit\n");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        invoiceRepo.DisplayInvoicesFullDetails();
                        break;
                    case "2":
                        invoiceRepo.AddInvoice();
                        break;
                    case "3": // New case for deleting an invoice.
                        invoiceRepo.DeleteInvoice();
                        break;
                    case "4":
                        productRepo.DisplayAllProductDetails();
                        break;
                    case "5":
                        productRepo.DisplayInvoicesByProduct();
                        break;
                    case "6":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nInvalid option. Please try again.");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
            }
        }

        // Open invoices (short format: invoice number, customer name)
        private static void DisplayOpenInvoices(InvoiceRepo invoiceRepo)
        {
            ColorRepo.ChangeTextColor("\n--- Open Invoices ---", ConsoleColor.DarkCyan);

            var invoices = invoiceRepo.GetInvoices();
            if (invoices.Count == 0)
            {
                ColorRepo.ChangeTextColor("\nNo invoices found.\n", ConsoleColor.Red);
            }
            else
            {
                foreach (var invoice in invoices)
                {
                    Console.WriteLine($"Invoice {invoice.InvoiceNumber}, Customer: {invoice.CustomerName}");
                }
            }
        }
    }
}
