using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace zxc {
    public class Terminal {
        public static KeyboardCondition BtnOpen = new KeyboardCondition(Keys.Enter);
        private const int MaxCharacters = 255;
        private const int MaxMessages = 50;
        
        public float Margin = 25; // margin from window
        public float Padding = 5; // spacing between messages
        public float BottomPadding = 5; // spacing from bottom
        public float FontHeight = 24; // px
        public float MessageHideTime = 20; // seconds
        public bool CursorBlink = true;
        public float CursorBlinkSpeed = 2;
        public string PreText = "";
        public string Prefix = "/";
        public Message[] Messages = new Message[MaxMessages];

        private static Dictionary<string, Func<string[], string>> funcs = new Dictionary<string, Func<string[], string>>();

        private Font font;
        private string sender;
        private bool active = false;
        private int indexMessages = 0;
        private float cursorBlinkOffset = 0;
        private StringBuilder currentMessage = new StringBuilder(MaxCharacters, MaxCharacters);

        public Terminal(Font font, string sender) {
            this.font = font;
            this.sender = sender;

            Terminal.AddFunction("clear", (args) => { CLS(); return null; });
            Terminal.AddFunction("help", (args) => {
                string str = "";

                foreach(var val in funcs) {
                    str += val.Key + " ";
                }

                return str;
            });
        }

        public void Update() {
            if (!active) {
                if (BtnOpen.Pressed())
                    Show(Input.IsKeyDown(Keys.LeftShift) || Input.IsKeyDown(Keys.RightShift));
                    
                return;
            }

            InputText.TextInputUpdate();

            if (Input.IsKeyPressed(Keys.Enter)) {
                string str = currentMessage.ToString();
                PushMessage(str, sender, 1);
                Hide();
            }

            if (Input.IsKeyPressed(Keys.Escape)) {
                Hide();
            }
        }

        public void Render(int virtualWidthUI, int virtualHeightUI) {
            // TODO: REWORK TERMINAL.cs & MESSAGE.cs
            // TODO: Hide over time
            // TODO: Limit chats width where if exceeds it breaks the line
            int startIndex = Messages.Length - indexMessages;
            float scale = FontHeight / font.YSmoothing;
            for (int i = Messages.Length - 1; i >= 0; i--) {
                if (Messages[i] == null)
                    continue;

                int index = Messages.Length - ((i+startIndex) % Messages.Length) - 1;

                Vector2 pos = CalcPos(index + 1);
                font.DrawText(Messages[i].ToString(), pos, scale, Color.White);
            }

            if (active) {
                Vector2 pos = CalcPos(0);
                float fontMid = (FontHeight / 5);
                
                // Draw BackGround
                Core.Batch.DrawRect(Margin / 2, pos.Y, virtualWidthUI - Margin, FontHeight + fontMid + fontMid, Util.Black50);
                
                string str = PreText + currentMessage.ToString();
                font.DrawText(str, pos, scale, Color.White);

                // Draw cursor
                if ((((Time.TimeAll + cursorBlinkOffset) * CursorBlinkSpeed)) % 2 < 1 && CursorBlink) {
                    float x = font.GetTextWidth(str, InputText.TextInputCursorPosition + PreText.Length) * scale;
                    Core.Batch.DrawRect(pos.X + x, pos.Y + fontMid, 1, FontHeight, Color.White);
                }
            }

            Vector2 CalcPos(int index) {
                index++;
                return new Vector2(Margin, virtualHeightUI - ((index) * FontHeight + (index - 1) * Padding) - Margin - BottomPadding);
            }
        }

        public static void AddFunction(string identifier, Func<string[], string> func) {
            funcs[identifier] = func;
        }

        public void CLS() {
            for (int i = 0; i < Messages.Length; i++) {
                Messages[i] = null;
            }
        }

        private void Show(bool commandMode) {
            if (commandMode) {
                currentMessage.Append(Prefix);
                InputText.TextInputCursorPosition = Prefix.Length;
            }

            cursorBlinkOffset = -(Time.TimeAll % 2);
            InputText.Text = currentMessage;
            active = true;
        }

        private void Hide() {
            InputText.TextInputClose();
            currentMessage.Clear();
            active = false;
        }

        private void PushMessage(string text, string author, int mode) {
            if (text == "")
                return;

            Messages[indexMessages] = new Message(author, text, mode);
            
            indexMessages++;
            if (indexMessages == Messages.Length) {
                indexMessages = 0;
            }

            if (text.IndexOf(Prefix) != 0)
                return;

            string output = "";
            string[] arr = text.Split(' ');
            string command = arr[0].Substring(Prefix.Length, arr[0].Length - Prefix.Length);
            if (funcs.TryGetValue(command, out Func<string[], string> func)) {
                output = func.Invoke(arr.Skip(1).ToArray());
            } else if (Prefix.Length > 0) {
                output = $"No command with the name '{command}' found";
            }

            if (output != null)
                PushMessage(output, "HAL", 0);
        }
    }
}
