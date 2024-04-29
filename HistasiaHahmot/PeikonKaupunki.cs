using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuestGame
{
    internal class PeikonKaupunki
    {
        public static void Start()
        {
            string answer;

            string start1 = "Saavuin Peikonkaupungin porteille. Peikkojen rakentama portti ja kaupungin ympäröivä aita vaikuttivat molemmat heikolta teolta." +
                " Kun astuin portista peremmälle, ryhmä peikkoja antoi minulle varovan katseen. Toivottavasti he eivät ota sitä liian pahalla," +
                " kun varastan muutaman höyhenen heiltä.";
            string start2 = "Peikot ovat kerääntyneet kaupungin keskelle isoon joukkoon. Yksi heistä, joka pitää höyhenkruunua päässä, seisoo lavalla," +
                " huutelemassa jotain tai toista kumppaneilleen. Minun on varmaan parempi välttää kaupungin keskustaa," +
                " jollen halua joutua suoraan peikkojen poliittisen kampanjoinnin keskelle.";
            string start3 = "Miten minä näen sen, voin joko kiertää oikeaa tai vasenta reunusta kaupungista. Oikealla, minä näen jonkinlaisen marketin." +
                " On sielläkin peikkoja runsaasti, mutta he eivät sentään vaikuta poliittisesti motivoidulta sortilta. Sillä välin," +
                " vasemmalla minä näen enemmän illanviettoa varten tarkoitettuja laitoksia; kapakoita, kasinoja ja yökerhoja." +
                " Karkaaminen on vaihtoehto myös, mutta siitä ei hyvä seuraa.";
            string startReturn = "Peikonkaupungin porteilla jälleen. Poliittinen kampanjointi ei ole lakannut kaupungin keskustassa, joten en voi sinne vieläkään astua." +
                " Voin mennä joko markettiin, tai yöelämä alueelle. Karkuun meno on epäsuositeltava vaihtoehto.";
            bool startVisited = false;

            //This is a temporary solution for introducing both types of quest.
            Console.WriteLine("Is this a kill quest? (Y/N)");
            answer = Console.ReadLine();

            bool questKill = answer == "y" ? true : false;

            if (!questKill)
            {
                if (startVisited == false)
                {
                    Console.WriteLine("\nGather Quest:");
                    Console.WriteLine(start1 + "\n");
                    Thread.Sleep(1000);
                    Console.WriteLine(start2 + "\n");
                    Thread.Sleep(1000);
                    Console.WriteLine(start3);
                    startVisited = true;
                }
                else
                {
                    Console.WriteLine(startReturn);
                }

                Console.WriteLine("\n1. Marketti\n2. Yöelämä alue\n3. Palaa aluevalintaan");
            }
            else
            {
                if (startVisited == false)
                {
                    Console.WriteLine("\nKill Quest:");
                    Console.WriteLine(start1 + "\n");
                    Thread.Sleep(1000);
                    Console.WriteLine(start2 + "\n");
                    Thread.Sleep(1000);
                    Console.WriteLine(start3);
                    startVisited = true;
                }
                else
                {
                    Console.WriteLine(startReturn);
                }

                Console.WriteLine("\n1. Marketti\n2. Yöelämä alue\n3. Palaa aluevalintaan");
            }
        }

    }
}
