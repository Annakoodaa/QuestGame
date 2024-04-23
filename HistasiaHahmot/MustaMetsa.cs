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
                "4. Palaa aluevalintaan."
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
            while (true)
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
                                Console.WriteLine();
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
                        MMEntrance(player);
                        break;
                }

                Thread.Sleep(1500);

            }
        }
        #endregion

        #region Metsänsyvyydet
        // Field for entry check.
        static bool metsanSyvReturn = false;
        static void Metsansyvyydet(Character player)
        {
            Console.Clear();
            switch (metsanSyvReturn)
            {
                // Trying to enter the second time.
                case true:
                    Console.Write("Katsoin syvyyksiin, harkiten paluuta. Sitten muistin viimeisen kokemukseni, ja totesin ottaneeni opiksi.");
                    break;
                // Entering for the first time.
                case false:
                    Console.Write("Keräsin rohkeuteni ja astuin mustametsän synkkiin syövereihin. " +
                            "Puskat ja pensaat olivat liian tiheästi kasvaneita katkaistakseen miekalla. " +
                            "Syrjin oksia ja lehtiä, tuntien niiden lyövän heti takaisin. Kaduin päätöksiäni jo nyt.\n");
                    Thread.Sleep(1000);
                    int gatheredAmount = Gathering.Gather(player);
                    Console.Write("Lopulta pääsin vain parinkymmenen metrin matkan päähän alkupisteestäni. " +
                        $"Keräsin satunnaiset sienet matkalla, ja lopulta sain {gatheredAmount} sientä tästä vaivasta. " +
                        "Ei tarpeeksi, jos minulta kysytään.");
                    Thread.Sleep(1000);
                    Console.Write("Palattuani alkuun, vannoin pysyvän kaukana noista syvyyksistä tästä edespäin. " +
                        "Mutta pitäisikö mennä kohti puu rakennusta, vaiko valita multapolun? ");
                    metsanSyvReturn = true;
                    break;       
            }
            // Return to forest entrance
            MMEntrance(player);
        }
        #endregion

        #region Puurakennus
        static bool _puuRakennusReturn = false;
        static bool _prGathered = false;
        static void PuuRakennus(Character player)
        {
            Console.Clear();
            // Intro Text
            string puuRakennusIntroP1 = "Kävelin läpi vyötärölle kurottavan ruohikon matkalla puurakennukselle. " +
                "Yhtäkkiä, tunsin jalkani osuvan johonkin. En ehtinyt edes rekisteröimään mitä tapahtui, ennen kuin tapasin maan halauksen. " +
                "Nousin kivuliaasti takaisin jaloilleni. Ruohikon alla oleva kostea sammal ei pehmentänyt laskeutumista, mutta kyllä se vaatteeni kasteli. " +
                "Jatkoin matkaa, tuntien itseni märäksi koiraksi.";
            string puuRakennusIntroP2 = "Kun saavuin rakennuksen edustalle, huomasin että rakennuksen katto oli romahtanut. " +
                "Sen sisimmissä ei näkynyt mitään muuta kuin romua, ja sen romun syövereistä kasvoi valtava mänty. " +
                "Piha-alueella ei ollut mitään muita puita, mutta neulaskatto oli silti ihan yhtä tiheä—täysin sen yhden männyn johdosta. " +
                "Oliko katto romahtanut männyn kasvusta, vai kasvoiko mänty katon romahduksesta? Ei kai sillä mitään väliä ole. ";
            string puuRakennusIntroP3 = "Piha-alueella näkyi useita kiinnostuksen kohteita. Rakennuksen ympärillä kasvoi sieniä, jotka voisin kerätä. " +
                "Pihan vasemmassa osassa näkyy olevan jonkinlainen varastorakennus. Voisin tietysti myös mennä penkomaan rakennuksen raunioita, jos sieltä vielä löytyisi jotain. ";

            // Writing intro text.
            Console.Write(puuRakennusIntroP1);
            Thread.Sleep(500);
            Console.Write(puuRakennusIntroP2);
            Thread.Sleep(500);
            Console.Write(puuRakennusIntroP3);

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
                        Console.Write("Keräsit jo sienet, eikä täältä löydy mitään muuta kerättävää\n");
                        break;
                    }
                    // Getting random number for gathering amount
                    int gatherAmount = Gathering.Gather(player);
                    Console.Write($"Kiersin ympäri rakennuksen, keräten kaikki sienet, jotka huomasin. Sain {gatherAmount} sientä.\n");
                    _prGathered = true;
                    break;
                // Varastorakennus
                case '2':
     
                    break;
                // Rauniot
                case '3':

                    break;
                // Return back to the entrance of the forest.
                case '4':
                    MMEntrance(player);
                    break;
            }
        }
        #endregion

        #region Kellari
        static bool _kellariReturn = false;
        static void Kellari(Character player)
        {

        }
        #endregion
        static void ResetArea()
        {
            _mmReturn = true;
            _hasKey = false;
            _cellarAttempted = false;
            _mpGathered = false;
            _multaPolkuReturn = false;
        }
    }
}
