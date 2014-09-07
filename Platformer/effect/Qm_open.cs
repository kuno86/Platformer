using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Qm_open : BaseObj
    {
        private short frame = 0;
        private short frameDelay = 0;

        public Qm_open(double x, double y)
            : base(x, y)
        {
            this.name = "?-Block_open";
            this.texture = Texture.qm_open;
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
        }

        public override string getName()
        { return name; }

        public override void process()
        {
            refreshColRect();
            frameDelay++;
            if (frameDelay == 3)
            {
                frame++;
                frameDelay = 0;
            }
            if (frame > 5)
            {
                x = -100;   //
                y = -100;   // delete yourself after Animation (Position out of Scene will cause delete) 
            }
            Image.drawTileFrame(texture, frame, 3, x, y);
        }


    }
}
