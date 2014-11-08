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
    class Hammerbros_hammer : BaseObj
    {
        private short frame = 0;
        private short frames;
        private short frameDelay = 0;
        public int hbW = 16;
        public int hbH = 16;
        private double yVel;
        //jumps 40 Pixel high
        public bool isDead = false; //does not respawn after it was killed


        public Hammerbros_hammer(double x, double y, bool dir = false, double yVel = -3.32, short type = 1)
            : base(x, y, 16, 16)
        {
            this.name = "Hammerbros_hammer";
            switch (type)
            {
                default:
                case 1: this.texture = Texture.smb1_hammerbrosHmr1; frames = 4; break;
            }
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.type = type;
            this.yVel = yVel;
            this.dir = dir; //Startdirection: true = Left ; false = Right
            this.colWithOthers = true;
            this.colWithBlocks = false;
        }

        public override string getName()
        { return name; }

        public override void process()
        {
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

            switch (frame)
            {
                case 0: 
                    colOffsetX = 3;
                    colOffsetY = 1;
                    colRect.w = 10;
                    colRect.h = 8; 
                    break;
                case 1: 
                    colOffsetX = 7;
                    colOffsetY = 3;
                    colRect.w = 8;
                    colRect.h = 10;
                    break;
                case 2:
                    colOffsetX = 3;
                    colOffsetY = 7;
                    colRect.w = 10;
                    colRect.h = 8;
                    break;
                case 3:
                    colOffsetX = 1;
                    colOffsetY = 3;
                    colRect.w = 8;
                    colRect.h = 10;
                    break;
            }

                  
            
            yVel += Map.gravity;
            y = y + yVel;

            if (dir)
                x -= 1.2;  //5.5 Blocks distance 
            else
                x += 1.2;

            refreshColRect();
            getColGrid();
                        
            Image.drawTileFrame(texture, frame, frames, x, y);
        }
        

    }
}




