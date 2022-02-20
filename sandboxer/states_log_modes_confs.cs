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
        WARN,
        ERROR
    }

    enum SecurityLevels {
        DEFAULT,
        UNRESTRICTED,
        CUSTOM,
    }

    enum RunningModes {
       CONSOLE,
       INTERACTIVE,
       POWERSHELLVM,
    }
}