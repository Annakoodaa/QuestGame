using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuestGame
{
    class Character
    {
        public string Name { get; set; }
        public string GameCharacter { get; set; }
        public string GoodTrait { get; set; }
        public string BadTrait { get; set; }

        // Combat Related
        readonly int maxHealth = 100;
        int health = 100;
        public int Defense { get; set; }
        public int AttackBonus { get; set; }
        public int GatheringSkill { get; set; }

        // Properties
        public int Health
        {
            get
            {
                return health;
            }
            set
            {
                if (value > maxHealth)
                {
                    health = maxHealth;
                }
                else
                {
                    health = value;
                }
            }
        }

        // Constructors
        public Character(string name, string goodTrait, string badTrait)
        {
            Name = name;
            GoodTrait = goodTrait;
            BadTrait = badTrait;
        }

        public void PrintInfo()
        {
            Console.WriteLine("Pelaajahahmosi:");
            Console.WriteLine($"Hahmo: {Name} \nHyvä ominaisuus: {GoodTrait} \nHuono ominaisuus: {BadTrait}");
            Console.WriteLine("***********************************\n");
        }
    }

    class Program
    {
        static Random random = new Random();

        static string GenerateGoodTrait()
        {
            string[] goodTraits = { "Vahva", "Notkea", "Viisas", "Nopea", "Ei mitään" };
            return goodTraits[random.Next(goodTraits.Length)];
        }

        static string GenerateBadTrait()
        {
            string[] badTraits = { "Heikko", "Hidas", "Hölmö", "Ajattelematon", "Ei mitään" };
            return badTraits[random.Next(badTraits.Length)];
        }

        static Character GenerateCharacter()
        {
            string[] names = { "Heimopäällikkö", "Sotilas", "Kyläläinen", };
            string name = names[random.Next(names.Length)];


            string goodTrait = GenerateGoodTrait();
            string badTrait = GenerateBadTrait();

            return new Character(name, goodTrait, badTrait);
        }

            static void Main(string[] args)
        {
            Console.WriteLine("Hei! Generoidaan sinulle hahmo!");
            Console.WriteLine("\nAnna hahmollesi nimi: ");
            string playerName = Console.ReadLine();
            Console.WriteLine("");
            Console.WriteLine($"Tervetuloa {playerName}!\n\nSeuraavaksi saat tiedot hahmostasi.");
            Console.WriteLine("***********************************");
            Character player = GenerateCharacter();

            // Char info printing
            player.PrintInfo();

            // Temp Stats
            player.GatheringSkill = 2;
            player.AttackBonus = 5;

            TheGame.StartAdventure(player);

            Console.ReadKey(true);
        }
    }

}
