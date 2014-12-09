using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Game
{
    class RollingRock : BaseObj          //Grows up to 3x9 (96x144) (center of circle-part is 6 (96px) blocks) Blocks, stays a second an shrinks to zero    (x*12, Y*11)
    {
        private double angle, r;
        private Point center = new Point();

        public RollingRock(double x, double y)
            : base(x, y,32,32)
        {
            this.name = "RollingRock";
            this.texture = Texture.smw2_rollingRock;
            this.colType = 1;
            this.x = x;
            this.y = y;
            this.w = 32;
            this.h = 32;
            this.r = w / 2;
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            this.colRect.w = (short)this.w;
            this.colRect.h = (short)this.h;
            this.colWithOthers = true;
            this.colWithBlocks = true;
        }

        public override string getName()
        { return name; }

        public override void doSubAI()
        {
            refreshColRect();
            getColGrid();

            Point colVectors = new Point();
            short colVectorsCount = 0;
            
            center.x = x + (w / 2);
            center.y = y + (h / 2);

            colVectors.x = center.x;
            colVectors.y = center.y;

            int xBegin, xEnd, yBegin, yEnd;

            if (x - 16 >= 0)
                xBegin = (int)x - 16;
            else
                xBegin = 0;
            if (x + w + 16 <= RootThingy.sceneX-1)
                xEnd = (int)x + w + 16;
            else
                xEnd = (int)RootThingy.sceneX-1;
            
            if (y - 16 >= 0)
                yBegin = (int)y - 16;
            else
                yBegin = 0;
            if (y + h + 16 <= RootThingy.sceneY-1)
                yEnd = (int)y + h + 16;
            else
                yEnd = (int)RootThingy.sceneY-1;


            for (int yy = yBegin; yy < yEnd; yy += Map.tileSize)
            {
                for (int xx = xBegin; xx < xEnd; xx += Map.tileSize)
                {
                    Point centerTile = new Point();
                    centerTile.x = (((int)(xx / 16)) * 16) + (Map.tileSize / 2);
                    centerTile.y = (((int)(yy / 16)) * 16) + (Map.tileSize / 2);

                    ColRect crect = new ColRect();
                    crect.x=(int)xx;
                    crect.y=(int)yy;
                    crect.w=16;
                    crect.h=16;

                    MyImage.endDraw2D();
                    GL.Begin(PrimitiveType.LineLoop);
                    GL.Color3(System.Drawing.Color.LimeGreen);
                    GL.Vertex2(crect.x, crect.y);
                    GL.Vertex2(crect.x + crect.w, crect.y);
                    GL.Vertex2(crect.x + crect.w, crect.y + crect.h);
                    GL.Vertex2(crect.x, crect.y + crect.h);
                    GL.End();
                    MyImage.beginDraw2D();
                    
                    
                    //if (Map.map[(int)((yy) / 16), (int)((xx) / 16), 0] == 1 && Collision.col_circleRect(x + (w / 2), y + (w / 2), w / 2, crect))
                    //    {
                    //        double xOff = (crect.x) - (center.x); //
                    //        double yOff = (crect.y) - (center.y); //Distance between Center of circle and center of colliding tile
                    //        colVectors.x += xOff - ((Map.tileSize / 2) + (w / 2));    //
                    //        colVectors.y += yOff - ((Map.tileSize / 2) + (w / 2));    //The actual Distance minus the minimum "not-colliding" distance
                    //        colVectorsCount++;
                    //        MyImage.endDraw2D();
                    //        GL.Begin(PrimitiveType.Lines);
                    //        GL.Color3(System.Drawing.Color.Magenta);
                    //        GL.Vertex2(center.x + xOff, center.y + yOff);
                    //        GL.Vertex2(center.x, center.y); //Kreismitte
                    //        GL.End();
                    //        MyImage.beginDraw2D();
                    //    }


                    if (Map.map[(int)((yy) / 16), (int)((xx) / 16), 0] == 1 && 
                        (
                            (Collision.col_circlePoint(center.x,center.y,(w/2),crect.x,crect.y)) || 
                            (Collision.col_circlePoint(center.x,center.y,(w/2),crect.x + crect.w,crect.y)) || 
                            (Collision.col_circlePoint(center.x,center.y,(w/2),crect.x + crect.w,crect.y + crect.h)) || 
                            (Collision.col_circlePoint(center.x,center.y,(w/2),crect.x,crect.y + crect.h))
                        ))
                    {
                        MyImage.endDraw2D();
                        GL.Begin(PrimitiveType.Lines);
                        GL.Color3(System.Drawing.Color.Magenta);
                        GL.Vertex2(centerTile.x + 8, centerTile.y);
                        GL.Vertex2(centerTile.x, centerTile.y); //Kreismitte
                        GL.End();
                        MyImage.beginDraw2D();
                    }


                }
            }

            colVectors.x = colVectors.x / colVectorsCount*-1;  //Average all collisions
            colVectors.y = colVectors.y / colVectorsCount*-1;  //

            MyImage.endDraw2D();
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(System.Drawing.Color.Aqua);
            GL.Vertex2(colVectors.x + center.x, colVectors.y + center.y);
            GL.Vertex2(center.x, center.y);
            GL.End();
            MyImage.beginDraw2D();

 
            //if (getColXY((int)x - 1, (int)y + (h / 2)) == 1) //Left wall ?
            //{
            //    dir = false;
            //    xVel = 0;
            //}
            //if (getColXY((int)x + w + 1, (int)y + (h / 2)) == 1)  //Right wall ?
            //{
            //    dir = true;
            //    xVel = 0;
            //}

            //if (getColXY((int)x + (w / 2), (int)y + h + 1) == 1)    //floorCol
            //{
            //    while (colBottom == 1)
            //    {
            //        y--;
            //        refreshColRect();
            //        getColGrid();
            //    }
            //    yVel = 0;
            //    onGround = true;
            //}
            //else
            //{
            //    yVel += Map.gravity;
            //    onGround = false;
            //}

            var keyboard = Keyboard.GetState();
            if (keyboard[Key.J])
            {
                if(xVel>-1.0)
                xVel -= 0.01;
            }
            if (keyboard[Key.L])
            {
                if(xVel<1.0)
                xVel += 0.01;
            }
            if (keyboard[Key.I])
            {
                if (yVel > -1.0)
                    yVel -= 0.01;
            }
            if (keyboard[Key.K])
            {
                if (yVel < 1.0)
                    yVel += 0.01;
            }

            if(!keyboard[Key.J] && !keyboard[Key.L])
            {
                if (xVel < 0)
                { xVel += 0.004; }
                if (xVel > 0)
                { xVel -= 0.004; }
                if (xVel > -0.004 && xVel < 0.004)
                    xVel = 0;
            }

            if (!keyboard[Key.I] && !keyboard[Key.K])
            {
                if (yVel < 0)
                { yVel += 0.004; }
                if (yVel > 0)
                { yVel -= 0.004; }
                if (yVel > -0.004 && yVel < 0.004)
                    yVel = 0;
            }

            angle += (xVel * 180 / ((w / 2) * Math.PI)) *-1;
            while (angle > 360)
                angle -= 360;
            while (angle < 0)
                angle += 360;

            x += xVel;
            y += yVel;
            //if (colVectorsCount > 0)
            //{
            //    x += -colVectors.x;
            //    y += -colVectors.y;
            //}

            if (RootThingy.debugInfo)
            {
                this.texture = Texture.circle32_debug;
                //MyImage.drawText("colVectors.x: " + colVectors.x, (int)x + w + 2, (int)y, System.Drawing.Color.Wheat, Texture.ASCII);
                //MyImage.drawText("colVectors.y: " + colVectors.y, (int)x + w + 2, (int)y + 12, System.Drawing.Color.Wheat, Texture.ASCII);
                MyImage.drawText("colVectorsCount: " + colVectorsCount, (int)x + w + 2, (int)y + 24, System.Drawing.Color.Wheat, Texture.ASCII);
            }
            else
                this.texture = Texture.smw2_rollingRock;
        }

        public override void doRender()
        {
            if(RootThingy.debugInfo)
            MyImage.drawImageRot(texture, x + (w / 2), y + (h / 2), angle);
            
        }

    }
}




