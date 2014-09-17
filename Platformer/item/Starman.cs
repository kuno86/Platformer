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
    class Starman : BaseObj
    {
        private short frame = 0;
        private short frames;
        private short frameDelay = 0;
        public int hbW = 16;
        public int hbH = 16;
                                //jumps 40 Pixel high
        public bool isDead = false; //does not respawn after it was killed
        private short state;


        public Starman(double x, double y, bool dir = false, short type = 1)
            : base(x, y, 16, 16)
        {
            this.name = "Starman";
            switch (type)
            {
                case 1: this.texture = Texture.smb1_starman; frames = 4; break;
                case 2: this.texture = Texture.smb2_starman; frames = 4; break;
                case 3: this.texture = Texture.smb3_starman; frames = 2; break;
                case 4: this.texture = Texture.smw_starman; frames = 2; break;
                default: this.texture = Texture.smb1_starman; frames = 4; break;
            }
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.type = type;
            this.dir = dir; //Startdirection: true = Left ; false = Right
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
                frame = 1;
            }

            if (getColXY((int)x - 1, (int)y + (h / 2)) == 1)
            {
                dir = false;
            }
            if (getColXY((int)x + w + 1, (int)y + (h / 2)) == 1)    //RWallCol
            {
                dir = true;
            }


            if (getColXY((int)x + (w / 2), (int)y + h + 1) == 1)    //floorCol
            {
                if (colBottom == 1)
                    yVel= -3.32;
            }

            if (getColXY((int)x + (w / 2), (int)y - 1) == 1)
            {
                yVel = 0;
            }
            yVel += Map.gravity;
            y=y+yVel;

            if (dir)
                x -= 0.6;
            else
                x += 0.6;

            Image.drawTileFrame(texture, frame, frames, x, y);
        }


    }
}



