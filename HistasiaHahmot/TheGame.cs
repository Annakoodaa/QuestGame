using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuestGame
{
    internal class TheGame
    {
        public static void StartAdventure(Character player)
        {
            // Generating a new Quest
            var Quest = QuestFactory.QuestGenerator();
            // Printing Quest
            Quest.QuestDescription();

            // Area choice
            Console.WriteLine("Valitse minne haluat mennä:");
            Console.Write("1. Mustametsä\n" +
                "2. Kyläpahanen\n" +
                "3. Peikonkaupunki\n" +
                "4. Kuolemanjärven kunta\n");
            // Input
            bool validInput = false;
            ConsoleKeyInfo pressedKey = new ConsoleKeyInfo();
            while (!validInput)
            {
                pressedKey = Console.ReadKey(true);
                switch (pressedKey.KeyChar)
                {
                    case char c when (c == '1' ||c == '2' || c == '3' || c == '4'):
                        validInput = true;
                        break;
                    default:
                        break;
                }
            }

            switch (pressedKey.KeyChar)
            {
                case '1':
                    MustaMetsa();
                    break;
                case '2':
                    KylaPahanen();
                    break;
                case '3':
                    PeikonKaupunki();
                    break;
                case '4':
                    KuolemanjarvenKunta();
                    break;
            }
            Console.WriteLine("to be continued.");
        }

        static void MustaMetsa()
        {
            Console.WriteLine("Mustametsä");
        }

        static void KylaPahanen()
        {
            Console.WriteLine("Kyläpahanen");
        }

        static void PeikonKaupunki()
        {
            Console.WriteLine("Peikonkaupunki");
        }

        static void KuolemanjarvenKunta()
        {
            Console.WriteLine("Kuolemanjärven kunta");
        }
    }
}
