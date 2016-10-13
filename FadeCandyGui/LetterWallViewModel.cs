using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenPixelControl;

namespace FadeCandyGui
{
    public class LetterWallViewModel
    {
        public LetterWallViewModel()
        {
            OnDuration = 1;
            OffDuration = 1;
            Message = "RUN";
            OpcServer = "127.0.0.1";
            OpcPort = OpcConstants.DefaultPort.ToString();
        }
        public double OnDuration { get; set; }
        public double OffDuration { get; set; }
        public string Message { get; set; }
        public string OpcServer { get; set; }
        public string OpcPort { get; set; }
    }
}
