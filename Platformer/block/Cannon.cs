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
        public bool onGround, falling;

        public Cannon(double x, double y, short type = 1, bool flipV=false, bool fixd=false)
            : base(x, y, 16, 16)
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
            this.blockTop = true;
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            this.colRect.w = (short)this.w;
            this.colRect.h = (short)this.h;
            this.flipV = flipV;
            this.type = type;
            this.colWithOthers = true;
            this.colWithBlocks = true;
        }

        public override string getName()
        { return name; }

        public override void process()
        {
            refreshColRect();
            getColGrid();

            if (getColXY((int)x + (w / 2), (int)y + h + 1) == 1)    //floorCol
            {
                while (colBottom == 1)
                {
                    y--;
                    refreshColRect();
                    getColGrid();
                }
                yVel = 0;
                onGround = true;
                falling = false;
            }
            else
            {
                yVel += Map.gravity;
                onGround = false;
                falling = true;
            }


            if (getColXY((int)x - 1, (int)y + (h / 2)) == 1)    //RightCol
            {
                xVel = 0;
                falling = true;
            }
            if (getColXY((int)x + w + 1, (int)y + (h / 2)) == 1)    //LeftCol
            {
                xVel = 0;
                falling = true;
            }

            y += yVel;
            x += xVel;

            if (wait > 0)
                wait--;

            for (int i = 0; i != Map.spriteArrMax; i++)
            {
                if (Map.spriteArray[i] != null && Map.spriteArray[i].name == "Player")
                {
                    if (wait <= 0 && (Map.spriteArray[i].colRect.x - (Map.spriteArray[i].colRect.w / 2) - x - (w / 2) > 16))
                    {
                        Map.spriteAdd(new Bulletbill(x + 16, y, false, type, homing));
                        Map.spriteAdd(new Smoke(x + 16, y, type));                     //shot right
                        wait = Map.rnd.Next(90, 300);
                    }

                    if (wait <= 0 && (Map.spriteArray[i].colRect.x + (Map.spriteArray[i].colRect.w / 2) - x + (w / 2) < -16))
                    {
                        Map.spriteAdd(new Bulletbill(x - 16, y, true, type, homing));
                        Map.spriteAdd(new Smoke(x - 16, y, type));                     //shoot left
                        wait = Map.rnd.Next(90, 300);
                    }


                }
            }
            
            Image.drawImage(texture, x, y,flipV);
            //Image.drawText("t:" + wait, (int)x, (int)y - 12, Color.White, Texture.ASCII);

        }




    }
}
