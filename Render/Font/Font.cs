using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace zxc {
    public class Font {
        public float YSmoothing = 0;
        public float XSmoothing = 0;
        protected List<Texture2D> Textures = new List<Texture2D>();
        protected Dictionary<char, Character> Characters = new Dictionary<char, Character>();

        public void DrawText(string text, Vector2 pos, float scale, Color color) {
            char[] arr = text.ToCharArray();
            pos.X += XSmoothing * scale;
            pos.Y += YSmoothing * scale;

            for (int i = 0; i < arr.Length; i++) {
                if (!Characters.TryGetValue(arr[i], out Character character))
                    continue;

                if (character.Page >= 0) {
                    Core.Batch.Draw(Textures[character.Page], pos + (new Vector2(character.XOffset, character.YOffset) * scale), new Rectangle(character.X, character.Y, character.Width, character.Height), color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                }
                
                pos.X += character.XAdvance * scale;
            }
        }

        public virtual float GetTextWidth(string text, int length) {
            float sum = 0;

            for (int i = 0; i < length; i++) {
                if (!Characters.TryGetValue(text[i], out Character character))
                    continue;

                sum += character.XAdvance;
            }

            return sum;
        }
    }
}
