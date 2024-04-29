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
        protected int _assignedAmount;
        int _amountLeft;

        // Properties
        public int AmountLeft
        {
            get
            {
                return _amountLeft;
            }
            set
            {
                _amountLeft = value;
            }
        }

        // Constructors
        public Quest(string action, string target, int amount)
        {
            _action = action;
            _target = target;
            _amountLeft = amount;
            _assignedAmount = amount;
        }

        public void QuestDescription()
        {
            // Quest text
            string questDescriptionText = $"Minun tehtäväni on {_action} {_target} {_assignedAmount}.\n" +
                "Voin mennä Mustametsään, Kyläpahaseen tai Peikonkaupunkiin.\n" +
                "Mustametsä on tunnettu huonosta näkyvyydestään, joka jättää seikkailijat heikoiksi hirviöiden yllätyshyökkäyksille,\n" +
                "mutta sen varjoisasta ja kosteasta ympäristöstä voi helposti löytyä kaikenlaisia sieniä.\n" +
                "Kyläpahanen on pieni kylä keskellä suurin piirtein turvallista maakuntaa. Siellä voi rosvojen uhriksi joutua,\n" +
                "tai jyrsijät saattavat alkaa maistella kantapäitä, mutta ei mitään sen vaarallisempaa—sitä paitsi, rosvojen\n" +
                "kätköistä voi helposti löytyä kolikoita oman taskun painottamiseksi.\n" +
                "Peikonkaupunki on melko ilmiselvä konsepti. Se on kaupunki täynnä peikkoja. Peikot useimmiten koristavat asusteitaan\n" +
                "höyhenillä.\n";

            // Text Writing
            Utilities.TextWriter(questDescriptionText);
        }
    }

    public class QuestFactory
    {
        // Random
        static Random s_rnd = new Random();

        public static Quest QuestGenerator()
        {
            //var paikka = new List<string> { "Mustametsä", "Kyläpahanen", "Peikonkaupunki" };
            //Console.WriteLine("\nLuodaan sinulle tehtävä.");
            var random = new Random();
            var action = new List<string> { "kerää", "tapa" };
            int actionIndex = random.Next(action.Count);
            Random maara = new Random();
            int amount = maara.Next(10, 50);

            // Gathering quest
            if (actionIndex == 0)
            {
                var targetObject = new List<string> { "sieniä", "höyheniä", "kolikoita" };

                int targetIndex = s_rnd.Next(targetObject.Count);

                return new Quest(action[actionIndex], targetObject[targetIndex], amount);
            }
            // Kill quest
            else if (actionIndex == 1)
            {
                var targetEnemy = new List<string> { "rottia", "mörköjä", "rosvoja" };
              
                int targetIndex = s_rnd.Next(targetEnemy.Count);

                return new Quest(action[actionIndex], targetEnemy[targetIndex], amount);
            }
            else
            {
                throw new Exception("Quest Index out of range");
            }

        }
    }
}
