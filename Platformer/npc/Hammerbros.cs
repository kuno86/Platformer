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
    class Hammerbros : BaseObj
    {
        public int hbW = 16;
        public int hbH = 16;
        private bool onGround;
        double yOld;
        private bool falling;
        private int frames;
        private short frame = 0;
        private short frameDelay;
        public bool isDead = false; //does not respawn after it was killed
        private short state;
        private bool jumping, jumpdown;
        private int jumpDelay;
        private int throwDelay;

        private AniFrame[][] stateArr = new AniFrame[6][]{                                                          //AniFrame info: ( <frame>, <vertical flip ?>, <horizontal flip ?> )
            new AniFrame[]{new AniFrame(00,false,false),new AniFrame(01,false,false)},                              //0 walk
            new AniFrame[]{new AniFrame(01,false,false)},                                                           //1 jump
            new AniFrame[]{new AniFrame(02,false,false),new AniFrame(03,false,false)},                              //2 walk with hammer out
            new AniFrame[]{new AniFrame(01,false,false)},                                                           //3 jump with hammer out
            new AniFrame[]{new AniFrame(02,false,false),new AniFrame(03,false,false),new AniFrame(01,false,false)}, //4 throw hammer
            new AniFrame[]{new AniFrame(02,false,true)},                                                            //5dead
        };

        public Hammerbros(double x, double y, bool dir = false, short type = 1)
            : base(x, y, 16, 16)
        {
            this.name = "Hammerbros";
            switch(type)
            {
                default:
                case 1: this.texture = Texture.smb1_hammerbros; frames = 4; break; 
        }
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 24;
            colOffsetX = 2;
            colOffsetY = 2;
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            colRect.w = 12;
            colRect.h = 14;

            this.dir = dir; //Startdirection: true = Left ; false = Right
            onGround = false;
            falling = true;
            jumping = false;
            jumpDelay=Map.rnd.Next(150, 300);
            throwDelay = Map.rnd.Next(30, 90);
        }

        public override string getName()
        { return name; }

        public override void doSubAI()
        {
            refreshColRect();
            getColGrid();

            if (throwDelay > 0)
                throwDelay--;
            if (throwDelay == 3)
            {
                state = 4;
                if (dir)     //Left
                    Map.spriteAdd(new Hammer_thrown(x + 1, y - 10, dir, -3 - (Map.rnd.NextDouble()), type));
                else
                    Map.spriteAdd(new Hammer_thrown(x - 1, y - 10, dir, -3 - (Map.rnd.NextDouble()), type));
            }
            if (throwDelay == 0)
            {
                state = 0;
                throwDelay = Map.rnd.Next(30, 90);
            }

            if (jumpDelay > 0 && !jumping && !falling)
            {
                jumpDelay--;
                state = 0;
            }
            if (jumpDelay == 0)
            {
                if (getColXY((int)x, (int)y - 16) == 1 && getColXY((int)x, (int)y - 32) != 1 && getColXY((int)x, (int)y - 48) != 1)
                {
                    jumping = true;
                    falling = false;
                    jumpdown = false;
                    state = 1;
                    yVel = 3.75;    // ~5,5 Blocks
                }
                else if (getColXY((int)x, (int)y - 16) == 1 && getColXY((int)x, (int)y - 32) != 1 && getColXY((int)x, (int)y - 16) != 1)
                {
                    yOld = y;
                    jumping = false;
                    falling = false;
                    jumpdown = true;
                    state = 1;
                    yVel = 1.2;
                }
            }

            if (jumping && !falling && !jumpdown)    //actually jumping
            {
                if (yVel >= 0)
                {
                    falling = true;
                    jumping = false;
                    jumpdown = false;
                }
            }
            else if (!jumping && !falling && jumpdown)    //jumping down
            {
                if (y >= yOld + colRect.h)      // has passed through the solid floor, can stand on floor again
                {
                    jumpdown = false;
                    jumping = false;
                    falling = true;
                }
            }


            if (getColXY((int)x - 1, (int)y + (h / 2)) == 1 && !jumping) //Left wall ?
            {
                dir = false;
                falling = true;
            }
            if (getColXY((int)x + w + 1, (int)y + (h / 2)) == 1 && !jumping)  //Right wall ?
            {
                dir = true;
                falling = true;
            }


            if (getColXY((int)x + (w / 2), (int)y + h + 1) == 1 && !jumping)    //floorCol
            {
                while (colBottom == 1)
                {
                    y--;
                    refreshColRect();
                    getColGrid();
                }
                yVel = 0;
                xVel = 0.3;
                onGround = true;
                falling = false;
                jumpDelay = Map.rnd.Next(150, 300);
            }
            else
            {
                yVel += Map.gravity;
                onGround = false;
                falling = true;
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
            MyImage.drawTileFrame(texture, (stateArr[state][frame].id), frames, x, y, stateArr[state][frame].flipV ^ dir, stateArr[state][frame].flipH);
        }
    }
}


