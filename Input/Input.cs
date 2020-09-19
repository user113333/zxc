using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace zxc {
    public static class Input {
        public static bool IsKeyPressed(Keys key) {
			return InputHelper.NewKeyboard.IsKeyDown(key) && !InputHelper.OldKeyboard.IsKeyDown(key);
		}

		public static bool IsKeyDown(Keys key) {
			return InputHelper.NewKeyboard.IsKeyDown(key);
		}

		public static bool IsKeyReleased(Keys key) {
			return !InputHelper.NewKeyboard.IsKeyDown(key) && InputHelper.OldKeyboard.IsKeyDown(key);
		}

		public static bool LeftMousePressed => InputHelper.NewMouse.LeftButton == ButtonState.Pressed && InputHelper.OldMouse.LeftButton == ButtonState.Released;
		public static bool LeftMouseDown => InputHelper.NewMouse.LeftButton == ButtonState.Pressed;
		public static bool LeftMouseReleased => InputHelper.NewMouse.LeftButton == ButtonState.Released && InputHelper.OldMouse.LeftButton == ButtonState.Pressed;
		public static bool RightMousePressed => InputHelper.NewMouse.RightButton == ButtonState.Pressed && InputHelper.OldMouse.RightButton == ButtonState.Released;
		public static bool RightMouseDown => InputHelper.NewMouse.RightButton == ButtonState.Pressed;
		public static bool RightMouseReleased => InputHelper.NewMouse.RightButton == ButtonState.Released && InputHelper.OldMouse.RightButton == ButtonState.Pressed;
		public static Vector2 MousePosRaw => InputHelper.NewMouse.Position.ToVector2();
		public static Vector2 MousePosVirtual => Core.Scene.Camera.ScaleScreenPositionToVirtualPosition(MousePosRaw);
		public static Vector2 MousePosUI => Core.Scene.Camera.ScaleScreenPositionToUIPosition(MousePosRaw);
		public static Vector2 MousePosDeltaRaw => (InputHelper.OldMouse.Position - InputHelper.NewMouse.Position).ToVector2();
		public static Vector2 MousePosDeltaVirtual => Core.Scene.Camera.ScaleScreenPositionToVirtualPosition(MousePosDeltaRaw);
		public static int MouseWheel => InputHelper.NewMouse.ScrollWheelValue;
		public static int MouseWheelDelta => InputHelper.NewMouse.ScrollWheelValue - InputHelper.OldMouse.ScrollWheelValue;
    }
}
