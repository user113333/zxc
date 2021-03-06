using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace zxc {
    /// <summary>
    /// Unorganized helper functions for Apos.Input.
    /// Also holds some important variables for this library to work correctly.
    /// </summary>
    public static class InputHelper {

        // Group: Public Variables

        /// <value>Pass your game class here.</value>
        public static Game Game {
            get;
            set;
        }
        /// <summary>
        /// Checks if the game is active. Usually this is when the game window has focus.
        /// </summary>
        public static bool IsActive => Game.IsActive && InputText.Text == null;
        /// <summary>
        /// The game window.
        /// </summary>
        public static GameWindow Window => Game.Window;
        /// <summary>
        /// The game window's width.
        /// </summary>
        public static int WindowWidth => Window.ClientBounds.Width;
        /// <summary>
        /// The game window's height.
        /// </summary>
        public static int WindowHeight => Window.ClientBounds.Height;
        /// <summary>
        /// The mouse's previous state.
        /// </summary>
        public static MouseState OldMouse => _oldMouse;
        /// <summary>
        /// The mouse's current state.
        /// </summary>
        public static MouseState NewMouse => _newMouse;
        /// <summary>
        /// The keyboard's previous state.
        /// </summary>
        public static KeyboardState OldKeyboard => _oldKeyboard;
        /// <summary>
        /// The keyboard's current state.
        /// </summary>
        public static KeyboardState NewKeyboard => _newKeyboard;
        /// <summary>
        /// An array with all gamepads' previous states.
        /// </summary>
        public static GamePadState[] OldGamePad => _oldGamePad;
        /// <summary>
        /// An array with all gamepads' current states.
        /// </summary>
        public static GamePadState[] NewGamePad => _newGamepad;
        /// <summary>
        /// An array with all gamepads' info.
        /// </summary>
        public static GamePadCapabilities[] GamePadCapabilities => _gamePadCapabilities;
        /// <summary>
        /// An array with all gamepads' deadzone settings.
        /// </summary>
        public static GamePadDeadZone[] GamePadDeadZone => _gamePadDeadZone;
        /// <summary>
        /// A touch collection that holds the previous and current touch locations.
        /// </summary>
        public static TouchCollection NewTouchCollection => _newTouchCollection;
        /// <summary>
        /// Gives info about a touch panel.
        /// </summary>
        public static TouchPanelCapabilities TouchPanelCapabilities => _touchPanelCapabilities;
        /// <summary>
        /// Maps a MouseButton to a function that can extract a specific ButtonState from a MouseState.
        /// </summary>
        public static Dictionary<MouseButton, Func<MouseState, ButtonState>> MouseButtons => _mouseButtons;
        /// <summary>
        /// Maps a GamePadButton to a function that can extract a specific ButtonState from a GamePadState.
        /// </summary>
        public static Dictionary<GamePadButton, Func<GamePadState[], int, ButtonState>> GamePadButtons => _gamePadButtons;

        // Group: Public Functions

        /// <summary>
        /// Call Setup in the game's LoadContent.
        /// </summary>
        /// <param name="game">Your game object.</param>
        public static void Setup(Game game) {
            Game = game;

            _newMouse = Mouse.GetState();
            _newKeyboard = Keyboard.GetState();
            _touchPanelCapabilities = TouchPanel.GetCapabilities();

            for (int i = 0; i < GamePad.MaximumGamePadCount; i++) {
                _gamePadDeadZone[i] = Microsoft.Xna.Framework.Input.GamePadDeadZone.None;
                _newGamepad[i] = GamePad.GetState(i, _gamePadDeadZone[i]);
                _gamePadCapabilities[i] = GamePad.GetCapabilities(i);
            }

            _newTouchCollection = TouchPanel.GetState();

            //This is boring but whatever, it only gets called once.
            //MonoGame doesn't offer TextInput under some platforms.
            EventInfo eInfo = Window.GetType().GetEvent("TextInput");
            if (eInfo != null) {
                Type handlerType = eInfo.EventHandlerType;
                MethodInfo invokeMethod = handlerType.GetMethod("Invoke");
                ParameterInfo[] parms = invokeMethod.GetParameters();
                Type[] parmTypes = new Type[parms.Length];
                for (int i = 0; i < parms.Length; i++) {
                    parmTypes[i] = parms[i].ParameterType;
                }

                Type processorType = typeof(InputProcessor<>);
                Type genericProcessorType = processorType.MakeGenericType(parmTypes[1]);

                object inputProcessor = genericProcessorType.GetConstructor(Type.EmptyTypes).Invoke(null);

                MethodInfo eventHandler = inputProcessor.GetType().GetMethod("processTextInput");
                Delegate d = Delegate.CreateDelegate(handlerType, eventHandler);
                eInfo.AddEventHandler(Window, d);
            }
        }

        /// <summary>
        /// Call this at the beginning of your update loop.
        /// </summary>
        public static void UpdateSetup() {
            _oldMouse = _newMouse;
            _oldKeyboard = _newKeyboard;
            _newGamepad.CopyTo(_oldGamePad, 0);

            _newMouse = Mouse.GetState();
            _newKeyboard = Keyboard.GetState();

            for (int i = 0; i < GamePad.MaximumGamePadCount; i++) {
                _newGamepad[i] = GamePad.GetState(i, GamePadDeadZone[i]);
                _gamePadCapabilities[i] = GamePad.GetCapabilities(i);
            }

            _newTouchCollection = TouchPanel.GetState();
            _touchPanelCapabilities = TouchPanel.GetCapabilities();
        }

        // Group: Private Functions

        /// <summary>
        /// This class is designed to help with receiving text input events through reflection magic.
        /// </summary>
        private class InputProcessor<T> {
            /// <summary>
            /// This function receives TextInput events from the game window.
            /// </summary>
            /// <param name="sender">This gets ignored.</param>
            /// <param name="e">Contains a character and a key.</param>
            public static void processTextInput(object sender, T e) {
                Type t = e.GetType();
                // For MonoGame versions below 3.8.
                PropertyInfo pk = t.GetProperty("Key");
                PropertyInfo pc = t.GetProperty("Character");

                if (pk != null && pc != null) {
                    _textEvents.Add(new KeyCharacter((Keys)pk.GetValue(e), (char)pc.GetValue(e)));
                } else {
                    // For MonoGame versions 3.8 and above.
                    FieldInfo fk = t.GetField("Key");
                    FieldInfo fc = t.GetField("Character");
                    if (fk != null && fc != null) {
                        _textEvents.Add(new KeyCharacter((Keys)fk.GetValue(e), (char)fc.GetValue(e)));
                    }
                }
            }
        }

        // Group: Private Variables

        /// <summary>
        /// The mouse's previous state.
        /// </summary>
        private static MouseState _oldMouse;
        /// <summary>
        /// The mouse's current state.
        /// </summary>
        private static MouseState _newMouse;
        /// <summary>
        /// The keyboard's previous state.
        /// </summary>
        private static KeyboardState _oldKeyboard;
        /// <summary>
        /// The keyboard's current state.
        /// </summary>
        private static KeyboardState _newKeyboard;
        /// <summary>
        /// An array with all gamepads' previous states.
        /// </summary>
        private static GamePadState[] _oldGamePad = new GamePadState[GamePad.MaximumGamePadCount];
        /// <summary>
        /// An array with all gamepads' current states.
        /// </summary>
        private static GamePadState[] _newGamepad = new GamePadState[GamePad.MaximumGamePadCount];
        /// <summary>
        /// An array with all gamepads' info.
        /// </summary>
        private static GamePadCapabilities[] _gamePadCapabilities = new GamePadCapabilities[GamePad.MaximumGamePadCount];
        /// <summary>
        /// An array with all gamepads' deadzone settings.
        /// </summary>
        private static GamePadDeadZone[] _gamePadDeadZone = new GamePadDeadZone[GamePad.MaximumGamePadCount];
        /// <summary>
        /// A touch collection that holds the previous and current touch locations.
        /// </summary>
        private static TouchCollection _newTouchCollection;
        /// <summary>
        /// Gives info about a touch panel.
        /// </summary>
        private static TouchPanelCapabilities _touchPanelCapabilities;
        /// <summary>
        /// Useful for handling text inputs from any keyboard layouts. This is useful when coding textboxes.
        /// </summary>
        private static List<KeyCharacter> _textEvents = new List<KeyCharacter>();
        private static Dictionary<MouseButton, Func<MouseState, ButtonState>> _mouseButtons = new Dictionary<MouseButton, Func<MouseState, ButtonState>> {
            {MouseButton.LeftButton, s => s.LeftButton},
            {MouseButton.MiddleButton, s => s.MiddleButton},
            {MouseButton.RightButton, s => s.RightButton},
            {MouseButton.XButton1, s => s.XButton1},
            {MouseButton.XButton2, s => s.XButton2},
        };
        private static Dictionary<GamePadButton, Func<GamePadState[], int, ButtonState>> _gamePadButtons = new Dictionary<GamePadButton, Func<GamePadState[], int, ButtonState>> {
            {GamePadButton.A, (s, i) => s[i].Buttons.A},
            {GamePadButton.B, (s, i) => s[i].Buttons.B},
            {GamePadButton.Back, (s, i) => s[i].Buttons.Back},
            {GamePadButton.X, (s, i) => s[i].Buttons.X},
            {GamePadButton.Y, (s, i) => s[i].Buttons.Y},
            {GamePadButton.Start, (s, i) => s[i].Buttons.Start},
            {GamePadButton.LeftShoulder, (s, i) => s[i].Buttons.LeftShoulder},
            {GamePadButton.LeftStick, (s, i) => s[i].Buttons.LeftStick},
            {GamePadButton.RightShoulder, (s, i) => s[i].Buttons.RightShoulder},
            {GamePadButton.RightStick, (s, i) => s[i].Buttons.RightStick},
            {GamePadButton.BigButton, (s, i) => s[i].Buttons.BigButton},
            {GamePadButton.Down, (s, i) => s[i].DPad.Down},
            {GamePadButton.Left, (s, i) => s[i].DPad.Left},
            {GamePadButton.Right, (s, i) => s[i].DPad.Right},
            {GamePadButton.Up, (s, i) => s[i].DPad.Up},
        };

        public static void SetNewKeyboard(KeyboardState state) {
            _newKeyboard = state;
        }
    }
}
