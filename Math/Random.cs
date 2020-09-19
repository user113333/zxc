using Microsoft.Xna.Framework;
using System;

namespace zxc {
    public static class Random {
        private static int _seed = Environment.TickCount;
        public static System.Random random = new System.Random(_seed);

        static public int GetSeed() {
            return _seed;
        }

        static public void SetSeed(int seed) {
            _seed = seed;
            random = new System.Random(_seed);
        }

        static public float NextFloat() {
            return (float)random.NextDouble();
        }

        static public float NextFloat(float max) {
            return (float)random.NextDouble() * max;
        }

        static public int NextInt(int max) {
            return random.Next(max);
        }

        static public float NextAngle() {
            return (float)random.NextDouble() * MathHelper.TwoPi;
        }

        public static Color NextColor() {
            return new Color(NextFloat(), NextFloat(), NextFloat());
        }

        static public int Range(int min, int max) {
            return random.Next(min, max);
        }

        static public float Range(float min, float max) {
            return min + NextFloat(max - min);
        }

        static public Vector2 Range(Vector2 min, Vector2 max) {
            return min + new Vector2(NextFloat(max.X - min.X), NextFloat(max.Y - min.Y));
        }

        static public float MinusOneToOne() {
            return NextFloat(2f) - 1f;
        }

        public static bool Chance(float percent) {
            return NextFloat() < percent;
        }

        public static bool Chance(int value) {
            return NextInt(100) < value;
        }

        public static T Choose<T>(T first, T second) {
            if (NextInt(2) == 0)
                return first;
            return second;
        }

        public static T Choose<T>(T first, T second, T third) {
            switch (NextInt(3)) {
                case 0:
                    return first;
                case 1:
                    return second;
                default:
                    return third;
            }
        }

        public static T Choose<T>(T first, T second, T third, T fourth) {
            switch (NextInt(4)) {
                case 0:
                    return first;
                case 1:
                    return second;
                case 2:
                    return third;
                default:
                    return fourth;
            }
        }
    }
}
