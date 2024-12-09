using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using OO_Harjoitus_1.Details;

namespace OO_Harjoitus_1.ProgramManager
{
    public class InvoiceRepo
    {
        private const string FilePath = "C:\\Users\\nasim\\Desktop\\Rakennus OY Billing\\billing.json";
        private List<InvoiceEntity> invoices;

        public InvoiceRepo()
        {
            invoices = LoadInvoicesFromFile();
        }

        public List<InvoiceEntity> GetInvoices()
        {
            return invoices;
        }

        /// <summary>
        /// Displayes a short list of invoices, unless no invoices are found.
        /// </summary>
        public void DisplayOpenInvoices()
        {
            if (invoices.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNo invoices found.\n");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n--- Open Invoices ---");
            Console.ForegroundColor = ConsoleColor.White;

            foreach (var invoice in invoices)
            {
                Console.WriteLine($"Invoice {invoice.InvoiceNumber}, Customer: {invoice.CustomerName}");
            }
        }

        /// <summary>
        /// Displays full details of all invoices
        /// </summary>
        public void DisplayInvoicesFullDetails()
        {
            if (invoices.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNo invoices found.\n");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n--- All Invoices ---");
            Console.ForegroundColor = ConsoleColor.White;

            foreach (var invoice in invoices)
            {
                invoice.PrintInvoiceDetails();
                Console.WriteLine("--------------------------");
            }
        }

        /// <summary>
        /// Adds a new invoice from user input
        /// </summary>
        public void AddInvoice()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n--- Adding new invoice ---\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Enter Invoice Number: ");
            int invoiceNumber = int.Parse(Console.ReadLine());

            Console.Write("Enter Customer Name: ");
            string customerName = Console.ReadLine();

            Console.Write("Enter Customer Address: ");
            string customerAddress = Console.ReadLine();

            DateTime date = GetValidDate("Enter Date (yyyy-MM-dd): ");
            DateTime dueDate = GetValidDate("Enter Due Date (yyyy-MM-dd): ");

            Console.Write("Enter Additional Info: ");
            string additionalInfo = Console.ReadLine();

            var newInvoice = new InvoiceEntity(invoiceNumber, customerName, customerAddress, date, dueDate, additionalInfo);

            Console.WriteLine("\nAdd products to the invoice (type 'done' to finish):");
            while (true)
            {
                Console.Write("Enter Product Name: ");
                string productName = Console.ReadLine();
                if (productName.ToLower() == "done") break;

                Console.Write("Enter Quantity: ");
                double quantity = double.Parse(Console.ReadLine());

                Console.Write("Enter Unit: ");
                string unit = Console.ReadLine();

                Console.Write("Enter Unit Price: ");
                double unitPrice = double.Parse(Console.ReadLine());

                var newProduct = new ProductEntity(productName, quantity, unit, unitPrice);
                newInvoice.AddProduct(newProduct);
            }

            invoices.Add(newInvoice);
            SaveInvoicesToFile();
            Console.WriteLine("Invoice added successfully!");
        }

        /// <summary>
        /// Deletes selected invoice
        /// </summary>
        public void DeleteInvoice()
        {
            if (invoices.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNo invoices to delete.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            Console.Write("Enter the Invoice Number to delete: ");
            bool isValid = int.TryParse(Console.ReadLine(), out int invoiceNumber);

            if (!isValid)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nInvalid input. Please enter a valid number.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            var invoiceToDelete = invoices.Find(invoice => invoice.InvoiceNumber == invoiceNumber);

            if (invoiceToDelete != null)
            {
                invoices.Remove(invoiceToDelete);
                SaveInvoicesToFile(); // Save the updated list to the file.
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Invoice #{invoiceNumber} has been deleted successfully.");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Invoice #{invoiceNumber} not found.");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        /// <summary>
        /// Loads a new invoice from .json.
        /// C:\\Users\\nasim\\Desktop\\Rakennus OY Billing\\billing.json
        /// </summary>
        /// <returns></returns>
        private List<InvoiceEntity> LoadInvoicesFromFile()
        {
            if (File.Exists(FilePath))
            {
                string jsonData = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<List<InvoiceEntity>>(jsonData) ?? new List<InvoiceEntity>();
            }
            return new List<InvoiceEntity>();
        }

        /// <summary>
        /// Saves invoice to .json. 
        /// C:\\Users\\nasim\\Desktop\\Rakennus OY Billing\\billing.json
        /// </summary>
        private void SaveInvoicesToFile()
        {
            string jsonData = JsonSerializer.Serialize(invoices, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, jsonData);
        }

        /// <summary>
        /// Prompts a valid date input
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        private DateTime GetValidDate(string prompt)
        {
            DateTime date;
            bool validDate = false;

            do
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                validDate = DateTime.TryParseExact(input, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out date);
                if (!validDate)
                {
                    Console.WriteLine("Invalid date format. Please enter a date in the format yyyy-MM-dd.");
                }

            } while (!validDate);

            return date;
        }
    }
}
