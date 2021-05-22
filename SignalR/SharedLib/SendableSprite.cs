using MichaelLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib
{
    public class SendableSprite
    {
        public Color[] Data { get; private set; }
        public Vector2 Position { get; set; }
        public Color Color { get; set; }
        public Vector2 Scale { get; set; }
        public object Tag { get; set; }
   
        private Texture2D texture;
        public int Width { get; }
        public int Height { get; }
        public Vector2 Origin { get; set; }
        public SendableSprite(Color[] data, Vector2 pos, Color col, Vector2 scale, int width)
        {
            Data = data;
            Position = pos;
            Color = col;
            Scale = scale;

            Width = width;
            Height = data.Length / width;
        }

        public void CreateTexture(GraphicsDevice device)
        {
            if (texture != null) return;
            texture = new Texture2D(device, Width, Height);
            texture.SetData(Data);
        }

        public void Draw(SpriteBatch sb)
        {
            if (texture == null) return;
            sb.Draw(texture, Position, null, Color, 0f, Origin, Scale, SpriteEffects.None, 0f);
        }
    }
}
