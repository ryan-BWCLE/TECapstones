using Capstone.Classes;
using Capstone.UI;
using MenuFramework;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            VMInventoryLoader vMInventoryLoader = new VMInventoryLoader();
            vMInventoryLoader.StockProducts();

            IEnumerable<Product> products = vMInventoryLoader.StockProducts();

            VendingMachine vendingMachine = new VendingMachine(products);

            MainMenu mainMenu = new MainMenu(vendingMachine);

            
        }

    }
}
