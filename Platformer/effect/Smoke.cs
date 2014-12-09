using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Smoke : BaseObj
    {
        private short frames;
        private short frame = 0;
        private short frameDelay = 0;

        public Smoke(double x, double y, short type=1)
            : base(x, y)
        {
            this.name = "Smoke";
            switch(type)
            {
                default:
                case 1: this.texture = Texture.smb1_smoke; frames = 3; break;
                case 2: this.texture = Texture.smb2_smoke; frames = 3; break;
                case 3:
                case 4: this.texture = Texture.smb3_smoke; frames = 4; break;
            }
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.colWithBlocks = false;
            this.colWithOthers = false;
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
            if (frame > frames)
            {
                x = -100;   //
                y = -100;   // delete yourself after Animation (Position out of Scene will cause delete) 
            }
        }

        public override void doRender()
        {
            MyImage.drawTileFrame(texture, frame, frames, x, y);
        }
    }
}
