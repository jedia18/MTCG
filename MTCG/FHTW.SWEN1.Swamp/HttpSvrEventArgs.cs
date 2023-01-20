using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Numerics;
using System.Text;



namespace MTCG
{
    /// <summary>This class provides event arguments for an HTTP server.</summary>
    public class HttpSvrEventArgs: EventArgs
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private members                                                                                          //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>TCP client.</summary>
        private TcpClient _Client;



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                             //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Creates a new instance of this class.</summary>
        public HttpSvrEventArgs()
        {}


        /// <summary>Creates a new instance of this class.</summary>
        /// <param name="tcp">HTTP message received from TCP listener.</param>
        public HttpSvrEventArgs(string tcp, TcpClient client)
        {
            _Client = client;
            PlainMessage = tcp;

            //It replaces all the line breaks of the input string(tcp) with the "\n" character, regardless of 
            //whether they were represented as "\r\n", "\r" or some other way.
            //then, the constructor splits the resulting string into an array of strings, where each element of 
            //the array represents one line of the original string, using the Split("\n") method.
            string[] lines = tcp.Replace("\r\n", "\n").Replace("\r", "\n").Split("\n");
            bool inheaders = true;
            List<HttpHeader> headers = new List<HttpHeader>();

            //It then iterates through each line of the resulting array, and if the current line is the first line, it 
            //extracts the method and path from it.If the current line is not the first line and headers haven't been 
            //read yet, it adds the current line as a new HttpHeader object to the headers list. Finally, if headers have 
            //been read, it appends the current line to the payload.
            //Once the iteration is complete, it assigns the headers list to the Headers property.
            for (int i = 0; i < lines.Length; i++)
            {
                if(i == 0)
                {
                    string[] inq = lines[0].Split(" ");
                    Method = inq[0];
                    Console.WriteLine("************" + Method + "****************");
                    Path = inq[1];
                    Console.WriteLine("--------------" + Path + "----------------");
                }
                else if(inheaders)
                {
                    if(string.IsNullOrWhiteSpace(lines[i]))
                    {
                        inheaders = false;
                    }
                    else 
                    { 
                        headers.Add(new HttpHeader(lines[i]));
                        //Console.WriteLine("§§§§§§§§§§" + lines[i] + "§§§§§§§§§§§§§");
                    }
                }
                else
                {
                    Payload += (lines[i] + "\r\n");
                }

                Headers = headers.ToArray();
            
            }
            //string aa = "curl\r\n-X\r\nGET\r\nhttp://localhost:12000/messages\r\n--header\r\n\"Content-Type:\r\ntext/plain\"\r\n-d\r\n\"\"";
            //Console.WriteLine(aa);
            //Console.WriteLine("");
            //string[] dd = aa.Replace("\r\n", "\n").Replace("\r", "\n").Split("\n");
            //for (int j = 0; j < dd.Length; j++)
            //{
            //    Console.WriteLine(dd[j]);
            //}
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                        //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets the plain message as received from TCP.</summary>
        public string PlainMessage
        {
            get; private set;
        }


        /// <summary>Get the HTTP method.</summary>
        public virtual string Method
        {
            get; protected set;
        }


        /// <summary>Gets the URL path.</summary>
        public virtual string Path
        {
            get; protected set;
        }


        /// <summary>Gets the HTTP headers.</summary>
        public HttpHeader[] Headers
        {
            get; private set;
        }


        /// <summary>Gets the HTTP payload.</summary>
        public string Payload
        {
            get; private set;
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public methods                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Returns a reply to the HTTP request.</summary>
        /// <param name="status">Status code.</param>
        /// <param name="payload">Payload.</param>
        public virtual void Reply(int status, string payload = null)
        {
            string data;

            switch(status)
            {                                                                   // create response status string from code
                case 200:
                    data = "HTTP/1.1 200 OK\n";
                    break;
                case 400:
                    data = "HTTP/1.1 400 Bad Request\n";
                    break;
                case 404:
                    data = "HTTP/1.1 404 Not Found\n";
                    break;
                default:
                    data = "HTTP/1.1 418 I'm a Teapot\n";
                    break;
            }

            if(string.IsNullOrEmpty(payload))
            {                                                                   // set Content-Length to 0 for empty content
                data += "Content-Length: 0\n";
            }
            data += "Content-Type: text/plain\n\n";

            if (payload != null) { data += payload; }

            byte[] dbuf = Encoding.ASCII.GetBytes(data);
            _Client.GetStream().Write(dbuf, 0, dbuf.Length);                    // send a response

            _Client.GetStream().Close();                                        // shut down the connection
            _Client.Dispose();
        }
    }
}
