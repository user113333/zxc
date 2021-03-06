﻿using Microsoft.Xna.Framework.Input;

namespace zxc {
    /// <summary>
    /// Checks various conditions on a specific mouse button.
    /// Non static methods implicitly make sure that the game is active. Otherwise returns false.
    /// </summary>
    public class MouseCondition : ICondition {

        // Group: Constructors

        /// <param name="button">The button to operate on.</param>
        public MouseCondition(MouseButton button) {
            _button = button;
        }

        // Group: Public Functions

        /// <returns>Returns true when the button was not pressed and is now pressed.</returns>
        public bool Pressed() {
            return Pressed(_button) && IsMouseValid(InputHelper.IsActive);
        }
        /// <returns>Returns true when the button is now pressed.</returns>
        public bool Down() {
            return Down(_button) && IsMouseValid(InputHelper.IsActive);
        }
        /// <returns>Returns true when the button was pressed and is now pressed.</returns>
        public bool DownOnly() {
            return DownOnly(_button) && IsMouseValid(InputHelper.IsActive);
        }
        /// <returns>Returns true when the button was pressed and is now not pressed.</returns>
        public bool Released() {
            return Released(_button) && IsMouseValid(InputHelper.IsActive);
        }

        // Group: Static Functions

        /// <returns>Returns true when the button was not pressed and is now pressed.</returns>
        public static bool Pressed(MouseButton button) {
            return InputHelper.MouseButtons[button](InputHelper.NewMouse) == ButtonState.Pressed &&
                   InputHelper.MouseButtons[button](InputHelper.OldMouse) == ButtonState.Released;
        }
        /// <returns>Returns true when the button is now pressed.</returns>
        public static bool Down(MouseButton button) {
            return InputHelper.MouseButtons[button](InputHelper.NewMouse) == ButtonState.Pressed;
        }
        /// <returns>Returns true when the button was pressed and is now pressed.</returns>
        public static bool DownOnly(MouseButton button) {
            return InputHelper.MouseButtons[button](InputHelper.NewMouse) == ButtonState.Pressed &&
                   InputHelper.MouseButtons[button](InputHelper.OldMouse) == ButtonState.Pressed;
        }
        /// <returns>Returns true when the button was pressed and is now not pressed.</returns>
        public static bool Released(MouseButton button) {
            return InputHelper.MouseButtons[button](InputHelper.NewMouse) == ButtonState.Released &&
                   InputHelper.MouseButtons[button](InputHelper.OldMouse) == ButtonState.Pressed;
        }
        /// <returns>Returns true when the mouse is within the game window.</returns>
        public static bool IsMouseValid(bool IsActive) {
            if (IsActive &&
                InputHelper.NewMouse.X >= 0 && InputHelper.NewMouse.X <= InputHelper.WindowWidth &&
                InputHelper.NewMouse.Y >= 0 && InputHelper.NewMouse.Y <= InputHelper.WindowHeight) {
                return true;
            }
            return false;
        }

        // Group: Private Variables

        /// <summary>
        /// The button that will be checked.
        /// </summary>
        private MouseButton _button;
    }
}
