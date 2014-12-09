using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Game
{
    class Blooper : BaseObj
    {
        public bool onGround, falling;
        private short delay;
        private short frame;

        public Blooper(double x, double y, short type = 0, bool dir = false)
            : base(x, y, 16, 16)
        {
            this.name = "Blooper";
            switch (type)
            {
                case 0:
                default: this.texture = Texture.smb1_blooper; break;
                case 1: this.texture = Texture.smb3_blooper; break;
                //case 2: this.texture = Texture.smb2_key; break;
                
            }
            this.blockTop = false;
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
            this.colWithBlocks = true;
            this.colWithOthers = true;
            this.delay = 5;
            this.frame = 0;
        }

        public override string getName()
        { return name; }

        public override void doSubAI()
        {
            refreshColRect();
            getColGrid();

            //this.inWater = false;
            //for (int i = 0; i <= Map.spriteArrMax; i++)
            //{
            //    if (Map.spriteArray[i] != null && Map.spriteArray[i].name == "WaterArea")
            //    {
            //        if (getCol2Obj(this.colRect, Map.spriteArray[i].colRect))
            //            this.inWater = true;
            //    }
            //}

            if (inWater())
            {
                for (int i = 0; i != Map.spriteArrMax; i++)
                {
                    if (Map.spriteArray[i] != null && Map.spriteArray[i].name == "Player")
                    {
                        if (Map.spriteArray[i].colRect.y < colRect.y)
                        {
                            if (delay > 0)
                            {
                                delay--;
                                if (delay == 15)
                                    frame = 1;
                            }

                            if (Map.spriteArray[i].colRect.x < colRect.x && delay == 0)   //Player on the left side
                            {
                                xVel = -2;
                                yVel = -2.8;
                                frame = 0;
                                delay = 60;
                            }
                            if (Map.spriteArray[i].colRect.x > colRect.x && delay == 0)   //Player on the left side
                            {
                                xVel = 2;
                                yVel = -2.8;
                                frame = 0;
                                delay = 60;
                            }
                        }
                    }
                }
            }

            
            if (getColXY((int)x + (w / 2), (int)y - 1) == 1)    //topCol
            {
                while (colTop == 1)
                {
                    y++;
                    refreshColRect();
                    getColGrid();
                }
                yVel = 0;
                onGround = false;
                falling = true;
            }
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
            
            
            if (xVel < 0)
            { xVel += 0.04; }
            if (xVel > 0)
            { xVel -= 0.04; }
            if (xVel > -0.04 && xVel < 0.04)
                xVel = 0;
                       

            y += yVel;
            x += xVel;
        }

        public override void doRender()
        {
            if (RootThingy.debugInfo)
            {
                MyImage.drawText("t: " + delay, (int)(x + w), (int)y, Color.White, Texture.ASCII);
                if (inWater())
                    MyImage.drawText("W1", (int)(x), (int)(y - 12), Color.White, Texture.ASCII);
                else
                    MyImage.drawText("W0", (int)(x), (int)(y - 12), Color.White, Texture.ASCII);
            }
            MyImage.drawTileFrame(texture, frame, 2, x, y);
        }
    }
}



