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
        }

        public override string getName()
        { return name; }

        public override void process()
        {
            var keyboard = Keyboard.GetState();

            if (keyboard[Key.Keypad4])  //Left
            { 
                xVel-=0.1; 
            }

            if (keyboard[Key.Keypad6])  //Right
            {
                xVel+=0.1;
            }

            if (keyboard[Key.Keypad4])  //Up
            {
                yVel -= 0.1;
            }

            if (keyboard[Key.Keypad6])  //Down
            {
                yVel += 0.1;
            }

            if (Map.map[(int)(x+xVel) / 16, (int)(y+yVel) / 16, 0] == 1)
            {
                y = (y + yVel);
            }



        }
    }
}
