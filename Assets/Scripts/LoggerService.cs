using System;

[System.Serializable]
public static class LoggerService
{
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error,
        Critical
    }

    public static LogLevel CurrentLogLevel = LogLevel.Critical;
    public static bool EnableLogging = true;
    public static event Action<string, LogLevel> OnLogMessage;

    public static void Log(object message, LogLevel level = LogLevel.Info, UnityEngine.Object context = null)
    {
        if (!EnableLogging || level < CurrentLogLevel) return;
        string formattedMessage = $"[{DateTime.Now:HH:mm:ss}] [{level}] {message}";

        // Вызываем Unity лог
        switch (level)
        {
            case LogLevel.Debug:
            case LogLevel.Info:
                UnityEngine.Debug.Log(formattedMessage, context);
                break;
            case LogLevel.Warning:
                UnityEngine.Debug.LogWarning(formattedMessage, context);
                break;
            case LogLevel.Error:
            case LogLevel.Critical:
                UnityEngine.Debug.LogError(formattedMessage, context);
                break;
        }

        // Вызываем событие для подписчиков
        OnLogMessage?.Invoke(formattedMessage, level);
    }

    // Вспомогательные методы
    public static void Debug(object message, UnityEngine.Object context = null)
        => Log("[DEBUG] " + message, LogLevel.Debug, context);

    public static void Info(object message, UnityEngine.Object context = null)
        => Log("[INFO]" + message, LogLevel.Info, context);

    public static void Warning(object message, UnityEngine.Object context = null)
        => Log("[WARNING]" + message, LogLevel.Warning, context);

    public static void Error(object message, UnityEngine.Object context = null)
        => Log("[ERROR]" + message, LogLevel.Error, context);

    public static void Critical(object message, UnityEngine.Object context = null)
        => Log("[CRITICAL]" + message, LogLevel.Critical, context);
}