using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuestGame
{
    internal class Gathering
    {
        // Random
        static Random s_rnd = new Random();

        public static int Gather(Character player)
        {
            // Gathering Defaults
            int maxAmount = 10;
            int minAmount = 1;

            // Area modifier?


            // Gathering skills effects
            // Currently it only increase the minimun gathering
            // amount by the value of gathering skill
            minAmount += player.GatheringSkill;

            // Chance for enemy encounter.
            //int ambush = s_rnd.Next(1,101);
            //if (ambush <= 3) 
            //{
            //    // Text for ambush.
            //    string ambushText = "Lähestyessäsi sieniä, huomaat vihollisia lähestymässä sinua\n";
            //    foreach(char c in ambushText)
            //    {
            //        Console.Write(c);
            //        Thread.Sleep(30);
            //    }
            //    Console.WriteLine("Paina näppäintä jatkaaksesi tappeluun.");
            //    Console.ReadKey(true);

            //    // Start Battle
            //    Combat.Battle(player);
            //}


            // Randomizing gathered amount.
            int gatheredAmount = s_rnd.Next(minAmount, maxAmount);

            return gatheredAmount;


            // Gathering Text.
            //string gatherSuccessText = $"Kumarruin sieniä keräämään. Yksi toisen perään, tungin sieniä reipasta tahtia nahkalaukkuuni. Sain {gatheredAmount} sientä\n";
            //foreach (char c in gatherSuccessText)
            //{
            //    Console.Write(c);
            //    Thread.Sleep(30);
            //}
            //Console.WriteLine("Paina nappia jatkaaksesi.");
            //Console.ReadKey(true);
        }
    }
}
