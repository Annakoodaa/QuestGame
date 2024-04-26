using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuestGame
{
    internal class MustaMetsa
    {
        #region Text Related
        static int s_textSpeed = 5;
        static int s_textChapterDelay = 1500;
        static int s_selectionDelay = 2000;
        #endregion

        #region Mustametsä wide fields
        static bool s_hasKey = false;
        static bool s_cellarAttempted = false;
        #endregion

        #region Mustametsä entrance
        // Field for returning check
        static bool s_mmReturn = false;

        // Add Quest quest Parameter for quest tracking I guess. Also for tracking kill/gather quest type.
        // For now Kill/Gather Field:
        static string s_questType = "kill";

        public static void MMEntrance(Character player)
        {
            Console.Clear();
            if (!s_mmReturn)
            {
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
                TextWriter(mmText1);

                Thread.Sleep(s_textChapterDelay);

                TextWriter(mmText2);

                s_mmReturn = true;
            }
            else
            {
                // Mustämetsä return text
                string mmReturnText = "Astuin jälleen mustametsän alkuun. Sammal edelleen uhkailee upottaa jalkani. " +
                    "Voin mennä multapolkua pitkin vasemmalla, mennä metsän syövereihin oikealla, tai tutkia mätänevää puurakennusta. " +
                    "Karkaus on myös vaihtoehto, mutten usko sitä tarpeelliseksi.\n\n";

                // Writing text
                TextWriter(mmReturnText);
            }
            // Selection window delay
            Thread.Sleep(s_selectionDelay);
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
                    MultaPolku(player);
                    break;
                case '2':
                    Metsansyvyydet(player);
                    break;
                case '3':
                    PuuRakennus(player);
                    break;
                case '4':
                    ResetArea();
                    TheGame.ChooseArea(player);
                    break;
            }
        }
        #endregion

        #region Multapolku
        // Field for returning to center
        static bool s_multaPolkuReturn = false;
        static bool s_mpGathered = false;

        static void MultaPolku(Character player)
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
                    // Multapolku Text
                    string mpText1 = "Kävelin multapolkua pitkin useita minuutteja, huitoen samalla puskia ynnä muita pensaita pois tieltäni miekallani. " +
                        "Lopulta, päädyin aukiolle. Aurinko säteili läpi neulaskaton vain tähän tiettyyn kohtaan. Aukio ei vaikuttanut olevan luonnollinen näky, " +
                        "sillä puiden raadot olivat jätetty aukiolla esille, todennäköisesti jotta seikkailijat voivat käyttää niitä istuimina. " +
                        "Aukion reunuksella on runsaasti sieniä. Kun katson aukiota tarkemmin, huomaan että osa maasta näyttää hauraammalta kuin muut. " +
                        "Alan kaivamaan haurasta maata kaksin käsin. Ennen pitkään, maa toi esiin puisen kellariluukun.\n\n";

                    string mpText2 = "Voin kerätä sienet ympäristöstä, tai koettaa avata kellariluukun. Polku on loppunut tähän, " +
                        "enkä uskalla vaeltaa metsän syvyyksiin suunnitelmatta, joten en voi liikkua muualle kuin takaisin alkuun.\n\n";

                    TextWriter(mpText1);
                    Thread.Sleep(s_textChapterDelay);
                    TextWriter(mpText2);

                    s_multaPolkuReturn = true;
                }
                // Else returning short description depending on if mushrooms were gathered or not.
                else
                {
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
                    TextWriter(mpReturnText);
                }

                Thread.Sleep(s_selectionDelay);
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
                            TextWriter(gatheredText);
                            break;
                        }
                        // Getting random number for gathering amount.
                        int gatherAmount = Gathering.Gather(player);

                        // Gathering text.
                        string gatheringText = $"Kumarruin sieniä keräämään. Yksi toisen perään, tungin sieniä reipasta tahtia nahkalaukkuuni. Sain {gatherAmount} sientä.\n\n";
                        TextWriter(gatheringText);

                        // set mushrooms to gathered.
                        s_mpGathered = true;
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
                                    TextWriter(kellariEnterText);
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
                                        openAttempt = "Kellariluukku ei avautunut tälläkään kertaa. Kahvan alla on pieni avaimenreikä. Löytyyköhän tähän avain jostain?\n\n";
                                        break;
                                    case false:
                                        openAttempt = "Vedin kellariluukun ruosteisesta kahvasta, mutta luukku ei liikahtanutkaan. " +
                                                "Harkitsin yrittää murskata luukun, mutta lohkeilevan puun alla näkyy metallinen hohto. " +
                                                "Parempi olla yrittämättä, ettei miekka tylsy.\n\n";
                                        s_cellarAttempted = true;
                                        break;
                                }
                                TextWriter(openAttempt);
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

                if (!enterKellari)
                {
                    Thread.Sleep(s_selectionDelay);
                    Console.Write("Paina nappia jatkaaksesi");
                    Console.ReadKey(true);
                }
            }

            if(prConnection)
            {
                PuuRakennus(player);
            }
            else if (enterKellari)
            {
                Kellari(player);
            }
            else
            {
                MMEntrance(player);
            }

        }
        #endregion

        #region Metsänsyvyydet
        // Field for entry check.
        static bool s_metsanSyvReturn = false;
        static void Metsansyvyydet(Character player)
        {
            Console.Clear();
            switch (s_metsanSyvReturn)
            {
                // Trying to enter the second time.
                case true:

                    string secondEntryText = "Katsoin syvyyksiin, harkiten paluuta. Sitten muistin viimeisen kokemukseni, ja totesin ottaneeni opiksi.\n";

                    TextWriter(secondEntryText);

                    break;
                // Entering for the first time.
                case false:
                    // Randomizing Gathered amount
                    int gatheredAmount = Gathering.Gather(player);

                    // Metsänsyvyydet Text.
                    string metSyvText1 = "Keräsin rohkeuteni ja astuin mustametsän synkkiin syövereihin. " +
                        "Puskat ja pensaat olivat liian tiheästi kasvaneita katkaistakseen miekalla. " +
                        "Syrjin oksia ja lehtiä, tuntien niiden lyövän heti takaisin. Kaduin päätöksiäni jo nyt.\n\n";

                    string metSyvText2 = "Lopulta pääsin vain parinkymmenen metrin matkan päähän alkupisteestäni. " +
                        $"Keräsin satunnaiset sienet matkalla, ja lopulta sain {gatheredAmount} sientä tästä vaivasta. " +
                        "Ei tarpeeksi, jos minulta kysytään.\n\n";
                    string metSyvText3 = "Palattuani alkuun, vannoin pysyvän kaukana noista syvyyksistä tästä edespäin. " +
                        "Mutta pitäisikö mennä kohti puu rakennusta, vaiko valita multapolun?\n";

                    // Writing text.
                    TextWriter(metSyvText1);

                    Thread.Sleep(s_textChapterDelay);

                    TextWriter(metSyvText2);

                    Thread.Sleep(s_textChapterDelay);

                    TextWriter(metSyvText3);

                    s_metsanSyvReturn = true;
                    break;
            }
            // Delay and wait for button press
            Thread.Sleep(s_selectionDelay);
            Console.Write("\nPaina nappia jatkaaksesi");
            Console.ReadKey(true);

            // Return to forest entrance
            MMEntrance(player);
        }
        #endregion

        #region Puurakennus
        static bool s_puuRakennusReturn = false;
        static bool s_prGathered = false;
        static bool s_buildingAttempted = false;
        static bool s_ruinsChecked = false;
        static void PuuRakennus(Character player)
        {
            bool returnToStart = false;
            bool mpConnection = false;

            while (!returnToStart)
            {
                Console.Clear();

                if (!s_puuRakennusReturn)
                {
                    // Intro Text
                    string puuRakennusIntroP1 = "Kävelin läpi vyötärölle kurottavan ruohikon matkalla puurakennukselle. " +
                         "Yhtäkkiä, tunsin jalkani osuvan johonkin. En ehtinyt edes rekisteröimään mitä tapahtui, ennen kuin tapasin maan halauksen. " +
                         "Nousin kivuliaasti takaisin jaloilleni. Ruohikon alla oleva kostea sammal ei pehmentänyt laskeutumista, mutta kyllä se vaatteeni kasteli. " +
                         "Jatkoin matkaa, tuntien itseni märäksi koiraksi.\n\n";
                    string puuRakennusIntroP2 = "Kun saavuin rakennuksen edustalle, huomasin että rakennuksen katto oli romahtanut. " +
                        "Sen sisimmissä ei näkynyt mitään muuta kuin romua, ja sen romun syövereistä kasvoi valtava mänty. " +
                        "Piha-alueella ei ollut mitään muita puita, mutta neulaskatto oli silti ihan yhtä tiheä—täysin sen yhden männyn johdosta. " +
                        "Oliko katto romahtanut männyn kasvusta, vai kasvoiko mänty katon romahduksesta? Ei kai sillä mitään väliä ole.\n\n";
                    string puuRakennusIntroP3 = "Piha-alueella näkyi useita kiinnostuksen kohteita. Rakennuksen ympärillä kasvoi sieniä, jotka voisin kerätä. " +
                        "Pihan vasemmassa osassa näkyy olevan jonkinlainen varastorakennus. Voisin tietysti myös mennä penkomaan rakennuksen raunioita, jos sieltä vielä löytyisi jotain.\n\n";

                    // Writing intro text.
                    TextWriter(puuRakennusIntroP1);

                    Thread.Sleep(s_textChapterDelay);

                    TextWriter(puuRakennusIntroP2);

                    Thread.Sleep(s_textChapterDelay);

                    TextWriter(puuRakennusIntroP3);

                    s_puuRakennusReturn = true;
                }
                else
                {
                    string returnText = "";
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
                    TextWriter(returnText);
                }

                Thread.Sleep(s_selectionDelay);
                // Player choice 
                Console.Write
                    (
                    "1. Kerää sienet\n" +
                    "2. Rakennus\n" +
                    "3. Rauniot\n" +
                    "4. Palaa alkuun\n\n"
                    );

                // Player input
                var pressedKey = Console.ReadKey(true);

                // Interaction choice
                switch (pressedKey.KeyChar)
                {
                    // Mushroom interactions.
                    case '1':
                        // if mushrooms are already gathered, inform player.
                        Console.Clear();
                        if (s_prGathered == true)
                        {
                            string failedGathering = "Keräsit jo sienet, eikä täältä löydy mitään muuta kerättävää\n";
                            TextWriter(failedGathering);

                            Thread.Sleep(s_selectionDelay);
                            Console.Write("Paina nappia jatkaaksesi");
                            Console.ReadKey(true);

                            break;
                        }
                        // Getting random number for gathering amount
                        int gatherAmount = Gathering.Gather(player);
                        // Gathering text and Writing it.
                        string gatheringText = $"Kiersin ympäri rakennuksen, keräten kaikki sienet, jotka huomasin. Sain {gatherAmount} sientä.\n";
                        TextWriter(gatheringText);
                        // setting mushrooms to gathered
                        s_prGathered = true;

                        // Delay and wait for button press
                        Thread.Sleep(s_selectionDelay);
                        Console.Write("\nPaina nappia jatkaaksesi");
                        Console.ReadKey(true);

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
                        TextWriter(buildingText);

                        // Delay and wait for button press.
                        Thread.Sleep( s_selectionDelay);
                        Console.Write("\nPaina nappia jatkaaksesi");
                        Console.ReadKey(true);

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

                        TextWriter(ruinsText);
                        Thread.Sleep(s_textChapterDelay);
                        TextWriter(ruinsText2);

                        Thread.Sleep(s_selectionDelay);
                        Console.Write("\nPaina nappia jatkaaksesi");
                        Console.ReadKey(true);

                        break;
                    // Return back to the entrance of the forest.
                    case '4':
                        returnToStart = true;
                        break;
                    default:
                        break;
                }
            }

            if (mpConnection)
            {
                MultaPolku(player);
            }
            else
            {
                MMEntrance(player);
            }

        }
        #endregion

        #region Kellari
        // Field for whether kellari has been opened or not
        static bool s_kellariOpened = false;

        static void Kellari(Character player)
        {
            Console.Clear();

            // Randomized gathered amount.
            int gatheredAmount = Gathering.Gather(player);

            // Kellari Text
            string kellariIntro = "Laitoin rakennukselta löytämäni avaimen kellariluukun avaimenreikään ja käänsin sitä. " +
                "Kun vetäisin kellariluukun kahvasta, se avautui märisten. Luukun toisella puolella oli tikkaat, jotka johtivat maanalaisiin syvyyksiin. " +
                "Aloin kiivetä tikkaita alas.\n\n";

            string kellariText1 = "Nyt olin kiitollisempi kuin koskaan lyhdystäni. Ilman sitä, en näkisi yhtikäs mitään kellarissa. " +
                "Kellari oli viileä ja märkä, ja jatkui pidemmälle kuin lyhtyni valo kantoi. Kellarin sisällä ei ollut mitään muuta kuin sieniä. " +
                "Mutta sieniä muuten riitti. Aloin keräämään niitä hirveää vauhtia. Sieni sienen jälkeen löysi tien nahkalaukkuuni. " +
                "En edes huomannut käveleväni yhä syvemmälle kellariin sienihurmassa.\n\n";

            string kellariText2 = $"Lopulta päädyin kellarin loppuun, jolloin myös sienet loppuivat. Sain {gatheredAmount} sientä. " +
                "Sienien loppu sai minut katsomaan ympärilleni ensimmäistä kertaa hetkeen. Kellari ei välttämättä ole edes oikea sana tälle paikalle. " +
                "Se on vain tunneli. Edessäni olevat tikkaat johtavat ylöspäin, enkä näe aloituspistettäni enää taakse katsoessa. Päätän kiivetä tikkaat ylös.\n\n";

            string kellariText3 = "Toinen kellariluukku löytyi tikkaiden päästä. Työnsin sen auki, ja huomasin olevani jonkinlaisen rakennuksen sisällä. " +
                "Rakennus on pieni, eikä sieltä löydy paljoa. Kellariluukku maassa on varmaankin kaikista kiinnostavin asia siellä. Edessäni näen liukuvan " +
                "peltioven, jonka kahvojen ympärillä on ruosteinen kettinki. Kettingissä on kiinni lukko. Kokeilen aikaisemmin löytämääni avainta lukkoon. " +
                "Lukko aukesi vastustamatta. Revin kettingin pois kahvojen ympäriltä ja avaan liukuvan peltioven.\n\n";

            string kellariEnd = "Olen löytänyt itseni puurakennuksen pihalta. No, tulipahan tutkittua varastorakennus samalla.\n\n";

            // setting kellari to opened.
            s_kellariOpened = true;

            // Writing text 
            TextWriter(kellariIntro);
            Thread.Sleep(s_textChapterDelay);
            TextWriter(kellariText1);
            Thread.Sleep(s_textChapterDelay);
            TextWriter(kellariText2);
            Thread.Sleep(s_textChapterDelay);
            TextWriter(kellariText3);
            Thread.Sleep(s_textChapterDelay);
            TextWriter(kellariEnd);

            Thread.Sleep(s_selectionDelay);
            Console.Write("Paina nappia jatkaaksesi");
            Console.ReadKey(true);

            // Go to Puurakennus
            PuuRakennus(player);
        }
        #endregion

        // Reset the area after leaving.
        static void ResetArea()
        {
            // Class wide
            s_hasKey = false;
            // Mustametsä Entrance
            s_mmReturn = false;
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
        }

        // Method for text writing
        static void TextWriter(string text)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(s_textSpeed);
            }
        }
    }
}
