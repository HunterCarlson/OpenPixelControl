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
            {'G', 0},
            {'H', 0},
            {'I', 0},
            {'J', 0},
            {'K', 0},
            {'L', 0},
            {'M', 0},
            {'N', 0},
            {'O', 0},
            {'P', 0},
            {'Q', 0},
            {'R', 0},
            {'S', 0},
            {'T', 0},
            {'U', 0},
            {'V', 0},
            {'W', 0},
            {'X', 0},
            {'Y', 0},
            {'Z', 0}
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
                    frame.Add(OpcConstants.OffPixel);
                }
            }
            return frame;
        }
    }
}