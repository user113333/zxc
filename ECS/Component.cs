namespace zxc {
    public class Component {
        public bool Updatable = true;
        public bool Drawable = true;
        public bool Enabled { get { return Updatable && Drawable; } set { Updatable = Drawable = value; } }
        public Entity Entity;
        public Scene Scene;
        public virtual void Initialize() { }
        public virtual void Update() { }
        public virtual void BackgroundRender() { }
        public virtual void Render() { }
        public virtual void ForegroundRender() { }
        public virtual void DebugRender() { }
        public virtual void Dispose() { }
    }
}
