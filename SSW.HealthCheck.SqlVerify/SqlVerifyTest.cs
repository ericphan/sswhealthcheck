namespace SSW.HealthCheck.SqlVerify
{
    using System;
    using System.Collections.Generic;

    using SSW.HealthCheck.Infrastructure;
    using SSW.SqlVerify.Core;

    public class SqlVerifyTest : ITest
    {
        private readonly ISqlVerify sqlVerify;

        private readonly string name = Titles.SqlVerifyTestTitle;

        private readonly string description = Titles.SqlVerifyTestDescription;

        private readonly bool isDefault = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlVerifyTest" /> class.
        /// </summary>
        /// <param name="sqlVerify">The SQL verify.</param>
        public SqlVerifyTest(ISqlVerify sqlVerify)
        {
            if (sqlVerify == null)
            {
                throw new ArgumentNullException("sqlVerify");
            }

            this.sqlVerify = sqlVerify;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlVerifyTest" /> class.
        /// </summary>
        /// <param name="sqlVerify">The SQL verify.</param>
        /// <param name="isDefault">The is default.</param>
        public SqlVerifyTest(ISqlVerify sqlVerify, bool isDefault)
        {
            if (sqlVerify == null)
            {
                throw new ArgumentNullException("sqlVerify");
            }

            this.sqlVerify = sqlVerify;
            this.isDefault = isDefault;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlVerifyTest" /> class.
        /// </summary>
        /// <param name="sqlVerify">The SQL verify.</param>
        /// <param name="isDefault">The is default.</param>
        /// <param name="name">The name.</param>
        public SqlVerifyTest(ISqlVerify sqlVerify, bool isDefault, string name)
        {
            if (sqlVerify == null)
            {
                throw new ArgumentNullException("sqlVerify");
            }

            this.sqlVerify = sqlVerify;
            this.isDefault = isDefault;
            this.name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlVerifyTest" /> class.
        /// </summary>
        /// <param name="sqlVerify">The SQL verify.</param>
        /// <param name="isDefault">if set to <c>true</c> [is default].</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        public SqlVerifyTest(ISqlVerify sqlVerify, bool isDefault, string name, string description)
        {
            if (sqlVerify == null)
            {
                throw new ArgumentNullException("sqlVerify");
            }

            this.sqlVerify = sqlVerify;
            this.isDefault = isDefault;
            this.name = name;
            this.description = description;
        }

        /// <summary>
        /// Gets the name for the test.
        /// </summary>
        /// <value></value>
        public string Name 
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// Gets the description for test.
        /// </summary>
        /// <value></value>
        public string Description 
        {
            get
            {
                return this.description;
            }
        }

        /// <summary>
        /// Gets a value that indicate if the test is to run by default.
        /// </summary>
        /// <value></value>
        public bool IsDefault 
        {
            get
            {
                return this.isDefault;
            }
        }

        /// <summary>
        /// Run the health check test.
        /// </summary>
        /// <param name="context">Test context</param>
        public void Test(ITestContext context)
        { 
            var result = this.sqlVerify.VerifyDb();
            if (!result)
            {
                Assert.Fails(Titles.FailureMessage);
            }
        }

        /// <summary>
        /// Gets or sets the widget actions.
        /// </summary>
        /// <value>The widget actions.</value>
        public IEnumerable<TestAction> TestActions
        {
            get
            {
                return null;
            }
        }
    }
}
