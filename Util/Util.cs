using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace zxc {
    public static class Util {
        public static Color Black50 = new Color(0, 0, 0, 125);

        public static string RandomString(int size = 38) {
            var builder = new StringBuilder();

            char ch;
            for (int i = 0; i < size; i++) {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * Random.NextFloat() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public static Texture2D DataToTexture2D(byte[] data, int width, int height) {
            Texture2D texture = new Texture2D(Core.Graphics.GraphicsDevice, width, height);

            Color[] colors = new Color[width * height];

            for (int i = 0; i < data.Length; i++) {
                //colors[i] = new Color(data[i], data[i], data[i], (byte)255);
                colors[i] = new Color(data[i], data[i], data[i], data[i]);
            }

            texture.SetData(colors);
            return texture;
        }

        public static Texture2D DataToTexture2D(float[,] data, int width, int height) {
            Texture2D texture = new Texture2D(Core.Graphics.GraphicsDevice, width, height);

            Color[] colors = new Color[width * height];

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    byte val = (byte)data[x, y];
                    colors[y*width+x] = new Color(val, val, val, (byte)255);
                }
            }

            texture.SetData(colors);
            return texture;
        }

        public static void ReplaceColor(this Texture2D texture, Color target, Color replacement) {
            Color[] colors = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(colors);

            for (int i = 0; i < colors.Length; i++) {
                if (colors[i] == target)
                    colors[i] = replacement;
            }

            texture.SetData(colors);
        }

        public static Texture2D CreateTexture(GraphicsDevice device, int width, int height, Color color) {
			var texture = new Texture2D(device, width, height);

			Color[] data = new Color[width * height];
			for(var pixel = 0; pixel < data.Length; pixel++)
			{
				data[pixel] = color;
			}

			texture.SetData( data );
			return texture;
		}

        public static Rectangle[] ToSpriteSheetRectangles(this Texture2D texture, int width, int height) {
            int realHeight = texture.Height / height;
            int realWidth = texture.Width / width;
            
            Rectangle[] rects = new Rectangle[realHeight * realWidth];
            int i = 0;
            
            for (int y = 0; y < realHeight; y += width) {
                for (int x = 0; x < realWidth; x += height) {
                    rects[i] = new Rectangle(x, y, width, height);
                }
            }

            return rects;
        }

        public static SpriteSheet ToSpriteSheet(this Texture2D texture, int width, int height) {
            return new SpriteSheet(texture, width, height);
        }
    }
}
