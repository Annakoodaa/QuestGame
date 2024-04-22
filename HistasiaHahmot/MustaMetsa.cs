using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame
{
    internal class MustaMetsa
    {
        public static void MMEntrance()
        {
            Console.WriteLine("Mustametsä:");
            Console.Write("Kun astuin mustametsään, tunsin kengänpohjani uppoavan sammaleen. Ilma oli kostea, eikä auringonvalo pilkistänyt tiheän neulaskaton läpi. " +
                "En nähnyt muutamaa metriä pidemmälle metsän syvyyksiin. Olin valmistautunut tähän. Irrotin lyhtyni vyötäröltäni, ja avasin sen luukun. " +
                "Luukun ruosteinen ratina avautuessaan kaikui hiljaisuudessa. Se otti muutaman yrityksen, mutta lopulta onnistuin sytyttämään liekin lyhdyn" +
                " sisällä—nyt toivotaan, ettei se sammu. \r\n\r\nNyt kun minulla on valoa, hahmotan ympäristöni enemmässä tarkkuudessa. Vasemmalle johtaa " +
                "lähes täysin tukkoon kasvanut multapolku. Oikealla, näen pelkkää pusikkoa niin pitkälle kuin silmä kantaa. Edessä päin näen jonkinlaisen " +
                "mätänevän puurakennuksen. Voin tietenkin myös lähteä takaisin mistä tulin, mutta ilman tarvitsemiani tavaroita, se tarkoittaisi tehtävän hylkäystä.\n");

            // 1 multapolku 2. Metsänsyvyydet 3. Rakennus
            Console.Write
                (
                "1. Multapolku\n" +
                "2. Metsänsyvyys\n" +
                "3. Puurakennus\n"
                );

            // Input
            bool validInput = false;
            ConsoleKeyInfo pressedKey = new ConsoleKeyInfo();
            while (!validInput)
            {
                pressedKey = Console.ReadKey(true);
                switch (pressedKey.KeyChar)
                {
                    case char c when (c == '1' || c == '2' || c == '3'):
                        validInput = true;
                        break;
                    default:
                        break;
                }
            }

        }

        static void MultaPolku()
        {
            Console.Write("Kävelin multapolkua pitkin useita minuutteja, huitoen samalla puskia ynnä muita pensaita pois tieltäni miekallani. " +
                "Lopulta, päädyin aukiolle. Aurinko säteili läpi neulaskaton vain tähän tiettyyn kohtaan. Aukio ei vaikuttanut olevan luonnollinen näky, " +
                "sillä puiden raadot olivat jätetty aukiolla esille, todennäköisesti jotta seikkailijat voivat käyttää niitä istuimina. " +
                "Aukion reunuksella on runsaasti sieniä. Kun katson aukiota tarkemmin, huomaan että osa maasta näyttää hauraammalta kuin muut. " +
                "Alan kaivamaan haurasta maata kaksin käsin. Ennen pitkään, maa toi esiin puisen kellariluukun. \r\n\r\nVoin kerätä sienet ympäristöstä, " +
                "tai koettaa avata kellariluukun. Polku on loppunut tähän, enkä uskalla vaeltaa metsän syvyyksiin suunnitelmatta, joten en voi liikkua " +
                "muualle kuin takaisin alkuun.\n");

            Console.Write
                (
                "1. Kerää sienet\n" +
                "2. Avaa kellari\n" +
                "3. Palaa alkuun\n"
                );
            /* ...
             * Jos pelaaja kerää sienet: Kumarruin sieniä keräämään. Yksi toisen perään, tungin sieniä reipasta tahtia nahkalaukkuuni. Sain {määrä} sientä. 

               Jos pelaaja kokeilee kellariluukkua ilman avainta ensimmäistä kertaa: Vedin kellariluukun ruosteisesta kahvasta, mutta luukku ei liikahtanutkaan. Harkitsin yrittää murskata luukun, mutta lohkeilevan puun alla näkyy metallinen hohto. Parempi olla yrittämättä, ettei miekka tylsy. 

               Jos pelaaja kokeilee kellariluukkua ilman avainta ensimmäisen kerran jälkeen: Kellariluukku ei avautunut tälläkään kertaa. Kahvan alla on pieni avaimenreikä. Löytyyköhän tähän avain jostain? 

               Multapolku—paluu, sienet kerätty: Aukiolla jälleen. Istahdan puun raadolle, pohtimaan seuraavaa liikkua. Voin kokeilla avata kellariluukun, mutta muuta ei tule mieleen. 

               Multapolku—paluu, sienet jäljellä: Aukiolla jälleen. Reunuksella olevat sienet vetävät katseeni niiden puoleen. Voin kerätä sienet, tai kokeilla avata kellariluukun. 
             */
        }
    }
}
