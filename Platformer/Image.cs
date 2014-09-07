using System;
using System.IO;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;

namespace Game
{
    public class Image : GameWindow
    {
        public static int activeTexture;
        private static int ASCII;
        public Image()
            : base(800, 600, GraphicsMode.Default, "Hoard of Upgrades")
        {
            //GL.ClearColor(0, 0.1f, 0.4f, 1);
            ASCII = LoadTexture(RootThingy.exePath + @"\gfx\tilesets\smb1nes.bmp");
        }


        public static int LoadTexture(string file)
        {
            if (!File.Exists(file))
            {
                Console.WriteLine("Texture '" + file + "' could not be loaded!");
                Console.ReadKey();
                return 0;
            }
            
            Bitmap bitmap = new Bitmap(file);
            bitmap.MakeTransparent(Color.Magenta);
            int tex;
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            GL.GenTextures(1, out tex);
            GL.BindTexture(TextureTarget.Texture2D, tex);

            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            return tex;
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// BEGIN DRAW 2D
        public static void beginDraw2D()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Ortho(0, RootThingy.windowX, RootThingy.windowY, 0, -1, 1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Disable(EnableCap.Lighting);
            GL.Enable(EnableCap.Texture2D);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// END DRAW 2D
        public static void endDraw2D()
        {
            GL.Disable(EnableCap.Texture2D);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Modelview);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// DRAW IMAGE
        public static void drawImage(int image, double x, double y, bool flipV=false, bool flipH=false)
        {
            int w;
            int h;
            GL.Color4(1, 1, 1, 1.0f);
            if (activeTexture != image)
                GL.BindTexture(TextureTarget.Texture2D, image);
            activeTexture = image;

            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureWidth, out w);
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureHeight, out h);

            GL.Begin(PrimitiveType.Quads);
            if(!flipV && !flipH)    //No flip
            {
                GL.TexCoord2(0, 0);
                GL.Vertex2(x, y);
                GL.TexCoord2(1, 0);
                GL.Vertex2(x + w, y);
                GL.TexCoord2(1, 1);
                GL.Vertex2(x + w, y + h);
                GL.TexCoord2(0, 1);
                GL.Vertex2(x, y + h);
            }
            else if (flipV && !flipH)   //Vertical(X) flip 
            {
                GL.TexCoord2(1, 0);
                GL.Vertex2(x, y);
                GL.TexCoord2(0, 0);
                GL.Vertex2(x + w, y);
                GL.TexCoord2(0, 1);
                GL.Vertex2(x + w, y + h);
                GL.TexCoord2(1, 1);
                GL.Vertex2(x, y + h);
            }
            else if (!flipV && flipH)    //Horizontal(y) flip
            {
                GL.TexCoord2(0, 1);
                GL.Vertex2(x, y);
                GL.TexCoord2(1, 1);
                GL.Vertex2(x + w, y);
                GL.TexCoord2(1, 0);
                GL.Vertex2(x + w, y + h);
                GL.TexCoord2(0, 0);
                GL.Vertex2(x, y + h);
            }
            else if (flipV && flipH)    //Vertical(x) + Horizontal(y) flip
            {
                GL.TexCoord2(1, 1);
                GL.Vertex2(x, y);
                GL.TexCoord2(0, 1);
                GL.Vertex2(x + w, y);
                GL.TexCoord2(0, 0);
                GL.Vertex2(x + w, y + h);
                GL.TexCoord2(1, 0);
                GL.Vertex2(x, y + h);
            }
            GL.End();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// DRAW SQUARE TILE
        public static void drawTileSquare(int image, int tileID, int tileSize, double x, double y, bool flipV = false, bool flipH = false)
        {
            int w;
            int h;
            if (activeTexture != image)
                GL.BindTexture(TextureTarget.Texture2D, image);
            activeTexture = image;
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureWidth, out w);
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureHeight, out h);

            double tileX = 0;
            double tileY = 0;
            if (tileID * tileSize <= w * h)
                tileX = tileID * tileSize;
            tileY = 0;
            if (tileID * tileSize >= w)
            {
                tileX = (tileID * tileSize) - ((tileID * tileSize) / w) * w;
                tileY = ((tileID * tileSize) / w) * tileSize;
            }

            GL.Begin(PrimitiveType.Quads);
            GL.Color4(1, 1, 1, 1.0f);
            if (!flipV && !flipH)    //No flip
            {
                GL.TexCoord2(tileX / w, tileY / h);
                GL.Vertex2(x, y);
                GL.TexCoord2((tileX + tileSize) / w, tileY / h);
                GL.Vertex2(x + tileSize, y);
                GL.TexCoord2((tileX + tileSize) / w, (tileY + tileSize) / h);
                GL.Vertex2(x + tileSize, y + tileSize);
                GL.TexCoord2(tileX / w, (tileY + tileSize) / h);
                GL.Vertex2(x, y + tileSize);
            }
            else if (flipV && !flipH)   //Vertical(X) flip 
            {
                GL.TexCoord2((tileX + tileSize) / w, tileY / h);
                GL.Vertex2(x, y);
                GL.TexCoord2(tileX / w, tileY / h);
                GL.Vertex2(x + tileSize, y);
                GL.TexCoord2(tileX / w, (tileY + tileSize) / h);
                GL.Vertex2(x + tileSize, y + tileSize);
                GL.TexCoord2((tileX + tileSize) / w, (tileY + tileSize) / h);
                GL.Vertex2(x, y + tileSize);
            }
            else if (!flipV && flipH)    //Horizontal(y) flip
            {
                GL.TexCoord2(tileX / w, (tileY + tileSize) / h);
                GL.Vertex2(x, y);
                GL.TexCoord2((tileX + tileSize) / w, (tileY + tileSize) / h);
                GL.Vertex2(x + tileSize, y);
                GL.TexCoord2((tileX + tileSize) / w, tileY / h);
                GL.Vertex2(x + tileSize, y + tileSize);
                GL.TexCoord2(tileX / w, tileY / h);
                GL.Vertex2(x, y + tileSize);
            }
            else if (flipV && flipH)    //Vertical(x) + Horizontal(y) flip
            {
                GL.TexCoord2((tileX + tileSize) / w, (tileY + tileSize) / h);
                GL.Vertex2(x, y);
                GL.TexCoord2(tileX / w, (tileY + tileSize) / h);
                GL.Vertex2(x + tileSize, y);
                GL.TexCoord2(tileX / w, tileY / h);
                GL.Vertex2(x + tileSize, y + tileSize);
                GL.TexCoord2((tileX + tileSize) / w, tileY / h);
                GL.Vertex2(x, y + tileSize);
            }
            GL.End();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// DRAW TILE
        public static void drawTileFrame(int image, int frameID, int frameCount, double x, double y, bool flipV = false, bool flipH = false)
        {
            int w;
            int h;
            if (activeTexture != image)
                GL.BindTexture(TextureTarget.Texture2D, image);
            activeTexture = image;
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureWidth, out w);
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureHeight, out h);

            while (frameID > frameCount - 1)
            {
                frameID -= frameCount;
            }
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(1, 1, 1, 1.0f);

            if (!flipV && !flipH)    //No flip
            {
                GL.TexCoord2((frameID) / (double)frameCount, 0);
                GL.Vertex2(x, y);
                GL.TexCoord2(((frameID + 1) / (double)frameCount), 0);
                GL.Vertex2(x + (w / frameCount), y);
                GL.TexCoord2(((frameID + 1) / (double)frameCount), 1);
                GL.Vertex2(x + (w / frameCount), y + h);
                GL.TexCoord2((frameID) / (double)frameCount, 1);
                GL.Vertex2(x, y + h);
            }
            else if (flipV && !flipH)   //Vertical(X) flip 
            {
                GL.TexCoord2((frameID+1) / (double)frameCount, 0);
                GL.Vertex2(x, y);
                GL.TexCoord2(((frameID) / (double)frameCount), 0);
                GL.Vertex2(x + (w / frameCount), y);
                GL.TexCoord2(((frameID) / (double)frameCount), 1);
                GL.Vertex2(x + (w / frameCount), y + h);
                GL.TexCoord2((frameID+1) / (double)frameCount, 1);
                GL.Vertex2(x, y + h);
            }
            else if (!flipV && flipH)    //Horizontal(y) flip
            {
                GL.TexCoord2((frameID) / (double)frameCount, 1);
                GL.Vertex2(x, y);
                GL.TexCoord2(((frameID + 1) / (double)frameCount), 1);
                GL.Vertex2(x + (w / frameCount), y);
                GL.TexCoord2(((frameID + 1) / (double)frameCount), 0);
                GL.Vertex2(x + (w / frameCount), y + h);
                GL.TexCoord2((frameID) / (double)frameCount, 0);
                GL.Vertex2(x, y + h);
            }
            else if (flipV && flipH)    //Vertical(x) + Horizontal(y) flip
            {
                GL.TexCoord2((frameID + 1) / (double)frameCount, 1);
                GL.Vertex2(x, y);
                GL.TexCoord2(((frameID) / (double)frameCount), 1);
                GL.Vertex2(x + (w / frameCount), y);
                GL.TexCoord2(((frameID) / (double)frameCount), 0);
                GL.Vertex2(x + (w / frameCount), y + h);
                GL.TexCoord2((frameID + 1) / (double)frameCount, 0);
                GL.Vertex2(x, y + h);
            }
            GL.End();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// DRAW TEXT
        public static void drawText(string text, int x, int y, Color textColor, int image)
        {
            char[] textArr = new char[text.Length];
            textArr = text.ToCharArray();
            int w;
            int h;

            GL.Color4(textColor);
            if (activeTexture != image)
                GL.BindTexture(TextureTarget.Texture2D, image);
            activeTexture = image;
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureWidth, out w);
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureHeight, out h);

            GL.Begin(PrimitiveType.Quads);
            foreach (char character in textArr)
            {
                GL.TexCoord2(((int)character) / 256.0, 0);
                GL.Vertex2(x, y);
                GL.TexCoord2((((int)character + 1) / 256.0), 0);
                GL.Vertex2(x + 8, y);
                GL.TexCoord2((((int)character + 1) / 256.0), 1);
                GL.Vertex2(x + 8, y + h);
                GL.TexCoord2(((int)character) / 256.0, 1);
                GL.Vertex2(x, y + h);
                x += 8;
            }
            GL.End();
        }



    }

}