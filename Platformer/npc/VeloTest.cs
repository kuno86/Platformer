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
    class VeloTest : BaseObj
    {
        
        public VeloTest(double x, double y)
            : base(x, y, 16, 16)
        {
            this.name = "VeloTest";
            this.texture = Texture.player;
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.colRect.x = x;
            this.colRect.y = y;
            this.colRect.w = w;
            this.colRect.h = h;
        }

        public override string getName()
        { return name; }

        public override void doSubAI()
        {
            var keyboard = Keyboard.GetState();

            if (keyboard[Key.Keypad4])  //Left
            {
                if (colLeft != 1)
                    xVel -= 0.1;
            }

            if (keyboard[Key.Keypad6])  //Right
            {
                if (colRight != 1)
                    xVel += 0.1;
            }

            if (keyboard[Key.Keypad8])  //Up
            {
                if (colTop != 1)
                    yVel -= 0.1;
            }

            if (keyboard[Key.Keypad5])  //Down
            {
                if (colBottom != 1)
                    yVel += 0.1;
            }


            refreshColRect();
            getColGrid();

            x += xVel;
            y += yVel;

            
            //else
            //{
            //    x += xVel;
            //    y += yVel;
            //}












            for (int yy = (int)colRect.y; yy <= (int)(colRect.y + colRect.h); yy += Map.tileSize)
            {
                for (int xx = (int)colRect.x; xx <= (int)(colRect.x + colRect.w); xx += Map.tileSize)
                {
                    refreshColRect();
                    if (getColXY(xx, yy) == 1)
                    {
                        Vector2 centerA = new Vector2((float)(colRect.x + colRect.w / 2), (float)(colRect.y + colRect.h / 2));
                        Vector2 centerB = new Vector2((float)(xx + Map.tileSize / 2), (float)(yy + Map.tileSize / 2));

                        float distanceX = centerA.X - centerB.X;
                        float distanceY = centerA.Y - centerB.Y;
                        float minDistanceX = colRect.w / 2 + Map.tileSize / 2;
                        float minDistanceY = colRect.h / 2 + Map.tileSize / 2;

                        Vector2 intersectionDepth = new Vector2();

                        if (Math.Abs(centerA.X - centerB.X) >= ((colRect.w / 2) + (Map.tileSize / 2)) ||
                           Math.Abs(centerA.Y - centerB.Y) >= ((colRect.h / 2) + (Map.tileSize / 2)))
                        { intersectionDepth.X = 0; intersectionDepth.Y = 0; }
                        float depthX, depthY;
                        if (centerA.X - centerB.X > 0)
                            depthX = minDistanceX - distanceX;
                        else
                            depthX = -minDistanceX - distanceX;
                        if (centerA.Y - centerB.Y > 0)
                            depthY = minDistanceY - distanceY;
                        else
                            depthY = -minDistanceY - distanceY;
                        intersectionDepth.X = depthX;
                        intersectionDepth.Y = depthY;
                        if (Math.Abs(intersectionDepth.Y) < Math.Abs(intersectionDepth.X))
                        {
                            if(depthY>0)
                                y++;
                            if (depthY < 0)
                                y--;
                            yVel = 0;
                        }
                        else
                        {
                            if (depthX > 0)
                                x++;
                            if (depthX < 0)
                                x--;
                            xVel = 0;
                        }
                    }
                }
            }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            //double xOffset = Math.Abs(x - Math.Round(x / 16) * 16)+1;
            //double yOffset = Math.Abs(y - Math.Round(y / 16) * 16)+1;

            //refreshColRect();
            //getColGrid();

            //if (yVel!=0 && (colBottom==1 || colTop==1))
            //{
            //    if (yVel > 0)
            //        yOffset *= -1;

            //    y += yOffset;
            //    yVel = 0;
            //}

            //if (xVel!=0 && (colLeft == 1 || colRight == 1))
            //{
            //    if (xVel > 0)
            //        xOffset *= -1;

            //    x += xOffset;
            //    xVel = 0;
            //}
        }

        public override void doRender()
        {
            //MyImage.drawText("X: " + Math.Abs(x - Math.Round(x / 16)*16) + " Y: " + Math.Abs(y - Math.Round(y / 16)*16), (int)x, (int)y - 16, Color.Aqua, Texture.ASCII);
            MyImage.drawImage(texture, x, y);
        }

    }
}
