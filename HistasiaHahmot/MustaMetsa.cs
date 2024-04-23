using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuestGame
{
    internal class MustaMetsa
    {
        #region Mustametsä wide fields
        static bool _hasKey = false;
        static bool _cellarAttempted = false;
        #endregion

        #region Mustametsä entrance
        // Field for returning check
        static bool _mmReturn = true;

        public static void MMEntrance(Character player)
        {
            Console.Clear();
            if (_mmReturn)
            {
                Console.WriteLine("Mustametsä:");
                Console.Write("Kun astuin mustametsään, tunsin kengänpohjani uppoavan sammaleen. Ilma oli kostea, eikä auringonvalo pilkistänyt tiheän neulaskaton läpi. " +
                    "En nähnyt muutamaa metriä pidemmälle metsän syvyyksiin. Olin valmistautunut tähän. Irrotin lyhtyni vyötäröltäni, ja avasin sen luukun. " +
                    "Luukun ruosteinen ratina avautuessaan kaikui hiljaisuudessa. Se otti muutaman yrityksen, mutta lopulta onnistuin sytyttämään liekin lyhdyn" +
                    " sisällä—nyt toivotaan, ettei se sammu. \n\n");
                Thread.Sleep(1000);
                Console.Write("Nyt kun minulla on valoa, hahmotan ympäristöni enemmässä tarkkuudessa. " +
                    "Vasemmalle johtaa lähes täysin tukkoon kasvanut multapolku. " +
                    "Oikealla, näen pelkkää pusikkoa niin pitkälle kuin silmä kantaa. " +
                    "Edessä päin näen jonkinlaisen mätänevän puurakennuksen. Voin tietenkin myös lähteä takaisin mistä tulin, " +
                    "mutta ilman tarvitsemiani tavaroita, se tarkoittaisi tehtävän hylkäystä.\n\n");
                _mmReturn = false;
            }
            else
            {
                Console.Write("Astuin jälleen mustametsän alkuun. Sammal edelleen uhkailee upottaa jalkani. " +
                    "Voin mennä multapolkua pitkin vasemmalla, mennä metsän syövereihin oikealla, tai tutkia mätänevää puurakennusta. " +
                    "Karkaus on myös vaihtoehto, mutten usko sitä tarpeelliseksi.\n");
            }

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
        static bool _multaPolkuReturn = false;
        static bool _mpGathered = false;

        static void MultaPolku(Character player)
        {
            bool cellarOpened = false;
            bool returnToStart = false;
            while (!returnToStart)
            {
                Console.Clear();

                // Multapolku introduction. Only on first entry.
                if (_multaPolkuReturn == false)
                {
                    Console.Write("Kävelin multapolkua pitkin useita minuutteja, huitoen samalla puskia ynnä muita pensaita pois tieltäni miekallani. " +
                        "Lopulta, päädyin aukiolle. Aurinko säteili läpi neulaskaton vain tähän tiettyyn kohtaan. Aukio ei vaikuttanut olevan luonnollinen näky, " +
                        "sillä puiden raadot olivat jätetty aukiolla esille, todennäköisesti jotta seikkailijat voivat käyttää niitä istuimina. " +
                        "Aukion reunuksella on runsaasti sieniä. Kun katson aukiota tarkemmin, huomaan että osa maasta näyttää hauraammalta kuin muut. " +
                        "Alan kaivamaan haurasta maata kaksin käsin. Ennen pitkään, maa toi esiin puisen kellariluukun. \r\n\r\nVoin kerätä sienet ympäristöstä, " +
                        "tai koettaa avata kellariluukun. Polku on loppunut tähän, enkä uskalla vaeltaa metsän syvyyksiin suunnitelmatta, joten en voi liikkua " +
                        "muualle kuin takaisin alkuun.\n\n");

                    Thread.Sleep(1000);

                    Console.Write("Voin kerätä sienet ympäristöstä, tai koettaa avata kellariluukun. Polku on loppunut tähän, " +
                        "enkä uskalla vaeltaa metsän syvyyksiin suunnitelmatta, joten en voi liikkua muualle kuin takaisin alkuun.\n");
                }
                // Else returning short description depending on if mushrooms were gathered or not.
                else
                {
                    switch (_mpGathered)
                    {
                        case true:
                            Console.Write("Aukiolla jälleen. Istahdan puun raadolle, pohtimaan seuraavaa liikkua. Voin kokeilla avata kellariluukun, mutta muuta ei tule mieleen.");
                            break;
                        case false:
                            Console.Write("Aukiolla jälleen. Reunuksella olevat sienet vetävät katseeni niiden puoleen. Voin kerätä sienet, tai kokeilla avata kellariluukun.");
                            break;
                    }
                }

                // Player input
                Console.Write
                    (
                    "1. Kerää sienet\n" +
                    "2. Avaa kellari\n" +
                    "3. Palaa alkuun\n\n"
                    );

                var pressedKey = Console.ReadKey(true);

                // Interaction choice
                switch (pressedKey.KeyChar)
                {
                    // Mushroom interactions.
                    case '1':
                        // if mushrooms are already gathered, inform player.
                        if (_mpGathered == true)
                        {
                            Console.Write("Keräsit jo sienet, eikä täältä löydy mitään muuta kerättävää\n");
                            break;
                        }
                        // Getting random number for gathering amount
                        int gatherAmount = Gathering.Gather(player);
                        Console.Write($"Kumarruin sieniä keräämään. Yksi toisen perään, tungin sieniä reipasta tahtia nahkalaukkuuni. Sain {gatherAmount} sientä.\n");
                        _mpGathered = true;
                        break;
                    // Cellar interactions.
                    case '2':
                        switch (_hasKey)
                        {
                            case true:
                                cellarOpened = true;
                                returnToStart = true;
                                break;
                            case false:
                                // Description depending if cellar was attempted prior.
                                switch (_cellarAttempted)
                                {
                                    case true:
                                        Console.Write("Kellariluukku ei avautunut tälläkään kertaa. Kahvan alla on pieni avaimenreikä. Löytyyköhän tähän avain jostain?\n");
                                        break;
                                    case false:
                                        Console.Write("Vedin kellariluukun ruosteisesta kahvasta, mutta luukku ei liikahtanutkaan. " +
                                                "Harkitsin yrittää murskata luukun, mutta lohkeilevan puun alla näkyy metallinen hohto. " +
                                                "Parempi olla yrittämättä, ettei miekka tylsy.\n");
                                        break;
                                }
                                break;
                        }
                        break;
                    // Return back to the entrance of the forest.
                    case '3':
                        returnToStart = true;
                        break;
                }
                Console.Write("Paina nappia jatkaaksesi");
                Console.ReadKey(true);
            }

            if (cellarOpened)
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
        static bool _metsanSyvReturn = false;
        static void Metsansyvyydet(Character player)
        {
            Console.Clear();
            switch (_metsanSyvReturn)
            {
                // Trying to enter the second time.
                case true:
                    Console.Write("Katsoin syvyyksiin, harkiten paluuta. Sitten muistin viimeisen kokemukseni, ja totesin ottaneeni opiksi.\n");
                    break;
                // Entering for the first time.
                case false:
                    Console.Write("Keräsin rohkeuteni ja astuin mustametsän synkkiin syövereihin. " +
                            "Puskat ja pensaat olivat liian tiheästi kasvaneita katkaistakseen miekalla. " +
                            "Syrjin oksia ja lehtiä, tuntien niiden lyövän heti takaisin. Kaduin päätöksiäni jo nyt.\n\n");
                    Thread.Sleep(1000);
                    int gatheredAmount = Gathering.Gather(player);
                    Console.Write("Lopulta pääsin vain parinkymmenen metrin matkan päähän alkupisteestäni. " +
                        $"Keräsin satunnaiset sienet matkalla, ja lopulta sain {gatheredAmount} sientä tästä vaivasta. " +
                        "Ei tarpeeksi, jos minulta kysytään.\n\n");
                    Thread.Sleep(1000);
                    Console.Write("Palattuani alkuun, vannoin pysyvän kaukana noista syvyyksistä tästä edespäin. " +
                        "Mutta pitäisikö mennä kohti puu rakennusta, vaiko valita multapolun?\n");
                    _metsanSyvReturn = true;
                    break;
            }
            Console.Write("Paina nappia jatkaaksesi");
            Console.ReadKey(true);

            // Return to forest entrance
            MMEntrance(player);
        }
        #endregion

        #region Puurakennus
        static bool _puuRakennusReturn = false;
        static bool _prGathered = false;
        static bool _buildingAttempted = false;
        static bool _ruinsChecked = false;
        static void PuuRakennus(Character player)
        {
            bool returnToStart = false;

            while (!returnToStart)
            {
                Console.Clear();

                if (!_puuRakennusReturn)
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
                    Console.Write(puuRakennusIntroP1);
                    Thread.Sleep(500);
                    Console.Write(puuRakennusIntroP2);
                    Thread.Sleep(500);
                    Console.Write(puuRakennusIntroP3);
                    _puuRakennusReturn = true;
                }
                else
                {
                    string returnText = "";
                    if (_prGathered && _ruinsChecked)
                    {
                        returnText = "Kävelin rakennusta kohti, varoen etten kompastu sammaleen alla piileskeleviin kiviin tai juuriin. " +
                            "Saavuin rakennuksen pihalle. En tiedä mitä minun täällä pitäisi tehdä, muuta kuin ehkä yrittää tutkia varastorakennusta.\n";
                    }
                    if (_prGathered && !_ruinsChecked)
                    {
                        returnText = "Kävelin rakennusta kohti, varoen etten kompastu sammaleen alla piileskeleviin kiviin tai juuriin. " +
                            "Saavuin rakennuksen pihalle. Voisin yrittää penkoa rakennuksen raunioita, vaikka sieltä ei välttämättä mitään löydykään. " +
                            "Voin myös kokeilla päästä varastorakennukseen sisään.\n";
                    }
                    if (!_prGathered && _ruinsChecked)
                    {
                        returnText = "Kävelin rakennusta kohti, varoen etten kompastu sammaleen alla piileskeleviin kiviin tai juuriin. " +
                            "Saavuin rakennuksen pihalle. Sienet, jotka kasvavat rakennuksen pielessä, vetivät katseeni puoleensa. Voisin tutkia varastorakennusta.\n";
                    }
                    if (!_prGathered && !_ruinsChecked)
                    {
                        returnText = "Kävelin rakennusta kohti, varoen etten kompastu sammaleen alla piileskeleviin kiviin tai juuriin. " +
                            "Saavuin rakennuksen pihalle. Sienet, jotka kasvavat rakennuksen pielessä, vetivät katseeni puoleensa. " +
                            "Voin kokeilla penkoa rakennuksen raunioiden syövereistä aarteita—jos sellaisia sieltä löytyy. Voisin tutkia varastorakennusta.\n";
                    }
                    Console.Write(returnText);
                }

                // Player input
                Console.Write
                    (
                    "1. Kerää sienet\n" +
                    "2. Mene rakennukseen\n" +
                    "3. Tutki Raunioita\n" +
                    "4. Palaa alkuun\n\n"
                    );

                var pressedKey = Console.ReadKey(true);

                // Interaction choice
                switch (pressedKey.KeyChar)
                {
                    // Mushroom interactions.
                    case '1':
                        // if mushrooms are already gathered, inform player.
                        if (_prGathered == true)
                        {
                            string failedGathering = "Keräsit jo sienet, eikä täältä löydy mitään muuta kerättävää\n";
                            Console.Write(failedGathering);

                            Console.Write("Paina nappia jatkaaksesi");
                            Console.ReadKey(true);

                            break;
                        }
                        // Getting random number for gathering amount
                        int gatherAmount = Gathering.Gather(player);
                        string gatheringText = $"Kiersin ympäri rakennuksen, keräten kaikki sienet, jotka huomasin. Sain {gatherAmount} sientä.\n";
                        Console.Write(gatheringText);
                        _prGathered = true;

                        Console.Write("Paina nappia jatkaaksesi");
                        Console.ReadKey(true);

                        break;
                    // Building
                    case '2':
                        string buildingText = "";
                        // Check whether door has been attempted before or not.
                        switch (_buildingAttempted)
                        {

                            case false:
                                buildingText = "Koitin varastorakennuksen liukuvaa peltiovea, mutta se ei liikahtanutkaan. " +
                                    "Potkaisin ovea kaikin voimin, eikä se näyttänyt merkkiäkään romahduksesta. Turha toivo.\n";
                                break;
                            case true:
                                buildingText = "Potkaisin peltiovea niin kovaa, että kaiku sai linnut pakenemaan. Ovi ei liikahtanutkaan. Turha toivo.\n";
                                break;
                        }
                        // writing text.
                        Console.Write(buildingText);

                        Console.Write("Paina nappia jatkaaksesi");
                        Console.ReadKey(true);

                        break;
                    // Rauniot
                    case '3':
                        string ruinsText = "Kävelin raunioiden ääreen ja aloin penkoa. Puuta, puuta, lisää puuta, pari metalli pannua, " +
                            "taas puuta, ja sitten vielä vähän lisää puuta.\n";
                        string ruinsText2 = "Juuri kun olin menettämässä toivon, huomasin jotain pientä ja metallista romun syövereissä. " +
                            "Aloitin kaivuu työn jälleen. Kun romu oli sivuutettu, sain käsiini pienen, ruosteisen avaimen. Tämä varmaan tulee hyödyksi.\n";

                        _hasKey = true;
                        _ruinsChecked = true;

                        Console.Write(ruinsText);
                        Thread.Sleep(500);
                        Console.Write(ruinsText2);

                        Console.Write("Paina nappia jatkaaksesi");
                        Console.ReadKey(true);

                        break;
                    // Return back to the entrance of the forest.
                    case '4':
                        returnToStart = true;
                        break;
                }
            }
            MMEntrance(player);
        }
        #endregion

        #region Kellari
        static bool _kellariReturn = false;
        static void Kellari(Character player)
        {
            string kellariIntro = "Laitoin rakennukselta löytämäni avaimen kellariluukun avaimenreikään ja käänsin sitä. " +
                "Kun vetäisin kellariluukun kahvasta, se avautui märisten. Luukun toisella puolella oli tikkaat, jotka johtivat maanalaisiin syvyyksiin. " +
                "Aloin kiivetä tikkaita alas.\n\n";

            string kellariText1 = "Nyt olin kiitollisempi kuin koskaan lyhdystäni. Ilman sitä, en näkisi yhtikäs mitään kellarissa. " +
                "Kellari oli viileä ja märkä, ja jatkui pidemmälle kuin lyhtyni valo kantoi. Kellarin sisällä ei ollut mitään muuta kuin sieniä. " +
                "Mutta sieniä muuten riitti. Aloin keräämään niitä hirveää vauhtia. Sieni sienen jälkeen löysi tien nahkalaukkuuni. " +
                "En edes huomannut käveleväni yhä syvemmälle kellariin sienihurmassa. ";

            string kellariText2 = "Lopulta päädyin kellarin loppuun, jolloin myös sienet loppuivat. Sain {määrä} sientä. " +
                "Sienien loppu sai minut katsomaan ympärilleni ensimmäistä kertaa hetkeen. Kellari ei välttämättä ole edes oikea sana tälle paikalle. " +
                "Se on vain tunneli. Edessäni olevat tikkaat johtavat ylöspäin, enkä näe aloituspistettäni enää taakse katsoessa. Päätän kiivetä tikkaat ylös. ";

            string kellariText3 = "Toinen kellariluukku löytyi tikkaiden päästä. Työnsin sen auki, ja huomasin olevani jonkinlaisen rakennuksen sisällä. " +
                "Rakennus on pieni, eikä sieltä löydy paljoa. Kellariluukku maassa on varmaankin kaikista kiinnostavin asia siellä. Edessäni näen liukuvan " +
                "peltioven, jonka kahvojen ympärillä on ruosteinen kettinki. Kettingissä on kiinni lukko. Kokeilen aikaisemmin löytämääni avainta lukkoon. " +
                "Lukko aukesi vastustamatta. Revin kettingin pois kahvojen ympäriltä ja avaan liukuvan peltioven. ";
            Console.WriteLine("Olet nyt kellarissa!");
        }
        #endregion
        static void ResetArea()
        {
            // Class wide
            _hasKey = false;
            // Mustametsä Entrane
            _mmReturn = true;
            // Multapolku
            _cellarAttempted = false;
            _mpGathered = false;
            _multaPolkuReturn = false;
            // Metsänsyvyydet
            _metsanSyvReturn = false;
            // Puurakennus
            _puuRakennusReturn = false;
            _prGathered = false;
            _buildingAttempted = false;
            _ruinsChecked = false;
            // Kellari
            _kellariReturn = false;
        }
    }
}
