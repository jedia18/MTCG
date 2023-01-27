using NUnit.Framework;
using MTCG;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection.Metadata;

namespace MTCG.Tests
{
    internal class StringHandlerTests
    {
        StringHandler str = new StringHandler();
        // thest the DivideString method
        [Test]
        public void DivideString_InputIsValid_ReturnsExpectedResults()
        {
            // Arrange
            string input = "HelloWorld";

            // Act
            string[] result = str.DivideString(input);

            // Assert
            Assert.That(result.Length, Is.EqualTo(2));
            Assert.That(result[0], Is.EqualTo("Hello"));
            Assert.That(result[1], Is.EqualTo("World"));
        }

        // test the TokenDistinguisher method
        [Test]
        public void TokenDistinguisher_Removes_Basic_Prefix()
        {
            // Arrange
            string input = "Basic kienboec-mtcgToken";
            string expected = "kienboec-mtcgToken";

            // Act
            string actual = str.TokenDistinguisher(input);

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        // test the TokenDistinguisher method
        [Test]
        public void UserDistinguisher_Removes_Constant_Strings()
        {
            // Arrange
            string input = "Basic kienboec-mtcgToken";
            string expected = "kienboec";

            // Act
            string actual = str.UserDistinguisher(input);

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }


    }
}
