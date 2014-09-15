using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Firebar_segment : BaseObj
    {
        public BaseObj owner;
        //jumps 16 
        private short frameCount = 0;
        private short frameDelay = 0;

        public Firebar_segment(double x, double y, bool dir = false)
            : base(x, y, 8, 8)
        {
            this.despawnOffScreen = false;
            this.name = "Firebar_segment";
            this.texture = Texture.fireballshot;
            this.x = x;
            this.y = y;
            this.w = 8;
            this.h = 8;
        }

        public override string getName()
        { return this.name; }

        public override void process()
        {
            refreshColRect();
            getColGrid();

            frameCount++;
            if (frameCount == 10)
            {
                frameDelay++;
                frameCount = 0;
                if (frameDelay == 3)
                    frameDelay = 0;
            }
            
           
            Image.drawTileFrame(texture, frameDelay, 4, x, y, dir);
        }

    }
}

