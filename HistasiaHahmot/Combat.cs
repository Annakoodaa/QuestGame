using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame
{
    class tempEnemy
    {
        public string Name = "Skeleton";

        public int Health = 95;
        public int Defense = 1;
        public int AttackBonus = -1;
    }
    public class Combat
    {
        static Random Rnd = new Random();

        public static void Battle(Character player)
        {
            Console.Clear();
            tempEnemy enemy = new tempEnemy();
             while(true)
            {
                //Console.Clear();

                // Player turn.
                Console.WriteLine($"Vihollinen: {enemy.Name} | Elämä: {enemy.Health}\n");
                Console.WriteLine($"Pelaaja: {player.Name} | Elämä {player.Health}");
                Console.WriteLine("---------");
                Console.WriteLine("Toimi:");
                Console.WriteLine("1. Hyökkää");
                Console.WriteLine("2. Pakene");
                int.TryParse(Console.ReadLine(),out int input);

                Console.WriteLine("==========");
                // Action Choice.
                if (input == 1)
                {
                    int playerDamage = Rnd.Next(5,10);
                    playerDamage -= enemy.Defense;
                    enemy.Health -= playerDamage;
                    Console.WriteLine($"Teit {playerDamage} vauriota.");
                    Console.WriteLine("---------");

                    // Break out of loops if enemy dies.
                    if (enemy.Health <= 0) 
                    {
                        Console.WriteLine($"{enemy.Name} kuoli. Paina nappia jatkaaksesi.");
                        Console.ReadKey();
                        break;
                    }
                }
                else if (input == 2)
                {
                    break;
                }

                // Enemy turn.
                Console.WriteLine("Vihollinen hyökkää");
                int enemyDamage = Rnd.Next(5,10);
                enemyDamage -= player.Defense;
                player.Health -= enemyDamage;
                Console.WriteLine($"Vihollinen teki {enemyDamage} vauriota.");
                Console.WriteLine("==========\n");

                // Break loop if player dies.
                if (player.Health <= 0)
                {
                    Console.WriteLine($"Sinä kuolit. Paina nappia jatkaaksesi.");
                    Console.ReadKey();
                    break;
                }

            }

        }
       
            
    }
}
