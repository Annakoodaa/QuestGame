using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuestGame
{
    public class Utilities
    {
        // Public utility fields.
        public const int s_selectionDelay = 2000;
        public const int s_TextChapterDelay = 1500;

        // Text speed.
        static int s_textSpeed = 20;

        // Method for text writing
        public static void TextWriter(string text)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(s_textSpeed);
            }
        }

        // Press button to continue
        public static void PressToContinue()
        {
            Thread.Sleep(s_selectionDelay);
            Console.Write("\nPaina nappia jatkaaksesi");
            Console.ReadKey(true);
        }
    }
}
