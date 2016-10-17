using System.Collections.Generic;

namespace OpenPixelControl
{
    public class FrameAnimation
    {
        public FrameAnimation(List<Frame> frames)
        {
            Frames = frames;
        }

        public FrameAnimation()
        {
            Frames = new List<Frame>();
        }

        public List<Frame> Frames { get; set; }

        public void Add(Frame frame)
        {
            Frames.Add(frame);
        }
    }
}