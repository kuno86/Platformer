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
    class Goomba :BaseObj
    {
        public int hbW = 16;
        public int hbH = 16;
        private bool onGround;
        private bool falling;
        private short frame=0;
        private short frameDelay;
        public bool isDead = false; //does not respawn after it was killed
        private short state;

        private AniFrame[][] stateArr = new AniFrame[2][]{                              //AniFrame info: ( <frame>, <vertical flip ?>, <horizontal flip ?> )
            new AniFrame[]{new AniFrame(00,false,false),new AniFrame(01,false,false)},  //walk
            new AniFrame[]{new AniFrame(02,false,false)},                               //dead
        };

        public Goomba(double x, double y, bool dir=false, short type=1)
            : base(x, y,16,16)
        {
            this.name = "Goomba";
            switch(type)
            {
                case 1: this.texture = Texture.smb1_goomba_1; break;    //Brown smb1
                case 2: this.texture = Texture.smb1_goomba_2; break;    //Blue smb1
                case 3: this.texture = Texture.smb1_goomba_3; break;    //Grey smb1
                default: this.texture = Texture.smb1_goomba_1; break;   //default Brown smb1
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

            //for (int i = 0; i != Map.spriteArray.Count(); i++)
            //{
            //    if (Map.spriteArray[i] != null)
            //    {
            //        if (Map.spriteArray[i].colWithOthers && this.id != Map.spriteArray[i].id)
            //        {
            //            if (getCol2Obj(colRect, Map.spriteArray[i].colRect))
            //            {
            //                dir = !dir; //Map.spriteArray[i].dir = !Map.spriteArray[i].dir;
            //            }
            //        }
            //    }
            //}

            if (getColXY((int)x -1, (int)y + (h / 2)) == 1) //Left wall ?
            { 
                dir = false;
                falling = true;
            }
            if (getColXY((int)x + w + 1, (int)y + (h/2)) == 1)  //Right wall ?
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

            animate();

        }


        private void animate()
        {
            frameDelay++;
            if (frameDelay == 9)
            { frame++; frameDelay = 0; }
            if (frame > stateArr[state].Length - 1)
                frame = 0;
            Image.drawTileFrame(texture, (stateArr[state][frame].id), 3, x, y, stateArr[state][frame].flipV ^ dir, stateArr[state][frame].flipH);
        }
    }
}
