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

        // Combat Related
        readonly int maxHealth = 100;
        int health = 100;
        public int _defense { get; set; }
        public int _attackBonus { get; set; }
        public int _gatheringSkill { get; set; }

        private int _strengthModifier = 0;
        private int _intelligenceModifier = 0;
        private int _dodgeHitModifier = 0;

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
        public int StrengthModifier
        {
            get { return _strengthModifier; }
        }
        public int IntelligenceModifier
        {
            get { return _intelligenceModifier; }
        }
        public int DodgeHitModifier
        {
            get { return _dodgeHitModifier; }
        }

        public int AttackBonus
        {
            get { return _attackBonus + StrengthModifier; }
        }
        public int Defense
        {
            get { return _defense + _dodgeHitModifier; }
        }
        public int GatheringSkill
        {
            get { return _gatheringSkill + _intelligenceModifier;  }
        }

        // Constructors
        public Character(string name)
        {
            Name = name;
        }


        public void SetTrait(string trait)
        {
            switch (trait)
            {
                case "Vahva":
                    _strengthModifier += 5; // Voimaa lisääntyy +X
                    break;
                case "Notkea":
                    _dodgeHitModifier += 3; // Väistömahdollisuus lisääntyy +X
                    break;
                case "Viisas":
                    _intelligenceModifier += 5; // Älykkyyttä lisääntyy +X
                    break;
                case "Heikko":
                    _strengthModifier -= 5; // Voimaa vähennetään +X
                    break;
                case "Hölmö":
                    _intelligenceModifier -= 5; // Älykkyyttä vähennetään +X
                    break;
                case "Ajattelematon":
                    _dodgeHitModifier -= 3; // Väistömahdollisuus vähennetään +X
                    break;
            }
        }


        public void PrintInfo()
        {
            Console.WriteLine("Pelaajahahmosi:");
            Console.WriteLine($"Hahmo: {Name} \nVoima ({_strengthModifier}): {GetStrengthTrait()} \n" +
                $"Älykkyyden ({_intelligenceModifier}): {GetIntelligenceTrait()} \nVäistö ({_dodgeHitModifier}): {GetDodgeTrait()}");
            Console.WriteLine("***********************************\n");
        }

        private string GetStrengthTrait()
        {
            if (_strengthModifier > 0)
                return "Vahva";
            else if (_strengthModifier < 0)
                return "Heikko";
            else
                return "Ei mitään";
        }

        private string GetIntelligenceTrait()
        {
            if (_intelligenceModifier > 0)
                return "Viisas";
            else if (_intelligenceModifier < 0)
                return "Hölmö";
            else
                return "Ei mitään";
        }

        private string GetDodgeTrait()
        {
            if (_dodgeHitModifier > 0)
                return "Notkea";
            else if (_dodgeHitModifier < 0)
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
            string[] allTraits = { "Vahva", "Notkea", "Viisas", "Heikko", "Hölmö", "Ajattelematon" };
            return allTraits[random.Next(allTraits.Length)];
        }

        static Character GenerateCharacter()
        {
            string[] names = { "Heimopäällikkö", "Sotilas", "Kyläläinen" };
            string name = names[random.Next(names.Length)];

            var character = new Character(name);

            character.SetTrait(GenerateTrait()); // Voima
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

            // Char info printing
            player.PrintInfo();

            TheGame.StartAdventure(player);

            Console.ReadKey(true);
        }
    }

}
