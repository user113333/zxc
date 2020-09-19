using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace zxc {
    public class Core : Game {
        public static SpriteBatch Batch;
        public static GraphicsDeviceManager Graphics;
        public static ContentManager GlobalContent;
        public static Texture2D BasicTexture;

        public static Scene Scene;
        public static bool loadTerminal = false;

        private string title_init;

        public Core(int width, int height, string title, bool isFullScreen = false) {
            Graphics = new GraphicsDeviceManager(this) {
                PreferredBackBufferWidth = width,
                PreferredBackBufferHeight = height,
                IsFullScreen = isFullScreen,
                SynchronizeWithVerticalRetrace = true
            };

            base.Content.RootDirectory = "Content";
            GlobalContent = base.Content;
            IsMouseVisible = true;
            IsFixedTimeStep = false;
            Window.AllowUserResizing = true;
            Window.AllowAltF4 = true;
            Graphics.PreferMultiSampling = true;

            title_init = title;
        }

        public Scene SetScene(Scene scene) {
            if (Scene != null) {
                Scene.Dispose();
            }
            
            Scene = scene;
            Scene.Content = new ContentManager(Services);
            Scene.Content.RootDirectory = "Content";
            Scene.Initialize();
            return Scene;
        }

        public virtual void Init() { }

        #region MONO_GAME
        protected override void Initialize() {
            Window.Title = title_init;
            Window.TextInput += InputText.WindowEvent;

            BasicTexture = Util.CreateTexture(Graphics.GraphicsDevice, 1, 1, new Color(255, 255, 255, 255));
            InputHelper.Setup(this);
            
            Init();
            base.Initialize();
        }

        protected override void LoadContent() {
            Batch = new SpriteBatch(base.GraphicsDevice);
        }

        protected override void Update(GameTime gameTime) {
            Time.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            InputHelper.UpdateSetup();

            Scene.Update();

#if DEBUG
            Scene.Camera.DebugUpdate();
#endif
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            Scene.Camera.BeginDraw();
            base.GraphicsDevice.Clear(Scene.ClearColor);

            Batch.Begin(sortMode: SpriteSortMode.Deferred);
            Scene.BackgroundRender();
            Batch.End();

            Batch.Begin(sortMode: SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp, effect: null, transformMatrix: Scene.Camera.GetTransformation(), blendState: BlendState.AlphaBlend);

            Scene.Render();

#if DEBUG
            Scene.DebugRender();
#endif

            Batch.End();

            Batch.Begin(transformMatrix: Scene.Camera.GetScaleOnlyTransformation());
            Scene.ForegroundRender();
            Batch.End();

            base.Draw(gameTime);
        }
        #endregion
    }
}
