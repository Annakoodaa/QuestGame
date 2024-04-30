using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuestGame
{
    internal class MustaMetsa
    {

        #region Mustametsä wide fields
        static bool s_hasKey = false;
        static bool s_cellarAttempted = false;
        static int s_enemyType = 1; // Enemy type 1 corresponding to mörkö
        static string s_enemyName = "mörköä";
        static string s_gatherType = "sientä";
        #endregion

        // Add Quest quest Parameter for quest tracking I guess. Also for tracking kill/gather quest type.
        // For now Kill/Gather Field:

        #region Mustametsä entrance
        // Field for returning check
        static bool s_mmReturn = false;
        static bool s_mmFirstReturn = true;

        public static void MMEntrance(Character player, Quest quest)
        {
            Console.Clear();
            if (!s_mmReturn)
            {
                quest.QuestUI();

                // Mustametsä text
                string mmText1 = "Kun astuin mustametsään, tunsin kengänpohjani uppoavan sammaleen. Ilma oli kostea, eikä auringonvalo pilkistänyt tiheän neulaskaton läpi. " +
                    "En nähnyt muutamaa metriä pidemmälle metsän syvyyksiin. Olin valmistautunut tähän. Irrotin lyhtyni vyötäröltäni, ja avasin sen luukun. " +
                    "Luukun ruosteinen ratina avautuessaan kaikui hiljaisuudessa. Se otti muutaman yrityksen, mutta lopulta onnistuin sytyttämään liekin lyhdyn" +
                    " sisällä—nyt toivotaan, ettei se sammu. \n\n";
                string mmText2 = "Nyt kun minulla on valoa, hahmotan ympäristöni enemmässä tarkkuudessa. " +
                    "Vasemmalle johtaa lähes täysin tukkoon kasvanut multapolku. " +
                    "Oikealla, näen pelkkää pusikkoa niin pitkälle kuin silmä kantaa. " +
                    "Edessä päin näen jonkinlaisen mätänevän puurakennuksen. Voin tietenkin myös lähteä takaisin mistä tulin, " +
                    "mutta ilman tarvitsemiani tavaroita, se tarkoittaisi tehtävän hylkäystä.\n\n";

                // Writing text
                Utilities.TextWriter(mmText1);

                Thread.Sleep(Utilities.s_TextChapterDelay);

                Utilities.TextWriter(mmText2);

                s_mmReturn = true;
            }
            else
            {
                quest.QuestUI();

                string mmReturnText = "";
                if (s_mmFirstReturn && quest.KillQuest)
                {
                    mmReturnText = "Astuin jälleen mustametsän alkuun—ja tapasin pienen ongelman. Tai ehkä olisi parempi sanoa “pieniä ongelmia.” Parvi mörköjä oli jäänyt odottamaan minua.";
                }
                else
                {
                    // Mustämetsä return text
                    mmReturnText = "Astuin jälleen mustametsän alkuun. Sammal edelleen uhkailee upottaa jalkani. " +
                        "Voin mennä multapolkua pitkin vasemmalla, mennä metsän syövereihin oikealla, tai tutkia mätänevää puurakennusta. " +
                        "Karkaus on myös vaihtoehto, mutten usko sitä tarpeelliseksi.\n\n";
                }
                // Writing text
                Utilities.TextWriter(mmReturnText);
                // Combat Trigger  {tappelu mörköjä vastaan tähän}
                if (s_mmFirstReturn && quest.KillQuest)
                {
                    s_mmFirstReturn = false;

                    // Combat, Return amount of killed enemies for use in QuestProgress() -method.
                    Utilities.PressToContinue();
                    int killedAmount = Combat.Battle(player,s_enemyType,3);

                    // Quest Progress
                    quest.QuestProgress(killedAmount, s_enemyName);

                    // return to field
                    MMEntrance(player, quest);
                }

            }
            // Selection window delay
            Thread.Sleep(Utilities.s_selectionDelay);
            // 1 multapolku 2. Metsänsyvyydet 3. Rakennus
            Console.Write
                (
                "1. Multapolku\n" +
                "2. Metsänsyvyys\n" +
                "3. Puurakennus\n" +
                "4. Palaa aluevalintaan.\n"
                );

            // Input
            bool validInput = false;
            ConsoleKeyInfo pressedKey = new ConsoleKeyInfo();
            while (!validInput)
            {
                pressedKey = Console.ReadKey(true);
                switch (pressedKey.KeyChar)
                {
                    case char c when (c == '1' || c == '2' || c == '3' || c == '4'):
                        validInput = true;
                        break;
                    default:
                        break;
                }
            }

            switch (pressedKey.KeyChar)
            {
                case '1':
                    MultaPolku(player, quest);
                    break;
                case '2':
                    Metsansyvyydet(player, quest);
                    break;
                case '3':
                    PuuRakennus(player, quest);
                    break;
                case '4':
                    ResetArea();
                    if (quest.QuestCompleted)
                    {
                        string endText = "Palattuani alkupisteeseen, sammutin lyhtyni ja astuin pois märältä sammaleelta vankalle tielle. Aika mennä kotiin ja raportoida tehtävä valmiiksi.";
                        Utilities.TextWriter(endText);
                    }
                    else
                    {
                        TheGame.ChooseArea(player, quest);
                    }                
                    break;
            }
        }
        #endregion

        #region Multapolku
        // Field for returning to center
        static bool s_multaPolkuReturn = false;
        static bool s_mpGathered = false;

        static void MultaPolku(Character player, Quest quest)
        {
            bool enterKellari = false;
            bool returnToStart = false;
            bool prConnection = false;

            while (!returnToStart)
            {
                Console.Clear();

                // Multapolku introduction. Only on first entry.
                if (s_multaPolkuReturn == false)
                {
                    s_multaPolkuReturn = true;

                    // Multapolku Text
                    string mpText1 = "Kävelin multapolkua pitkin useita minuutteja, huitoen samalla puskia ynnä muita pensaita pois tieltäni miekallani. " +
                        "Lopulta, päädyin aukiolle. Aurinko säteili läpi neulaskaton vain tähän tiettyyn kohtaan. Aukio ei vaikuttanut olevan luonnollinen näky, " +
                        "sillä puiden raadot olivat jätetty aukiolla esille, todennäköisesti jotta seikkailijat voivat käyttää niitä istuimina. " +
                        "Aukion reunuksella on runsaasti sieniä. Kun katson aukiota tarkemmin, huomaan että osa maasta näyttää hauraammalta kuin muut. " +
                        "Alan kaivamaan haurasta maata kaksin käsin. Ennen pitkään, maa toi esiin puisen kellariluukun.\n\n";

                    // Kill Quest Specific text 
                    string mpBattleText = "Ennen kuin ehdin kokeilla avata kellariluukkua, kuulin takanani taaperrusta. Käännyttyäni ympäri, tapasin mörkö parven katseet. " +
                        "Vaistoni hyppäsivät kehiin—siirsin jalkani oikeaan asentoon ja tartuin miekkani kahvaan.";

                    //
                    string mpText2 = "Voin kerätä sienet ympäristöstä, tai koettaa avata kellariluukun. Polku on loppunut tähän, " +
                        "enkä uskalla vaeltaa metsän syvyyksiin suunnitelmatta, joten en voi liikkua muualle kuin takaisin alkuun.\n\n";

                    quest.QuestUI();

                    Utilities.TextWriter(mpText1);
                    Thread.Sleep(Utilities.s_TextChapterDelay);

                    // Killquest specific text and battle trigger
                    if (quest.KillQuest)
                    {
                        Utilities.TextWriter(mpBattleText);

                        // Combat, Returns enemies killed for use in QuestProgress() -method
                        Utilities.PressToContinue();
                        int killedAmount = Combat.Battle(player, s_enemyType, 3);

                        // Quest Progress
                        quest.QuestProgress(killedAmount, s_enemyName);

                        // Return to start
                        MultaPolku(player, quest);
                    }

                    Utilities.TextWriter(mpText2);


                }
                // Else returning short description depending on if mushrooms were gathered or not.
                else
                {
                    quest.QuestUI();

                    string mpReturnText = "";
                    switch (s_mpGathered)
                    {
                        case true:
                            mpReturnText = "Aukiolla jälleen. Istahdan puun raadolle, pohtimaan seuraavaa liikettä. Voin kokeilla avata kellariluukun, mutta muuta ei tule mieleen.\n\n";
                            break;
                        case false:
                            mpReturnText = "Aukiolla jälleen. Reunuksella olevat sienet vetävät katseeni niiden puoleen. Voin kerätä sienet, tai kokeilla avata kellariluukun.\n\n";
                            break;
                    }
                    Utilities.TextWriter(mpReturnText);
                }

                Thread.Sleep(Utilities.s_selectionDelay);
                // Player input
                Console.Write
                    (
                    "1. Kerää sienet\n" +
                    "2. Kellari\n" +
                    "3. Palaa alkuun\n\n"
                    );

                var pressedKey = Console.ReadKey(true);

                // Interaction choice
                switch (pressedKey.KeyChar)
                {
                    // Mushroom interactions.
                    case '1':
                        Console.Clear();
                        // if mushrooms are already gathered, inform player.
                        if (s_mpGathered == true)
                        {
                            string gatheredText = "Keräsit jo sienet, eikä täältä löydy mitään muuta kerättävää\n\n";
                            Utilities.TextWriter(gatheredText);
                            break;
                        }
                        // Getting random number for gathering amount.
                        int gatherAmount = Gathering.Gather(player);

                        // Gathering text.
                        string gatheringText = $"Kumarruin sieniä keräämään. Yksi toisen perään, tungin sieniä reipasta tahtia nahkalaukkuuni. Sain {gatherAmount} sientä.\n\n";

                        // Quest Counting
                        quest.QuestProgress(gatherAmount, s_gatherType);

                        // Text Writing
                        Utilities.TextWriter(gatheringText);

                        // set mushrooms to gathered.
                        s_mpGathered = true;

                        // Press to Continue.
                        Utilities.PressToContinue();
                        break;
                    // Cellar interactions.
                    case '2':
                        switch (s_hasKey)
                        {
                            case true:
                                if (s_kellariOpened)
                                {
                                    string kellariEnterText = "Kiipeän tikkaat alas kellariin ja seuraan kellarin ahdasta tunnelia seuraaville tikkaille. Tikkaat kiivettyäni, olen puurakennuksen pihalla.\n\n";
                                    prConnection = true;
                                    Utilities.TextWriter(kellariEnterText);
                                }
                                if (s_kellariOpened == false)
                                {
                                    enterKellari = true;
                                }
                                s_kellariOpened = true;
                                // Forces out of loop.
                                returnToStart = true;
                                // if kellari is opened set to enter the kellari.

                                break;
                            case false:
                                Console.Clear();
                                string openAttempt = "";
                                // Description depending if cellar was attempted prior.
                                switch (s_cellarAttempted)
                                {
                                    case true:
                                        // text depending on quest type
                                        if (quest.KillQuest)
                                        {
                                            openAttempt = "Kellariluukku ei avautunut tälläkään kertaa. Mörköjen melu ei ole lakannut.";
                                        }
                                        else
                                        {
                                            openAttempt = "Kellariluukku ei avautunut tälläkään kertaa. Kahvan alla on pieni avaimenreikä. Löytyyköhän tähän avain jostain?\n\n";
                                        }
                                        break;
                                    case false:
                                        openAttempt = "Vedin kellariluukun ruosteisesta kahvasta, mutta luukku ei liikahtanutkaan. " +
                                                "Harkitsin yrittää murskata luukun, mutta lohkeilevan puun alla näkyy metallinen hohto. " +
                                                "Parempi olla yrittämättä, ettei miekka tylsy.";
                                        // Added part depending on quest type
                                        if (quest.KillQuest)
                                        {
                                            openAttempt += " Luukun toiselta puolelta kuuluu kaikenlaista meteliä. Sieltä varmaankin möröt sikiää.";
                                        }
                                        openAttempt += "\n\n";
                                        s_cellarAttempted = true;
                                        break;
                                }
                                Utilities.TextWriter(openAttempt);
                                Utilities.PressToContinue();
                                break;
                        }
                        break;
                    // Return back to the entrance of the forest.
                    case '3':
                        returnToStart = true;
                        break;
                    default:
                        break;
                }

                //if (!enterKellari || !prConnection || !returnToStart)
                //{
                //    Utilities.PressToContinue();
                //}
            }

            if (prConnection)
            {
                PuuRakennus(player, quest);
            }
            else if (enterKellari)
            {
                Kellari(player,quest);
            }
            else
            {
                MMEntrance(player, quest);
            }

        }
        #endregion

        #region Metsänsyvyydet
        // Field for entry check.
        static bool s_metsanSyvReturn = false;
        static void Metsansyvyydet(Character player,Quest quest)
        {
            Console.Clear();

            quest.QuestUI();

            switch (s_metsanSyvReturn)
            {
                // Trying to enter the second time.
                case true:

                    string secondEntryText = "Katsoin syvyyksiin, harkiten paluuta. Sitten muistin viimeisen kokemukseni, ja totesin ottaneeni opiksi.\n";

                    Utilities.TextWriter(secondEntryText);

                    break;
                // Entering for the first time.
                case false:
                    // Metsänsyvyydet Text.
                    string metSyvText1 = "Keräsin rohkeuteni ja astuin mustametsän synkkiin syövereihin. " +
                        "Puskat ja pensaat olivat liian tiheästi kasvaneita katkaistakseen miekalla. " +
                        "Syrjin oksia ja lehtiä, tuntien niiden lyövän heti takaisin. Kaduin päätöksiäni jo nyt.\n\n";

                    string metSyvText2 = "";
                    string metSyvText3 = "";

                    // Kill quest specific text.
                    if (quest.KillQuest)
                    {
                        metSyvText2 = "Yhtäkkiä huomasin jonkin välähtävän hämärässä. Hyppäsin taakse ja väistin mörköjen yllätyshyökkäyksen hiuksenleveydellä. “Sitten menoksi”, minä totesin, tarttuessani miekkaani.";

                        metSyvText3 = "Taistelun jälkeen, peräännyin syvyyksistä takaisin alkuun. Parempi välttää tuon tempun toistoa.";
                    }
                    // Gathering quest specific text.
                    else
                    {
                        // Randomizing Gathered amount
                        int gatherAmount = Gathering.Gather(player);

                        metSyvText2 = "Lopulta pääsin vain parinkymmenen metrin matkan päähän alkupisteestäni. " +
                            $"Keräsin satunnaiset sienet matkalla, ja lopulta sain {gatherAmount} sientä tästä vaivasta. " +
                            "Ei tarpeeksi, jos minulta kysytään.\n\n";

                        // Quest Counting
                        quest.QuestProgress(gatherAmount, s_gatherType);

                        metSyvText3 = "Palattuani alkuun, vannoin pysyvän kaukana noista syvyyksistä tästä edespäin. " +
                            "Mutta pitäisikö mennä kohti puu rakennusta, vaiko valita multapolun?\n";
                    }

                    //{taistelu mörköjä vastaan tähän}

                    // Writing text.
                    Utilities.TextWriter(metSyvText1);

                    Thread.Sleep(Utilities.s_TextChapterDelay);

                    Utilities.TextWriter(metSyvText2);
                    if (quest.KillQuest)
                    {
                        // Combat, returns enemies killed for use in QuestProgress() -method.
                        Utilities.PressToContinue();
                        int killedAmount = Combat.Battle(player, s_enemyType, 3);

                        // Quest Progress
                        quest.QuestProgress(killedAmount, s_enemyName);

                    }
                    else
                    {
                        Thread.Sleep(Utilities.s_TextChapterDelay);
                    }

                    Utilities.TextWriter(metSyvText3);

                    s_metsanSyvReturn = true;
                    break;
            }
            // Delay and wait for button press
            Utilities.PressToContinue();

            // Return to forest entrance
            MMEntrance(player, quest);
        }
        #endregion

        #region Puurakennus
        static bool s_puuRakennusReturn = false;
        static bool s_prGathered = false;
        static bool s_buildingAttempted = false;
        static bool s_ruinsChecked = false;
        static void PuuRakennus(Character player, Quest quest)
        {
            bool returnToStart = false;
            bool mpConnection = false;

            while (!returnToStart)
            {
                Console.Clear();

                if (!s_puuRakennusReturn)
                {
                    // Intro Text
                    string puuRakennusIntroP1 = "";
                    string puuRakennusIntroP2 = "";
                    string puuRakennusIntroP3 = "";

                    if (quest.KillQuest)
                    {
                        puuRakennusIntroP1 = "Kävelin läpi vyötärölle kurottavan ruohikon matkalla puurakennukselle. Yhtäkkiä, mustia muotoja hyppäsi ruohikon seasta. " +
                            "En ehtinyt edes rekisteröimään mitä tapahtui, ennen kuin möröt olivat piirittäneet minut.\n\n";
                        puuRakennusIntroP2 = "saavuin rakennuksen edustalle ja huomasin että rakennuksen katto oli romahtanut. " +
                            "Sen sisimmissä ei näkynyt mitään muuta kuin romua, ja sen romun syövereistä kasvoi valtava mänty. Piha-alueella ei ollut mitään muita puita, " +
                            "mutta neulaskatto oli silti ihan yhtä tiheä—täysin sen yhden männyn johdosta. Oliko katto romahtanut männyn kasvusta, vai kasvoiko mänty katon romahduksesta? " +
                            "Ei kai sillä mitään väliä ole.\n\n";
                        puuRakennusIntroP3 = "Piha-alueen vasemmassa osassa näkyi jonkinlainen varastorakennus. Siinä oli ollut ovi joskus, mutta se on täysin roskattu. " +
                            "Kun menin katsomaan sisälle, näin vain avatun kellariluukun maassa, joka johti maan uumeniin. Mörköjen meteli kuului kellarista tänne asti.\n\n" +
                            "Voit mennä kellariin tai palata alkuun.\n\n";
                    }
                    else
                    {
                        puuRakennusIntroP1 = "Kävelin läpi vyötärölle kurottavan ruohikon matkalla puurakennukselle. " +
                             "Yhtäkkiä, tunsin jalkani osuvan johonkin. En ehtinyt edes rekisteröimään mitä tapahtui, ennen kuin tapasin maan halauksen. " +
                             "Nousin kivuliaasti takaisin jaloilleni. Ruohikon alla oleva kostea sammal ei pehmentänyt laskeutumista, mutta kyllä se vaatteeni kasteli. " +
                             "Jatkoin matkaa, tuntien itseni märäksi koiraksi.\n\n";
                        puuRakennusIntroP2 = "Kun saavuin rakennuksen edustalle, huomasin että rakennuksen katto oli romahtanut. " +
                            "Sen sisimmissä ei näkynyt mitään muuta kuin romua, ja sen romun syövereistä kasvoi valtava mänty. " +
                            "Piha-alueella ei ollut mitään muita puita, mutta neulaskatto oli silti ihan yhtä tiheä—täysin sen yhden männyn johdosta. " +
                            "Oliko katto romahtanut männyn kasvusta, vai kasvoiko mänty katon romahduksesta? Ei kai sillä mitään väliä ole.\n\n";
                        puuRakennusIntroP3 = "Piha-alueella näkyi useita kiinnostuksen kohteita. Rakennuksen ympärillä kasvoi sieniä, jotka voisin kerätä. " +
                            "Pihan vasemmassa osassa näkyy olevan jonkinlainen varastorakennus. Voisin tietysti myös mennä penkomaan rakennuksen raunioita, jos sieltä vielä löytyisi jotain.\n\n";
                    }

                    quest.QuestUI();

                    // Writing intro text.
                    Utilities.TextWriter(puuRakennusIntroP1);

                    // {taistelu mörköjä vastaan tähän}
                    if (quest.KillQuest)
                    {
                        // Combat, returns enemies killed for use in QuestProgress() -method.
                        Utilities.PressToContinue();
                        int killedAmount = Combat.Battle(player, s_enemyType, 3);

                        if(killedAmount > 0)
                        {
                            puuRakennusIntroP2 = "Kun päihitin möröt, " + puuRakennusIntroP2;
                        }
                        else
                        {
                            puuRakennusIntroP2 = "Möröiltä paettuani, " + puuRakennusIntroP2;
                        }


                        // Quest progress
                        quest.QuestProgress(killedAmount, s_enemyName);
                    }
                    else
                    {
                        Thread.Sleep(Utilities.s_TextChapterDelay);
                    }

                    Utilities.TextWriter(puuRakennusIntroP2);

                    Thread.Sleep(Utilities.s_TextChapterDelay);

                    Utilities.TextWriter(puuRakennusIntroP3);

                    s_puuRakennusReturn = true;
                }
                else
                {
                    quest.QuestUI();

                    string returnText = "";
                    if (quest.KillQuest)
                    {
                        returnText = "Pihalla jälleen. Voin mennä takaisin aloituspisteeseeni tai kellariin.";
                    }
                    if (s_prGathered && s_ruinsChecked)
                    {
                        returnText = "Kävelin rakennusta kohti, varoen etten kompastu sammaleen alla piileskeleviin kiviin tai juuriin. " +
                            "Saavuin rakennuksen pihalle. En tiedä mitä minun täällä pitäisi tehdä, muuta kuin ehkä yrittää tutkia varastorakennusta.\n" +
                            "\n";
                    }
                    if (s_prGathered && !s_ruinsChecked)
                    {
                        returnText = "Kävelin rakennusta kohti, varoen etten kompastu sammaleen alla piileskeleviin kiviin tai juuriin. " +
                            "Saavuin rakennuksen pihalle. Voisin yrittää penkoa rakennuksen raunioita, vaikka sieltä ei välttämättä mitään löydykään. " +
                            "Voin myös kokeilla päästä varastorakennukseen sisään.\n\n";
                    }
                    if (!s_prGathered && s_ruinsChecked)
                    {
                        returnText = "Kävelin rakennusta kohti, varoen etten kompastu sammaleen alla piileskeleviin kiviin tai juuriin. " +
                            "Saavuin rakennuksen pihalle. Sienet, jotka kasvavat rakennuksen pielessä, vetivät katseeni puoleensa. Voisin tutkia varastorakennusta.\n\n";
                    }
                    if (!s_prGathered && !s_ruinsChecked)
                    {
                        returnText = "Kävelin rakennusta kohti, varoen etten kompastu sammaleen alla piileskeleviin kiviin tai juuriin. " +
                            "Saavuin rakennuksen pihalle. Sienet, jotka kasvavat rakennuksen pielessä, vetivät katseeni puoleensa. " +
                            "Voin kokeilla penkoa rakennuksen raunioiden syövereistä aarteita—jos sellaisia sieltä löytyy. Voisin tutkia varastorakennusta.\n\n";
                    }
                    Utilities.TextWriter(returnText);
                }

                Thread.Sleep(Utilities.s_selectionDelay);
                // Player choice
                if (quest.KillQuest)
                {
                    Console.Write
                        (
                        "1. Kellari\n" +
                        "2. Palaa alkuun\n\n"
                        );

                }
                else
                {
                    Console.Write
                        (
                        "1. Kerää sienet\n" +
                        "2. Rakennus\n" +
                        "3. Rauniot\n" +
                        "4. Palaa alkuun\n\n"
                        );
                }

                // Player input
                var pressedKey = Console.ReadKey(true);

                if (quest.KillQuest)
                {
                    switch (pressedKey.KeyChar)
                    {
                        case '1':
                            Kellari(player, quest);
                            break;
                        case '2':
                            returnToStart = true;
                            break;
                    }
                }
                else
                {
                    // Interaction choice | Gathering quest.
                    switch (pressedKey.KeyChar)
                    {
                        // Mushroom interactions.
                        case '1':
                            // if mushrooms are already gathered, inform player.
                            Console.Clear();
                            if (s_prGathered == true)
                            {
                                string failedGathering = "Keräsit jo sienet, eikä täältä löydy mitään muuta kerättävää\n";
                                Utilities.TextWriter(failedGathering);

                                Utilities.PressToContinue();

                                break;
                            }
                            // Getting random number for gathering amount
                            int gatherAmount = Gathering.Gather(player);
                            // Gathering text and Writing it.
                            string gatheringText = $"Kiersin ympäri rakennuksen, keräten kaikki sienet, jotka huomasin. Sain {gatherAmount} sientä.\n";
                            Utilities.TextWriter(gatheringText);

                            // Quest Counting
                            quest.QuestProgress(gatherAmount, s_gatherType);

                            // setting mushrooms to gathered
                            s_prGathered = true;

                            // Delay and wait for button press
                            Utilities.PressToContinue();

                            break;
                        // Building
                        case '2':
                            Console.Clear();
                            string buildingText = "";
                            // Check whether door has been attempted before or not.
                            // then set appropriate text.
                            if (s_kellariOpened)
                            {
                                buildingText = "Menin varastorakennukseen ja kiipesin tikkaat alas kellariin. Kävelin kellarin ahtaan tunnelin läpi toisille tikkaille, ja kiipesin ne ylös. Olen nyt aukiolla.";

                                returnToStart = true;
                                mpConnection = true;
                            }
                            else
                            {
                                switch (s_buildingAttempted)
                                {

                                    case false:
                                        buildingText = "Koitin varastorakennuksen liukuvaa peltiovea, mutta se ei liikahtanutkaan. " +
                                            "Potkaisin ovea kaikin voimin, eikä se näyttänyt merkkiäkään romahduksesta. Turha toivo.\n";
                                        s_buildingAttempted = true;
                                        break;
                                    case true:
                                        buildingText = "Potkaisin peltiovea niin kovaa, että kaiku sai linnut pakenemaan. Ovi ei liikahtanutkaan. Turha toivo.\n";
                                        break;
                                }

                            }

                            // writing text.
                            Utilities.TextWriter(buildingText);

                            // Delay and wait for button press.
                            Utilities.PressToContinue();

                            break;
                        // Rauniot
                        case '3':
                            Console.Clear();
                            // Ruins Text.
                            string ruinsText = "Kävelin raunioiden ääreen ja aloin penkoa. Puuta, puuta, lisää puuta, pari metalli pannua, " +
                                "taas puuta, ja sitten vielä vähän lisää puuta.\n";
                            string ruinsText2 = "Juuri kun olin menettämässä toivon, huomasin jotain pientä ja metallista romun syövereissä. " +
                                "Aloitin kaivuu työn jälleen. Kun romu oli sivuutettu, sain käsiini pienen, ruosteisen avaimen. Tämä varmaan tulee hyödyksi.\n";

                            // Get key for cellar 
                            s_hasKey = true;
                            // Set ruins to searched/checked.
                            s_ruinsChecked = true;

                            Utilities.TextWriter(ruinsText);
                            Thread.Sleep(Utilities.s_TextChapterDelay);
                            Utilities.TextWriter(ruinsText2);

                            Utilities.PressToContinue();

                            break;
                        // Return back to the entrance of the forest.
                        case '4':
                            returnToStart = true;
                            break;
                        default:
                            break;
                    }
                }
            }

            // if Kellari has been opened and entering warehouse go through to Multapolku.
            if (mpConnection)
            {
                MultaPolku(player, quest);
            }
            // if not entering kellari go back to start.
            else
            {
                MMEntrance(player, quest);
            }

        }
        #endregion

        #region Kellari
        // Field for whether kellari has been opened or not
        static bool s_kellariOpened = false;
        // Kill quest specific check if kellari has been cleared
        static bool s_kellariCleared = false;

        static void Kellari(Character player, Quest quest)
        {
            Console.Clear();

            string kellariOpening = "";
            string kellariText1 = "";
            string kellariText2 = "";
            string kellariText3 = "";

            if (quest.KillQuest && s_kellariCleared)
            {
                kellariText1 = "Saavuin kellarin syövereihin. Minulla ei ole mitään tekemistä täällä. Parempi kääntyä ympäri.";
                Utilities.TextWriter(kellariText1);
            }
            // Kill quest text.
            else if (quest.KillQuest)
            {
                s_kellariCleared = true;

                kellariText1 = "Kiipesin tikkaat alas kellariin. Mörköjen tekemä meteli nousi ja nousi, kaikuen kellarin kivi seinämissä. " +
                    "Aika hiljentää ne! Kun jalkani tapasivat kylmän kiven, meteli lakkasi, ihan kuin joku olisi jo tehnyt minun työni puolestani. " +
                    "Olisi töykeää antaa heidän odottaa minua liian pitkään.\n\n";
                kellariText2 = "Askeleeni kaikui kävellessäni syvemmälle. Ennen pitkään, saavuin määränpäähäni." +
                    " “Antakaa tulla!” minä sanoin. Yksi mörkö yritti heti avata kaulani. Väistin mörön kynnet ja nostin miekkani " +
                    "huotrastaan—miekkani leikkasi mörön vaivattomasti. Sitten vielä loput.\n\n";
                kellariText3 = "Taistelun päätyttyä, minulla oli viimein aikaa perehtyä ympäristööni tarkemmin, mutta ei siinä paljoa perehdyttävää ollut. " +
                    "Kellari loppuu tähän, eikä muita suuntia ole kuin takaisin. Tällä puolellakin vaikutti olleen joskus tikkaat, mutta niistä ei ole muuta kuin puu silppua jäljellä.\n\n";
            }
            // Gathering Quest text.
            else
            {
                // setting kellari to opened.
                s_kellariOpened = true;
                // Randomized gathered amount.
                int gatherAmount = Gathering.Gather(player);

                // Kellari Text
                kellariOpening = "Laitoin rakennukselta löytämäni avaimen kellariluukun avaimenreikään ja käänsin sitä. " +
                    "Kun vetäisin kellariluukun kahvasta, se avautui märisten. Luukun toisella puolella oli tikkaat, jotka johtivat maanalaisiin syvyyksiin. " +
                    "Aloin kiivetä tikkaita alas.\n\n";

                kellariText1 = "Nyt olin kiitollisempi kuin koskaan lyhdystäni. Ilman sitä, en näkisi yhtikäs mitään kellarissa. " +
                    "Kellari oli viileä ja märkä, ja jatkui pidemmälle kuin lyhtyni valo kantoi. Kellarin sisällä ei ollut mitään muuta kuin sieniä. " +
                    "Mutta sieniä muuten riitti. Aloin keräämään niitä hirveää vauhtia. Sieni sienen jälkeen löysi tien nahkalaukkuuni. " +
                    "En edes huomannut käveleväni yhä syvemmälle kellariin sienihurmassa.\n\n";

                kellariText2 = $"Lopulta päädyin kellarin loppuun, jolloin myös sienet loppuivat. Sain {gatherAmount} sientä. " +
                    "Sienien loppu sai minut katsomaan ympärilleni ensimmäistä kertaa hetkeen. Kellari ei välttämättä ole edes oikea sana tälle paikalle. " +
                    "Se on vain tunneli. Edessäni olevat tikkaat johtavat ylöspäin, enkä näe aloituspistettäni enää taakse katsoessa. Päätän kiivetä tikkaat ylös.\n\n";

                // Quest Counting
                quest.QuestProgress(gatherAmount, s_gatherType);

                kellariText3 = "Toinen kellariluukku löytyi tikkaiden päästä. Työnsin sen auki, ja huomasin olevani jonkinlaisen rakennuksen sisällä. " +
                    "Rakennus on pieni, eikä sieltä löydy paljoa. Kellariluukku maassa on varmaankin kaikista kiinnostavin asia siellä. Edessäni näen liukuvan " +
                    "peltioven, jonka kahvojen ympärillä on ruosteinen kettinki. Kettingissä on kiinni lukko. Kokeilen aikaisemmin löytämääni avainta lukkoon. " +
                    "Lukko aukesi vastustamatta. Revin kettingin pois kahvojen ympäriltä ja avaan liukuvan peltioven.\n\n" +
                    "Olen löytänyt itseni puurakennuksen pihalta. No, tulipahan tutkittua varastorakennus samalla.\n\n";

            }

            quest.QuestUI();

            // Writing text 
            // Opening only happens on Gathering quests.
            // Is only used if Kellari has not been cleared in Kill quest.
            if (!s_kellariCleared)
            {
                if (!quest.KillQuest)
                {
                    Utilities.TextWriter(kellariOpening);
                    Thread.Sleep(Utilities.s_TextChapterDelay);
                }
                Utilities.TextWriter(kellariText1);

                Thread.Sleep(Utilities.s_TextChapterDelay);

                Utilities.TextWriter(kellariText2);

                // {vaikeampi tappelu mörköjä vastaan tähän} 
                if (quest.KillQuest)
                {
                    // Combat, returns enemies killed for use in QuestProgress() -method.
                    Utilities.PressToContinue();
                    int killedAmount = Combat.Battle(player, s_enemyType, 6);

                    // Quest progress.
                    quest.QuestProgress(killedAmount, s_enemyName);
                }
                else
                {
                    Thread.Sleep(Utilities.s_TextChapterDelay);
                }

                Utilities.TextWriter(kellariText3);
            }

            Utilities.PressToContinue();

            // Go to Puurakennus
            PuuRakennus(player, quest);
        }
        #endregion

        // Reset the area after leaving.
        static void ResetArea()
        {
            // Class wide
            s_hasKey = false;
            // Mustametsä Entrance
            s_mmReturn = false;
            s_mmFirstReturn = true;
            // Multapolku
            s_cellarAttempted = false;
            s_mpGathered = false;
            s_multaPolkuReturn = false;
            // Metsänsyvyydet
            s_metsanSyvReturn = false;
            // Puurakennus
            s_puuRakennusReturn = false;
            s_prGathered = false;
            s_buildingAttempted = false;
            s_ruinsChecked = false;
            // Kellari
            s_kellariOpened = false;
            s_kellariCleared = false;
        }


    }
}
