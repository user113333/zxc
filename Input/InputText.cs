using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace zxc {
    public static class InputText {
        public static StringBuilder Text = null;
		public static int TextInputCursorPosition = 0;

		public static void WindowEvent(object sender, TextInputEventArgs e) {
			if (Text == null)
				return;

			switch ((int)e.Character) {
				case 8:
					TextInputDelete(false, false);
					return;

				case 127:
					TextInputDelete(true, false);
					return;

				case 9:
					return;
			}

			if (char.IsControl(e.Character))
				return;

			if (Text.Length >= Text.MaxCapacity)
				return;

			Text.Insert(TextInputCursorPosition, e.Character);
			TextInputCursorPosition++;
		}

		public static void TextInputUpdate() {
			if (Input.IsKeyPressed(Keys.W) && Input.IsKeyDown(Keys.LeftControl)) {
				TextInputDelete(false, true);
			}

			if (Input.IsKeyPressed(Keys.End)) {
                TextInputCursorPosition = Text.Length;
            }

            if (Input.IsKeyPressed(Keys.Home)) {
                TextInputCursorPosition = 0;
            }
            
            if (Input.IsKeyPressed(Keys.Left)) {
                TextInputMoveCursor(false, Input.IsKeyDown(Keys.LeftControl));
            }

            if (Input.IsKeyPressed(Keys.Right)) {
                TextInputMoveCursor(true, Input.IsKeyDown(Keys.LeftControl));
            }
		}

		public static void TextInputClose() {
			Text = null;
			TextInputCursorPosition = 0;
		}

		public static void TextInputMoveCursor(bool right, bool fullWord) {
			int shift = right ? 1 : -1;
			var str = Text.ToString();

			if (fullWord) {
				if (TextInputCursorPosition + shift >= 0 && TextInputCursorPosition + shift <= str.Length - 1 && shift == -1) {
					TextInputCursorPosition += shift;
				}

				while (TextInputCursorPosition + shift >= 0 && TextInputCursorPosition + shift <= str.Length - 1 && str[TextInputCursorPosition + shift] != ' ') {
					TextInputCursorPosition += shift;
				}

				if (TextInputCursorPosition + shift >= 0 && TextInputCursorPosition + shift <= str.Length && shift == 1) {
					TextInputCursorPosition += shift;
				}
			} else {
				TextInputCursorPosition += shift;

				if (TextInputCursorPosition < 0)
					TextInputCursorPosition = 0;

				if (TextInputCursorPosition > str.Length)
					TextInputCursorPosition = str.Length;
			}
		}

		public static void TextInputDelete(bool delete, bool fullWord) {
			var str = Text.ToString();
			if (delete) {
				if (TextInputCursorPosition >= str.Length)
					return;

				Text.Remove(TextInputCursorPosition, 1);
				return;
			}

			if (TextInputCursorPosition == 0)
				return;

			if (fullWord) {
				Text.Remove(--TextInputCursorPosition, 1);

				while (TextInputCursorPosition - 1 >= 0 && TextInputCursorPosition - 1 <= str.Length - 1 && str[TextInputCursorPosition - 1] != ' ') {
					Text.Remove(--TextInputCursorPosition, 1);
				}
			} else {
				Text.Remove(--TextInputCursorPosition, 1);
			}
		}
    }
}
