using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Generator : BaseObj
    {
        private int spriteID = 103;
        private short tickDelay, tickCount;
        private bool warp;
        private short direction;
        private short limit;
        private List<int> creations = new List<int>();
        private bool canSpawn = true;
        private bool isWarping=false;
        private bool colWithBlocksTemp;
        private bool colWithOthersTemp;
        ColRect spawnSpace;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="spriteID"></param>
        /// <param name="tickDelay">Delay between spawns (60 ticks =1s)</param>
        /// <param name="warp">True=Warp out; False=shoot out</param>
        /// <param name="direction">0=Moving Right, 1=Moving Down, 2=Moving Left, 3=Moving Up</param>
        /// <param name="limit">how many sprites this generator will produce before stop</param>
        public Generator(double x, double y, int spriteID, short tickDelay=60, bool warp = false, short direction = 3, short limit=100, double velocity=4)
            : base(x, y)
        {
            this.name = "Generator";
            this.texture = Texture.generator;
            this.spriteID = spriteID;
            this.tickDelay = tickDelay;
            this.tickCount = 0;
            this.warp = warp;
            this.direction = direction;
            this.limit = limit;
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            spawnSpace.x = x;
            spawnSpace.y = y;
            spawnSpace.w = w;
            spawnSpace.h = h;
            switch (direction)
            {
                case 0:
                default:
                    spawnSpace.x += w + 1;
                    this.xVel = velocity;
                    this.yVel = 0;
                    break;
                case 1:
                    spawnSpace.y += h + 1;
                    this.xVel = 0;
                    this.yVel = velocity;
                    break;
                case 2:
                    spawnSpace.x -= w - 1;
                    this.xVel = velocity * -1;
                    this.yVel = 0;
                    break;
                case 3:
                    spawnSpace.y -= h - 1;
                    this.xVel = 0;
                    this.yVel = velocity * -1;
                    break;
            }
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            this.colRect.w = (short)this.w;
            this.colRect.h = (short)this.h;
            this.colWithOthers = true;
            this.colWithBlocks = false;
        }

        public override string getName()
        { return name; }
        
        public override void doSubAI()
        {
            if (tickCount < tickDelay)
                tickCount++;

            refreshColRect();

            if (warp && isWarping)
            {
                switch (direction)
                {
                    case 0: //warp Right
                        if (Map.spriteArray[creations.Last()].colRect.x <= colRect.x)
                        {
                            Map.spriteArray[creations.Last()].y = y;
                            Map.spriteArray[creations.Last()].x += 0.5;
                        }
                        else
                        {
                            canSpawn = true;
                            Map.spriteArray[creations.Last()].colWithBlocks = colWithBlocksTemp;
                            Map.spriteArray[creations.Last()].colWithOthers = colWithOthersTemp;
                            Map.spriteArray[creations.Last()].x = spawnSpace.x;
                            Map.spriteArray[creations.Last()].y = spawnSpace.y;
                        }
                        break;
                    case 1: //warp Down
                        if (Map.spriteArray[creations.Last()].colRect.y <= colRect.y)
                        {
                            Map.spriteArray[creations.Last()].x = x;
                            Map.spriteArray[creations.Last()].y += 0.5;
                        }
                        else
                        {
                            canSpawn = true;
                            Map.spriteArray[creations.Last()].colWithBlocks = colWithBlocksTemp;
                            Map.spriteArray[creations.Last()].colWithOthers = colWithOthersTemp;
                            Map.spriteArray[creations.Last()].x = spawnSpace.x;
                            Map.spriteArray[creations.Last()].y = spawnSpace.y;
                        }
                        break;
                    case 2: //warp Left
                        if (Map.spriteArray[creations.Last()].colRect.x + Map.spriteArray[creations.Last()].colRect.w >= colRect.x)
                        {
                            Map.spriteArray[creations.Last()].y = y;
                            Map.spriteArray[creations.Last()].x -= 0.5;
                        }
                        else
                        {
                            canSpawn = true;
                            Map.spriteArray[creations.Last()].colWithBlocks = colWithBlocksTemp;
                            Map.spriteArray[creations.Last()].colWithOthers = colWithOthersTemp;
                            Map.spriteArray[creations.Last()].x = spawnSpace.x;
                            Map.spriteArray[creations.Last()].y = spawnSpace.y;
                        }
                        break;
                    case 3: //warp Up
                        if (Map.spriteArray[creations.Last()].colRect.y + Map.spriteArray[creations.Last()].colRect.h >= colRect.y)
                        {
                            Map.spriteArray[creations.Last()].x = x;
                            Map.spriteArray[creations.Last()].y -= 0.5;
                        }
                        else
                        {
                            canSpawn = true;
                            Map.spriteArray[creations.Last()].colWithBlocks = colWithBlocksTemp;
                            Map.spriteArray[creations.Last()].colWithOthers = colWithOthersTemp;
                            Map.spriteArray[creations.Last()].x = spawnSpace.x;
                            Map.spriteArray[creations.Last()].y = spawnSpace.y;
                        }
                        break;
                }
            }

            for (int i = 0; i != Map.spriteArrMax; i++)     //if there is anything blocking the spawnspace, reset the counter for this tick
            {
                if (Map.spriteArray[i] != null && Map.spriteArray[i].colWithBlocks)
                {
                    if (getCol2Obj(Map.spriteArray[i].colRect, this.spawnSpace))
                    {
                        tickCount--;
                        break;
                    }

                }
            }


            if (tickCount >= tickDelay)     //is it time to spawn ?
            {
                if (canSpawn && creations.Count < limit)    //has this generator reached max Objects count limit ?
                {
                    if (warp)
                    {
                        creations.Add(Map.spriteAdd(DeepCopySprite(this.spriteID)));
                        Map.spriteArray[creations.Last()].setXY(x, y);
                        colWithBlocksTemp = Map.spriteArray[creations.Last()].colWithBlocks;
                        Map.spriteArray[creations.Last()].colWithBlocks = false;
                        colWithOthersTemp = Map.spriteArray[creations.Last()].colWithOthers;
                        Map.spriteArray[creations.Last()].colWithOthers = false;
                        isWarping = true;
                    }
                    else
                    {
                        creations.Add(Map.spriteAdd(DeepCopySprite(this.spriteID)));
                        Map.spriteArray[creations.Last()].setXY(spawnSpace.x, spawnSpace.y);
                        Map.spriteArray[creations.Last()].setXYVel(this.xVel, this.yVel);
                        Map.spriteAdd(new Smoke(spawnSpace.x, spawnSpace.y, type));
                    }
                }
                tickCount = 0;
            }

            if (creations.Count > 0)
            {
                for (int i = 0; i <= creations.Count - 1; i++)
                {
                    if (Map.spriteArray[creations[i]] == null /*|| Map.spriteArray[creations[i]].metaData[0] != "fromGenerator" + this.id*/)
                    {
                        creations.RemoveAt(i); i++;
                    }
                }
            }
        }

        public override void doRender()
        {
            MyImage.drawTileFrame(texture, direction, 4, x, y);
        }

    }
}



