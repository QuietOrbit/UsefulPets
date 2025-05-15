using System;
using StardewModdingAPI;

namespace UsefulPets.Util
{
    /// <summary>
    /// Utility for safely executing mod logic with fallback behavior and logging support.
    /// Used to prevent mod errors from affecting the game's core logic.
    /// </summary>
    internal static class SafeHelper
    {
        /// <summary>
        /// Executes an action safely. If an exception occurs, it is caught and logged, and the game continues normally.
        /// </summary>
        /// <param name="monitor">The SMAPI monitor used to log errors.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="context">A description of the context or caller for logging purposes.</param>
        public static void Run(IMonitor monitor, Action action, string context)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                LogHelper.Warn(monitor, "SafeHelper", $"Error during {context}: {ex}");
            }
        }

        /// <summary>
        /// Executes a function safely and returns a fallback value if an exception occurs.
        /// </summary>
        /// <typeparam name="T">The return type of the function.</typeparam>
        /// <param name="monitor">The SMAPI monitor used to log errors.</param>
        /// <param name="func">The function to evaluate.</param>
        /// <param name="context">A description of the context or caller for logging purposes.</param>
        /// <param name="fallback">The fallback value to return if the function fails.</param>
        /// <returns>The result of <paramref name="func"/> or the fallback value on error.</returns>
        public static T Run<T>(IMonitor monitor, Func<T> func, string context, T fallback)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                LogHelper.Warn(monitor, "SafeHelper", $"Error during {context}: {ex}");
                return fallback;
            }
        }

        /// <summary>
        /// Executes a function safely and returns true on failure.
        /// Commonly used in Harmony prefix patches to allow game behavior to continue.
        /// </summary>
        public static bool RunTrue(IMonitor monitor, Func<bool> func, string context) =>
            Run(monitor, func, context, true);

        /// <summary>
        /// Executes a function safely and returns false on failure.
        /// </summary>
        public static bool RunFalse(IMonitor monitor, Func<bool> func, string context) =>
            Run(monitor, func, context, false);
    }
}
