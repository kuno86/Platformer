using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Game
{
    class Keyhole : BaseObj          //Grows up to 3x9 (96x144) (center of circle-part is 6 (96px) blocks) Blocks, stays a second an shrinks to zero    (x*12, Y*11)
    {
        private short frame = 0;
        private short frames;
        private short frameDelay = 0;
        private double size = 0;     //radius for drawing the circle-part of the keyhole
        private bool animating=false;
        private bool done=false;
        private bool wait = false;
        private short waitCounter = 0;
        private double adder = -1.4;

        public Keyhole(double x, double y)
            : base(x, y,16,16)
        {
            this.name = "Keyhole";
            this.texture = Texture.smw_keyhole;
            
            this.x = x;
            this.y = y;
            this.w = 16;
            this.h = 16;
            this.colRect.x = (short)this.x;
            this.colRect.y = (short)this.y;
            this.colRect.w = (short)this.w;
            this.colRect.h = (short)this.h;
            this.colWithOthers = false;
            this.colWithBlocks = false;
        }

        public override string getName()
        { return name; }

        public override void doSubAI()
        {
            refreshColRect();
            frameDelay++;
            if (frameDelay == 3)
            {
                frame++;
                frameDelay = 0;
            }
            if (frame > 4)
            {
                frame = 1;
            }

            for (int i = 0; i != Map.spriteArrMax; i++)
            {
                if (Map.spriteArray[i] != null && Map.spriteArray[i].name == "Key")
                {
                    if (getCol2Obj(colRect, Map.spriteArray[i].colRect) &&
                       (this.id != Map.spriteArray[i].id))
                    {
                        animating = true;
                    }
                }
            }

            if (animating && !done)
            {
                animate();
            }
        }

        public override void doRender()
        {
            MyImage.drawImage(texture, x, y);
        }

        private void animate()
        {
            if (!wait)
                size -= adder;
            if (size >= 144) //reached max size, wait 60 frames
            {
                wait = true;
            }
            if (wait && waitCounter != 60)
                waitCounter++;
            if (size >= 144 && waitCounter == 60)
            {
                adder = adder * -1;
                wait = false;
            }
            if (adder > 0 && size <= 0)
            {
                animating = false;
                //Map.FinishLevel();
                done = true;    //don't activate twice
            }

            GL.Color4(0, 0, 0, 1.0f);
            MyImage.endDraw2D();
            GL.Begin(PrimitiveType.Polygon);
            for (double i = 0; i < 2 * Math.PI; i += Math.PI / 120)
                GL.Vertex3(x + 12 + Math.Cos(i) * (size * 0.33 / 1.05), y + h - (size * 0.66 / 1.05) + Math.Sin(i) * (size * 0.33 / 1.05), adder);
            GL.End();
            GL.Color4(0, 0, 0, 1.0f);
            GL.Begin(PrimitiveType.Triangles);
            GL.Vertex3(x + 12 - (size * 0.33 / 1.08), y + 13, adder);
            GL.Vertex3(x + 12 + (size * 0.33 / 1.08), y + 13, adder);
            GL.Vertex3(x + 12, y + 13 - size / 1.2, adder);
            GL.End();

            MyImage.beginDraw2D();
        }
    }
}



