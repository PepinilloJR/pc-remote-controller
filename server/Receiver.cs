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

        public string listen()
        {
            byte[] buffer = new byte[1024];

            int bytesRead = client.Receive(buffer);

            string crude = Encoding.UTF8.GetString(buffer);
            string message = crude.TrimEnd('\r', '\n', '\0');

            return message; 
        }


    }
}
