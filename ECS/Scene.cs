using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace zxc {
    public class Scene {
        public ContentManager Content;
        public RenderCamera Camera;
        public Color ClearColor = Color.Black;

        private readonly List<Entity> entities = new List<Entity>();
        private long entityIndex = 0;
        private Terminal terminal;

        public Scene() {
            Camera = new RenderCamera();
        }

        public Entity CreateEntity(string name) {
            Entity entity = new Entity(name);
            entities.Add(entity);

            entity.Scene = this;
            entity.ID = entityIndex;
            entityIndex++;
            return entity;
        }

        public void RemoveEntity(int id) {
            for (int i = 0; i < entities.Count; i++) {
                if (entities[i].ID == id)
                    entities.RemoveAt(i);
            }
        }

        public Entity GetEntity(string name) {
            for (int i = 0; i < entities.Count; i++) {
                if (entities[i].Name == name)
                    return entities[i];
            }

            return null;
        }

        public Entity GetEntity(int id) {
            for (int i = 0; i < entities.Count; i++) {
                if (entities[i].ID == id)
                    return entities[i];
            }

            return null;
        }

        public virtual void Initialize() {
            for (int i = 0; i < entities.Count; i++) {
                entities[i].Initialize();
            }
        }


        public virtual void Update() {
            terminal?.Update();

            for (int i = 0; i < entities.Count; i++) {
                if (entities[i].Updatable)
                    entities[i].Update();
            }
        }

        public virtual void BackgroundRender() {
            for (int i = 0; i < entities.Count; i++) {
                if (entities[i].Drawable)
                    entities[i].BackgroundRender();
            }
        }

        public virtual void Render() {
            for (int i = 0; i < entities.Count; i++) {
                if (entities[i].Drawable)
                    entities[i].Render();
            }
        }

        public virtual void ForegroundRender() {
            for (int i = 0; i < entities.Count; i++) {
                if (entities[i].Drawable)
                    entities[i].ForegroundRender();
            }

            terminal?.Render(Camera.VirtualWidthUI, Camera.VirtualHeightUI);
        }

        public virtual void DebugRender() {
            for (int i = 0; i < entities.Count; i++) {
                entities[i].DebugRender();
            }
        }

        public virtual void Dispose() {
            for (int i = 0; i < entities.Count; i++) {
                entities[i].Dispose();
            }

            Content.Dispose();
        }

        protected void LoadTerminal(Font font) {
            terminal = new Terminal(font, "player");
        }
    }
}
