using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace zxc {
    public class RenderCamera {
        public float Ratio = 16f/9f;
        public float Zoom = 1f;
        public Vector2 Position = new Vector2(0, 0);
        public float PositionZ = -5f;
        public int VirtualWidth = 1600;
        public int VirtualHeight = 900;
        public int VirtualWidthUI = 1600;
        public int VirtualHeightUI = 900;
        public Matrix ProjectionMatrix { get; private set; }
        public Matrix ViewMatrix { get; private set; }
        public Matrix WorldMatrix { get; private set; }

        private Vector3 PositionOffset = new Vector3(0, 0, 0);

        public Matrix GetTransformation() {
            return Matrix.CreateTranslation(
                (new Vector3(-Position.X, -Position.Y, 0)) + PositionOffset) * Matrix.CreateScale (
                (float)Core.Graphics.GraphicsDevice.Viewport.Width / VirtualWidth,
                (float)Core.Graphics.GraphicsDevice.Viewport.Height / VirtualHeight,
                1f
            ) * Matrix.CreateScale(Zoom);
        }

        public Matrix GetScaleOnlyTransformation() {
            return Matrix.CreateScale (
                (float)Core.Graphics.GraphicsDevice.Viewport.Width / VirtualWidthUI,
                (float)Core.Graphics.GraphicsDevice.Viewport.Height / VirtualHeightUI,
                1f
            );
        }

        private void ScaledViewport() {
            Viewport vp = Core.Graphics.GraphicsDevice.Viewport;
            
            float width = Core.Graphics.GraphicsDevice.PresentationParameters.Bounds.Width;
            float height = Core.Graphics.GraphicsDevice.PresentationParameters.Bounds.Height;

            if (height * Ratio > width) {
                height = width / Ratio;
                vp.X = 0;
                vp.Y = (int)((Core.Graphics.GraphicsDevice.PresentationParameters.Bounds.Height - height) / 2);
            } else if (width > height * Ratio) {
                width = height * Ratio;
                vp.Y = 0;
                vp.X = (int)((Core.Graphics.GraphicsDevice.PresentationParameters.Bounds.Width - width) / 2);
            } else {
                vp.X = 0;
                vp.Y = 0;
            }

            vp.Width = (int)width;
            vp.Height = (int)height;

            Core.Graphics.GraphicsDevice.Viewport = vp;
        }

        public void BeginDraw() {
            PositionOffset.X = VirtualWidth / 2 * (1 / Zoom);
            PositionOffset.Y = VirtualHeight / 2 * (1 / Zoom);
            ScaledViewport();

            Vector3 cameraTarget = new Vector3(Position, 0.0f);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), Core.Graphics.GraphicsDevice.Viewport.AspectRatio, 1f, 1000f);
            ViewMatrix = Matrix.CreateLookAt(new Vector3(Position, PositionZ), cameraTarget, Vector3.Up);
            WorldMatrix = Matrix.CreateWorld(cameraTarget, Vector3.Forward, Vector3.Up);
        }

        public void DebugUpdate() {
            //ZOOM
            if (!(Input.IsKeyDown(Keys.LeftControl)))
                return;
            
            Zoom += Input.MouseWheelDelta / 300.0f * Zoom / 2;

            //POSITION
            if (Input.LeftMouseDown) {
                Position += Input.MousePosDeltaVirtual;
            }
        }

        public void SetResolution(int i, bool isFullScreen = false) {
            int width = 1600;
            
            switch (i) {
                case 0: width = 426; break;
                case 1: width = 800; break;
                case 2: width = 1024; break;
                case 3: width = 1280; break;
                case 4: width = 1600; break;
                case 5: width = 1920; break;
                case 6: width = 2560; break;
                case 7: width = 3840; break;
            }

            int height = (int)Mathf.Round((float)width / Ratio);

            Core.Graphics.PreferredBackBufferWidth = width;
            Core.Graphics.PreferredBackBufferHeight = height;
            Core.Graphics.IsFullScreen = isFullScreen;
            Core.Graphics.ApplyChanges();
        }

        public Vector2 ScaleScreenPositionToVirtualPosition(Vector2 pos) {
            pos.X *= ((float)VirtualWidth / Core.Graphics.GraphicsDevice.Viewport.Width);
            pos.Y *= ((float)VirtualHeight / Core.Graphics.GraphicsDevice.Viewport.Height);

            return pos / Zoom;
        }

        public Vector2 ScaleScreenPositionToUIPosition(Vector2 pos) {
            pos.X *= ((float)VirtualWidth / Core.Graphics.GraphicsDevice.Viewport.Width);
            pos.Y *= ((float)VirtualHeight / Core.Graphics.GraphicsDevice.Viewport.Height);

            return pos;
        }

        public Vector2 ScreenToWorldPosition(Vector2 pos) {
            pos.X *= ((float)VirtualWidth / Core.Graphics.GraphicsDevice.Viewport.Width);
            pos.Y *= ((float)VirtualHeight / Core.Graphics.GraphicsDevice.Viewport.Height);
            
            return pos / Zoom + Position;
        }

        public Vector2 WorldToScreenPosition(Vector2 pos) {
            pos.X *= (Core.Graphics.GraphicsDevice.Viewport.Width / VirtualWidth);
            pos.Y *= (Core.Graphics.GraphicsDevice.Viewport.Height / VirtualHeight);

            return pos / Zoom - Position;
        }
    }
}
