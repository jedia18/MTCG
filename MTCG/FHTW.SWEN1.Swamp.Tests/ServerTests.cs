using Moq;
using NUnit.Framework;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;



namespace MTCG.Tests
{
    public class ServerTests
    {
        /// <summary>Tests HttpHeader</summary>
        [Test]
        public void HttpHeader_Constructor_ParsesNameAndValue()
        {
            // Arrange
            string header = "Content-Type: application/json";

            // Act
            var httpHeader = new HttpHeader(header);

            // Assert
            Assert.That(httpHeader.Name, Is.EqualTo("Content-Type"));
            Assert.That(httpHeader.Value, Is.EqualTo("application/json"));
        }


        /// <summary>Tests event argument class HttpSvrEventArgs.</summary>
        [Test]
        public void TestEventArgs()
        {
            HttpSvrEventArgs args = new HttpSvrEventArgs("POST /users HTTP/1.1\r\nHost: localhost:10001\r\nUser-Agent: curl/7.83.1\r\nAccept: */*\r\nContent-Type: text/plain\r\nContent-Length: 0", null);

            Assert.That(args.Method, Is.EqualTo("POST"));
            Assert.That(args.Path, Is.EqualTo("/users"));
            Assert.That(args.Headers[0].Value, Is.EqualTo("localhost:10001"));
        }
    }
}