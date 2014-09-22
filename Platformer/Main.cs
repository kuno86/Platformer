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
    class RootThingy
    {
        public static int windowX = 1280;
        public static int windowY = 688;
        public static string exePath = Environment.CurrentDirectory;
        public static double sceneX = 640;
        public static int sceneY = 480;
        public static bool debugInfo=true;

        public static Random rnd = new Random();

        public struct Point
        {
            public double x;
            public double y;
        }

        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [STAThread]
        public static void Main()
        {

            Random rnd = new Random();
            using (var game = new GameWindow())
            {
                game.Load += (sender, e) =>
                {
                    // setup settings, load textures, sounds
                    game.X = 32;
                    game.Y = 16;
                    game.VSync = VSyncMode.On;
                    game.Width = windowX;
                    game.Height = windowY;
                    game.WindowBorder = WindowBorder.Fixed; //Disables the resizable windowframe
                    GL.Enable(EnableCap.Blend);                                                     //These lines
                    GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);  //enable transparency using alpha-channel
                    

                    //////////////////////////////////////////////////////////////////////////////////////////////// Load all Background-Objects from the Folder
                    int i = 0;
                    foreach (string bmpFile in Directory.GetFiles((exePath + @"\gfx\backgrounds\tiles"), "*.bmp"))  //get all background tiles and load them
                    {
                        Texture.bgTilesArray[i, 0] = Image.LoadTexture(bmpFile);
                        string framesCount = (bmpFile.Replace((exePath + @"\gfx\backgrounds\tiles\"), "").Substring(4, 2));   //get Animation FrameCount ( "<id><id><id>_<frameCount><frameCount>.bmp")
                        if (!int.TryParse(framesCount, out Texture.bgTilesArray[i, 1]))
                            Texture.bgTilesArray[i, 1] = 1;
                        i++;
                    }
                    ////////////////////////////////////////////////////////////////////////////////////////////////

                };

                Map karte = new Map(16); 

                game.Resize += (sender, e) =>
                {
                    //sceneX = game.Height;
                    //sceneY = game.Width;
                    GL.Viewport(0, 0, windowX, windowY);
                };
                var mouse = Mouse.GetState();
                game.UpdateFrame += (sender, e) =>
                {
                    // add game logic, input handling
                    mouse = Mouse.GetState();

                    if (game.Keyboard[Key.Escape])
                    {
                        game.Exit();
                    }
                                        
                    game.Title = (("FPS: " + (int)(game.RenderFrequency) +" ; "+ Math.Round(game.RenderTime*1000,2)+"ms/frame"));
                    
                    
                    Map.fpsLine.Insert(0,(int)game.RenderFrequency);
                    while (Map.fpsLine.Count > 230)
                        Map.fpsLine.RemoveAt(Map.fpsLine.Count - 1);

                };
                
                game.RenderFrame += (sender, e) =>
                {
                    // render graphics
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();
                    GL.Ortho(0, windowX, windowY, 0, -1000, 1000);  //Render  distant objects smaller
                    
                    GL.MatrixMode(MatrixMode.Modelview);
                    
                    karte.draw();    ///////////////////////////////////////// MAP-Object
                                        
                    karte.process(game.Mouse.X, game.Mouse.Y);

                    game.SwapBuffers();
                };

                // Run the game at 60 updates per second
                game.Run(199.0);
            }
        }

//End of Root =============================================================

    }
}