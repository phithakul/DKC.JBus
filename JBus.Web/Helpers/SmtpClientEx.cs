using System;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Reflection;

namespace JBus.Web.Helpers
{
    // The current implementation of the System.Net.Mail.SmtpClient
    // uses the NetBIOS name of the computer in the HELO / EHLO commands.
    // Many anti-spam systems require the FQDN instead.
    // As a result, email sent with the SmtpClient class is often blocked.

    /// <summary>
    /// An extended <see cref="SmtpClient"/> which sends the
    /// FQDN of the local machine in the EHLO/HELO command.
    /// </summary>
    internal class SmtpClientEx : SmtpClient
    {
        private static readonly FieldInfo localHostName = GetLocalHostNameField();

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpClientEx"/> class
        /// that sends e-mail by using the specified SMTP server and port.
        /// </summary>
        /// <param name="host">
        /// A <see cref="String"/> that contains the name or
        /// IP address of the host used for SMTP transactions.
        /// </param>
        /// <param name="port">
        /// An <see cref="Int32"/> greater than zero that
        /// contains the port to be used on host.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="port"/> cannot be less than zero.
        /// </exception>
        public SmtpClientEx(string host, int port)
            : base(host, port)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpClientEx"/> class
        /// that sends e-mail by using the specified SMTP server.
        /// </summary>
        /// <param name="host">
        /// A <see cref="String"/> that contains the name or
        /// IP address of the host used for SMTP transactions.
        /// </param>
        public SmtpClientEx(string host)
            : base(host)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpClientEx"/> class
        /// by using configuration file settings.
        /// </summary>
        public SmtpClientEx()
        {
            Initialize();
        }

        /// <summary>
        /// Gets or sets the local host name used in SMTP transactions.
        /// </summary>
        /// <value>
        /// The local host name used in SMTP transactions.
        /// This should be the FQDN of the local machine.
        /// </value>
        /// <exception cref="ArgumentNullException">
        /// The property is set to a value which is
        /// <see langword="null"/> or <see cref=""""/>.
        /// </exception>
        public string LocalHostName
        {
            get
            {
                if (null == localHostName) return null;
                return (string)localHostName.GetValue(this);
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }
                if (null != localHostName)
                {
                    localHostName.SetValue(this, value);
                }
            }
        }

        /// <summary>
        /// Returns the price "localHostName" field.
        /// </summary>
        /// <returns>
        /// The <see cref="FieldInfo"/> for the private
        /// "localHostName" field.
        /// </returns>
        //private static FieldInfo GetLocalHostNameField()
        //{
        //    BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
        //    return typeof(SmtpClient).GetField("localHostName", flags);
        //}

        // The "localHostName" field was changed to "clientDomain" in .NET 2.0.50727.4927;
        // .NET 4.0 still uses the same field. Since this problem was *NOT* fixed in .NET 4.0 RTM as advised,
        // the previous workaround is still required. To make it work with the later versions,
        // the GetLocalHostNameField method should be changed to:
        private static FieldInfo GetLocalHostNameField()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            FieldInfo result = typeof(SmtpClient).GetField("clientDomain", flags);
            if (null == result) result = typeof(SmtpClient).GetField("localHostName", flags);
            return result;
        }

        /// <summary>
        /// Initializes the local host name to
        /// the FQDN of the local machine.
        /// </summary>
        private void Initialize()
        {
            IPGlobalProperties ip = IPGlobalProperties.GetIPGlobalProperties();
            if (!string.IsNullOrEmpty(ip.HostName) && !string.IsNullOrEmpty(ip.DomainName))
            {
                this.LocalHostName = ip.HostName + "." + ip.DomainName;
            }
        }
    }
}