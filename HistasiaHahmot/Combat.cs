using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuestGame
{
    public class Combat
    {
        // Enemy Types
        static List<Enemy> EnemyTypes = new List<Enemy>() {
                new Enemy("zombie",25,2,-1),
                new Enemy("mörkö",50,0,2),
                new Enemy("rotta",10,-2,0),
                new Enemy("rosvo",40,2,2) };

        // Random
        static Random s_rnd = new Random();

        // Needs player object from main.
        internal static void Battle(Character player, int enemyType, int enemyAmount)
        {
            Console.Clear();

            // Balance
            // Player
            int plMaxDmg = 24;
            int plMinDmg = 4;
            // Player Health Restore
            int plHpRestMax = 20;
            int plHpRestMin = 10;
            // Enemy
            int enMaxDmg = 12;
            int enMinDmg = 2;

            // TEST: choose EnemyType
            //int enemyType = 1;
            //int enemyAmount = 1;
            int enemyCount = enemyAmount;
            // TEST END

            bool fleeing = false;

            // loop for amount of enemies.
            for (int i = 0; i < enemyAmount; i++)
            {
                if (player.Health <= 0 || fleeing == true)
                    break;
                Enemy enemy = new Enemy(EnemyTypes[enemyType]);
                // Combat
                while (true)
                {

                    // Player turn.
                    Console.WriteLine("---------");
                    Console.WriteLine("Vihollisia jäljellä: " + enemyCount);
                    Console.WriteLine($"Vihollinen: {enemy.Name} | Elämä: {enemy.Health} | Puolustus: {enemy.Defense} | Hyökkäys: {enemy.AttackBonus}\n");
                    Console.WriteLine($"{player.Name}: {player.CharacterClass} | Elämä {player.Health} | Puolustus: {player.Defense} | Hyökkäys: {player.AttackBonus}");
                    Console.WriteLine("---------");
                    // Player actions text
                    string plActionsText = "\nToimi:\n1. Hyökkää\n2. Pakene\n";
                    Utilities.TextWriter(plActionsText);

                    //input check
                    bool validInput = false;
                    ConsoleKeyInfo pressedKey = new ConsoleKeyInfo();
                    while (!validInput)
                    {
                        pressedKey = Console.ReadKey(true);
                        switch (pressedKey.KeyChar)
                        {
                            case '1':
                                validInput = true;
                                break;
                            case '2':
                                validInput = true;
                                break;
                            default:
                                break;
                        }
                    }

                    Console.WriteLine("==========");

                    // Action Choices.
                    // Attack
                    if (pressedKey.KeyChar == '1')
                    {
                        int playerDamage = s_rnd.Next(plMinDmg, plMaxDmg + 1);
                        playerDamage += player.AttackBonus;
                        playerDamage -= enemy.Defense;
                        enemy.Health -= playerDamage;
                        // player attack damage text.
                        string plAtkText = $"Teit {playerDamage} vauriota.\n---------\n";
                        Utilities.TextWriter(plAtkText);

                        // Enemy Death.
                        if (enemy.Health <= 0)
                        {
                            // HP restore after kill
                            int HPrestore = s_rnd.Next(plHpRestMin, plHpRestMax + 1);
                            player.Health += HPrestore;
                            // Enemy death notification text.
                            string enemyDeathText = $"{enemy.Name} kuoli.\nSaat {HPrestore} hpta takaisin\n";
                            Utilities.TextWriter(enemyDeathText);
                            enemyCount -= 1;
                            Thread.Sleep(50);
                            Console.WriteLine("Paina nappia jatkaaksesi");
                            Console.ReadKey(true);
                            break;
                        }
                    }
                    // Flee
                    else if (pressedKey.KeyChar == '2')
                    {
                        Console.Clear();
                        fleeing = true;
                        // Player fleeing text.
                        string plFleeText = "Juoksit pakoon taistelusta.\n";
                        Utilities.TextWriter(plFleeText);
                        //Console.WriteLine("Juoksit pakoon taistelusta.");
                        Thread.Sleep(50);
                        Console.WriteLine("Paina nappia jatkaaksesi");
                        Console.ReadKey(true);
                        break;
                    }

                    Thread.Sleep(100);

                    // Enemy turn.
                    int enemyDamage = s_rnd.Next(enMinDmg, enMaxDmg + 1);
                    enemyDamage += enemy.AttackBonus;
                    enemyDamage -= player.Defense;
                    player.Health -= enemyDamage;

                    // Text for enemy attacks
                    string enemyAtkText = $"Vihollinen hyökkää!\n";
                    Utilities.TextWriter(enemyAtkText);
                    Thread.Sleep(500);
                    // Text for enemy attack damage
                    string enemyDmgText = $"Vihollinen teki {enemyDamage} vauriota.";
                    Utilities.TextWriter(enemyDmgText);
                    // Player Death.
                    if (player.Health <= 0)
                    {
                        PlayerDeath();
                        break;
                    }
                    Thread.Sleep(1500);
                    Console.Clear();
                }
                Console.Clear();
            }
        }

        public static void EnemyDeath()
        {

        }

        public static void PlayerDeath()
        {
            string plDeathText = $"\nSinä kuolit.\nPaina nappia jatkaaksesi.";
            Utilities.TextWriter(plDeathText);
            //Console.WriteLine($"Sinä kuolit. Paina nappia jatkaaksesi.");
            Console.ReadKey(true);
        }
    }
}
