using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class AniFrame
    {
        public short id;
        public bool flipV;
        public bool flipH;

        public AniFrame(short id, bool flipV, bool flipH)
        {
            this.id = id;
            this.flipV = flipV;
            this.flipH = flipH;
        }
    }
}
