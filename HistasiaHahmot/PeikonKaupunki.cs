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
        static Random rnd = new Random();
        static bool startVisited = false;
        static bool answered = false;
        static bool questKill;
        static int enemyType = 2;
        static int enemyAmount = rnd.Next(2, 6);
        static string enemyName = "rottaa";
        static string gatheringType = "höyhentä";

        public static void Start(Character player, Quest quest)
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
            string end = "Astuin Peikonkaupungin porteista ulos ja jäin odottamaan seuraavaa kärryä. Kohta pääsen raportoimaan tehtävän suoritetuksi ja saan päivän palkkani.";

            Console.Clear();

            if (!quest.KillQuest)
            {
                if (startVisited == false)
                {
                    quest.QuestUI();

                    TextWriter(start1);
                    Continue();
                    TextWriter(start2);
                    Continue();
                    TextWriter(start3);
                    startVisited = true;
                }
                else
                {
                    quest.QuestUI();

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
                            MarkettiGather(player, quest);
                            break;
                        case "2":
                            validInput = true;
                            YoelamaAlueGather(player, quest);
                            break;
                        case "3":
                            validInput = true;
                            if (quest.QuestCompleted)
                            {
                                TextWriter(end);
                                Continue();
                                TheGame.ChooseArea(player, quest);
                            }
                            else
                            {
                                TheGame.ChooseArea(player, quest);
                            }
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
                    quest.QuestUI();

                    TextWriter(start1);
                    Continue();
                    TextWriter(start2);
                    Continue();
                    TextWriter(start3);
                    startVisited = true;
                }
                else
                {
                    quest.QuestUI();

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
                            MarkettiKill(player, quest);
                            break;
                        case "2":
                            validInput = true;
                            YoelamaAlueKill(player, quest);
                            break;
                        case "3":
                            validInput = true;
                            if (quest.QuestCompleted)
                            {
                                TextWriter(end);
                                Continue();
                                TheGame.ChooseArea(player, quest);
                            }
                            else
                            {
                                TheGame.ChooseArea(player, quest);
                            }
                            break;
                        default:
                            TextWriter("Sopimaton syöttö");
                            break;
                    }
                } while (validInput == false);
            }
        }

        static bool marketActionComplete = false;
        static bool marketVisitedGather = false;

        static void MarkettiGather(Character player, Quest quest)
        {
            int amount = Gathering.Gather(player); // Quest Tracking implemented under comment: // Quest Progress
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

            if (marketVisitedGather == false)
            {
                quest.QuestUI();

                TextWriter(marketIntro1);
                Continue();
                TextWriter(marketIntro2);
                Continue();
                TextWriter(marketIntro3);
                marketVisitedGather = true;
            }
            else if (marketVisitedGather == true && marketActionComplete == true)
            {
                quest.QuestUI();

                TextWriter(marketReturnActionComplete);
            }
            else
            {
                quest.QuestUI();

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
                        Console.Clear();

                        quest.QuestUI();

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
                        TextWriter(marketAction21); // You feathers here?
                        // Quest Progress
                        quest.QuestProgress(amount,gatheringType);
                        Continue();
                        marketActionComplete = true;
                        MarkettiGather(player, quest);
                    }
                    else
                    {
                        quest.QuestUI();

                        TextWriter("Olen puhunut hänelle jo, enkä ole rupatterevalla mielellä.");
                        Continue();
                        MarkettiGather(player, quest);
                    }
                    break;
                case "2":
                    validInput = true;
                    AsutusalueGather(player, quest);
                    break;
                case "3":
                    validInput = true;
                    Start(player, quest);
                    break;
                default:
                    TextWriter("Sopimaton syöttö");
                    break;
            } while (validInput == false);
        }

        static bool asutusalueActionComplete = false;
        static bool asutusalueVisitedGather = false;

        static void AsutusalueGather(Character player, Quest quest)
        {
            int amount = Gathering.Gather(player); // Quest Tracking implemented under comment: // Quest Progress
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

            if (asutusalueVisitedGather == false)
            {
                quest.QuestUI();

                TextWriter(asutusalueIntro1);
                Continue();
                TextWriter(asutusalueIntro2);
                Continue();
                TextWriter(asutusalueIntro3);
                asutusalueVisitedGather = true;
            }
            else if (asutusalueVisitedGather == true && asutusalueActionComplete == true)
            {
                quest.QuestUI();

                TextWriter(asutusalueReturnActionComplete);
            }
            else
            {
                quest.QuestUI();

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
                        if (asutusalueActionComplete == false)
                        {
                            Console.Clear();

                            quest.QuestUI();

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
                            // Quest Progress
                            quest.QuestProgress(amount, gatheringType);
                            Continue();
                            asutusalueActionComplete = true;
                            AsutusalueGather(player, quest);
                        }
                        else
                        {
                            quest.QuestUI();

                            TextWriter("Lapset ovat sisällä lepäämässä. Ei ole ketään jonka kanssa leikkiä.");
                            Continue();
                            AsutusalueGather(player, quest);
                        }
                        break;
                    case "2":
                        validInput = true;
                        AsemaGather(player, quest);
                        break;
                    case "3":
                        validInput = true;
                        MarkettiGather(player, quest);
                        break;
                    default:
                        TextWriter("Sopimaton syöttö");
                        break;
                }
            } while (validInput == false);
        }

        static bool asemaActionComplete = false;
        static bool asemaVisitedGather = false;
        static void AsemaGather(Character player, Quest quest)
        {
            int amount = Gathering.Gather(player); // Quest Tracking implemented under comment: // Quest Progress
            bool validInput = false;
            string answerKey;
            string asemaIntro1 = "Saavuin Peikonkaupungin takaporteille. Niiden vieressä sijaitsee vartijoiden asema," +
                " jonka ympärillä on runsaasti haarniskoihin pukeutuneita peikkoja. Osa heistä vaikuttavat vain laiskottelevan, kun taas toiset vartioivat.";
            string asemaIntro2 = "Laiskottelijoiden joukossa on kolmikko, jotka pelaavat korttia tynnyrin päällä. Heillä on myös tavaroita tynnyrin päällä," +
                " jotka ovat todennäköisesti heidän pottinsa. Huomasin höyheniä yhdessä potissa.";
            string asemaIntro3 = "Voisin liittyä peliin ja yrittää voittaa koko potin. Voin myös mennä asutusalueelle tai yöelämä alueelle.\n";
            string asemaAction1 = "Liityin vartijoiden joukkoon tynnyrin ympärillä. He katsoivat minua odottavasti, joten laitoin tynnyrille pottini; koristellun puukon." +
                " Vartijat vaikuttivat tyytyväisiltä ja alkoivat sekoittamaan kortteja.";
            string asemaAction2 = $"Viisitoista minuuttia myöhemmin, olen {amount} höyhentä rikkaampi. Onnetar oli korttieni puolella tänään.";
            string asemaReturnActionIncomplete = "Asemalla on yhä yhtä monipuolista tekemistä kuin ennen. Kolmikko yhä pelaa kortteja." +
                " Voin liittyä heidän seuraan, mennä asutusalueelle, tai mennä yöelämä alueelle.\n";
            string asemaReturnActionComplete = "Asemalla on yhä yhtä monipuolista tekemistä kuin ennen, mutta en näe enää mitään syytä liittyä mukaan." +
                " Voin mennä asutusalueelle tai yöelämä alueelle.\n";

            Console.Clear();

            if (asemaVisitedGather == false)
            {
                quest.QuestUI();

                TextWriter(asemaIntro1);
                Continue();
                TextWriter(asemaIntro2);
                Continue();
                TextWriter(asemaIntro3);
                asemaVisitedGather = true;
            }
            else if (asemaVisitedGather == true && asemaActionComplete == true)
            {
                quest.QuestUI();

                TextWriter(asemaReturnActionComplete);
            }
            else
            {
                quest.QuestUI();

                TextWriter(asemaReturnActionIncomplete);
            }

            do
            {
                Console.WriteLine("\n1. Liity korttipeliin\n2. Asutusalue\n3. Yöelämä alue");
                answerKey = Console.ReadLine();

                switch (answerKey)
                {
                    case "1":
                        validInput = true;
                        if (asemaActionComplete == false)
                        {
                            Console.Clear();

                            quest.QuestUI();

                            TextWriter(asemaAction1);
                            Continue();
                            TextWriter(asemaAction2);
                            // Quest Progress
                            quest.QuestProgress(amount, gatheringType);
                            Continue();
                            asemaActionComplete = true;
                            AsemaGather(player, quest);
                        }
                        else
                        {
                            quest.QuestUI();

                            TextWriter("En näe mitään syytä liittyä seuraan toista kertaa.");
                            Continue();
                            AsemaGather(player, quest);
                        }
                        break;
                    case "2":
                        validInput = true;
                        AsutusalueGather(player, quest);
                        break;
                    case "3":
                        validInput = true;
                        YoelamaAlueGather(player, quest);
                        break;
                    default:
                        TextWriter("Sopimaton syöttö");
                        break;
                }
            } while (validInput == false);
        }

        static bool yoelamaAlueActionComplete = false;
        static bool yoelamaAlueVisitedGather = false;
        static void YoelamaAlueGather(Character player, Quest quest)
        {
            int amount = Gathering.Gather(player); // Quest Tracking implemented under comment: // Quest Progress
            bool validInput = false;
            string answerKey;
            string yoelamaAlueIntro1 = "Koko vasen puoli Peikonkaupungista vaikutti olevan yöelämää varten. Kasinoissa peikot joko voittivat kaiken tai menettivät sen." +
                " Kapakoissa jotkin peikot ryyppäsivät jo nyt. Yökerhot olivat vielä kiinni.";
            string yoelamaAlueIntro2 = "Kun kävelin läpi yöelämä alueen, huomasin tavaraa erään kapakan penkeillä. Juopot olivat vissiin unohtaneet joitain tavaroitaan." +
                " Tavaroiden joukossa oli höyheniä.";
            string yoelamaAlueIntro3 = "Voisin koittaa ottaa höyhenet itselleni, mennä vartioiden asemalle, tai mennä alkuun.\n";
            string yoelamaAlueAction1 = "Astuin kapakkaan ja koitin ottaa höyhenet baarimikon huomaamatta. Epäonnistuin.";
            string yoelamaAlueAction2 = "“Hei, mitäs koitat?” baarimikko kysyi.";
            string yoelamaAlueAction3 = "“Minä vain... Tuota noin... Olin matkalla ulos.”";
            string yoelamaAlueAction4 = "“Niinpä niin. Jos ne höyhenet sinua niin paljon kiinnostaa, ota vain.”";
            string yoelamaAlueAction5 = "“Onko tässä jonkinlainen juju?”";
            string yoelamaAlueAction6 = "“Ei mitään jujua! Se joka ne tänne jätti oli täysi mäntti, joten minua ei sen kummemmin kiinnosta," +
                " jos hänen jättämänsä tavarat maagisesti katoavat.”";
            string yoelamaAlueAction7 = "Päätin olla väittämättä vastaan. Otin höyhenet ja muut joukossa olevat tavarat, ennen kuin lähdin ripeästi kapakasta." +
                $" Sain {amount} höyhentä.";
            string yoelamaAlueReturnActionIncomplete = "Yöelämä alueella ei vieläkään ole paljoa ruuhkaa. Kapakan penkillä sijaitsevia höyheniä ei olla tultu hakemaan vielä." +
                " Voin mennä vartioiden asemalle tai alkuun.\n";
            string yoelamaAlueReturnActionComplete = "Yöelämä alueella ei vieläkään ole paljoa ruuhkaa. Voin mennä vartioiden asemalle tai alkuun.\n";

            Console.Clear();

            if (yoelamaAlueVisitedGather == false)
            {
                quest.QuestUI();

                TextWriter(yoelamaAlueIntro1);
                Continue();
                TextWriter(yoelamaAlueIntro2);
                Continue();
                TextWriter(yoelamaAlueIntro3);
                yoelamaAlueVisitedGather = true;
            }
            else if (yoelamaAlueVisitedGather == true && yoelamaAlueActionComplete == true)
            {
                quest.QuestUI();

                TextWriter(yoelamaAlueReturnActionComplete);
            }
            else
            {
                quest.QuestUI();

                TextWriter(yoelamaAlueReturnActionIncomplete);
            }

            do
            {
                Console.WriteLine("\n1. “Lainaa” kapakasta höyheniä\n2. Asema\n3. Aloitus");
                answerKey = Console.ReadLine();

                switch (answerKey)
                {
                    case "1":
                        validInput = true;
                        if (yoelamaAlueActionComplete == false)
                        {
                            Console.Clear();

                            quest.QuestUI();

                            TextWriter(yoelamaAlueAction1);
                            Continue();
                            TextWriter(yoelamaAlueAction2);
                            Continue();
                            TextWriter(yoelamaAlueAction3);
                            Continue();
                            TextWriter(yoelamaAlueAction4);
                            Continue();
                            TextWriter(yoelamaAlueAction5);
                            Continue();
                            TextWriter(yoelamaAlueAction6);
                            Continue();
                            TextWriter(yoelamaAlueAction7);
                            // Quest Progress
                            quest.QuestProgress(amount, gatheringType);
                            Continue();
                            yoelamaAlueActionComplete = true;
                            YoelamaAlueGather(player, quest);
                        }
                        else
                        {
                            quest.QuestUI();

                            TextWriter("Minulla ei ole enään mitään asiaa siellä.");
                            Continue();
                            YoelamaAlueGather(player, quest);
                        }
                        break;
                    case "2":
                        validInput = true;
                        AsemaGather(player, quest);
                        break;
                    case "3":
                        validInput = true;
                        Start(player, quest);
                        break;
                    default:
                        TextWriter("Sopimaton syöttö");
                        break;
                }
            } while (validInput == false);
        }

        static bool marketVisitedKill = false;
        static void MarkettiKill(Character player, Quest quest)
        {
            bool validInput = false;
            string answerKey;
            string markettiIntro1 = "Kävelin läpi peikko ruuhkien marketille. Molemmat puolet tiestä oli täynnä kauppoja." +
                " Kauppiaat huutelivat ohimeneville tarjouksista ja tuotteista, joita kaikki varmasti tarvitsevat.";
            string markettiIntro2 = "Tehtävän antoni mukaisesti, kysyin ohi mennessäni kauppiailta, oliko heillä ongelmia rottien kanssa.";
            string markettiIntro3 = "“No, kyllä,” vastaus kuului.";
            string markettiIntro4 = "Kauppias vei minut kauppansa takahuoneeseen, jossa näin rottia juoksentelevan ympäriinsä." +
                " Tartuin miekkaani ja valmistauduin hoitelemaan ne.";
            string markettiIntro5 = "Tämän prosessin pääsin toistamaan vielä muutaman kerran muille kauppiaille, mutta loppujen lopuksi, marketti oli käyty läpi." +
                " Voin edetä asutusalueelle, tai mennä takaisin portille.\n";
            string markettiReturn = "Marketti on yhtä vilkas kuin ennenkin. Ei tule mitään mieleen, jota voisin tehdä täällä. Voin mennä asutusalueelle, tai porteille.\n";

            Console.Clear();

            if (marketVisitedKill == false)
            {
                quest.QuestUI();

                TextWriter(markettiIntro1);
                Continue();
                TextWriter(markettiIntro2);
                Continue();
                TextWriter(markettiIntro3);
                Continue();
                TextWriter(markettiIntro4);
                Continue();
                // Combat and Quest Progress
                int killAmount = Combat.Battle(player, enemyType, enemyAmount);
                quest.QuestProgress(killAmount, enemyName);
                // Combat and Quest Progress end
                TextWriter(markettiIntro5);
                marketVisitedKill = true;
            }
            else
            {
                quest.QuestUI();

                TextWriter(markettiReturn);
            }

            do
            {
                Console.WriteLine("\n1. Asutusalue\n2. Aloitus");
                answerKey = Console.ReadLine();

                switch(answerKey)
                {
                    case "1":
                        validInput = true;
                        AsutusalueKill(player, quest);
                        break;
                    case "2":
                        validInput = true;
                        Start(player, quest);
                        break;
                    default:
                        TextWriter("Sopimaton syöttö");
                        break;
                }
            } while (validInput == false);
        }

        static bool asutusalueVisitedKill = false;
        static void AsutusalueKill(Character player, Quest quest)
        {
            bool validInput = false;
            string answerKey;
            string asutusalueIntro1 = "Marketista eteen päin, löydän itseni asutusalueelta. Täältä ei paljoa ruuhkaa löydy," +
                " sillä suurin osa peikoista on joko kaupungin keskustassa tai marketissa. Se ruuhka mitä löytyy, tosin, on lapsia. He leikkivät asutusalueen teillä," +
                " piittaamatta muista ohikulkijoista. Ennen pitkään, yksi heistä tömähti minuun ja lensi maahan.";
            string asutusalueIntro2 = "“Sori herra!”";
            string asutusalueIntro3 = "Ennen kuin ehdin kysyä häneltä, että oliko hän kunnossa, hän juoksi jo pois. Kylläpä näillä tenavilla riittää energiaa." +
                " Kunpa voisin sanoa samoin omalta osaltani. Aloin koputtamaan ovelle oven jälkeen," +
                " kiertämässä suuria asutusalueella sijaitsevia kerrostaloja hitaasti mutta varmasti.";
            string asutusalueIntro4 = "“Rottiako?” asukas kysyi. “No, kyllä niitä löytyy. Tule peremmälle vain.”";
            string asutusalueIntro5 = "Astuin asunnon syövereihin, jossa jälleen kuulin pikkuruista taaperrusta. Rotat yrittävät paeta.";
            string asutusalueIntro6 = "Asunto hoidettu, seuraava. Seuraava. Vielä seuraava.";
            string asutusalueIntro7 = ". . .";
            string asutusalueIntro8 = "Lopulta, pääsin käytyä kaikki asutusalueen asunnot läpi, mutta kyllä se kesti myöskin." +
                " Voin edetä vartioiden asemalle tai palata markettiin.\n";
            string asutusalueReturn = "Palasin asutusalueelle. Olen käynyt koko paikan läpi, joten minulla ei tule tekemistä täällä mieleen." +
                " Voin mennä markettiin tai vartioiden asemalle.\n";

            Console.Clear();

            if (asutusalueVisitedKill == false)
            {
                quest.QuestUI();

                TextWriter(asutusalueIntro1);
                Continue();
                TextWriter(asutusalueIntro2);
                Continue();
                TextWriter(asutusalueIntro3);
                Continue();
                TextWriter(asutusalueIntro4);
                Continue();
                TextWriter(asutusalueIntro5);
                Continue();
                // Combat and Quest progress
                int killAmount = Combat.Battle(player, enemyType, enemyAmount);
                quest.QuestProgress(killAmount, enemyName);
                // Combat and Quest Progress end
                TextWriter(asutusalueIntro6 + "\n\n");
                TextWriter(asutusalueIntro7 + "\n\n");
                TextWriter(asutusalueIntro8);
                asutusalueVisitedKill = true;
            }
            else
            {
                quest.QuestUI();

                TextWriter(asutusalueReturn);
            }

            do
            {
                Console.WriteLine("\n1. Asema\n2. Marketti");
                answerKey = Console.ReadLine();

                switch (answerKey)
                {
                    case "1":
                        validInput = true;
                        AsemaKill(player, quest);
                        break;
                    case "2":
                        validInput = true;
                        MarkettiKill(player, quest);
                        break;
                    default:
                        TextWriter("Sopimaton syöttö");
                        break;
                }
            } while (validInput == false);
        }

        static bool asemaVisitedKill = false;
        static void AsemaKill(Character player, Quest quest)
        {
            bool validInput = false;
            string answerKey;
            string asemaIntro1 = "Saavuin Peikonkaupungin takaporteille. Niiden vieressä sijaitsee vartijoiden asema," +
                " jonka ympärillä on runsaasti haarniskoihin pukeutuneita peikkoja. Osa heistä vaikuttavat vain laiskottelevan, kun taas toiset vartioivat.";
            string asemaIntro2 = "Menin yhdelle vartioista kysymään, että onko rotta ongelmia. Hän johdatti minut kellariin, jonka molemmat seinämät olivat tyrmillä täytetty." +
                " Rottia näkyi enemmän kuin rikollisia tosin.";
            string asemaIntro3 = "Rottien hoideltua, minut talutettiin suoraan kellarista ulos. Aika jatkaa matkaa yöelämä alueelle tai takaisin asutusalueelle.\n";
            string asemaReturn = "Asemalla on yhä yhtä monipuolista tekemistä kuin ennen, mutta en näe mitään syytä liittyä mukaan." +
                " Voin mennä asutusalueelle tai yöelämä alueelle.\n";

            Console.Clear();

            if (asemaVisitedKill == false)
            {
                quest.QuestUI();

                TextWriter(asemaIntro1);
                Continue();
                TextWriter(asemaIntro2);
                Continue();
                // Combat and Quest progress
                int killAmount = Combat.Battle(player, enemyType, enemyAmount);
                quest.QuestProgress(killAmount, enemyName);
                // Combat and Quest Progress end
                TextWriter(asemaIntro3);
                asemaVisitedKill = true;
            }
            else
            {
                quest.QuestUI();

                TextWriter(asemaReturn);
            }

            do
            {
                Console.WriteLine("\n1. Asutusalue\n2. Yöelämä alue");
                answerKey = Console.ReadLine();

                switch (answerKey)
                {
                    case "1":
                        validInput = true;
                        AsutusalueKill(player, quest);
                        break;
                    case "2":
                        validInput = true;
                        YoelamaAlueKill(player, quest);
                        break;
                    default:
                        TextWriter("Sopimaton syöttö");
                        break;
                }
            } while (validInput == false);
        }

        static bool yoelamaAlueVisitedKill = false;
        static void YoelamaAlueKill(Character player, Quest quest)
        {
            bool validInput = false;
            string answerKey;
            string yoelamaAlueIntro1 = "Koko vasen puoli Peikonkaupungista vaikutti olevan yöelämää varten. Kasinoissa peikot joko voittivat kaiken tai menettivät sen." +
                " Kapakoissa jotkin peikot ryyppäsivät jo nyt. Yökerhot olivat vielä kiinni.";
            string yoelamaAlueIntro2 = "Kun kävelin läpi yöelämä alueen, kyselin jokaisesta laitoksesta, että onko rotta ongelmia. Ja lähes joka kerta," +
                " minut päästettiin sisään tekemään työni. Saman voi kertoa tällä kertaa. Toivottavasti miekka ei tylsy tämän jatkuvan maan tökkimisen johdosta.";
            string yoelamaAlueIntro3 = "Viimeisen laitoksen läpi käytyä, huokaisin helpotuksesta. Voin joko mennä vartioiden asemalle, tai palata porteille.\n";
            string yoelamaAlueReturn = "Yöelämä alueella ei vieläkään ole paljoa ruuhkaa. Voin mennä vartioiden asemalle tai alkuun.\n";

            Console.Clear();

            if (yoelamaAlueVisitedKill == false)
            {
                quest.QuestUI();

                TextWriter(yoelamaAlueIntro1);
                Continue();
                TextWriter(yoelamaAlueIntro2);
                Continue();
                // Combat and Quest Progress
                int killAmount = Combat.Battle(player, enemyType, enemyAmount);
                quest.QuestProgress(killAmount, enemyName);
                // Combat and Quest Progress end
                TextWriter(yoelamaAlueIntro3);
                yoelamaAlueVisitedKill = true;
            }
            else
            {
                quest.QuestUI();

                TextWriter(yoelamaAlueReturn);
            }

            do
            {
                Console.WriteLine("\n1. Asema\n2. Aloitus");
                answerKey = Console.ReadLine();

                switch (answerKey)
                {
                    case "1":
                        validInput = true;
                        AsemaKill(player, quest);
                        break;
                    case "2":
                        validInput = true;
                        Start(player, quest);
                        break;
                    default:
                        TextWriter("Sopimaton syöttö");
                        break;
                }
            } while (validInput == false);
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
