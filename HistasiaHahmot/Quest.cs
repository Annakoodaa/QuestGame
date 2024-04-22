using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuestGame
{
    internal class Quest
    {
        static Quest()
        {
            var paikka = new List<string> { "Mustametsä", "Kyläpahanen", "Peikonkaupunki", "Kuolemanjärven kunta" };
            Console.WriteLine("\nLuodaan sinulle tehtävä.");
            Console.WriteLine("\nEnsimmäiseksi valitaan sinulle toiminta!");
            var random = new Random();
            var toiminta = new List<string> { "Kerää", "Tapa" };
            int index = random.Next(toiminta.Count);
            Console.WriteLine(toiminta[index]);
            Random maara = new Random();
            int maara1 = maara.Next(10, 50);

            // Gathering quest
            if (index == 0)
            {
                Console.WriteLine("\nSitten valitaan keräämisen kohde!");
                var random1 = new Random();
                var kohdeTavara = new List<string> { "sieniä", "timantteja", "höyheniä", "kolikoita" };
                int index1 = random1.Next(kohdeTavara.Count);
                Console.WriteLine(kohdeTavara[index1]);

                Console.WriteLine($"\nSeuraavaksi määritellään monia {kohdeTavara[index1]} sinun pitää kerätä.");
             
                Console.WriteLine(maara1);

                Console.WriteLine("\nPaikka mistä nämä löytyy on:");       
                var random3 = new Random();
                int index3 = random3.Next(paikka.Count);
                Console.WriteLine(paikka[index3]);

                Console.WriteLine($"\nTehtäväsi on: \n*{toiminta[index]} {kohdeTavara[index1]} {maara1}kpl paikassa {paikka[index3]}.*");

            }
            // Kill quest
            if (index == 1)
            {
                Console.WriteLine("\nSitten valitaan tappamisen kohde!");

                var kohdeVihollinen = new List<string> { "rottia", "mörköjä", "zombeja", "rosvoja" };
                var random2 = new Random();
                int index2 = random2.Next(kohdeVihollinen.Count);
                Console.WriteLine(kohdeVihollinen[index2]);

                Console.WriteLine($"\nSeuraavaksi määritellään monia {kohdeVihollinen[index2]} sinun pitää tappaa:");
                //Random maara = new Random(); dfgdfg
                //int maara1 = maara.Next(10, 50);
                Console.WriteLine(maara1);

                Console.WriteLine("\nPaikka mistä nämä löytyy on:");
                var random3 = new Random();
                int index3 = random3.Next(paikka.Count);
                Console.WriteLine(paikka[index3]);

                Console.WriteLine($"\nTehtäväsi on: \n*{toiminta[index]} {kohdeVihollinen[index2]} {maara1}kpl paikassa {paikka[index3]}.*");
            }
        }
    }
}
