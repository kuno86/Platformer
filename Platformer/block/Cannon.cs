using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Game
{
    class Cannon : BaseObj
    {
        private short frame = 0;
        private short frameDelay = 0;
        private bool flipV;
        private int wait;
        private bool homing;

        public Cannon(double x, double y, short type = 1, bool flipV=false)
            : base(x, y)
        {
            this.name = "Cannon";
            switch (type)
            {
                case 1: this.texture = Texture.smb1_cannon; break;
                case 2: this.texture = Texture.smb3_cannon; homing = false; break;
                case 3: this.texture = Texture.smb3_cannon; homing = true; break;
                case 4: this.texture = Texture.smw_cannon; break;
                default: this.texture = Texture.smb1_cannon; break;
            }
            wait = Map.rnd.Next(90, 300);
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.flipV = flipV;
            this.type = type;
            this.colWithOthers = true;
        }

        public override string getName()
        { return name; }

        public override void process()
        {
            if (wait > 0)
                wait--;

            for (int i = 0; i != Map.spriteList.Count(); i++)
            {
                if (Map.spriteList[i].name == "Player")
                {
                    if (wait <= 0 && (Map.spriteList[i].colRect.x - (Map.spriteList[i].colRect.w / 2) - x - (w / 2) > 16))
                    { 
                        Map.spriteList.Add(new Bulletbill(x + 16, y, false, type, homing));
                        Map.spriteList.Add(new Smoke(x + 16, y, type));                     //shot right
                        wait = Map.rnd.Next(90, 300);
                    }

                    if (wait <= 0 && (Map.spriteList[i].colRect.x + (Map.spriteList[i].colRect.w / 2) - x + (w / 2) < -16))
                    { 
                        Map.spriteList.Add(new Bulletbill(x - 16, y, true, type, homing));
                        Map.spriteList.Add(new Smoke(x - 16, y, type));                     //shoot left
                        wait = Map.rnd.Next(90, 300);
                    }
                        
                    
                }
            }
            
            Image.drawImage(texture, x, y,flipV);
            //Image.drawText("t:" + wait, (int)x, (int)y - 12, Color.White, Texture.ASCII);

        }




    }
}
