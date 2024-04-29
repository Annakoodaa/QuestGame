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

        static bool asutusalueActionComplete = false;
        static bool asutusalueVisited = false;

        static void AsutusalueGather(Character player)
        {
            int amount = Gathering.Gather(player);
            bool validInput = false;
            string answerKey;
            string asutusalueIntro1 = "Marketista eteen päin, löydän itseni asutusalueelta. Täältä ei paljoa ruuhkaa löydy," +
                " sillä suurin osa peikoista on joko kaupungin keskustassa tai marketissa. Se ruuhka mitä löytyy, tosin, on lapsia. He leikkivät asutusalueen teillä," +
                " piittaamatta muista ohikulkijoista. Ennen pitkään, yksi heistä tömähti minuun ja lensi maahan.";
            string asutusalueIntro2 = "“Sori herra!”";
            string asutusalueIntro3 = "Ennen kuin ehdin kysyä häneltä, että oliko hän kunnossa, hän juoksi jo pois. Kylläpä näillä tenavilla riittää energiaa." +
                " Katsoin heidän leikkiä hetken aikaa, ja huomasin että he kilpailevat eräästä höyhenkruunusta." +
                " Ehkä voisin liittyä kilpailuun yrittääkseni saada sen kruunun itselleni, tai voin myös jatkaa matkaa edessä päin näkyvälle portille ja vartioiden asemalle." +
                " Voin tietysti myös mennä takaisin markettiin.\n";
            string asutusalueAction1 = "Kävelin lapsien luokse ja kysyin mitä he tekivät.";
            string asutusalueAction2 = "“Me kilpaillaan höyhenkruunusta!” yksi heistä vastasi innolla.";
            string asutusalueAction3 = "“Voinko liittyä mukaan?”";
            string asutusalueAction4 = "Lapset vaikuttivat epävarmoilta. “Osaatko sinä muka leikkiä hippaa?”";
            string asutusalueAction5 = "“Osaan,” minä sanoin. Jos kyseessä on hippa, tämän pitäisi olla helppoa.";
            string asutusalueAction6 = "Lapset hyväksyivät minut heidän kilpailuunsa ja minusta tehtiin jäänyt. Heti kun aloitus aika oli loppu," +
                " ryntäsin lasten perään—mutta tein pienen lasku virheen tähän ryhtyessä. Nämä lapset ovat nopeita!" +
                " Miten ihmeessä heidän pienet jalkansa pystyvät pinkomaan tämmöistä vauhtia!?";
            string asutusalueAction7 = "En aio kuitenkaan hävitä. Heti kun pääsin kädenpituuteen yhdestä tenavasta, kurotin kaikin voimin ja juuri osuin hänen selkäänsä." +
                " “Jäit kiinni,” minä sanoin. Tenavalla kesti hetki käsittää mitä tapahtui, jonka minä käytin hyödyksi hyppäämällä ylös." +
                " Sain kiinni yläpuolellani olevasta parvekkeesta ja vedin itseni sen päälle.";
            string asutusalueAction8 = "“Hei! Ei tuo ole reilua!” tenava huudahti.";
            string asutusalueAction9 = "“Elämä harvemmin on”, minä väitin.";
            string asutusalueAction10 = "Ja siinä oli minun toinen laskuvirhe.";
            string asutusalueAction11 = "Lapset päättivät, että jos minä pelaan epäreilusti, heidän pitää pelata epäreilusti." +
                " He asettivat eroavaisuutensa sivulle ja alkoivat yrittää saada minut kiinni. Yksi heistä jäi odottamaan parvekkeen alle," +
                " samalla kun kaksi muuta ryntäsivät asuntoon. Ennen kun tiesin mitä ajatella, olin piiritetty.";
            string asutusalueAction12 = "“Antaudu, herra!” yksi huusi.";
            string asutusalueAction13 = "“Olet piiritetty!” toinen lisäsi.";
            string asutusalueAction14 = "“Olenko tosiaan?” minä kysyin. “Ette sattuneet lähettämään jotakuta yläkertaan, vai mitä?”";
            string asutusalueAction15 = "Lapset vaikuttivat yllättyneiltä.";
            string asutusalueAction16 = "“Otan tuon ei:nä,” minä sanoin, ennen kuin kurotin seuraavan kerroksen parvekkeelle. Lapset syöksyivät minua kohti," +
                " mutta ehdin vetää itseni ylös ennen kuin he pääsivät minuun kiinni. Päätin tehdä asioista vielä vähän vaikeampaa heille," +
                " joten hyppäsin vastapäätä olevan lyhyemmän rakennuksen katolle parvekkeelta.";
            string asutusalueAction17 = "Kun lapset viimein pääsivät parvekkeelle, he olivat ällikällä. “Mitä sinä siellä teet!?”";
            string asutusalueAction18 = "“Olen karkuteillä. Mitä muutakaan?”";
            string asutusalueAction19 = "Nauruni selvästi sai tenavat ärsyyntyneiksi. Kolmas laskuvirhe; yksi heistä on uhkarohkea." +
                " Uhkarohkea tenava juoksi ja loikkasi minua kohti, jättäen tukevan maan taakseen—mutta hän on vasta lapsi. Se oli minullekin melko vaikea hyppy," +
                " joten hänellä ei ollut mitään mahdollisuutta yltää toiselle puolelle.";
            string asutusalueAction20 = "Juoksin katon reunalle ja kurotin lasta kohti. Sain hänen ranteestaan kiinni, mutta hän painoi enemmän kuin odotin." +
                " Menetin tasapainoni ja tipahdin reunan yli—sain kuitenkin katon reunasta vielä toisella kädellä kiinni, kiitos hyvien refleksini.";
            string asutusalueAction21 = "“Herra!” yksi peikkolapsi, joka oli vielä parvekkeen turvallisemmalla puolella, huusi.";
            string asutusalueAction22 = "“Mene hakemaan vanhempasi!” minä huusin takaisin.";
            string asutusalueAction23 = "“S-selvä!”";
            string asutusalueAction24 = "Uhkarohkea tenava piteli minun kädestäni kiinni kaikin voimin. Nostin hänet katolle ensin, ja seurasin perässä." +
                " Hän vaikutti erittäin pelästyneeltä, ihan syystäkin.";
            string asutusalueAction25 = "“Älä tee noin enää”, minä sanoin.";
            string asutusalueAction26 = "“En”, hän vannoi.";
            string asutusalueAction27 = "“Hyvä.”";
            string asutusalueAction28 = "Muutamassa minuutissa, kuulin meteliä alhaalta. Peikkojen vanhemmat oli hakenut vartijat auttamaan meitä." +
                " Vartijat laittoivat tikkaat talon seinustalle ja hakivat uhkarohkean tenavan pois, ennen kuin jäivät tukemaan tikkaita alhaalta samalla kun tulin alas itse.";
            string asutusalueAction29 = "Sain kuunnella moitteita peikkojen vanhemmilta jonkin aikaa. Ennen kuin jatkoin matkaa," +
                $" uhkarohkea tenava toi höyhenkruunun minulle kiitoksena. Sain {amount} höyhentä.";
            string asutusalueReturnActionIncomplete = "Palasin asutusalueelle, eikä lasten kilpailu ole vieläkään lakannut." +
                " Voisin pyytää liittyä mukaan, tai mennä vartioiden asemalle, tai mennä markettiin.\n";
            string asutusalueReturnActionComplete = "Palasin asutusalueelle. Lapset ovat turvassa omassa kotonaan, mutta se myös tarkoittaa," +
                " ettei minulla ole mitään tekemistä täällä. Voin mennä markettiin tai vartioiden asemalle.\n";

            Console.Clear();

            if (asutusalueVisited == false)
            {
                TextWriter(asutusalueIntro1);
                Continue();
                TextWriter(asutusalueIntro2);
                Continue();
                TextWriter(asutusalueIntro3);
                asutusalueVisited = true;
            }
            else if (asutusalueVisited == true && asutusalueActionComplete == true)
            {
                TextWriter(asutusalueReturnActionComplete);
            }
            else
            {
                TextWriter(asutusalueReturnActionIncomplete);
            }

            do
            {
                Console.WriteLine("\n1. Liity lasten leikkiin\n2. Asema\n3. Marketti");
                answerKey = Console.ReadLine();

                switch (answerKey)
                {
                    case "1":
                        validInput = true;
                        TextWriter(asutusalueAction1);
                        Continue();
                        TextWriter(asutusalueAction2);
                        Continue();
                        TextWriter(asutusalueAction3);
                        Continue();
                        TextWriter(asutusalueAction4);
                        Continue();
                        TextWriter(asutusalueAction5);
                        Continue();
                        TextWriter(asutusalueAction6);
                        Continue();
                        TextWriter(asutusalueAction7);
                        Continue();
                        TextWriter(asutusalueAction8);
                        Continue();
                        TextWriter(asutusalueAction9);
                        Continue();
                        TextWriter(asutusalueAction10);
                        Continue();
                        TextWriter(asutusalueAction11);
                        Continue();
                        TextWriter(asutusalueAction12);
                        Continue();
                        TextWriter(asutusalueAction13);
                        Continue();
                        TextWriter(asutusalueAction14);
                        Continue();
                        TextWriter(asutusalueAction15);
                        Continue();
                        TextWriter(asutusalueAction16);
                        Continue();
                        TextWriter(asutusalueAction17);
                        Continue();
                        TextWriter(asutusalueAction18);
                        Continue();
                        TextWriter(asutusalueAction19);
                        Continue();
                        TextWriter(asutusalueAction20);
                        Continue();
                        TextWriter(asutusalueAction21);
                        Continue();
                        TextWriter(asutusalueAction22);
                        Continue();
                        TextWriter(asutusalueAction23);
                        Continue();
                        TextWriter(asutusalueAction24);
                        Continue();
                        TextWriter(asutusalueAction25);
                        Continue();
                        TextWriter(asutusalueAction26);
                        Continue();
                        TextWriter(asutusalueAction27);
                        Continue();
                        TextWriter(asutusalueAction28);
                        Continue();
                        TextWriter(asutusalueAction29);
                        Continue();
                        asutusalueActionComplete = true;
                        AsutusalueGather(player);
                        break;
                    case "2":
                        validInput = true;
                        AsemaGather(player);
                        break;
                    case "3":
                        validInput = true;
                        MarkettiGather(player);
                        break;
                    default:
                        TextWriter("Sopimaton syöttö");
                        break;
                }
            } while (validInput == false);
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
