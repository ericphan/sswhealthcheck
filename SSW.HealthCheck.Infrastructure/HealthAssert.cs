namespace SSW.HealthCheck.Infrastructure
{
    public static class Assert
    {
        /// <summary>
        /// Fails the test.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Fails(string message)
        {
            throw new HealthException(message);
        }

        /// <summary>
        /// Passes with warning current test.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void PassWithWarning(string message)
        {
            throw new PassedWithWarningException(message);
        }

        /// <summary>
        /// Fails the test.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static void Fails(string format, params object[] args)
        {
            Fails(string.Format(format, args));
        }

        /// <summary>
        /// Passes with warning current test.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static void PassWithWarning(string format, params object[] args)
        {
            PassWithWarning(string.Format(format, args));
        }
    }
}
