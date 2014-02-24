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
        /// Fails the test.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static void Fails(string format, params object[] args)
        {
            Fails(string.Format(format, args));
        }
    }
}
