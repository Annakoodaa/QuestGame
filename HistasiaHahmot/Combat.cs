using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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
    }

    public class Combat
    {
        // Enemy Types
        static List<Enemy> enemies = new List<Enemy>() {
                new Enemy("zombie",25,2,-1),
                new Enemy("mörkö",50,0,2),
                new Enemy("rotta",10,-2,0),
                new Enemy("rosvo",40,2,2) };

        // Random
        static Random Rnd = new Random();

        // ongelmana tarvii tuoda Characterin player olio mainistä.
        public static void Battle(Character player)
        {
            Console.Clear();
            var enemy = enemies[1];

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
                    int playerDamage = Rnd.Next(2, 12);
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
                        Console.ReadKey(true);
                        break;
                    }
                }
                // Flee
                else if (pressedKey.KeyChar == '2')
                {
                    Console.Clear();
                    Console.WriteLine("Juoksit pakoon taistelusta.");
                    Console.WriteLine("Paina nappia jatkaaksesi");
                    Console.ReadKey(true);
                    break;
                }

                // Enemy turn.
                Console.WriteLine("Vihollinen hyökkää");
                int enemyDamage = Rnd.Next(2, 12);
                enemyDamage += enemy.AttackBonus;
                enemyDamage -= player.Defense;
                player.Health -= enemyDamage;
                Console.WriteLine($"Vihollinen teki {enemyDamage} vauriota.");
                Console.WriteLine("==========\n\n");

                // Player Death.
                if (player.Health <= 0)
                {
                    Console.WriteLine($"Sinä kuolit. Paina nappia jatkaaksesi.");
                    Console.ReadKey(true);
                    break;
                }
            }
        }        
    }
}
