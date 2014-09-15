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
            if(egg)
                this.name = "Spiny-Egg";
            else
                this.name = "Spiny";
            switch (type)
            {
                case 1: this.texture = Texture.smb1_spiny; break;
                default: this.texture = Texture.smb1_spiny; break;
            }
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.type = type;
            this.egg = egg;
            if (egg)
                state = 0;
            else
                state = 1;
            this.dir = dir; //Startdirection: true = Left ; false = Right
            onGround = false;
            falling = true;
            this.colWithOthers = true;
        }

        public override string getName()
        { return name; }

        public override void process()
        {
            refreshColRect();
            getColGrid();
            for (int i = 0; i != Map.spriteArrMax; i++)
            {
                if (Map.spriteArray[i] != null)
                {
                    if (Map.spriteArray[i].colWithOthers && this.id != Map.spriteArray[i].id)
                    {
                        if (getCol2Obj(colRect, Map.spriteArray[i].colRect))
                        {
                            dir = !dir; //Map.spriteArray[i].dir = !Map.spriteArray[i].dir;
                        }
                    }
                }
            }
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
                if (colBottom == 1)
                    y--;
                if (egg)
                {
                    egg = false;
                    state = 1;
                    this.name = "Spiny";
                }
                onGround = true;
                falling = false;
            }
            else
            {
                y++;
                onGround = false;
                falling = true;
            }

            if (dir)
                x -= 0.3;
            else
                x += 0.3;

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

