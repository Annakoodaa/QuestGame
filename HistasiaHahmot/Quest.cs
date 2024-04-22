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
        // Random
        static Random s_rnd = new Random();

        static Quest()
        {
            //var paikka = new List<string> { "Mustametsä", "Kyläpahanen", "Peikonkaupunki", "Kuolemanjärven kunta" };
            Console.WriteLine("\nLuodaan sinulle tehtävä.");
            Console.WriteLine("\nEnsimmäiseksi valitaan sinulle action!");
            var random = new Random();
            var action = new List<string> { "Kerää", "Tapa" };
            int actionIndex = random.Next(action.Count);
            Console.WriteLine(action[actionIndex]);
            Random maara = new Random();
            int amount = maara.Next(10, 50);

            // Gathering quest
            if (actionIndex == 0)
            {
                var targetObject = new List<string> { "sieniä", "timantteja", "höyheniä", "kolikoita" };
                int targetIndex = s_rnd.Next(targetObject.Count);
                Console.WriteLine(targetObject[targetIndex]);

                QuestDescription(action[actionIndex], targetObject[targetIndex], amount);
            }
            // Kill quest
            if (actionIndex == 1)
            {
                var targetEnemy = new List<string> { "rottia", "mörköjä", "zombeja", "rosvoja" };
                int targetIndex = s_rnd.Next(targetEnemy.Count);
                Console.WriteLine(targetEnemy[targetIndex]);

                QuestDescription(action[actionIndex], targetEnemy[targetIndex], amount);
            }
        }

        static void QuestDescription(string action, string kohde, int maara)
        {
            string questDescriptionText = $"Minun tehtäväni on {action} {maara} {kohde}.\n" +
                "Voin mennä Mustametsään, Kyläpahaseen, Peikonkaupunkiin, tai Kuolemanjärven kunnalle.\n" +
                "Mustametsä on tunnettu huonosta näkyvyydestään, joka jättää seikkailijat heikoiksi hirviöiden yllätyshyökkäyksille,\n" +
                "mutta sen varjoisasta ja kosteasta ympäristöstä voi helposti löytyä kaikenlaisia sieniä.\n" +
                "Kyläpahanen on pieni kylä keskellä suurin piirtein turvallista maakuntaa. Siellä voi rosvojen uhriksi joutua,\n" +
                "tai jyrsijät saattavat alkaa maistella kantapäitä, mutta ei mitään sen vaarallisempaa—sitä paitsi, rosvojen\n" +
                "kätköistä voi helposti löytyä kolikoita oman taskun painottamiseksi.\n" +
                "Peikonkaupunki on melko ilmiselvä konsepti. Se on kaupunki täynnä peikkoja. Peikot useimmiten koristavat asusteitaan\n" +
                "höyhenillä.\n" +
                "Kuolemanjärven kunta on nykyään pelkkä kasa romua asuntojen muodossa. Zombeja liikkuu ympäri ämpäri läpi vuorokauden.\n" +
                "Mutta huhut kertovat siellä olleen suuri aarrekammio. Kaikenlaisia jalokiviä varmasti löytyisi kammion sisimmistä\n" +
                "Jos se on olemassa.";

            foreach(char c in questDescriptionText)
            {
                Console.Write(c);
                Thread.Sleep(10);
            }
        }
    }
}
