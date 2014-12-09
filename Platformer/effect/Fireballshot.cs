using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Fireballshot : Player
    {
        public BaseObj owner;
                                //jumps 16 
        private short frameCount = 0;
        private short frameDelay = 0;
        public bool harmPlayers = true;

        public Fireballshot(double x, double y, BaseObj owner, bool dir = false)
            : base(x, y)
        {
            this.name = "Fireballshot";
            this.texture = Texture.fireballshot;
            this.x = x;
            this.y = y;
            this.w = 8;
            this.h = 8;
            this.colOffsetX = (short)this.x;
            this.colOffsetY = (short)this.y;
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            this.colRect.w = (short)this.w;
            this.colRect.h = (short)this.h;
            this.owner = owner;
            if (owner.name == "Player")   //Check who spawned the fireball and set its harming accordingly 
                harmPlayers=false;
            this.dir = dir;
            this.colWithOthers = true;
            this.colWithBlocks = true;
        }

        public override string getName()
        { return (name.ToString()) + "(" + (harmPlayers.ToString()) + ")"; }
        
        public override void doSubAI()
        {
            refreshColRect();
            getColGrid();
            if (dir)    //Left
            { x -= 2.0; }
            else        //Right
            { x += 2.0; }

            frameCount++;
            if (frameCount == 10)
            {
                frameDelay++;
                frameCount = 0;
                if (frameDelay == 3)
                    frameDelay = 0;
            }

            if (getColXY((int)x - 1, (int)y + (h / 2)) == 1)
            {
                Map.spriteAdd(new Fireball_hit(x - w, y - h));
                x = -100;
                y = -100;
            }
            if (getColXY((int)x + w + 1, (int)y + (h / 2)) == 1)
            {
                Map.spriteAdd(new Fireball_hit(x - w, y - h));
                x = -100;
                y = -100;
            }

            if (getColXY((int)x + (w / 2), (int)y + h + 1) == 1)    //floorCol
            {
                if (colBottom == 1)
                    yVel = -1.328;
            }

            if (getColXY((int)x + (w / 2), (int)y - 1) == 1)
            {
                yVel = 0;
            }

            yVel += Map.gravity;
            y = y + yVel;
        }

        public override void doRender()
        {
            MyImage.drawTileFrame(texture, frameDelay, 4, x, y, dir);
        }

    }
}
