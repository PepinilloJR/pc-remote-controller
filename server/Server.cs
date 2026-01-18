using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Controller;
using Reception;
namespace ServerController
{
    internal class Server
    {
        private IPAddress ipAddress { get; set; }
        private IPEndPoint ipEndPoint { get; set; }
        private Socket socket { get; set; }
        private int port { get; set; }

        private Socket client { get; set; }

        public Server() { }

        public Server(int port)
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                Console.WriteLine(ni.NetworkInterfaceType);
                if (
                    (ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet || ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                    &&
                    (ni.OperationalStatus == OperationalStatus.Up)
                    && (!ni.Description.ToLower().Contains("radmin") && !ni.Description.ToLower().Contains("hamachi")) // Add here any adapters that might interfere with the correct IP address

                    )
                {
                    // here i can see if the ip is IPV4
                    foreach (UnicastIPAddressInformation uip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (uip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            this.ipAddress = IPAddress.Parse(uip.Address.ToString());
                            break;
                        }


                    }
                    break;
                }
            }
            this.port = port;

            this.ipEndPoint = new IPEndPoint(this.ipAddress, this.port);

        }

        public Server(String ip, int port)
        {
            this.ipAddress = IPAddress.Parse(ip);
            this.port = port;

            this.ipEndPoint = new IPEndPoint(this.ipAddress, this.port);
        }


        public Server createSocket()
        {
            this.socket = new Socket(this.ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);


            return this;
        }

        public Server bind()
        {
            this.socket.Bind(this.ipEndPoint);

            return this;
        }

        public Server listen()
        {
            this.socket.Listen(2);

            return this;
        }

        public async Task Receive(IControllerBase controller, Receiver receiver, CancellationTokenSource tokenSource, CancellationToken token)
        {

            string messageAcumulator = "";

            while (true)
            {

                Task timeout = Task.Delay(10000);
                Task<string> listen = receiver.listen(token);

                var completed = await Task.WhenAny(timeout, listen);

                if (completed == timeout)
                {
                    tokenSource.Cancel();
                }

                string message = await listen;

                if (string.IsNullOrEmpty(message))
                {
                    tokenSource.Cancel();
                }


                foreach (char i in message)
                {
                    messageAcumulator += i;
                    if (i == ';')
                    {
                        FunctionSelector.selectFunction(messageAcumulator, controller);
                        messageAcumulator = "";
                    }
                    
                }
                
            }
        }

        public Socket awaitConnection()
        {
            Console.WriteLine("Waiting for connection with controller on " + this.ipEndPoint.Address + ":" + this.ipEndPoint.Port + "...");
            Socket client = this.socket.Accept();
            Console.WriteLine("A connection was made with " + client.Ttl + ":" + this.ipEndPoint.Port + "...");

            return client;

        }

        public void Close()
        {
            this.socket.Close();
            this.socket.Dispose();
        }

    }
}
