using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK.Input;
using System.Threading;

namespace Game
{
    class Spring_p : BaseObj
    {
        private short frame = 0;
        private short frames;
        private short frameDelay = 0;
        public bool active;
        public bool onGround, falling;
        public double springVel;
        private ColRect cRect;

        public Spring_p(double x, double y)
            : base(x, y, 16, 16)
        {
            this.name = "Spring_p"; 
            this.texture = Texture.smw_spring_p; 
            this.frames = 3;
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            colRect.w = 16;
            colRect.h = 16;
            this.springVel = -3.2;
            cRect.x = x;
            cRect.y = y;
            cRect.w = w;
            cRect.h = h;
            this.type = type;
            grabable = true;
            active = false;
            this.colWithBlocks = true;
            this.colWithOthers = true;
        }

        public override string getName()
        { return name; }
                
        public override void doSubAI()
        {
            refreshColRect();

            cRect.x = x;
            cRect.y = y;
            cRect.w = w;
            cRect.h = h;

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
            frame = 0;

            var keyboard = Keyboard.GetState();

            for (int i = 0; i != Map.spriteArrMax; i++)
            {
                if (Map.spriteArray[i] != null && Map.spriteArray[i].id != this.id)
                {
                    if (Map.spriteArray[i].colWithOthers || colWithBlocks)
                    {
                        if (getCol2Obj(Map.spriteArray[i].colRect, cRect))
                        {
                            if (Map.spriteArray[i].colRect.y + Map.spriteArray[i].colRect.h - y >= 4 && Map.spriteArray[i].colRect.y + Map.spriteArray[i].colRect.h - y < 16)
                                frame = 1;
                            if (Map.spriteArray[i].colRect.y + Map.spriteArray[i].colRect.h - y >= 6 && Map.spriteArray[i].colRect.y + Map.spriteArray[i].colRect.h - y < 16)
                            {
                                frame = 2;
                                if (Map.spriteArray[i].name == "Player")
                                {
                                    if (keyboard[Key.Z] || keyboard[Key.Y])
                                        Map.spriteArray[i].setXYVel(Map.spriteArray[i].xVel, springVel - 3.2);
                                    else
                                        Map.spriteArray[i].setXYVel(Map.spriteArray[i].xVel, springVel);
                                }
                                else
                                { Map.spriteArray[i].setXYVel(Map.spriteArray[i].xVel, springVel); }
                            }
                        }
                    }
                }
            }
        }

        public override void doRender()
        {
            MyImage.drawTileFrame(texture, frame, frames, x, y);
            //MyImage.drawText((Map.spriteArray[i].colRect.y + Map.spriteArray[i].colRect.h - y).ToString(), (int)x + w + 3, (int)y, Color.Red, Texture.ASCII);
            //MyImage.drawText("yVel: " + Map.spriteArray[i].yVel, (int)x + w + 3, (int)y + 12, Color.Red, Texture.ASCII);
        }

    }
}

