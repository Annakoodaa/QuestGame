using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuestGame
{
    public class Quest
    {
        // Fields
        string _action;
        string _target;
        int _amount;

        // Properties
        public int Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                _amount = value;
            }
        }

        // Constructors
        public Quest(string action, string target, int amount)
        {
            _action = action;
            _target = target;
            _amount = amount;
        }

        public void QuestDescription()
        {
            string questDescriptionText = $"Minun tehtäväni on {_action} {_target} {_amount}.\n" +
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
                "Jos se on olemassa.\n";

            foreach (char c in questDescriptionText)
            {
                Console.Write(c);
                Thread.Sleep(10);
            }
        }
    }

    public class QuestFactory
    {
        // Random
        static Random s_rnd = new Random();

        public static Quest QuestGenerator()
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

                return new Quest(action[actionIndex], targetObject[targetIndex], amount);
            }
            // Kill quest
            else if (actionIndex == 1)
            {
                var targetEnemy = new List<string> { "rottia", "mörköjä", "zombeja", "rosvoja" };
                int targetIndex = s_rnd.Next(targetEnemy.Count);
                Console.WriteLine(targetEnemy[targetIndex]);

                return new Quest(action[actionIndex], targetEnemy[targetIndex], amount);
            }
            else
            {
                throw new Exception("Quest Index out of range");
            }

        }
    }
}
