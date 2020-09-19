namespace zxc {
    /// <summary>
    /// Combines a bunch of ICondition in order to trigger when at least one is true.
    /// </summary>
    /// <see cref="AllCondition"/>
    public class AnyCondition : ICondition {

        // Group: Constructors

        /// <summary>
        /// AnyCondition with initial ICondition array or empty.
        /// </summary>
        /// <param name="conditions">An array of ICondition.</param>
        public AnyCondition(params ICondition[] conditions) {
            _conditions = conditions;
        }

        // Group: Public Functions

        /// <returns>
        /// Returns true when at least one condition triggers as pressed.
        /// </returns>
        public bool Pressed() {
            bool pressed = false;
            foreach (ICondition cs in _conditions) {
                pressed = cs.Pressed();
                if (pressed) {
                    break;
                }
            }
            return pressed;
        }
        /// <returns>
        /// Returns true when at least one condition triggers as Down.
        /// </returns>
        public bool Down() {
            bool Down = false;
            foreach (ICondition cs in _conditions) {
                Down = cs.Down();
                if (Down) {
                    break;
                }
            }
            return Down;
        }
        /// <returns>
        /// Returns true when at least one condition triggers as Down only.
        /// </returns>
        public bool DownOnly() {
            bool DownOnly = false;
            foreach (ICondition cs in _conditions) {
                DownOnly = cs.DownOnly();
                if (DownOnly) {
                    break;
                }
            }
            return DownOnly;
        }
        /// <returns>
        /// Returns true when at least one condition triggers as released.
        /// </returns>
        public bool Released() {
            bool released = false;
            foreach (ICondition cs in _conditions) {
                released = cs.Released();
                if (released) {
                    break;
                }
            }
            return released;
        }

        // Group: Private Variables

        /// <summary>
        /// An array of ICondition.
        /// </summary>
        private ICondition[] _conditions;
    }
}
