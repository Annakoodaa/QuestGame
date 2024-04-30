using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace QuestGame
{
    public class Quest
    {
        // Fields
        protected string _action;
        protected string _target;
        protected int _assignedAmount;
        protected int _amountLeft;
        protected bool _killQuest;
        protected bool _questCompleted;

        // Properties
        public int AmountLeft
        {
            get
            {
                return _amountLeft;
            }
        }
        public int AssignedAmount
        {
            get { return _assignedAmount; }
        }
        public bool KillQuest
        {
            get { return _killQuest; }
        }
        public bool QuestCompleted
        {
            get { return  _questCompleted; }
        }

        // Constructors
        public Quest(string action, string target, int amount)
        {
            _action = action;
            _target = target;
            _amountLeft = amount;
            _assignedAmount = amount;
            _killQuest = action == "tapa" ? true : false;
        }

        public void QuestDescription()
        {
            string actionFormatted = "";
            switch (_action)
            {
                case "tapa":
                    actionFormatted = "tappaa";
                    break;
                case "kerää":
                    actionFormatted = "kerätä";
                    break;
                default:
                    throw new Exception("Unknown action!");
            }

            // Quest text
            string questDescriptionText = $"Minun tehtäväni on {actionFormatted} {_assignedAmount} {_target}.\n" +
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

        public void QuestUI()
        {
            string actionFormatted = "";
            switch (_action)
            {
                case "tapa":
                    actionFormatted = "tapettu";
                    break;
                case "kerää":
                    actionFormatted = "kerätty";
                    break;
                default:
                    throw new Exception("Unknown action!");
            }

            string questUI = $"Tehtävä: [{_assignedAmount - _amountLeft}/{_assignedAmount}] {_target} {actionFormatted}.";
            Console.Write(new string(' ', (Console.WindowWidth - questUI.Length) / 2));
            Console.WriteLine(questUI);
            string divider = "***********************************\n\n";
            Console.Write(new string(' ', (Console.WindowWidth - divider.Length) / 2));
            Console.WriteLine(divider);
        }

        public void QuestProgress(int amount, string target)
        {
            if(target == _target)
            {
                _amountLeft -= amount;
            }
            if(AmountLeft <= 0)
            {
                _questCompleted = true;
            }
        }
    }

    public class QuestFactory
    {
        // Random
        static Random s_rnd = new Random();

        public static Quest QuestGenerator()
        {
            // Randomizing quest type.
            var action = new List<string> { "kerää", "tapa" };
            int actionIndex = s_rnd.Next(action.Count);

            // Randomizing amount.
            int amount = s_rnd.Next(6, 18);

            // Gathering quest
            if (actionIndex == 0)
            {
                // Randomizing gather target.
                var targetObject = new List<string> { "sientä", "höyhentä", "kolikkoa" };
                int targetIndex = s_rnd.Next(targetObject.Count);

                return new Quest(action[actionIndex], targetObject[targetIndex], amount);
            }
            // Kill quest
            else if (actionIndex == 1)
            {
                // Randomizing kill target.
                var targetEnemy = new List<string> { "rottaa", "mörköä", "rosvoa" };            
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
