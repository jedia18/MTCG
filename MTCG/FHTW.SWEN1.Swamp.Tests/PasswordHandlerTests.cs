using MTCG.Authentication;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG.Tests
{
    internal class PasswordHandlerTests
    {
        [Test]
        public void TestHashPassword()
        {
            string password = "password123";
            string expectedHash = "EF92B778BAFE771E89245B89ECBC08A44A4E166C06659911881F383D4473E94F";

            PasswordHandler hashedPassword = new PasswordHandler();
            string actualHash = hashedPassword.HashPassword(password);

            Assert.That(actualHash, Is.EqualTo(expectedHash));
        }
    }
}
