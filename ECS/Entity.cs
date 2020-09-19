using System.Collections.Generic;

namespace zxc {
    public class Entity : Transform {
        public bool Updatable = true;
        public bool Drawable = true;
        public bool Enabled { get { return Updatable && Drawable; } set { Updatable = Drawable = value; } }
        public string Name;
        public long ID;
        public Scene Scene;

        readonly private List<Component> components = new List<Component>();

        public Entity(string name) {
            Name = name;
        }

        public void AddComponent(Component component) {
            component.Entity = this;
            component.Scene = this.Scene;
            components.Add(component);
        }

        public T GetComponent<T>() where T : Component {
            for (int i = 0; i < components.Count; i++) {
                if (components[i] is T)
                    return components[i] as T;
            }

            return null;
        }

        public void RemoveComponent<T>() where T : Component {
            for (int i = 0; i < components.Count; i++) {
                if (components[i] is T)
                    components.RemoveAt(i);
            }
        }

        public void Initialize() {
            for (int i = 0; i < components.Count; i++)
                components[i].Initialize();
        }

        public void Update() {
            for (int i = 0; i < components.Count; i++) {
                if (components[i].Updatable)
                    components[i].Update();
            }
        }

        public void BackgroundRender() {
            for (int i = 0; i < components.Count; i++) {
                if (components[i].Drawable)
                    components[i].BackgroundRender();
            }
        }

        public void Render() {
            for (int i = 0; i < components.Count; i++) {
                if (components[i].Drawable)
                    components[i].Render();
            }
        }

        public void ForegroundRender() {
            for (int i = 0; i < components.Count; i++) {
                if (components[i].Drawable)
                    components[i].ForegroundRender();
            }
        }

        public void DebugRender() {
            for (int i = 0; i < components.Count; i++) {
                components[i].DebugRender();
            }
        }

        public void Dispose() {
            for (int i = 0; i < components.Count; i++) {
                components[i].Dispose();
            }
        }
    }
}
