using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Fireball_hit : BaseObj
    {

        private short frame = 0;
        private short frameDelay = 0;

        public Fireball_hit(double x, double y) 
            : base(x, y)
        {
            this.name = "Fireball_hit";
            this.texture = Texture.fireball_hit;
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.colWithOthers = false;
            this.colWithBlocks = false;
        }

        public override string getName()
        { return name; }

        public override void doSubAI()
        {
            refreshColRect();
            frameDelay++;
            if (frameDelay == 3)
            {
                frame++;
                frameDelay = 0;
            }
            if (frame > 3)
            {
                x = -100;   //
                y = -100;   // delete yourself after Animation (Position out of Scene will cause delete) 
            }
        }

        public override void doRender()
        {
            MyImage.drawTileFrame(texture, frame, 3, x, y);
        }    
    }
}
