using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TrueTypeSharp;

namespace zxc {
    public class FontTT : Font {
        private TrueTypeFont font;
        private float scale;

        // Japanese, Vietnamese, Korean, Simplified Chinese, Taiwanese
        // fontSize in px (font height)
        public FontTT(string path, float fontSize) {
            font = new TrueTypeFont("./Content/" + path);
            scale = font.GetScaleForPixelHeight(fontSize);
            YSmoothing = fontSize;
            XSmoothing = 1;
        }

        public void LoadRange(char start, char end) {
            for (char i = start; i < end; i++) {
                int id = font.FindGlyphIndex(i);
                var data = font.GetGlyphBitmap(id, scale, scale, out int width, out int height, out int xOffset, out int yOffset);
                font.GetGlyphHMetrics(id, out int advance, out int bearing);
                font.GetGlyphBitmapBox(id, scale, scale, out int x0, out int y0, out int x1, out int y1);

                Characters[i] = new Character() {
                    Page = data.Length == 0 ? -1 : Textures.Count,
                    Id = (int)i,
                    X = 0,
                    Y = 0,
                    Width = width,
                    Height = height,
                    XOffset = x0,
                    YOffset = y0,
                    XAdvance = advance * scale,
                };

                if (data.Length != 0) {
                    Texture2D tex = Util.DataToTexture2D(data, width, height);
                    Textures.Add(tex);
                }
            }
        }
    }
}
