using System;
using System.Collections.Generic;

namespace OO_Harjoitus_1.Details
{
    public class InvoiceEntity
    {
        public int InvoiceNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public string AdditionalInfo { get; set; }
        public List<ProductEntity> ProductLines { get; set; }

        public InvoiceEntity(int invoiceNumber, string customerName, string customerAddress, DateTime date, DateTime dueDate, string additionalInfo)
        {
            InvoiceNumber = invoiceNumber;
            CustomerName = customerName;
            CustomerAddress = customerAddress;
            Date = date;
            DueDate = dueDate;
            AdditionalInfo = additionalInfo;
            ProductLines = new List<ProductEntity>();
        }

        public void AddProduct(ProductEntity product)
        {
            ProductLines.Add(product);
        }

        // -----------------------------------------------------
        //              PRINT CONFIGURATION
        // -----------------------------------------------------

        /// <summary>
        /// Print invoice details + product details (if any)
        /// </summary>
        public void PrintInvoiceDetails()
        {
            Console.WriteLine($"Invoice Number: {InvoiceNumber}");
            Console.WriteLine($"Customer: {CustomerName}");
            Console.WriteLine($"Address: {CustomerAddress}");
            Console.WriteLine($"Date: {Date.ToShortDateString()}, Due Date: {DueDate.ToShortDateString()}");
            Console.WriteLine($"Additional Info: {AdditionalInfo}");
            Console.WriteLine("\nProducts:");

            double totalBalance = 0; 

            foreach (var product in ProductLines)
            {
                Console.WriteLine($"- {product.ProductName} | Quantity: {product.Quantity} | Price: {product.UnitPrice} euros");
                totalBalance += product.Quantity * product.UnitPrice; 
            }

            // Display the total balance after listing all products
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nTotal Remaining Balance (invoice #{InvoiceNumber}): {totalBalance} euros");
            Console.ForegroundColor = ConsoleColor.White;
        }

    }
}
