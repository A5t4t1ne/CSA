using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Explorer700Demo;

public class Webserver
{
    // private readonly TcpClient Client;
    private byte[]? addr { get; }
    private int port { get; }

    private bool listening = false;
    
    public Webserver(string addr, int port)
    {
        this.addr = parseIP(addr);
        this.port = port;
    }

    /// <summary>
    /// Starts up the server in a new Thread.
    /// </summary>
    public void Start()
    {
        Console.WriteLine("Starting web server");
        Thread t = new Thread(StartTcp);
        listening = true;
        t.Start();
    }


    public void Stop()
    {
        listening = false;
    }
    
    
    /// <summary>
    /// Tries to startup a Socket for TCP connection
    /// </summary>
    private void StartTcp()
    {
        try
        {
            if (this.addr == null)
            {
                Console.WriteLine("IP not valid: {0}", this.addr);
                return;
            }
            
            // IPAddress ip = Dns.GetHostEntry("jart").AddressList[0];
            IPAddress ip = new IPAddress(this.addr);
            
            TcpListener listen = new TcpListener(ip, this.port);
            listen.Start();
            Console.WriteLine("Listening on {0}:{1}", ip, port);
            while (listening)
            {
                TcpClient client = listen.AcceptTcpClient();
                Console.WriteLine("Verbindung zu {0} aufgebaut", client.Client.RemoteEndPoint);

                // Read the HTTP request
                // StreamReader reader = new StreamReader(client.GetStream());
                // string request = reader.ReadToEnd();
                
                // Read the HTTP request
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string request = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                // Parse the request URL
                string[] requestParts = request.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                string requestLine = requestParts[0];
                string[] requestLineParts = requestLine.Split(' ');
                string requestMethod = requestLineParts[0];
                string requestUrl = requestLineParts[1];
                
                string response = "";
                string filePath = "./";
                if (requestUrl == "/download")
                {
                    filePath += "./log.txt";
                    response = "HTTP/1.1 200 OK\r\n";
                    response += "Content-Type: application/force-download; charset=utf-8\r\n";
                    response += "Content-Disposition: inline; filename=log.txt\r\n\r\n";
                }
                else if (requestUrl == "/log.txt")
                {
                    filePath += "./log.txt";
                    response = "HTTP/1.1 200 OK\r\n";
                    response += "Content-Type: text/plain; charset=utf-8\r\n";
                }
                else
                {
                    filePath += "html/index.html";
                    response = "HTTP/1.1 200 OK\r\n";
                    response += "Content-Type: text/html;\r\n";
                }

                string fileContents;
                try
                {
                    using StreamReader sr = new StreamReader(filePath);
                    fileContents = sr.ReadToEnd();
                }
                catch (Exception e)
                {
                    Console.WriteLine("File not found: {0}", e);
                    fileContents = "Log file not found";
                }
                if (requestUrl != "/download")
                {
                    response += "Content-Length: " + fileContents.Length + "\r\n\r\n";
                }

                response += fileContents;

                // Send the HTTP response
                StreamWriter writer = new StreamWriter(client.GetStream());
                writer.Write(response);
                writer.Flush();

                client.Close();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Webserver chose NO because: {0}", e);
        }
    }

    private byte[]? parseIP(string addr)
    {
        try
        {
            byte[] b_addr = new byte[4];
            string[] nums = addr.Split(".");
            for (var i = 0; i < 4; i++)
            {
                b_addr[i] = Byte.Parse(nums[i]);
            }

            return b_addr;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}
