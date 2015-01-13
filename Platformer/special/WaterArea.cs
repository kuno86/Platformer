using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Game
{
    class WaterArea : BaseObj
    {
        private short frame = 0;
        private short frames;
        private short frameDelay = 0;

        public WaterArea(double x, double y, short w=16, short h=16)
            : base(x, y, w, h)
        {
            this.name = "WaterArea";
            this.type = type;
            this.texture = Texture.watertint;
            this.renderOrder = 1;
            this.despawnOffScreen = false;

            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
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
            refreshColRect();
            x += xVel;  //
            y += yVel;  // In case of Layers etc...    
        }

        public override void doRender()
        {
            for (int yy = (int)colRect.y; yy < (int)(colRect.y + colRect.h); yy += Map.tileSize)
            {
                for (int xx = (int)colRect.x; xx < (int)(colRect.x + colRect.w); xx += Map.tileSize)
                {
                    MyImage.drawImage(texture, xx, yy);
                }
            }
            //MyImage.endDraw2D();
            //GL.Begin(PrimitiveType.LineLoop);
            //GL.Color3(System.Drawing.Color.LightBlue);
            //GL.Vertex2(colRect.x, colRect.y);
            //GL.Vertex2(colRect.x + colRect.w, colRect.y);
            //GL.Vertex2(colRect.x + colRect.w, colRect.y + colRect.h);
            //GL.Vertex2(colRect.x, colRect.y + colRect.h);
            //GL.End();
            //MyImage.beginDraw2D();
        }


    }
}
