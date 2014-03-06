namespace SSW.HealthCheck.Infrastructure.Tests
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Net.Configuration;
    using System.Net.Sockets;
    using System.Text;

    using SSW.HealthCheck.Infrastructure;

    /// <summary>
    /// The smtp test. Checks if smtp is reachable
    /// </summary>
    public class SmtpTest : ITest
    {
        /// <summary>
        /// Helper class to test SMTP connection. 
        /// from http://stackoverflow.com/questions/372742/can-i-test-smtpclient-before-calling-client-send
        /// </summary>
        public static class SmtpHelper
        {
            /// <summary>
            /// test the smtp connection by sending a HELO command
            /// </summary>
            /// <param name="mailSettings"></param>
            /// <returns>Result of the testing connection</returns>
            public static bool TestConnection(MailSettingsSectionGroup mailSettings)
            {
                if (mailSettings == null)
                {
                    throw new ConfigurationErrorsException(Labels.CouldNotReadSmtpSettings);
                }

                return TestConnection(mailSettings.Smtp.Network.Host, mailSettings.Smtp.Network.Port);
            }

            /// <summary>
            /// Test the smtp connection by sending a HELO command
            /// </summary>
            /// <param name="smtpServerAddress"></param>
            /// <param name="port"></param>
            public static bool TestConnection(string smtpServerAddress, int port)
            {
                var hostEntry = Dns.GetHostEntry(smtpServerAddress);
                var endPoint = new IPEndPoint(hostEntry.AddressList[0], port);
                using (var tcpSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
                {
                    // try to connect and test the rsponse for code 220 = success
                    tcpSocket.Connect(endPoint);
                    if (!CheckResponse(tcpSocket, 220))
                    {
                        return false;
                    }

                    // send HELO and test the response for code 250 = proper response
                    SendData(tcpSocket, string.Format("HELO {0}\r\n", Dns.GetHostName()));
                    if (!CheckResponse(tcpSocket, 250))
                    {
                        return false;
                    }

                    // if we got here it's that we can connect to the smtp server
                    return true;
                }
            }

            private static void SendData(Socket socket, string data)
            {
                byte[] dataArray = Encoding.ASCII.GetBytes(data);
                socket.Send(dataArray, 0, dataArray.Length, SocketFlags.None);
            }

            private static bool CheckResponse(Socket socket, int expectedCode)
            {
                while (socket.Available == 0)
                {
                    System.Threading.Thread.Sleep(100);
                }

                var responseArray = new byte[1024];
                socket.Receive(responseArray, 0, socket.Available, SocketFlags.None);
                string responseData = Encoding.ASCII.GetString(responseArray);
                int responseCode = Convert.ToInt32(responseData.Substring(0, 3));
                if (responseCode == expectedCode)
                {
                    return true;
                }

                return false;
            }
        }

        private readonly string name = Labels.SmtpTestTitle;
        private readonly string description = Labels.SmtpTestDescription;
        private readonly bool isDefault = true;
        private int order;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpTest" /> class.
        /// </summary>
        /// <param name="order">The order in which test will appear in the list.</param>
        public SmtpTest(int order)
        {
            this.Order = order;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpTest" /> class.
        /// </summary>
        /// <param name="name">The test name.</param>
        /// <param name="order">The order in which test will appear in the list.</param>
        public SmtpTest(string name, int order)
        {
            this.name = name;
            this.Order = order;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpTest" /> class.
        /// </summary>
        /// <param name="name">The test name.</param>
        /// <param name="description">The test description.</param>
        /// <param name="order">The order in which test will appear in the list.</param>
        public SmtpTest(string name, string description, int order)
        {
            this.name = name;
            this.description = description;
            this.Order = order;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpTest" /> class.
        /// </summary>
        /// <param name="isDefault">Run test by default.</param>
        /// <param name="order">The order in which test will appear in the list.</param>
        public SmtpTest(bool isDefault, int order)
        {
            this.isDefault = isDefault;
            this.Order = order;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpTest" /> class.
        /// </summary>
        /// <param name="name">The test name.</param>
        /// <param name="description">The test description.</param>
        /// <param name="isDefault">
        /// Flag indicating if test will be run when page loads. 
        /// True - test will run everytime page is loaded, False - test will be triggered manually by user
        /// </param>
        /// <param name="order">The order in which test will appear in the list.</param>
        public SmtpTest(string name, string description, bool isDefault, int order)
        {
            this.name = name;
            this.description = description;
            this.isDefault = isDefault;
            this.Order = order;
        }

        /// <summary>
        /// Gets the description for test.
        /// </summary>
        /// <value></value>
        public string Description
        {
            get { return this.description; }
        }

        /// <summary>
        /// Gets or sets the order in which test appears.
        /// </summary>
        /// <value>The order.</value>
        public int Order
        {
            get
            {
                return this.order;
            }

            set
            {
                this.order = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the test is to run by default.
        /// </summary>
        /// <value>The is default.</value>
        public bool IsDefault
        {
            get { return this.isDefault; }
        }

        /// <summary>
        /// Gets the name for the test.
        /// </summary>
        /// <value></value>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Run the health check.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Test(ITestContext context)
        {
            var mailSettings = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~")
                .GetSectionGroup(@"system.net/mailSettings") as MailSettingsSectionGroup;

            try
            {
                if (!SmtpHelper.TestConnection(mailSettings))
                {
                    Assert.Fails(Labels.SmtpTestFailure);
                }
            }
            catch (Exception ex)
            {
                Assert.Fails(Labels.SmtpTestException, ex.Message);
            }
        }
    }
}