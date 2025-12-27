using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Reception
{

    class Receiver {

        private Socket client { get; set; }

        public Receiver (Socket c) {  client = c; } 

        async public Task<String> listen(CancellationToken token)
        {
            byte[] buffer = new byte[1024];

            int bytesRead = await client.ReceiveAsync(buffer, token);

            if (bytesRead <= 0)
                return string.Empty;

            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            return message; 
        }

        // method to mantain client connected (power cable uplogged?)
        public void keepAlive(Socket clientSocket)
        {
            try
            {
                byte[] buffer = new byte[1024];
                buffer = Encoding.UTF8.GetBytes("check");
                clientSocket.Send(buffer);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}
