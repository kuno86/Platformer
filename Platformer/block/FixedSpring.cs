using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK.Input;
using System.Threading;

namespace Game
{
    class FixedSpring : BaseObj
    {
        private short frame = 0;
        private short frames;
        private short frameDelay = 0;
        public bool active;
        public bool onGround, falling;
        public double springVel;
        private ColRect cRect;

        public FixedSpring(double x, double y, short type)
            : base(x, y, 16, 32)
        {
            this.name = "FixedSpring";
            switch(type)
            {
                default:
                case 0: this.texture = Texture.smb1_springFixed1; springVel = 4; break;
                case 1: this.texture = Texture.smb1_springFixed2; springVel = 7; break;
            }
            this.frames = 3;
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 32;
            colRect.w = 16;
            colRect.h = 32;
            cRect.x = x;
            cRect.y = y;
            cRect.w = w;
            cRect.h = h;
            this.type = type;
            grabable = false;
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
                            if (Map.spriteArray[i].colRect.y + Map.spriteArray[i].colRect.h - y >= 8 && Map.spriteArray[i].colRect.y + Map.spriteArray[i].colRect.h - y < 16)
                                frame = 1;
                            if (Map.spriteArray[i].colRect.y + Map.spriteArray[i].colRect.h - y >= 12 && Map.spriteArray[i].colRect.y + Map.spriteArray[i].colRect.h - y < 16)
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


