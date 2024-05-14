using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
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
                Console.WriteLine("Fauschi IP idiot {0}", this.addr);
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
                StreamReader reader = new StreamReader(client.GetStream());
                // string request = reader.ReadToEnd();

                // Parse the request and generate a response
                string response = "HTTP/1.1 200 OK\r\nContent-Type: text/html\r\n\r\n<html><body>Hello, World!</body></html>";

                // Send the HTTP response
                StreamWriter writer = new StreamWriter(client.GetStream());
                writer.Write(response);
                writer.Flush();

                client.Close();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Isch am umebitche: {0}", e);
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