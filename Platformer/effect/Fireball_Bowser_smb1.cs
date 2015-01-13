using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Fireball_Bowser_smb1 : BaseObj
    {
        public BaseObj owner;
        //jumps 16 
        private short frameCount = 0;
        private short frameDelay = 0;
        public bool harmPlayers = true;

        public Fireball_Bowser_smb1(double x, double y, bool dir = false)
            : base(x, y)
        {
            this.name = "Fireball_Bowser_smb1";
            this.texture = Texture.smb1_bowser_fire;
            this.x = x;
            this.y = y;
            this.w = 24;
            this.h = 16;
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            this.colRect.w = (short)this.w;
            this.colRect.h = (short)this.h;
            
            this.dir = dir;
            this.colWithOthers = true;
            this.colWithBlocks = true;
        }

        public override string getName()
        { return this.name; }

        public override void doSubAI()
        {
            refreshColRect();
            getColGrid();
            if (dir)    //Left
            { xVel = -1; }
            else        //Right
            { xVel = 1; }

            frameCount++;
            if (frameCount == 10)
            {
                frameDelay++;
                frameCount = 0;
                if (frameDelay == 3)
                    frameDelay = 0;
            }

            if (getColXY((int)x - 1, (int)y + (h / 2)) == 1)    //Left Collision
            {
                Map.spriteAdd(new Fireball_hit(x - 8, y));
                x = -100;
                y = -100;
            }
            if (getColXY((int)x + w + 1, (int)y + (h / 2)) == 1)    //Right Collision
            {
                Map.spriteAdd(new Fireball_hit(x + w - 8, y));
                x = -100;
                y = -100;
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
            }
            else
            {
                yVel = 0.7;
            }

            if (getColXY((int)x + (w / 2), (int)y - 1) == 1)
            {
                yVel = 0;
            }

            y = y + yVel;
            x = x + xVel;
        }

        public override void doRender()
        {
            MyImage.drawTileFrame(texture, frameDelay, 3, x, y, dir);
        }

    }
}

