using Microsoft.Xna.Framework;

namespace zxc {
    public static class Time {
        public static float TimeAll;
        public static float UnscaledTimeAll;
        public static float DeltaTime;
        public static float UnscaledDeltaTime;
        public static float TimeScale = 1f;
        public static uint FrameCount = 0;

        internal static void Update(float dt) {
            DeltaTime = dt * TimeScale;
            UnscaledDeltaTime = dt;
            TimeAll += DeltaTime;
            UnscaledTimeAll += UnscaledDeltaTime;
            FrameCount++;
        }
    }
}
