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
        private bool onGround, falling;


        public Potaboo(double x, double y, bool dir = false, short type = 1, double yVel= -3.32)
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
            this.type = type;
            this.yVel = yVel;
            this.dir = dir; //Startdirection: true = Up/Left(-) ; false = Down/Right(+)
            jumpDelay = Map.rnd.Next(20, 120);
        }

        public override string getName()
        { return name; }

        public override void process()
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
                        
            if (jumpDelay == 0)
            {
                yVel = -3.32;
                dir = true;
            }
            if (getColXY((int)x + (w / 2), (int)(y + yVel + h + 1)) == 1)    //floorCol
            {
                if (colBottom == 1)
                {
                    y --;
                }
                onGround = true;
                falling = false;
            }

            if (getColXY((int)x + (w / 2), (int)y - 1) == 1)
            {
                if (colTop == 1)
                    yVel += Map.gravity;
                onGround = false;
                falling = true;
            }
            else
            {
                yVel += Map.gravity;
                onGround = false;
                falling = true;
            }

            y += yVel;


            Image.drawTileFrame(texture, frame, frames, x, y, false, dir);
        }


    }
}




