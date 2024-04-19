using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuestGame
{
    class Enemy
    {
        // Fields.
        protected string _name;
        protected int _health;
        protected int _defense;
        protected int _attackBonus;

        // Properties.
        public string Name
        {
            get { return _name; }
        }
        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }
        public int Defense
        {
            get { return (_defense); }
        }
        public int AttackBonus
        {
            get { return _attackBonus; }
        }

        // Constructors.
        public Enemy(string name, int health, int defense, int attackbonus)
        {
            _name = name;
            _health = health;
            _defense = defense;
            _attackBonus = attackbonus;
        }
        public Enemy(Enemy enemy)
        {
            _name = enemy.Name;
            _health = enemy.Health;
            _defense = enemy.Defense;
            _attackBonus = enemy.AttackBonus;
        }
    }

    public class Combat
    {
        // Enemy Types
        static List<Enemy> EnemyTypes = new List<Enemy>() {
                new Enemy("zombie",25,2,-1),
                new Enemy("mörkö",50,0,2),
                new Enemy("rotta",10,-2,0),
                new Enemy("rosvo",40,2,2) };

        // Random
        static Random Rnd = new Random();
        // Text character writing speed.
        static int textSpeed = 30;

        // Needs player object from main.
        public static void Battle(Character player)
        {
            Console.Clear();
            // Simple system just repeating for the enemy amount, if enemies drop something might have to be redone.
            // or just make a seperate drop function after battle to collect loot.

            // choose EnemyType
            int enemyType = 1;
            int enemyAmount = 3;
            int enemyCount = enemyAmount;

            // loop for amount of enemies.
            for (int i = 0; i < enemyAmount; i++)
            {
                if (player.Health <= 0)
                    break;
                Enemy enemy = new Enemy(EnemyTypes[enemyType]);
                // Combat
                while (true)
                {

                    // Player turn.
                    Console.WriteLine("---------");
                    Console.WriteLine("Vihollisia jäljellä: " + enemyCount);
                    Console.WriteLine($"Vihollinen: {enemy.Name} | Elämä: {enemy.Health} | Puolustus: {enemy.Defense} | Hyökkäys: {enemy.AttackBonus}\n");
                    Console.WriteLine($"Pelaaja: {player.Name} | Elämä {player.Health} | Puolustus: {player.Defense} | Hyökkäys: {player.AttackBonus}");
                    Console.WriteLine("---------");
                    // Player actions text
                    string plActionsText = "\nToimi:\n1. Hyökkää\n2. Pakene\n";
                    foreach (char c in plActionsText)
                    {
                        Console.Write(c);
                        Thread.Sleep(textSpeed);
                    }
                    //Console.WriteLine("\nToimi:");
                    //Console.WriteLine("1. Hyökkää");
                    //Console.WriteLine("2. Pakene");

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
                        int playerDamage = Rnd.Next(4, 24);
                        playerDamage += player.AttackBonus;
                        playerDamage -= enemy.Defense;
                        enemy.Health -= playerDamage;
                        // player attack damage text.
                        string plAtkText = $"Teit {playerDamage} vauriota.\n---------\n";
                        foreach (char c in plAtkText)
                        {
                            Console.Write(c);
                            Thread.Sleep(textSpeed);
                        }
                        //Console.WriteLine($"Teit {playerDamage} vauriota.");
                        //Console.WriteLine("\n---------");

                        // Enemy Death.
                        if (enemy.Health <= 0)
                        {
                            // HP restore after kill
                            int HPrestore = Rnd.Next(10, 20);
                            player.Health += HPrestore;
                            // Enemy death notification text.
                            string enemyDeathText = $"{enemy.Name} kuoli.\nSaat {HPrestore} hpta takaisin\n";
                            foreach (char c in enemyDeathText)
                            {
                                Console.Write(c);
                                Thread.Sleep(textSpeed);
                            }
                            //Thread.Sleep(100);
                            //Console.WriteLine($"{enemy.Name} kuoli.");
                            //Thread.Sleep(100);
                            //Console.WriteLine($"saat {HPrestore} hpta takaisin");
                            // 
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
                        // Player fleeing text.
                        string plFleeText = "Juoksit pakoon taistelusta.";
                        foreach (char c in plFleeText)
                        {
                            Console.Write(c);
                            Thread.Sleep(textSpeed);
                        }
                        //Console.WriteLine("Juoksit pakoon taistelusta.");
                        Thread.Sleep(50);
                        Console.WriteLine("Paina nappia jatkaaksesi");
                        Console.ReadKey(true);
                        break;
                    }

                    Thread.Sleep(100);

                    // Enemy turn.
                    int enemyDamage = Rnd.Next(2, 12);
                    enemyDamage += enemy.AttackBonus;
                    enemyDamage -= player.Defense;
                    player.Health -= enemyDamage;

                    // Text for enemy attacks
                    string enemyAtkText = $"Vihollinen hyökkää!\n";
                    foreach (char c in enemyAtkText)
                    {
                        Console.Write(c);
                        Thread.Sleep(textSpeed);
                    }
                    Thread.Sleep(500);
                    // Text for enemy attack damage
                    string enemyDmgText = $"Vihollinen teki {enemyDamage} vauriota.\n==========";
                    foreach (char c in enemyDmgText)
                    {
                        Console.Write(c);
                        Thread.Sleep(textSpeed);
                    }
                    //Console.WriteLine("Vihollinen hyökkää");
                    //Console.WriteLine($"Vihollinen teki {enemyDamage} vauriota.");
                    //Console.WriteLine("\n==========\n\n");

                    // Player Death.
                    if (player.Health <= 0)
                    {
                        PlayerDeath();
                        break;
                    }
                    Thread.Sleep(1500);
                    Console.Clear();
                }
            }
        }

        public static void EnemyDeath()
        {

        }
        public static void PlayerDeath()
        {
            string plDeathText = $"Sinä kuolit.\nPaina nappia jatkaaksesi.";
            foreach (char c in plDeathText)
            {
                Console.Write(c);
                Thread.Sleep(textSpeed);
            }
            //Console.WriteLine($"Sinä kuolit. Paina nappia jatkaaksesi.");
            Console.ReadKey(true);
        }
    }
}
