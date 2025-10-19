
using Controllers;
using Reception;
using ServerController;
using System.Net.Sockets;
using System.Threading;


namespace main
{
    class Program
    {
        public static void Main(string[] args)
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
                if (op.Equals("1")) {
                    server = new Server(8000);
                } else if (op.Equals("2")) {
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

                } else { continue; }


                Socket cliente = server.createSocket().bind().listen().awaitConnection();

                Console.WriteLine("conexion iniciada");

                Receiver receiver = new Receiver(cliente);

                CancellationTokenSource token = new CancellationTokenSource();

                CancellationToken tokenCancelacion = token.Token;


                Task recibir = Task.Run(async () =>
                {
                    while (true)
                    {
                        CancellationTokenSource token_t = new CancellationTokenSource();

                        CancellationToken tokenCancelacion_t = token_t.Token;
                        Task timeout = Task.Run(() =>
                        {
                            Task.Delay(10000, tokenCancelacion_t).Wait();
                            if (tokenCancelacion_t.IsCancellationRequested) { return; }
                            else { token.Cancel(); }
                        });
                        string mensaje = await receiver.listen(tokenCancelacion);
                        token_t.Cancel();
                        Console.WriteLine("Mensaje recibido: " + mensaje);
                        if (mensaje == "") { token.Cancel(); }  


                        if (mensaje == "alive")
                        {

                            continue;
                        }
                        else if (mensaje.Length == 1)
                        {
                            Console.WriteLine(mensaje);
                            WindowsController.WriteText(mensaje.ToCharArray()[0]);
                        }
                        else if (mensaje.Contains("special"))
                        {
                            mensaje = mensaje.Replace("special", "");
                            WindowsController.WriteTextSpecial(mensaje);
                        } else if (mensaje.Contains("vol"))
                        {
                            WindowsController.VolumenChange(mensaje);
                        }
                        else
                        {

                            WindowsController.MoveMouse(mensaje);
                            WindowsController.ClickMouse(mensaje);
                        }
                    }
                });


                try {
                    Task.WaitAll(recibir);
                    server.Close(); 
                    cliente.Close();
                    cliente.Dispose();  
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Posible desconexion repentina del cliente");
                    server.Close();
                    cliente.Close();
                    cliente.Dispose();
                }

                
            }
        }

    }

}

