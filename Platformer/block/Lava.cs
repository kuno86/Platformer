using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Lava : BaseObj
    {
        private short frame = 0;
        private short frames;
        private short frameDelay = 0;
        private BaseObj content = null;
        private bool flipV, flipH;

        public Lava(double x, double y, short type = 1, bool flipV=false, bool flipH=false)
            : base(x, y)
        {
            this.name = "Lava";
            this.type = type;
            switch (type)
            {
                default:
                case 1: this.texture = Texture.smb1_lava; frames = 8; this.w = 16; this.h = 16; break;
                case 2: this.texture = Texture.smb3_lava1; frames = 4; this.w = 16; this.h = 16; break;
                case 3: this.texture = Texture.smb3_lava22; frames = 4; this.w = 32; this.h = 32; break;
                case 4: this.texture = Texture.smb3_lava45; frames = 4; this.w = 16; this.h = 32; break;
            }
            this.x = x;
            this.y = y;
            
            this.colWithOthers = true;
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
            if (frame > frames)
            {
                frame = 1;
            }

            for (int i = 0; i != Map.spriteArrMax; i++)
            {
                if (Map.spriteArray[i] != null)
                {
                    if (getCol2Obj(colRect, Map.spriteArray[i].colRect))
                    {

                    }
                }
            }

            Image.drawTileFrame(texture, frame, frames, x, y,flipV,flipH);

        }


    }
}




