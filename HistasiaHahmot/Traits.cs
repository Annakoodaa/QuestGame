using System;
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
        private int strengthModifier = 0;
        private int speedModifier = 0;
        private int intelligenceModifier = 0;
        private int DodgeHitModifier = 0;

        public Character(string name)
        {
            Name = name;
        }

        public void SetTrait(string trait)
        {
            switch (trait)
            {
                case "Vahva":
                    strengthModifier += 10; // Voimaa lisääntyy 10%
                    break;
                case "Notkea":
                    DodgeHitModifier += 5; // Väistömahdollisuus lisääntyy 10%
                    break;
                case "Viisas":
                    intelligenceModifier += 5; // Älykkyyttä lisääntyy 10%
                    break;
                case "Nopea":
                    speedModifier += 10; // Nopeus lisääntyy 10%
                    break;
                case "Heikko":
                    strengthModifier -= 10; // Voimaa vähennetään 10%
                    break;
                case "Hidas":
                    speedModifier -= 10; // Nopeus vähenee 10%
                    break;
                case "Hölmö":
                    intelligenceModifier -= 5; // Älykkyyttä vähennetään 10%
                    break;
                case "Ajattelematon":
                    DodgeHitModifier -= 5; // Väistömahdollisuus vähennetään 10%
                    break;
            }
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Hahmo: {Name} \nVoima ({strengthModifier}%): {GetStrengthTrait()} \nNopeus ({speedModifier}%): {GetSpeedTrait()} \nÄlykkyyden ({intelligenceModifier}%): {GetIntelligenceTrait()} \nVäistö ({DodgeHitModifier}%): {GetDodgeTrait()}");
        }

        private string GetStrengthTrait()
        {
            if (strengthModifier > 0)
                return "Vahva";
            else if (strengthModifier < 0)
                return "Heikko";
            else
                return "Ei mitään";
        }

        private string GetSpeedTrait()
        {
            if (speedModifier > 0)
                return "Nopea";
            else if (speedModifier < 0)
                return "Hidas";
            else
                return "Ei mitään";
        }

        private string GetIntelligenceTrait()
        {
            if (intelligenceModifier > 0)
                return "Viisas";
            else if (intelligenceModifier < 0)
                return "Hölmö";
            else
                return "Ei mitään";
        }

        private  string GetDodgeTrait()
        {
            if (DodgeHitModifier > 0)
                return "Notkea";
            else if (DodgeHitModifier < 0)
                return "Ajattelematon";
            else
                return "Ei mitään";
        }

    }

    class Program
    {
        static Random random = new Random();

        static string GenerateTrait()
        {
            string[] allTraits = { "Vahva", "Notkea", "Viisas", "Nopea", "Heikko", "Hidas", "Hölmö", "Ajattelematon" };
            return allTraits[random.Next(allTraits.Length)];
        }

        static Character GenerateCharacter()
        {
            string[] names = { "Heimopäällikkö", "Sotilas", "Kyläläinen" };
            string name = names[random.Next(names.Length)];

            var character = new Character(name);

            character.SetTrait(GenerateTrait()); // Voima
            character.SetTrait(GenerateTrait()); // Nopeus
            character.SetTrait(GenerateTrait()); // Älykkyys
            character.SetTrait(GenerateTrait()); // Väistö

            return character;
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

            Console.WriteLine("Pelaajahahmosi:");
            player.PrintInfo();
            Console.WriteLine("***********************************\n");

            Console.ReadKey();
        }
    }
}
