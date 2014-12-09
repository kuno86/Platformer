using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing.Imaging;
using System.Threading;

namespace Game
{
    class Potaboo : BaseObj
    {
        private short frame = 0;
        private short frames;
        private short frameDelay = 0;
        public int hbW = 16;
        public int hbH = 16;
        //jumps 40 Pixel high
        public bool isDead = false; //does not respawn after it was killed
        private int jumpDelay;
        private double originY;
        private double jumpVel;

        public Potaboo(double x, double y, bool dir = false, short type = 1, double jumpVel= -3.32)
            : base(x, y, 16, 16)
        {
            this.name = "Potaboo";
            switch (type)
            {
                default:
                case 1: this.texture = Texture.smb1_potaboo; frames = 3; break;
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
            this.jumpVel = jumpVel;
            originY=y;
            this.dir = dir; //Startdirection: true = Up/Left(-) ; false = Down/Right(+)
            jumpDelay = Map.rnd.Next(20, 120);
        }

        public override string getName()
        { return name; }

        public override void doSubAI()
        {
            refreshColRect();
            getColGrid();
            frameDelay++;
            if (frameDelay == 3)
            {
                frame++;
                frameDelay = 0;
            }
            if (frame > frames)
            {
                frame = 0;
            }



            if (y >= originY)
            {
                yVel = 0;
                y = originY;
                if (jumpDelay > 0)
                    jumpDelay--;
                if (jumpDelay == 0)
                {
                    jumpDelay = Map.rnd.Next(80, 160);
                    yVel = jumpVel;
                }
            }

            if (yVel >= 0)
                dir = true;
            else
                dir = false;

            yVel += Map.gravity;
            y += yVel;
        }

        public override void doRender()
        {
            MyImage.drawTileFrame(texture, frame, frames, x, y, false, dir);
            //Image.drawText(("t " + jumpDelay), (int)x, (int)originY + 12, Color.White, Texture.ASCII);
        }

    }
}




