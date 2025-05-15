using StardewModdingAPI;

namespace UsefulPets.Util
{
    internal static class LogHelper
    {
        private const string Tag = "[UsefulPets]";

        public static void Debug(IMonitor monitor, string context, string message) =>
            monitor.Log($"{Tag} [{context}] {message}", LogLevel.Debug);

        public static void Info(IMonitor monitor, string context, string message) =>
            monitor.Log($"{Tag} [{context}] {message}", LogLevel.Info);

        public static void Warn(IMonitor monitor, string context, string message) =>
            monitor.Log($"{Tag} [{context}] {message}", LogLevel.Warn);

        public static void Error(IMonitor monitor, string context, string message) =>
            monitor.Log($"{Tag} [{context}] {message}", LogLevel.Error);
    }
}