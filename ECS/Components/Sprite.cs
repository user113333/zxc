using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace zxc {
    public class Sprite : Component {
        private Texture2D texture;
        private Color color =  Color.White;

        public void LoadTexture(string path, bool setEntityOriginCenter = false) {
            LoadTexture(Scene.Content.Load<Texture2D>(path), setEntityOriginCenter);
        }

        public void LoadTexture(Texture2D texture, bool setEntityOriginCenter = false) {
            this.texture = texture;

            if (setEntityOriginCenter)
                Entity.Origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        public void SetColor(Color color) {
            this.color = color;
        }

        public override void Render() {
            Core.Batch.Draw(texture, Entity.Position, null, color, Entity.Rotation, Entity.Origin, Entity.Scale, Entity.Effect, 0);
        }
    }
}
