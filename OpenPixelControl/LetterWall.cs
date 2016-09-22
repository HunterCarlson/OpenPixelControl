using System.Collections.Generic;
using System.Linq;

namespace OpenPixelControl
{

    // TODO: move to GUI project when done with console testing
    // belongs with StrangerThings project, not OPC
    public class LetterWall
    {
        private const int LedStrandLen = 50;

        // TODO: get colors for each light from reference images
        private static readonly Dictionary<char, Pixel> LetterColorMap = new Dictionary<char, Pixel>();

        private static readonly Dictionary<char, int> LetterIndexMap = new Dictionary<char, int>
        {
            {'A', 49},
            {'B', 48},
            {'C', 47},
            {'D', 46},
            {'E', 45},
            {'F', 44},
            {'G', 43},
            {'H', 42},
            {'I', 41},
            {'J', 40},
            {'K', 39},
            {'L', 38},
            {'M', 37},
            {'N', 36},
            {'O', 35},
            {'P', 34},
            {'Q', 33},
            {'R', 32},
            {'S', 31},
            {'T', 30},
            {'U', 29},
            {'V', 28},
            {'W', 27},
            {'X', 26},
            {'Y', 25},
            {'Z', 24}
        };

        public static int GetLetterIndex(char c)
        {
            return LetterIndexMap[c];
        }

        public static List<Pixel> CreateLetterFrame(char c)
        {
            List<Pixel> frame = new List<Pixel>();
            int letterIndex = GetLetterIndex(c);

            foreach (int i in Enumerable.Range(0,LedStrandLen))
            {
                if (i == letterIndex)
                {
                    frame.Add(new Pixel(100, 100, 100));
                }
                else
                {
                    frame.Add(OpcConstants.DarkPixel);
                }
            }
            return frame;
        }
    }
}