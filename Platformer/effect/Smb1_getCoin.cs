using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Smb1_getCoin : BaseObj
    {

        private short frame = 0;
        private short frameDelay = 0;

        public Smb1_getCoin(double x, double y)
            : base(x, y)
        {
            this.name = "Smb1_getCoin";
            this.texture = Texture.smb1_getCoin;
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
            y--;
            if (frameDelay == 3)
            {
                frame++;
                frameDelay = 0;
            }
            if (frame > 7)
            {
                x = -100;   //
                y = -100;   // delete yourself after Animation (Position out of Scene will cause delete) 
            }

            Image.drawTileFrame(texture, frame, 7, x, y);
        }


    }
}