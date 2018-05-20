using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        private static SerialPort _serialPort;
        public static event EventHandler s_KeyEventHandler;
        static void Main(string[] args)
        {


            //Helper.WriteSubtotale(-200);
            //Helper.ReadLevels();
            //Helper.PartialCoinWithdrowal();
            //CashmaticCommands.WriteAnnulla();

            CashmaticCommands.WriteSubtotale(100);

            //CashmaticCommands.WriteSubtotale(-200);
            while (true)
            {
                int saldato = CashmaticCommands.ReadSaldato();
                int pagato = CashmaticCommands.ReadPagato();
                int erogato = CashmaticCommands.ReadErogato();
                int nonerogato = CashmaticCommands.ReadNonerogato();
                int erogazione = CashmaticCommands.ReadFile("erogazione.txt");
                int subtotale = CashmaticCommands.ReadFile("subtotale.txt");


                Console.WriteLine("Saldato: " + saldato);
                Console.WriteLine("Pagato: " + pagato);
                Console.WriteLine("Erogato: " + erogato);
                Console.WriteLine("Nonerogato: " + nonerogato);
                Console.WriteLine("Erogazione: " + erogazione);
                Console.WriteLine("Subtotale: " + subtotale);


#if DEBUG
                Console.WriteLine("Press enter to close...");
                Console.ReadLine();
#endif

            }



            //string line = Console.ReadLine(); // Read string from console
            //int value;
            //if (int.TryParse(line, out value)) // Try to parse the string as an integer
            //{
            //    Console.Write("Multiply integer by 10: ");
            //    Console.WriteLine(value * 10); // Multiply the integer and display it
            //}
            //else
            //{
            //    Console.WriteLine("Not an integer!");
            //}
        }


    }
}
