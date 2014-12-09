using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Game
{
    class Brickshatter : BaseObj
    {
        private short frames;
        private short frame = 0;
        private short frameDelay = 0;
        private double xVel, yVel;

        public Brickshatter(int textur, short frames, double x, double y, double xVel, double yVel)
            : base(x, y)
        {
            this.name = "Brickshatter";
            this.texture = textur;
            this.frames = frames;
            this.x = x;
            this.y = y;
            this.xVel = xVel;
            this.yVel = yVel;
            this.w = 8;
            this.h = 8;
            this.colOffsetX = (short)this.x;
            this.colOffsetY = (short)this.y;
            this.colRect.w = (short)this.w;
            this.colRect.h = (short)this.h;
            this.colWithBlocks = false;
            this.colWithOthers = false;
        }

        public override string getName()
        { return name; }

        public override void doSubAI()
        {
            yVel += Map.gravity;

            frameDelay++;
            if (frameDelay == 3)
            {
                frame++;
                frameDelay = 0;

                if (xVel - 1 > 0 || xVel + 1 < 0)
                {
                    if (xVel > 0)
                        xVel -= 0.75;
                    else if (xVel < 0)
                        xVel += 0.75;
                }
            }
            if (frame > frames)
            {
                frame = 1;
            }
            x += xVel;
            y += yVel;
        }

        public override void doRender()
        {
            MyImage.drawTileFrame(texture, frame, frames, x, y);
        }
    }
}
