﻿using System;
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
    class Mushroom : BaseObj
    {
        public int hbW = 16;
        public int hbH = 16;
        private bool onGround;
        private bool falling;
        public bool isDead = false; //does not respawn after it was killed
        private short state;


        public Mushroom(double x, double y, bool dir = false, short type=0)
            : base(x, y, 16, 16)
        {
            this.name = "Mushroom";
            switch (type)
            {
                case 0: this.texture = Texture.smb1_mushroom; break;
                case 1: this.texture = Texture.smb2_mushroom; break;
                default: this.texture = Texture.smb1_mushroom; break;
            }
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            this.colRect.w = (short)this.w;
            this.colRect.h = (short)this.h;
            this.dir = dir; //Startdirection: true = Left ; false = Right
            onGround = false;
            falling = true;
            this.colWithBlocks = true;
            this.colWithOthers = true;
        }

        public override string getName()
        { return name; }

        public override void doSubAI()
        {
            refreshColRect();
            getColGrid();

            if (getColXY((int)x - 1, (int)y + (h / 2)) == 1)
            {
                dir = false;
                falling = true;
            }
            if (getColXY((int)x + w + 1, (int)y + (h / 2)) == 1)
            {
                dir = true;
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

            y = y + yVel;

            if (dir)
                x -= 0.3;
            else
                x += 0.3;
        }

        public override void doRender()
        {
            MyImage.drawImage(texture, x, y);
        }

    }
}


