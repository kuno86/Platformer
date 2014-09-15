using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Game  //Only purpose is to hold all the textures
{
    static class Texture
    {
        public static int[,] bgTilesArray = new int[Directory.GetFiles((Environment.CurrentDirectory + @"\gfx\backgrounds\tiles"), "*.bmp").GetLength(0), 2];  //create an array withe the length of found BMPs. 2nd field = animation frameCount

        //Other
        public static int tileSet =             Image.LoadTexture(RootThingy.exePath + @"\gfx\tilesets\smb1nes.bmp");//
        public static int backGround =          Image.LoadTexture(RootThingy.exePath + @"\gfx\backgrounds\smb3castle.bmp");//
        public static int ASCII =               Image.LoadTexture(RootThingy.exePath + @"\gfx\ASCII-Characters_8x12.bmp");//
        
        //Player
        public static int player =              Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\player\player_debug.bmp");//
        public static int mario_small =         Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\player\mario_small.bmp");//
        public static int mario_big =           Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\player\mario_big.bmp");//
        public static int mario_fire =          Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\player\mario_fire.bmp");//

        //NPCs
        public static int smw_boo =             Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smw_boo.bmp");//
        public static int smb1_goomba_1 =       Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb1_goomba_brown.bmp");//
        public static int smb1_goomba_2 =       Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb1_goomba_blue.bmp");//
        public static int smb1_goomba_3 =       Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb1_goomba_grey.bmp");//

        public static int smb1_koopa_green =    Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb1_koopa_green.bmp");//
        public static int smb1_koopa_red =      Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb1_koopa_red.bmp");//
        public static int smb1_bulletbill =     Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb1_bulletbill.bmp");//
        public static int smb3_bulletbill_1 =   Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb3_bulletbill_1.bmp");//
        public static int smb3_bulletbill_2 =   Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb3_bulletbill_2.bmp");//
        public static int smw_bulletbill =      Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smw_bulletbill.bmp");//
        public static int smb1_buzzybeetle =    Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb1_buzzybeetle.bmp");//
        public static int smb1_spiny =          Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb1_spiny.bmp");//
        public static int smb1_piranha_green =  Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb1_piranha_green.bmp");//
        public static int smb1_piranha_red =    Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb1_piranha_red.bmp");//
        public static int smb1_hammerbros =     Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb1_hammerbros.bmp");//
        public static int smb1_potaboo =        Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb1_potaboo.bmp");//

        //Items
        public static int smb1_mushroom =       Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb1_mushroom.bmp");//
        public static int smb2_mushroom =       Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb2_mushroom.bmp");//
        public static int smb1_mushroom_p =     Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb1_mushroom_poison.bmp");//
        public static int smb1_fireflower =     Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb1_fireflower.bmp");//
        public static int smb3_fireflower =     Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb3_fireflower.bmp");//
        public static int smw_fireflower =      Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smw_fireflower.bmp");//
        public static int smb1_starman =        Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb1_starman.bmp");//
        public static int smb2_starman =        Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb2_starman.bmp");//
        public static int smb3_starman =        Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb3_starman.bmp");//
        public static int smw_starman =         Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smw_starman.bmp");//
        public static int smb1_1up =            Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb1_1up.bmp");//
        public static int smb2_1up =            Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb2_1up.bmp");//
        public static int smw_3up =             Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smw_3up.bmp");//

        public static int smb1_coin1 =          Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb1_coin1.bmp");//
        public static int smb1_coin2 =          Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb1_coin2.bmp");//
        public static int smb2_coin =           Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb2_coin.bmp");//
        public static int smb3_coin1 =          Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb3_coin1.bmp");//
        public static int smw_coin1 =           Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smw_coin1.bmp");//

        public static int smw_spring_p =        Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smw_springboard.bmp");
                
        public static int smb2_pow =            Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb2_pow.bmp");//
        public static int smb2_pow_on =         Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb2_pow_on.bmp");//

        public static int smw_pSwitch_b =       Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smw_p-switch_b.bmp");//
        public static int smb3_pSwitch_b =      Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb3_p-switch_b.bmp");//
        public static int smw_pSwitch_s =       Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smw_p-switch_s.bmp");
        public static int smb3_pSwitch_g =      Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb3_p-switch_g.bmp");

        public static int smb2_key =            Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb2_key.bmp");//
        public static int smw_key =             Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smw_key.bmp");//
        
        //Blocks
        public static int smb1_qm =             Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb1_qm.bmp");//
        public static int smb1_qm_e =           Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb1_qm_e.bmp");//
        public static int smb3_qm =             Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb3_qm.bmp");//
        public static int smb3_qm_e =           Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb3_qm_e.bmp");//
        public static int smw_qm =              Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smw_qm.bmp");//
        public static int smw_qm_e =            Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smw_qm_e.bmp");//
        public static int smb1_bricks1 =        Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb1_bricks1.bmp");//
        public static int smb1_bricks2 =        Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb1_bricks2.bmp");//
        public static int smb1_bricks3 =        Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb1_bricks3.bmp");//
        public static int smb3_bricks =         Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb3_bricks.bmp");//

        public static int smw_peaSpring =       Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smw_pea-spring.bmp");

        public static int smb1_cannon =         Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb1_cannon.bmp");//
        public static int smb3_cannon =         Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb3_cannon.bmp");//
        public static int smw_cannon =          Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smw_cannon.bmp");//

        public static int smb1_firebarBlock =   Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb1_firebar-block.bmp");//

        public static int smb1_lava =           Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb1_lava.bmp");//
        public static int smb3_lava1 =          Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb3_lava1.bmp");//
        public static int smb3_lava22 =         Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb3_lava22.bmp");//
        public static int smb3_lava45 =         Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb3_lava45.bmp");//

        //effects
        public static int smw_sparkle =         Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\smw_sparkle.bmp");//
        public static int mini_smoke =          Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\mini-smoke.bmp");//
        public static int smb1_smoke =          Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\smb1_smoke.bmp");//
        public static int smb2_smoke =          Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\smb2_smoke.bmp");//
        public static int smb3_smoke =          Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\smb3_smoke.bmp");//

        public static int fireballshot =        Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\fireballshot.bmp");//
        public static int fireball_hit =        Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\fireball_hit.bmp");//
        public static int smb1_hammerbrosHmr1 = Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\smb1_hammerbros_hammer.bmp");

        public static int qm_open =             Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\open-questionmark-block.bmp");//
        public static int smb1_brickSh1 =       Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\smb1_brickshatter1.bmp");//
        public static int smb1_brickSh2 =       Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\smb1_brickshatter2.bmp");//
        public static int smb1_brickSh3 =       Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\smb1_brickshatter3.bmp");//
        public static int smb3_brickSh1 =       Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\smb3_brickshatter1.bmp");//
        public static int smw_brickSh1 =        Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\smw_brickshatter1.bmp");//

        public static int smb1_getCoin =        Image.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\smb1_getCoin.bmp");//

    }
}
