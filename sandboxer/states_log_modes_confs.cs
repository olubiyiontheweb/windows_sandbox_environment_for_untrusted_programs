namespace sandboxer.Definitions
{
    enum States {
        START,
        INIT,
        RUNNING,
        EXIT,
        ERROR
    }

    enum LogModes {
        CONSOLE,
        FILE,
        BOTH
    }

    enum LogLevels {
        DEBUG,
        INFO,
        WARNING,
        ERROR,
        FATAL
    }

    enum SecurityLevels {
        DEFAULT,
        UNRESTRICTED,
        CUSTOM,
    }

    public enum RunningModes {
       CONSOLE,
       POWERSHELLVM,
       INTERACTIVE,
    }
}