namespace SSW.HealthCheck.SqlVerify
{
    using System;

    using SSW.HealthCheck.Infrastructure;
    using SSW.HealthCheck.Infrastructure.Tests;
    using SSW.SqlVerify.Core;

    /// <summary>
    /// Custom verification test for Health check UI
    /// </summary>
    public class SqlVerifyTest : GenericTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlVerifyTest"/> class.
        /// </summary>
        /// <param name="sqlVerify">The SQL verify.</param>
        /// <param name="isDefault">if set to <c>true</c> [is default].</param>
        public SqlVerifyTest(ISqlVerify sqlVerify, bool isDefault = false) 
            : base(Titles.SqlVerifyTestTitle, Titles.SqlVerifyTestDescription, isDefault, null)
        {
            if (sqlVerify == null)
            {
                throw new ArgumentNullException("sqlVerify");
            }

            this.Method = testContext =>
                {
                    var result = sqlVerify.VerifyDb();
                    if (!result)
                    {
                        Assert.Fails(Titles.FailureMessage);
                    }
                };
        }
    }
}
