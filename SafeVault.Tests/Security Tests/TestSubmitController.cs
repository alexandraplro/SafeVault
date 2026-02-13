using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using SafeVault.Web.Controllers;
using SafeVault.Web.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Helpers;

namespace SafeVault.Tests.SecurityTests
{
    public class FakeUserRepository : IUserRepository
    {
        public void InsertUser(string username, string email)
        {
            // Do nothing — simulate success
        }
    }

    public class TestSubmitController
    {
        [Test]
        public void Submit_ShouldEncode_XSSPayload()
        {
            var repo = new FakeUserRepository();
            var controller = new SubmitController(repo);

            var result = controller.Submit("validuser", "evil<script>alert(1)</script>@test.com")
                as OkObjectResult;

            Assert.That(result, Is.Not.Null);

            var json = result!.Value!.ToString();

            Assert.That(json, Does.Not.Contain("<script>"));
            Assert.That(json, Does.Not.Contain("</script>"));
        }
    }
}
