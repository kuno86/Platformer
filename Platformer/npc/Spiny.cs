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
    class Spiny : BaseObj
    {
        public int hbW = 16;
        public int hbH = 16;
        private bool onGround;
        private bool falling;
        private short frame = 0;
        private short frameDelay;
        public bool isDead = false; //does not respawn after it was killed
        private short state;
        private bool egg;

        private AniFrame[][] stateArr = new AniFrame[3][]{                              //AniFrame info: ( <frame>, <vertical flip ?>, <horizontal flip ?> )
            new AniFrame[]{new AniFrame(00,false,false),new AniFrame(01,false,false)},  //Egg
            new AniFrame[]{new AniFrame(02,false,false),new AniFrame(03,false,false)},  //walk
            new AniFrame[]{new AniFrame(03,false,true)},                                //dead
        };

        public Spiny(double x, double y, bool dir = false, short type = 1, bool egg=false)
            : base(x, y, 16, 16)
        {
            if (egg)
            {
                this.name = "Spiny-Egg";
                this.colWithOthers = false;     //it only Hatches on hitting Blocks
            }
            else
            {
                this.name = "Spiny";
                this.colWithOthers = true;
            }
            switch (type)
            {
                case 1: this.texture = Texture.smb1_spiny; break;
                default: this.texture = Texture.smb1_spiny; break;
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
            this.egg = egg;
            if (egg)
                state = 0;
            else
                state = 1;
            this.dir = dir; //Startdirection: true = Left ; false = Right
            onGround = false;
            falling = true;
            this.colWithBlocks = true;
        }

        public override string getName()
        { return name; }

        public override void process()
        {
            refreshColRect();
            getColGrid();
            
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
                if (egg)
                {
                    egg = false;
                    state = 1;
                    this.name = "Spiny";
                    this.colWithOthers = true;
                }
                yVel = 0;
                xVel = 0.3;
                onGround = true;
                falling = false;
            }
            else
            {
                yVel+=Map.gravity;
                onGround = false;
                falling = true;
            }

            y = y + yVel;
            if (dir)
                x -= xVel;
            else
                x += xVel;

            animate();

        }


        private void animate()
        {
            frameDelay++;
            if (frameDelay == 9)
            { frame++; frameDelay = 0; }
            if (frame > stateArr[state].Length - 1)
                frame = 0;
            Image.drawTileFrame(texture, (stateArr[state][frame].id), 4, x, y, stateArr[state][frame].flipV ^ dir, stateArr[state][frame].flipH);
        }
    }
}

