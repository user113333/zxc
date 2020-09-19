using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace zxc {
    public class SpriteSheet {
        public Texture2D Texture;
        public Frame[] Frames;

        public SpriteSheet(Texture2D texture, int width, int height) {
            var rects = texture.ToSpriteSheetRectangles(width, height);
            for (int i = 0; i < rects.Length; i++) {
                Frames[i] = new Frame(rects[i]);
            }
        }
    }

    public class Frame {
        public Rectangle SourceRectangle;

        public Frame(Rectangle rectangle) {
            SourceRectangle = rectangle;
        }
    }
}
