using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;

namespace EmeraldsCalloutPackLSPDFR
{
    public class Utilities
    {
        public bool IsEndCalloutKeyDown()
        {
            return Game.IsKeyDownRightNow(System.Windows.Forms.Keys.End);
        }
    }
}
