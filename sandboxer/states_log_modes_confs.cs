namespace sandboxer {
    enum States {
        START,
        INIT,
        RUNNING,
        STOPPED,
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
        SECURED,
        DEBUG
    }

    enum RunningModes {
    CONSOLE,
    INTERACTIVE,
    }
}