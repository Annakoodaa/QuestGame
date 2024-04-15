﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame
{
   
    class Character
    {
        public string Name { get; set; }
        public string GameCharacter { get; set; }
        public string GoodTrait { get; set; }
        public string BadTrait { get; set; }

        public Character(string name, string goodTrait, string badTrait)
        {
            Name = name;          
            GoodTrait = goodTrait;
            BadTrait = badTrait;
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Hahmo: {Name} \nHyvä ominaisuus: {GoodTrait} \nHuono ominaisuus: {BadTrait}");
        }
    }

    class Program
    {
        static Random random = new Random();
        static string GenerateGoodTrait()
        {
            string[] goodTraits = { "Vahva", "Notkea", "Viisas", "Nopea", "Ei mitään"};
            return goodTraits[random.Next(goodTraits.Length)];
        }

        static string GenerateBadTrait()
        {
            string[] badTraits = { "Heikko", "Hidas", "Hölmö", "Ajattelematon", "Ei mitään" };
            return badTraits[random.Next(badTraits.Length)];
        }

        static Character GenerateCharacter()
        {
            string[] names = {"Heimopäällikkö", "Sotilas", "Kyläläinen",  };
            string name = names[random.Next(names.Length)];
            //int health = random.Next(50, 101);


            string goodTrait = GenerateGoodTrait();
            string badTrait = GenerateBadTrait();

            return new Character(name, goodTrait, badTrait);
        }
        //static Character GenerateEnemy()
        //{
        //    string[] names = { "Kuningas", "Trollit", "Monsterit" };
        //    string name = names[random.Next(names.Length)];
        //    int health = random.Next(50, 101);
        
        //    string goodTrait = GenerateGoodTrait();
        //    string badTrait = GenerateBadTrait();

        //    return new Character(name, goodTrait, badTrait);
        //}

            static void Main(string[] args)
        {
            Console.WriteLine("Hei! Generoidaan sinulle hahmo!");
            Console.WriteLine("\nAnna hahmollesi nimi: ");           
            string playerName = Console.ReadLine();
            Console.WriteLine("");
            Console.WriteLine($"Tervetuloa {playerName}!\n\nSeuraavaksi saat tiedot hahmostasi.");
            Console.WriteLine("***********************************"); 
            Character player = GenerateCharacter();
          

            Console.WriteLine("Pelaajahahmosi:");
            player.PrintInfo();
            Console.WriteLine("***********************************\n");

            var Quest = new Quest();

            Console.ReadKey();
        }
    }

}
