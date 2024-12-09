using System;
using System.Collections.Generic;
using OO_Harjoitus_1.Details;

namespace OO_Harjoitus_1.ProgramManager
{
    public class ProductRepo
    {
        private List<InvoiceEntity> invoices;

        public ProductRepo(List<InvoiceEntity> invoices)
        {
            this.invoices = invoices;
        }



        /// <summary>
        /// Displays all unique products in the system
        /// </summary>
        public void DisplayAllProductDetails()
        {
            var allProducts = new List<string>();

            foreach (var invoice in invoices)
            {
                foreach (var product in invoice.ProductLines)
                {
                    if (!allProducts.Contains(product.ProductName))
                    {
                        allProducts.Add(product.ProductName);
                    }
                }
            }

            if (allProducts.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNo product data found.\n");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n--- All Product Details ---");
            Console.ForegroundColor = ConsoleColor.White;

            foreach (var product in allProducts)
            {
                Console.WriteLine($"Product: {product}");
            }
        }

        /// <summary>
        /// Displays all invoices that contain a specific product
        /// </summary>
        public void DisplayInvoicesByProduct()
        {
            Console.Write("Enter the product name to search: ");
            string productName = Console.ReadLine();

            var invoicesWithProduct = invoices.FindAll(invoice =>
                invoice.ProductLines.Exists(line => line.ProductName.Equals(productName, StringComparison.OrdinalIgnoreCase)));

            if (invoicesWithProduct.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"No invoices found with product {productName}.\n");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"\n--- Invoices with product {productName} ---");
            Console.ForegroundColor = ConsoleColor.White;

            foreach (var invoice in invoicesWithProduct)
            {
                invoice.PrintInvoiceDetails();
                Console.WriteLine("--------------------------");
            }
        }
    }
}
