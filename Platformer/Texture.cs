using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Game  //Only purpose is to hold all the textures
{
    static class Texture
    {
        public static int[,] bgTilesArray = new int[Directory.GetFiles((Environment.CurrentDirectory + @"\gfx\backgrounds\tiles"), "*.bmp").GetLength(0)+
                                                    Directory.GetFiles((Environment.CurrentDirectory + @"\gfx\backgrounds\tiles"), "*.png").GetLength(0), 2];  //create an array withe the length of found BMPs. 2nd field = animation frameCount

        //Other
        public static int tileSet =             MyImage.LoadTexture(RootThingy.exePath + @"\gfx\tilesets\smb1nes.bmp");//
        public static int backGround =          MyImage.LoadTexture(RootThingy.exePath + @"\gfx\backgrounds\smb3castle.bmp");//
        public static int ASCII =               MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\technical\ASCII-Characters_8x12.bmp");//
        //public static int warp_nr =             MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\technical\warp-nr_4x8.bmp");//
        //public static int warpIcons =           MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\technical\warp-icons_8x8.bmp");//
        public static int generator =           MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\technical\generator.bmp");//
        public static int watertint =           MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\technical\watertint.png");//
        public static int waterTop =            MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\technical\smb1_watertop.png");//

        public static int circle32_debug =      MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\special\circle32_debug.bmp");//

        public static int smw_keyhole =         MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\special\smw_keyhole.bmp");//
        public static int smw2_rollingRock =    MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\special\smw2_rolling_rock.bmp");//
        
        //Player
        public static int player =              MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\player\player_debug.bmp");//
        public static int mario_small =         MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\player\mario_small.bmp");//
        public static int mario_big =           MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\player\mario_big.bmp");//
        public static int mario_fire =          MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\player\mario_fire.bmp");//

        //NPCs
        public static int smw_boo =             MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smw_boo.bmp");//
        public static int smb1_goomba_1 =       MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb1_goomba_brown.bmp");//
        public static int smb1_goomba_2 =       MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb1_goomba_blue.bmp");//
        public static int smb1_goomba_3 =       MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb1_goomba_grey.bmp");//

        public static int smb1_koopa_green =    MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb1_koopa_green.bmp");//
        public static int smb1_koopa_red =      MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb1_koopa_red.bmp");//
        public static int smb1_bulletbill =     MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb1_bulletbill.bmp");//
        public static int smb3_bulletbill_1 =   MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb3_bulletbill_1.bmp");//
        public static int smb3_bulletbill_2 =   MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb3_bulletbill_2.bmp");//
        public static int smw_bulletbill =      MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smw_bulletbill.bmp");//
        public static int smb1_buzzybeetle =    MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb1_buzzybeetle.bmp");//
        public static int smb1_spiny =          MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb1_spiny.bmp");//
        public static int smb1_piranha_green =  MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb1_piranha_green.bmp");//
        public static int smb1_piranha_red =    MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb1_piranha_red.bmp");//
        public static int smb1_hammerbros =     MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb1_hammerbros.bmp");//
        public static int smb1_lakitu =         MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb1_lakitu.bmp");//
        public static int smb1_potaboo =        MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb1_potaboo.bmp");//

        public static int smb1_blooper =        MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb1_blooper.bmp");//
        public static int smb3_blooper =        MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb3_blooper.bmp");//
        public static int smb1_cheepcheepG =    MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb1_cheepcheep_green.bmp");//
        public static int smb1_cheepcheepR =    MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\npc\smb1_cheepcheep_red.bmp");//

        //Items
        public static int smb1_mushroom =       MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb1_mushroom.bmp");//
        public static int smb2_mushroom =       MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb2_mushroom.bmp");//
        public static int smb1_mushroom_p =     MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb1_mushroom_poison.bmp");//
        public static int smb1_fireflower =     MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb1_fireflower.bmp");//
        public static int smb3_fireflower =     MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb3_fireflower.bmp");//
        public static int smw_fireflower =      MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smw_fireflower.bmp");//
        public static int smb1_starman =        MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb1_starman.bmp");//
        public static int smb2_starman =        MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb2_starman.bmp");//
        public static int smb3_starman =        MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb3_starman.bmp");//
        public static int smw_starman =         MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smw_starman.bmp");//
        public static int smb1_1up =            MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb1_1up.bmp");//
        public static int smb2_1up =            MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb2_1up.bmp");//
        public static int smw_3up =             MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smw_3up.bmp");//

        public static int smb1_coin1 =          MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb1_coin1.bmp");//
        public static int smb1_coin2 =          MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb1_coin2.bmp");//
        public static int smb2_coin =           MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb2_coin.bmp");//
        public static int smb3_coin1 =          MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb3_coin1.bmp");//
        public static int smw_coin1 =           MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smw_coin1.bmp");//

        public static int smw_spring_p =        MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smw_springboard.bmp");
                
        public static int smb2_pow =            MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb2_pow.bmp");//
        public static int smb2_pow_on =         MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb2_pow_on.bmp");//

        public static int smw_pSwitch_b =       MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smw_p-switch_b.bmp");//
        public static int smb3_pSwitch_b =      MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb3_p-switch_b.bmp");//
        public static int smw_pSwitch_s =       MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smw_p-switch_s.bmp");
        public static int smb3_pSwitch_g =      MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb3_p-switch_g.bmp");

        public static int smb2_key =            MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smb2_key.bmp");//
        public static int smw_key =             MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\item\smw_key.bmp");//
        
        //Blocks
        public static int smb1_qm =             MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb1_qm.bmp");//
        public static int smb1_qm_e =           MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb1_qm_e.bmp");//
        public static int smb3_qm =             MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb3_qm.bmp");//
        public static int smb3_qm_e =           MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb3_qm_e.bmp");//
        public static int smw_qm =              MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smw_qm.bmp");//
        public static int smw_qm_e =            MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smw_qm_e.bmp");//
        public static int smb1_bricks1 =        MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb1_bricks1.bmp");//
        public static int smb1_bricks2 =        MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb1_bricks2.bmp");//
        public static int smb1_bricks3 =        MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb1_bricks3.bmp");//
        public static int smb3_bricks =         MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb3_bricks.bmp");//

        public static int smb1_platform =       MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb1_platform.bmp");//

        public static int smb1_wine =           MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb1_wine.bmp");//
        public static int smb1_winehead =       MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb1_winehead.bmp");//

        public static int smw_peaSpring =       MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smw_pea-spring.bmp");

        public static int smb1_cannon =         MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb1_cannon.bmp");//
        public static int smb3_cannon =         MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb3_cannon.bmp");//
        public static int smw_cannon =          MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smw_cannon.bmp");//

        public static int smb1_firebarBlock =   MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb1_firebar-block.bmp");//

        public static int smb1_lava =           MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb1_lava.bmp");//
        public static int smb3_lava1 =          MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb3_lava1.bmp");//
        public static int smb3_lava22 =         MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb3_lava22.bmp");//
        public static int smb3_lava45 =         MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\block\smb3_lava45.bmp");//

        //effects
        public static int smw_sparkle =         MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\smw_sparkle.bmp");//
        public static int mini_smoke =          MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\mini-smoke.bmp");//
        public static int smb1_smoke =          MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\smb1_smoke.bmp");//
        public static int smb2_smoke =          MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\smb2_smoke.bmp");//
        public static int smb3_smoke =          MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\smb3_smoke.bmp");//

        public static int smb1_pulley =         MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\smb1_balanceplatform_pulley.bmp");

        public static int fireballshot =        MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\fireballshot.bmp");//
        public static int fireball_hit =        MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\fireball_hit.bmp");//
        public static int smb1_hammerbrosHmr1 = MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\smb1_hammerbros_hammer.bmp");

        public static int qm_open =             MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\open-questionmark-block.bmp");//
        public static int smb1_brickSh1 =       MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\smb1_brickshatter1.bmp");//
        public static int smb1_brickSh2 =       MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\smb1_brickshatter2.bmp");//
        public static int smb1_brickSh3 =       MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\smb1_brickshatter3.bmp");//
        public static int smb3_brickSh1 =       MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\smb3_brickshatter1.bmp");//
        public static int smw_brickSh1 =        MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\smw_brickshatter1.bmp");//

        public static int smb1_getCoin =        MyImage.LoadTexture(RootThingy.exePath + @"\gfx\sprites\effect\smb1_getCoin.bmp");//

    }
}
