using System;

namespace zxc {
    public class Message {
        public string Author;
        public string Text;
        public string Date;
        public float Time;
        public int Mode;

        public Message(string author, string text, int mode) {
            Author = author;
            Text = text;
            Mode = mode;
            Date = DateTime.Now.ToShortTimeString();
            Time = zxc.Time.TimeAll;
        }

        public Message(string author, string text, int mode, string date) {
            Author = author;
            Text = text;
            Mode = mode;
            Date = date;
            Time = zxc.Time.TimeAll;
        }

        public override string ToString() {
            switch (Mode) {
                case 1: return "[" + (Author.Length == 0 ? "user" : Author) + "] " + Text;
                default: return Text;
            }
        }
    }
}
