using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace zxc {
    public enum RepeatMode {
        Once = 0b00,
        OnceWithReverse = 0b10,
        Loop = 0b01,
        LoopWithReverse = 0b11
    }

    public class Animator : Component {
        private SpriteSheet spriteSheet;
        private List<Animation> animations = new List<Animation>();
        private Color color =  Color.White;
        private float lastFrameTime = 0f;
        private int animationIndex = -1;
        private int fps = 20;

        public Animator(SpriteSheet spriteSheet, int fps = 20) {
            this.spriteSheet = spriteSheet;
        }

        public void SetColor(Color color) {
            this.color = color;
        }

        public void AddAnimation(int start, int end, RepeatMode repeatMode, Func<int> function) {
            animations.Add(new Animation(start, end, repeatMode, function));
        }
        
        public void AddAnimation(int start, int end, RepeatMode repeatMode) {
            animations.Add(new Animation(start, end, repeatMode, null));
        }

        public void AddAnimation(int start, int end) {
            animations.Add(new Animation(start, end, RepeatMode.Loop, null));
        }

        public void Start(int i) {
            lastFrameTime = Time.TimeAll;
            animationIndex = i;
            animations[animationIndex].Start();
        }

        public override void Update() {
            if (Time.TimeAll > lastFrameTime + 1 / fps) {
                lastFrameTime = Time.TimeAll;

                if (animations[animationIndex].NextFrame()) {
                    animationIndex = animations[animationIndex].End();
                }
            }
        }

        public override void Render() {
            if (animationIndex < 0)
                return;

            Core.Batch.Draw(spriteSheet.Texture, Entity.Position, spriteSheet.Frames[animations[animationIndex].GetFrame()].SourceRectangle, color, Entity.Rotation, Entity.Origin, Entity.Scale, Entity.Effect, 0);
        }
    }

    public class Animation {
        private Tuple<int, int> pair;
        private RepeatMode repeatMode;
        private Func<int> onEnd;
        private int frameIndex;

        public Animation(int start, int end, RepeatMode repeatMode, Func<int> function) {
            pair = new Tuple<int, int>(start, end);
            this.repeatMode = repeatMode;
            onEnd = function;
        }

        public void Start() {
            frameIndex = pair.Item1;
        }

        public int End() {
            if (onEnd != null)
                return onEnd.Invoke();

            return -1;
        }

        public bool NextFrame() {
            if (frameIndex == pair.Item2) {
                if (((int)repeatMode & 0b01) == 0) {
                    return true; // returns if animation is completed
                } else {
                    frameIndex = pair.Item1;
                    return false;
                }
            }

            frameIndex++;
            return false;
        }

        public int GetFrame() {
            if (((int)repeatMode & 0b10) == 0)
                return frameIndex;
            else
                return pair.Item2 - (pair.Item1 - frameIndex);
        }
    }
}
