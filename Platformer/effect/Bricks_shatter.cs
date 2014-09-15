using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Bricks_shatter : BaseObj
    {
        private short frames;

        public Bricks_shatter(double x, double y, short type=1)
            : base(x, y)
        {
            this.name = "Bricks";
            this.type = type;
            switch (type)
            {
                case 1: this.texture = Texture.smb1_brickSh1; this.frames = 4; break;
                case 2: this.texture = Texture.smb1_brickSh2; this.frames = 4; break;
                case 3: this.texture = Texture.smb1_brickSh3; this.frames = 4; break;
                case 4: this.texture = Texture.smb3_brickSh1; this.frames = 4; break;
                case 5: this.texture = Texture.smw_brickSh1; this.frames = 2; break;
                default: this.texture = Texture.smb1_brickSh1; this.frames = 4; break;
            }
            this.x = x;
            this.y = y;
            this.w = 8;
            this.h = 8;
        }

        public override string getName()
        { return name; }

        public override void process()
        {
            Map.spriteAdd(new Brickshatter(texture, frames, x+8, y, 2, -4));       //Higher part that goes right
            Map.spriteAdd(new Brickshatter(texture, frames, x, y, -2, -4));        //Higher part that goes Left
            Map.spriteAdd(new Brickshatter(texture, frames, x+8, y+8, 2, -1.33));  //Lower part that goes right
            Map.spriteAdd(new Brickshatter(texture, frames, x, y+8, -2, -1.33));   //Lower part that goes Left
            x = -100;
            y = -100;
        }


    }
}
