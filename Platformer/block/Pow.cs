using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Pow : BaseObj
    {
        private short frame = 0;
        private short frameDelay = 0;
        public bool onGround;

        public Pow(double x, double y)
            : base(x, y)
        {
            this.name = "POW-Block";
            this.texture = Texture.smb2_pow;
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            this.colRect.w = (short)this.w;
            this.colRect.h = (short)this.h;
            this.colWithOthers = true;
            grabable = true;
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
            if (frame > 8)
            {
                frame = 1;
            }
            Image.drawTileFrame(texture, frame, 8, x, y);

            
            for (int i = 0; i != Map.spriteArrMax; i++)
            {
                if (Map.spriteArray[i] != null && Map.spriteArray[i].name == "Player")
                {
                    if (Map.spriteArray[i].x + Map.spriteArray[i].w > x &&
                        Map.spriteArray[i].x < x + w &&
                        Map.spriteArray[i].y + Map.spriteArray[i].h > y &&
                        Map.spriteArray[i].y < y + h)
                    {
                        Map.spriteAdd(new Smb2_pow_on(x, y));
                        x = -100;
                        y = -100;
                    }

                }
            }

        }


    }
}

