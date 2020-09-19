namespace zxc {
    /// <summary>
    /// Combines a bunch of ICondition. All conditions must be true for this to trigger.
    /// </summary>
    /// <see cref="AnyCondition"/>
    public class AllCondition : ICondition {

        // Group: Constructors

        /// <summary>
        /// AllCondition with initial needed conditions or empty.
        /// </summary>
        public AllCondition(params ICondition[] conditions) {
            _conditions = conditions;
        }

        // Group: Public Functions

        /// <returns>
        /// Returns true when all the needed conditions are Down and at least one triggers as pressed.
        /// </returns>
        public bool Pressed() {
            bool pressed = false;
            bool Down = true;

            foreach (ICondition c in _conditions) {
                pressed = pressed || c.Pressed();
                if (pressed) {
                    break;
                }
            }
            foreach (ICondition c in _conditions) {
                Down = Down && c.Down();
                if (!Down) {
                    break;
                }
            }

            return pressed && Down;
        }
        /// <returns>
        /// Returns true when all the needed conditions are Down.
        /// </returns>
        public bool Down() {
            bool Down = true;

            foreach (ICondition c in _conditions) {
                Down = Down && c.Down();
                if (!Down) {
                    break;
                }
            }

            return Down;
        }
        /// <returns>
        /// Returns true when all the needed conditions were Down and are now Down.
        /// </returns>
        public bool DownOnly() {
            bool Down = true;

            foreach (ICondition c in _conditions) {
                Down = Down && c.DownOnly();
                if (!Down) {
                    break;
                }
            }

            return Down;
        }
        /// <returns>
        /// Returns true when at least one needed condition is released and the other needed conditions are Down.
        /// </returns>
        public bool Released() {
            bool released = false;
            bool Down = true;

            foreach (ICondition c in _conditions) {
                released = released || c.Released();
                if (released) {
                    break;
                }
            }
            foreach (ICondition c in _conditions) {
                Down = Down && (c.Down() || c.Released());
                if (!Down) {
                    break;
                }
            }

            return released && Down;
        }

        // Group: Private Variables

        /// <summary>
        /// An array of ICondition.
        /// </summary>
        private ICondition[] _conditions;
    }
}
