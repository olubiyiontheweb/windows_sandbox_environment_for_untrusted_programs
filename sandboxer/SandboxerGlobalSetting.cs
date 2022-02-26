using sandboxer.Definitions;

namespace sandboxer
{
    static class SandboxerGlobalSetting
    {
        #region Sandboxer global settings

        // Initializing default settings for the Sanboxer
        // private variables 
        private static int verbose = 0;
        private static RunningModes running_mode = RunningModes.CONSOLE;
        private static LogModes log_mode = LogModes.CONSOLE;
        private static SecurityLevels security_level = SecurityLevels.DEFAULT;
        private static States state;

        // public getter and setter methods
        public static int Verbose
        {
            get { return verbose; }
            set { verbose = value; }
        }
        public static RunningModes RunningMode
        {
            get { return running_mode; }
            set { running_mode = value; }
        }
        public static LogModes LogMode
        {
            get { return log_mode; }
            set { log_mode = value; }
        }
        public static SecurityLevels SecurityLevel
        {
            get { return security_level; }
            set { security_level = value; }
        }
        public static States State
        {
            get { return state; }
            set { state = value; }
        }        

        #endregion
    }
}
