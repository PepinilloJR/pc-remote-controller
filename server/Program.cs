
using Controller;
using Reception;
using ServerController;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;


namespace main
{
    class Program
    {
        public static async Task Main(string[] args)
        {

            while (true)
            {
                Console.WriteLine("Welcome to PC-remote-controller.");

                Console.WriteLine("1. Start service");
                Console.WriteLine("2. Start service with non-default IP and PORT");
                Console.WriteLine("");
                Console.Write("->");
                String op = Console.ReadLine();

                Server server;
                if (op.Equals("1"))
                {
                    server = new Server(8000);
                }
                else if (op.Equals("2"))
                {
                    Console.WriteLine("");
                    Console.Write("IP ->");
                    String ip = Console.ReadLine();
                    Console.WriteLine("");
                    Console.Write("Port ->");
                    String port = Console.ReadLine();

                    try
                    {
                        server = new Server(ip, int.Parse(port));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("The IP or PORT specified is not valid or not available");
                        Console.WriteLine("There's something wrong with the format provided or the port is being used by another service");
                        continue;
                    }

                }
                else { continue; }

                while (true)
                {
                    Socket cliente = server.createSocket().bind().listen().awaitConnection();

                    Console.WriteLine("Connection successfully established");

                    Receiver receiver = new Receiver(cliente);

                    CancellationTokenSource tokenSource = new CancellationTokenSource();

                    CancellationToken token = tokenSource.Token;

                    IControllerBase controller;

                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        controller = new LinuxController();
                    }
                    else
                    {
                        controller = new WindowsController();
                    }


                    Task recibir = server.Receive(controller, receiver, tokenSource, token);

                    try
                    {
                        await recibir;
                        server.Close();
                        cliente.Close();
                        cliente.Dispose();

                    }
                    catch (TaskCanceledException ex)
                    {
                        Console.WriteLine("Client disconnected");
                        server.Close();
                        cliente.Close();
                        cliente.Dispose();
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Unexpected client disconnection, keeping the socket open for fast reconnection");
                        Console.WriteLine(ex.Message);

                        server.Close();
                        cliente.Close();
                        cliente.Dispose();
                    }

                }
            }
        }

    }

}

