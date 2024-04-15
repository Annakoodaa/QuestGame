using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame
{
    class tempEnemy
    {
        public string Name = "Zombi";
        public int Health = 25;
        public int Defense = 1;
        public int AttackBonus = -1;
    }

    public class Combat
    {
        static Random Rnd = new Random();

        // ongelmana tarvii tuoda Characterin player olio mainistä.
        public static void Battle(Character player)
        {
            Console.Clear();
            tempEnemy enemy = new tempEnemy();

            // Combat
            while (true)
            {

                // Player turn.
                Console.WriteLine($"Vihollinen: {enemy.Name} | Elämä: {enemy.Health} | Puolustus: {enemy.Defense} | Hyökkäys: {enemy.AttackBonus}\n");
                Console.WriteLine($"Pelaaja: {player.Name} | Elämä {player.Health} | Puolustus: {player.Defense} | Hyökkäys: {player.AttackBonus}\n");
                Console.WriteLine("---------");
                Console.WriteLine("Toimi:");
                Console.WriteLine("1. Hyökkää");
                Console.WriteLine("2. Pakene");
                int.TryParse(Console.ReadLine(), out int input);

                Console.WriteLine("==========");
                // Action Choice.
                if (input == 1)
                {
                    int playerDamage = Rnd.Next(5, 10);
                    playerDamage += player.AttackBonus;
                    playerDamage -= enemy.Defense;
                    enemy.Health -= playerDamage;
                    Console.WriteLine($"Teit {playerDamage} vauriota.");
                    Console.WriteLine("---------");

                    // Enemy Death.
                    if (enemy.Health <= 0)
                    {
                        // HP restoren tapon jälkeen
                        int HPrestore = Rnd.Next(5, 15);
                        player.Health += HPrestore;
                        // Vihollisen kuoleman ilmoittaminen.
                        Console.WriteLine($"{enemy.Name} kuoli.");
                        Console.WriteLine($"saat {HPrestore} hpta takaisin");
                        Console.WriteLine("Paina nappia jatkaaksesi");
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
                int enemyDamage = Rnd.Next(5, 10);
                enemyDamage += enemy.AttackBonus;
                enemyDamage -= player.Defense;
                player.Health -= enemyDamage;
                Console.WriteLine($"Vihollinen teki {enemyDamage} vauriota.");
                Console.WriteLine("==========\n\n");

                // Player Death.
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
