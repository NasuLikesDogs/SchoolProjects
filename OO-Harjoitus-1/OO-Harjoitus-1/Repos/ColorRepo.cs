using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OO_Harjoitus_1.Repos
{
    public class ColorRepo

    // ColorRepo.ChangeTextColor("Text", ConsoleColor.Red);

    {
        // RED
        public static void ChangeTextColor(string text, ConsoleColor color)
        {
            // Tallenna nykyinen väri
            var originalColor = Console.ForegroundColor;

            // Vaihda tekstin väri
            Console.ForegroundColor = color;

            // Tulosta teksti
            Console.WriteLine(text);

            // Palauta alkuperäinen väri
            Console.ForegroundColor = originalColor;
        }
    }
}

