using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Smb2_pow_on : BaseObj
    {
        private short frame = 0;
        private short frameDelay = 0;

        public Smb2_pow_on(double x, double y)
            : base(x, y)
        {
            this.name = "Pow_on";
            this.texture = Texture.smb2_pow_on;
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.colWithBlocks = false;
            this.colWithOthers = false;
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
            if (frame == 4)
            {
                x = -100;   //
                y = -100;   // delete yourself after Animation (Position out of Scene will cause delete) 
                Map.powActive = 2;
            }
            Image.drawTileFrame(texture, frame, 4, x-12, y);
        }

        

    }
}
