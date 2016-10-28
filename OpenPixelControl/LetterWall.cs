using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenPixelControl
{
    // TODO: move to GUI project when done with console testing
    // belongs with StrangerThings project, not OPC
    public class LetterWall
    {
        private const int LedStrandLen = 50;

        private static readonly Dictionary<char, int> LetterIndexMap = new Dictionary<char, int>
        {
            {'A', 2},
            {'B', 3},
            {'C', 4},
            {'D', 5},
            {'E', 6},
            {'F', 7},
            {'G', 8},
            {'H', 9},
            {'I', 26},
            {'J', 25},
            {'K', 24},
            {'L', 23},
            {'M', 22},
            {'N', 21},
            {'O', 20},
            {'P', 19},
            {'Q', 17},
            {'R', 34},
            {'S', 35},
            {'T', 36},
            {'U', 37},
            {'V', 38},
            {'W', 39},
            {'X', 40},
            {'Y', 41},
            {'Z', 42}
        };

        private readonly Pixel[] _lightColors =
        {
            Pixel.RedPixel(), Pixel.YellowPixel(), Pixel.GreenPixel(), Pixel.BluePixel()
        };

        private readonly Random _rng = new Random();

        private static int GetLetterIndex(char c)
        {
            return LetterIndexMap[c];
        }

        public List<Pixel> CreateLetterFrame(char c)
        {
            var frame = new List<Pixel>();
            int letterIndex;

            if (LetterIndexMap.ContainsKey(c))
                letterIndex = GetLetterIndex(c);
            else
                letterIndex = -1;

            foreach (var i in Enumerable.Range(0, LedStrandLen))
                if (i == letterIndex)
                {
                    var rand = _rng.Next(4);
                    var pixel = _lightColors[rand];
                    frame.Add(pixel);
                }
                else
                {
                    frame.Add(OpcConstants.DarkPixel);
                }
            return frame;
        }
    }
}