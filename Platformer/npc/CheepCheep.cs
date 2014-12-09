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
    class CheepCheep : BaseObj
    {
        private double angle, adder, angleDeg;
        public int hbW = 16;
        public int hbH = 16;
        private bool falling;
        private short frame = 0;
        private short frameDelay;
        public bool isDead = false; //does not respawn after it was killed
        private short state;

        private AniFrame[][] stateArr = new AniFrame[2][]{                              //AniFrame info: ( <frame>, <vertical flip ?>, <horizontal flip ?> )
            new AniFrame[]{new AniFrame(00,false,false),new AniFrame(01,false,false)},  //swim
            new AniFrame[]{new AniFrame(02,false,false)},                               //dead
        };

        public CheepCheep(double x, double y, bool dir = false, short type = 0)
            : base(x, y, 16, 16)
        {
            
            switch (type)
            {
                default:
                case 0: this.texture = Texture.smb1_cheepcheepG; this.name = "CheepCheepG"; xVel = 0.3; break;    //Green smb1
                case 1: this.texture = Texture.smb1_cheepcheepR; this.name = "CheepCheepR"; xVel = 0.5; break;    //Red smb1
                
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
            this.dir = dir; //Startdirection: true = Left ; false = Right
            if (dir)
                xVel = xVel * -1;
            onGround = false;
            adder = 0.03;
            falling = true;
            this.colWithOthers = true;
        }

        public override string getName()
        { return name; }

        public override void doSubAI()
        {
            refreshColRect();
            getColGrid();


            if (inWater())
            {
                if (type == 0)
                { yVel = 0; }
                if (type == 1)
                {
                    angle += adder;

                    angleDeg = angle / 0.0174532925; //0.002777778;
                    while (angleDeg > 360)
                        angleDeg -= 360;
                    yVel = Math.Sin(angle) * 0.25;
                }

            }
            else
            {
                if (getColXY((int)x - 1, (int)y + (h / 2)) == 1) //Left wall ?
                {
                    dir = false;
                    falling = true;
                }
                if (getColXY((int)x + w + 1, (int)y + (h / 2)) == 1)  //Right wall ?
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
                    xVel = 0.0;
                    onGround = true;
                    falling = false;
                }
                else
                {
                    yVel += Map.gravity;
                    onGround = false;
                    falling = true;
                }
            }

            y = y + yVel;
            if (dir)
                x -= xVel;
            else
                x += xVel;
        }

        public override void doRender()
        {
            animate();
        }

        private void animate()
        {
            frameDelay++;
            if (frameDelay == 9)
            { frame++; frameDelay = 0; }
            if (frame > stateArr[state].Length - 1)
                frame = 0;
            MyImage.drawTileFrame(texture, (stateArr[state][frame].id), 2, x, y, stateArr[state][frame].flipV ^ dir, stateArr[state][frame].flipH);

        }
    }
}

