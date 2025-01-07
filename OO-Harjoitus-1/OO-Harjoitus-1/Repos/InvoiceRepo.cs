using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using OO_Harjoitus_1.Details;
using OO_Harjoitus_1.Repos;

namespace OO_Harjoitus_1.ProgramManager
{
    public class InvoiceRepo
    {
        private readonly string FilePath;
        private List<InvoiceEntity> invoices;

        public InvoiceRepo()
        {
            string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Rakennus OY Billing");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            FilePath = Path.Combine(directoryPath, "billing.json");
            invoices = LoadInvoicesFromFile();

            try
            {
                invoices = LoadInvoicesFromFile();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading invoices: {ex.Message}");
                invoices = new List<InvoiceEntity>();
            }

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
                ColorRepo.ChangeTextColor("\nNo product data found.\n", ConsoleColor.Red);
                return;
            }

            ColorRepo.ChangeTextColor("\n--- Open Invoices ---", ConsoleColor.Magenta);

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
                ColorRepo.ChangeTextColor("\nNo invoices found.\n", ConsoleColor.Red);
                return;
            }

            ColorRepo.ChangeTextColor("\n--- All Invoices ---", ConsoleColor.Magenta);

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
            ColorRepo.ChangeTextColor("\n--- Adding new invoice ---\n", ConsoleColor.Blue);

            Console.Write("Enter Invoice Number: ");
            int invoiceNumber = int.Parse(Console.ReadLine());

            Console.Write("Enter Contractor Name: ");
            string contractor = Console.ReadLine();

            Console.Write("Enter Contractor Address: ");
            string contractorAddress = Console.ReadLine();

            Console.Write("Enter Customer Name: ");
            string customerName = Console.ReadLine();

            Console.Write("Enter Customer Address: ");
            string customerAddress = Console.ReadLine();

            // validoidaan päivämäärä
            DateTime date = GetValidDate("Enter Invoice Start Date (yyyy-MM-dd): ");
            DateTime dueDate = GetValidDate("Enter Invoice Due Date (yyyy-MM-dd): ");

            Console.Write("Enter Additional Info: ");
            string additionalInfo = Console.ReadLine();

            var newInvoice = new InvoiceEntity(invoiceNumber, contractor, contractorAddress, customerName, customerAddress, date, dueDate, additionalInfo);

            ColorRepo.ChangeTextColor("\nLISTING A NEW PRODUCT. Type 'done' to finish.\n", ConsoleColor.Green);
            while (true)
            {
                Console.Write("Enter Product Name: ");
                string productName = Console.ReadLine();
                if (productName.ToLower() == "done")
                    break;

                // Kysy muut tiedot vain, jos käyttäjä ei lopeta
                Console.Write("Enter Quantity: ");
                if (!double.TryParse(Console.ReadLine(), out double quantity))
                {
                    Console.WriteLine("Invalid quantity. Please try again.");
                    continue;
                }

                Console.Write("Enter Unit: ");
                string unit = Console.ReadLine();

                Console.Write("Enter Unit Price: ");
                if (!double.TryParse(Console.ReadLine(), out double unitPrice))
                {
                    Console.WriteLine("Invalid unit price. Please try again.");
                    continue;
                }

                Console.Write("Enter Time Used (hours, rounded up to the next full hour): ");
                if (!int.TryParse(Console.ReadLine(), out int timeUsed))
                {
                    Console.WriteLine("Invalid time used. Please try again.");
                    continue;
                }

                Console.Write("Enter Hourly Cost: ");
                if (!double.TryParse(Console.ReadLine(), out double timeCost))
                {
                    Console.WriteLine("Invalid hourly cost. Please try again.");
                    continue;
                }

                var newProduct = new ProductEntity(timeUsed, timeCost, productName, quantity, unit, unitPrice);
                newInvoice.AddProduct(newProduct);
            }

            invoices.Add(newInvoice);
            SaveInvoicesToFile();
            ColorRepo.ChangeTextColor("Invoice added successfully!\n", ConsoleColor.Green);
        }

        /// <summary>
        /// Deletes selected invoice
        /// </summary>
        public void DeleteInvoice()
        {
            if (invoices.Count == 0)
            {
                ColorRepo.ChangeTextColor("\nNo invoices to delete.", ConsoleColor.Red);
                return;
            }

            Console.Write("Enter the Invoice Number to delete: ");
            bool isValid = int.TryParse(Console.ReadLine(), out int invoiceNumber);

            if (!isValid)
            {
                ColorRepo.ChangeTextColor("\nInvalid input. Please enter a valid number.", ConsoleColor.Red);
                return;
            }

            var invoiceToDelete = invoices.Find(invoice => invoice.InvoiceNumber == invoiceNumber);

            if (invoiceToDelete != null)
            {
                invoices.Remove(invoiceToDelete);
                SaveInvoicesToFile();
                ColorRepo.ChangeTextColor($"Invoice #{invoiceNumber} has been deleted successfully.", ConsoleColor.Green);
            }
            else
            {
                ColorRepo.ChangeTextColor($"Invoice #{invoiceNumber} not found.", ConsoleColor.Red);
            }
        }

        /// <summary>
        /// Loads a new invoice from .json.
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

