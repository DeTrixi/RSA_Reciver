using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RSAReciver
{
    class TcpServer
    {
        public event EventHandler<string> MessageReceived;

        public void TcpServerStart(string serverIp, int portNo)
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[512];
                String data = null;

                // Enter the listening loop.
                while (true)
                {

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    System.Net.Sockets.TcpClient client = server.AcceptTcpClient();
                    
                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.UTF8.GetString(bytes, 0, i);
                        MessageReceived?.Invoke(this, data);

                        // Process the data sent by the client.
                        data = data.ToUpper();

                        byte[] msg = Encoding.UTF8.GetBytes(data);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                MessageBox.Show($"{e}");
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }
        }
    }
}