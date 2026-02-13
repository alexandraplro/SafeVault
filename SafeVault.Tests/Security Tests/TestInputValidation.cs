using NUnit.Framework;
using SafeVault.Web.Security;
using System;

namespace SafeVault.Tests.SecurityTests
{
    [TestFixture]
    public class TestInputValidation
    {
        [Test]
        public void TestForSQLInjection()
        {
            var malicious = "Robert'); DROP TABLE Users;--";

            Assert.Throws<ArgumentException>(() =>
            {
                InputValidator.ValidateUsername(malicious);
            });
        }

        [Test]
        public void TestForXSS()
        {
            var malicious = "<script>alert('xss');</script>";

            var encoded = InputValidator.EncodeForHtml(malicious);

            Assert.That(encoded, Does.Not.Contain("<script>"));
            Assert.That(encoded, Does.Not.Contain("</script>"));
            Assert.That(encoded, Does.Contain("&lt;script&gt;"));
        }
    }
}
