using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QuestGame
{
    internal class PeikonKaupunki
    {
        static bool startVisited = false;
        public static void Start(Character player)
        {
            string answer;
            string answerKey;
            bool validInput = false;

            string start1 = "Saavuin Peikonkaupungin porteille. Peikkojen rakentama portti ja kaupungin ympäröivä aita vaikuttivat molemmat heikolta teolta." +
                " Kun astuin portista peremmälle, ryhmä peikkoja antoi minulle varovan katseen. Toivottavasti he eivät ota sitä liian pahalla," +
                " kun varastan muutaman höyhenen heiltä.";
            string start2 = "Peikot ovat kerääntyneet kaupungin keskelle isoon joukkoon. Yksi heistä, joka pitää höyhenkruunua päässä, seisoo lavalla," +
                " huutelemassa jotain tai toista kumppaneilleen. Minun on varmaan parempi välttää kaupungin keskustaa," +
                " jollen halua joutua suoraan peikkojen poliittisen kampanjoinnin keskelle.";
            string start3 = "Miten minä näen sen, voin joko kiertää oikeaa tai vasenta reunusta kaupungista. Oikealla, minä näen jonkinlaisen marketin." +
                " On sielläkin peikkoja runsaasti, mutta he eivät sentään vaikuta poliittisesti motivoidulta sortilta. Sillä välin," +
                " vasemmalla minä näen enemmän illanviettoa varten tarkoitettuja laitoksia; kapakoita, kasinoja ja yökerhoja." +
                " Karkaaminen on vaihtoehto myös, mutta siitä ei hyvä seuraa.\n";
            string startReturn = "Peikonkaupungin porteilla jälleen. Poliittinen kampanjointi ei ole lakannut kaupungin keskustassa, joten en voi sinne vieläkään astua." +
                " Voin mennä joko markettiin, tai yöelämä alueelle. Karkuun meno on epäsuositeltava vaihtoehto.\n";

            //This is a temporary solution for introducing both types of quest.
            Console.WriteLine("Is this a kill quest? (Y/N)");
            answer = Console.ReadLine();

            bool questKill = answer == "y" ? true : false;

            Console.Clear();

            if (!questKill)
            {
                if (startVisited == false)
                {
                    Console.WriteLine("Gather Quest:");
                    TextWriter(start1);
                    Continue();
                    TextWriter(start2);
                    Continue();
                    TextWriter(start3);
                    startVisited = true;
                }
                else
                {
                    TextWriter(startReturn);
                }

                do
                {
                    Console.WriteLine("\n1. Marketti\n2. Yöelämä alue\n3. Palaa aluevalintaan");
                    answerKey = Console.ReadLine();

                    switch (answerKey)
                    {
                        case "1":
                            validInput = true;
                            MarkettiGather(player);
                            break;
                        case "2":
                            validInput = true;
                            YoelamaAlueGather(player);
                            break;
                        case "3":
                            validInput = true;
                            TheGame.ChooseArea(player);
                            break;
                        default:
                            TextWriter("Sopimaton syöttö");
                            break;
                    }
                } while (validInput == false);
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

        static bool marketActionComplete = false;
        static bool marketVisited = false;

        static void MarkettiGather(Character player)
        {
            int amount = Gathering.Gather(player);
            bool validInput = false;
            string answerKey;
            string marketIntro1 = "Kävelin läpi peikko ruuhkien marketille. Molemmat puolet tiestä oli täynnä kauppoja." +
                " Kauppiaat huutelivat ohimeneville tarjouksista ja tuotteista, joita kaikki varmasti tarvitsevat.";
            string marketIntro2 = "“Höyheniä! Höyheniä halvalla!”";
            string marketIntro3 = "No siinäpä viimein diili, johon saatan tarttua—mutta ei se ole ainoa vaihtoehto. Kun katson pidemmälle, näen asutusalueen edessäpäin." +
                " Voin puhua kauppiaalle, mennä asutusalueelle, tai kääntyä takaisin alkuun.\n";
            string marketAction1 = "Kauppias huomasi minun lähestyvän ja nosti ääntään vielä vähän. “Saisiko olla halpoja höyheniä!? Voi käyttää kaikenlaisiin koristeisiin!”";
            string marketAction2 = "Kauppiaan myyntipuhe sai korvani soimaan. “Paljollako irtoaa?” minä kysyin.";
            string marketAction3 = "“Vaikkapa viidelläkymmenellä kolikolla.”";
            string marketAction4 = "“Viisikymmentä kolikkoa? Yhdestä höyhenestä?”";
            string marketAction5 = "“Jep.”";
            string marketAction6 = "“Tuo ei ole tarjous, vaan moottoritie ryöstö”, minä sanoin. “Oletettavasti, sinulla on joku muukin maksutapa mielessä?”";
            string marketAction7 = "“Työ.”";
            string marketAction8 = "“Tietenkin”, minä sanoin huokauksella. “No, mikä on pielessä? Anti tulla vaan.”";
            string marketAction9 = "“Minä haluan tietää mitä kilpailijani veloittaa”, hän sanoi. “Joten, haluan että menet heidän kauppoihinsa kyselemään.”";
            string marketAction10 = "“Mikset mene itse?”";
            string marketAction11 = "“Koska he eivät koskaan kertoisi hintojaan kilpailijalle. Etkö tiedä liikeasioiden perusteitakaan?”";
            string marketAction12 = "“Pakko myöntää, että en”, minä sanoin. “Selvä, menen kyselemään.”";
            string marketAction13 = "Kävelin läpi marketin, kyselemässä jokaiselta kauppiaalta, että paljollako he myyvät sitä sun tätä." +
                " Yksikään heistä ei ollut myymässä höyheniä viidelläkymmenellä kolikolla, joka oli suuri helpotus. Mutta siihen hyvät puolet loppui." +
                " Suurin osa hinnoista oli erittäin paljon korkeammalla kuin niiden pitäisi. Maitoa parilla kympillä, suolaa kolmellakymmenellä ja leipää satasella." +
                " Mitä ihmettä? Nyt kun tulee mieleen, käytämmekö me edes samaa valuuttaa?";
            string marketAction14 = "Palasin alkuperäisen kauppiaan luokse ja raportoin hinnat. “Muuten, mitä valuuttaa te käytätte?”";
            string marketAction15 = "“Kolikoita.”";
            string marketAction16 = "“Mutta mitä kolikoita?”";
            string marketAction17 = "“No kolikoita, kolikoita.”";
            string marketAction18 = "“Voisitko näyttää minulle näitä kolikoita?”";
            string marketAction19 = "Kauppias oli hämmentynyt pyynnöstäni, mutta otti yhden kolikon esiin ja näytti sitä minulle. Se oli tehty puusta, eikä siitä löytynyt mitään valuutan merkkiä." +
                " Vertasin sitä omaani, joka oli tehty kullatusta metallista.";
            string marketAction20 = "“Ilmankos ne hinnat kuulostivat niin korkeilta.”";
            string marketAction21 = $"Kauppias antoi minulle ne höyhenet, jotka hän lupasi. Sain {amount} höyhentä. Tulipa opittua jotain uutta tässä samalla.";
            string marketReturnActionComplete = "Marketti on yhtä vilkas kuin ennenkin. Ei tule mitään mieleen, jota voisin tehdä täällä." +
                " Voin mennä asutusalueelle, tai porteille.\n";
            string marketReturnActionIncomplete1 = "Marketti on yhtä vilkas kuin ennenkin, ja se kauppias on yhä huutelemassa niistä höyhenistä.";
            string marketReturnActionIncomplete2 = "“Höyheniä halvalla! Tulkaa ostamaan höyheniä halvalla!”";
            string marketReturnActionIncomplete3 = "Voin mennä puhumaan kauppiaalle, jatkaa asutusalueelle, tai palata porteille.\n";

            Console.Clear();

            if (marketVisited == false)
            {
                TextWriter(marketIntro1);
                Continue();
                TextWriter(marketIntro2);
                Continue();
                TextWriter(marketIntro3);
                marketVisited = true;
            }
            else if (marketVisited == true && marketActionComplete == true)
            {
                TextWriter(marketReturnActionComplete);
            }
            else
            {
                TextWriter(marketReturnActionIncomplete1);
                Continue();
                TextWriter(marketReturnActionIncomplete2);
                Continue();
                TextWriter(marketReturnActionIncomplete3);
            }

            Console.WriteLine("\n1. Puhu kauppiaalle\n2. Asutusalue\n3. Aloitus");
            answerKey = Console.ReadLine();

            switch (answerKey)
            {
                case "1":
                    validInput = true;
                    if (marketActionComplete == false)
                    {
                        TextWriter(marketAction1);
                        Continue();
                        TextWriter(marketAction2);
                        Continue();
                        TextWriter(marketAction3);
                        Continue();
                        TextWriter(marketAction4);
                        Continue();
                        TextWriter(marketAction5);
                        Continue();
                        TextWriter(marketAction6);
                        Continue();
                        TextWriter(marketAction7);
                        Continue();
                        TextWriter(marketAction8);
                        Continue();
                        TextWriter(marketAction9);
                        Continue();
                        TextWriter(marketAction10);
                        Continue();
                        TextWriter(marketAction11);
                        Continue();
                        TextWriter(marketAction12);
                        Continue();
                        TextWriter(marketAction13);
                        Continue();
                        TextWriter(marketAction14);
                        Continue();
                        TextWriter(marketAction15);
                        Continue();
                        TextWriter(marketAction16);
                        Continue();
                        TextWriter(marketAction17);
                        Continue();
                        TextWriter(marketAction18);
                        Continue();
                        TextWriter(marketAction19);
                        Continue();
                        TextWriter(marketAction20);
                        Continue();
                        TextWriter(marketAction21);
                        Continue();
                        marketActionComplete = true;
                        MarkettiGather(player);
                    }
                    else
                    {
                        TextWriter("Olen puhunut hänelle jo, enkä ole rupatterevalla mielellä.");
                        Continue();
                        MarkettiGather(player);
                    }
                    break;
                case "2":
                    validInput = true;
                    AsutusalueGather(player);
                    break;
                case "3":
                    validInput = true;
                    Start(player);
                    break;
                default:
                    TextWriter("Sopimaton syöttö");
                    break;
            } while (validInput == false);
        }

        static void AsutusalueGather(Character player)
        {
            Console.WriteLine("Asutusalue");
        }

        static void AsemaGather(Character player)
        {
            Console.WriteLine("Asema");
        }

        static void YoelamaAlueGather(Character player)
        {
            Console.WriteLine("Yöelämä alue");
        }

        static void TextWriter(string text)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(10);
            }
        }

        static void Continue()
        {
            Console.WriteLine("\n\n");
            Console.ReadKey();
        }
    }
}
