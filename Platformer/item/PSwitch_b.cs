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
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            this.colRect.w = (short)this.w;
            this.colRect.h = (short)this.h;
            this.type = type;
            grabable = true;
            pressed = false;
            this.colWithBlocks = true;
            this.colWithOthers = true;
        }

        public override string getName()
        { return name; }

        public override void doSubAI()
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

            y = y + yVel;
            x += xVel;

            if (!pressed)
            {
                frameDelay++;
                if (frameDelay == 3)
                {
                    frame++;
                    frameDelay = 0;
                }
                if (frame > frames - 1)
                {
                    frame = 1;
                }
            }

            if (Map.pSwitchTimer_b > 0 && !pressed)
            {
                pressed = true;
                frameDelay = 90;

                double tempX;                                                                               //
                double tempY;                                                                               //
                short tempType;
                if (!Map.pSwitch_b)
                {
                    Map.pSwitch_b = true;
                    for (int i = 0; i != Map.spriteArrMax; i++)                                           //
                    {                                                                                           //
                        if (Map.spriteArray[i] != null)
                        {
                            if (Map.spriteArray[i].name == "Coin")                                                   //
                            {                                                                                       //
                                tempX = Map.spriteArray[i].x;                                            //
                                tempY = Map.spriteArray[i].y;                                            //Buffer X, Y and type of the object to be replaced
                                tempType = Map.spriteArray[i].type;                                      //
                                Map.spriteArray[i] = new Qm_e(tempX, tempY, tempType);               //Overwrite position with new object data from Buffers
                            }                                                                                       //
                            else if (Map.spriteArray[i].name == "?-Block_e")                                         //
                            {                                                                                       //
                                tempX = Map.spriteArray[i].x;                                                        //
                                tempY = Map.spriteArray[i].y;                                                        //
                                tempType = Map.spriteArray[i].type;                                                  //
                                Map.spriteArray[i] = new Coin(tempX, tempY, tempType);                               //
                                Map.map[(int)y / 16, (int)x / 16, 0] = 0;
                            }                                                                                       //
                        }
                    }//
                }
            }

            if (pressed)
            {
                frameDelay--;
                if (frameDelay <= 0)
                {
                    x = -100;
                    y = -100;
                }
                
            }
        }

        public override void doRender()
        {
            if(pressed)
                MyImage.drawTileFrame(texture, frames - 1, frames, x, y);
            else
                MyImage.drawTileFrame(texture, frame - 1, frames, x, y);

            //MyImage.drawText(frameDelay.ToString(), (int)x, (int)y - 24, Color.Blue, Texture.ASCII);
        }

    }
}
