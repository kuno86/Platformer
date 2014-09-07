using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Game
{
    class PSwitch_b : BaseObj
    {
        private short frame = 0;
        private short frames;
        private short frameDelay = 0;
        public bool pressed;
        public bool onGround, falling;

        public PSwitch_b(double x, double y, short type = 2)
            : base(x, y, 16, 16)
        {
            this.name = "PSwitch_b";
            switch(type)
            {
                case 1: this.texture = Texture.smw_pSwitch_b; frames = 2; break;
                case 2: this.texture = Texture.smb3_pSwitch_b; frames = 4; break;
                default: this.texture = Texture.smw_pSwitch_b; frames = 2; break;
            }
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.type = type;
            grabable = true;
            pressed = false;
            this.colWithOthers = true;
        }

        public override string getName()
        { return name; }

        public override void process()
        {
            refreshColRect();
            getColGrid();
            
            if (getColXY((int)x + (w / 2), (int)y + h + 1) == 1)    //floorCol
            {
                if (colBottom == 1)
                    y--;
                onGround = true;
                falling = false;
            }
            else
            {
                y++;
                onGround = false;
                falling = true;
            }

            if (getColXY((int)x + (w / 2), (int)y - 1) == 1) 
            {
                if(colTop==1)
                    y++;
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
            x += xVel;

            if (!pressed)
            {
                frameDelay++;
                if (frameDelay == 3)
                {
                    frame++;
                    frameDelay = 0;
                }
                if (frame > frames-1)
                {
                    frame = 1;
                }
                Image.drawTileFrame(texture, frame-1, frames, x, y);
            }

            if (Map.pSwitchTimer_b > 0 && !pressed)
            {
                pressed = true;
                frameDelay = 90;
                Map.pSwitch_b = true;

                double tempX;                                                                               //
                double tempY;                                                                               //
                short tempType;                                                                             //
                for (int i = 0; i != Map.spriteList.Count(); i++)                                           //
                {                                                                                           //
                    if (Map.spriteList[i].name == "Coin")                                                   //
                    {                                                                                       //
                        tempX = Map.spriteList[i].x;                                            //
                        tempY = Map.spriteList[i].y;                                            //Buffer X, Y and type of the object to be replaced
                        tempType = Map.spriteList[i].type;                                      //
                        Map.spriteList[i] = new Qm_e(tempX, tempY, tempType);               //Overwrite position with new object data from Buffers
                    }                                                                                       //
                    else if (Map.spriteList[i].name == "?-Block_e")                                         //
                    {                                                                                       //
                        tempX = Map.spriteList[i].x;                                                        //
                        tempY = Map.spriteList[i].y;                                                        //
                        tempType = Map.spriteList[i].type;                                                  //
                        Map.spriteList[i] = new Coin(tempX, tempY, tempType);                               //
                    }                                                                                       //
                }                                                                                           //
            }

            if (pressed)
            {
                Image.drawTileFrame(texture, frames-1, frames, x, y);
                frameDelay--;
                if (frameDelay <= 0)
                {
                    x = -100;
                    y = -100;
                }
                Image.drawText(frameDelay.ToString(), (int)x, (int)y - 24, Color.Blue, Texture.ASCII);
            }
            
        }


    }
}
