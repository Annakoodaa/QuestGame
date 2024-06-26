﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
            var quest = QuestFactory.QuestGenerator();
            // Printing Quest
            quest.QuestDescription();

            // Invoking ChooseArea() with the player object as parameter
            ChooseArea(player, quest);
        }

        // Area choice
        public static void ChooseArea(Character player, Quest quest)
        {
            Console.WriteLine("Valitse minne haluat mennä:");
            Console.Write
                (
                "1. Mustametsä\n" +
                "2. Peikonkaupunki\n"                
                );

            // Input
            bool validInput = false;
            ConsoleKeyInfo pressedKey = new ConsoleKeyInfo();
            while (!validInput)
            {
                pressedKey = Console.ReadKey(true);
                switch (pressedKey.KeyChar)
                {
                    case char c when (c == '1' || c == '2' || c == '3' ):
                        validInput = true;
                        break;
                    default:
                        break;
                }
            }

            // Area Selection based on input
            switch (pressedKey.KeyChar)
            {
                case '1':
                    MustaMetsa.MMEntrance(player,quest);
                    break;
                case '2':
                    PeikonKaupunki.Start(player, quest);
                    break;
            }
        }

    }
}
