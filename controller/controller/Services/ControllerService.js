import { useRef } from 'react';
import TcpSocket from 'react-native-tcp-socket';


export const sendMessage = (message) => {
    try {
        client.write(message)

    } catch (error) {
        console.error(error)
    }
}



const options = {
  port: 8000,
  host: '192.168.100.3',
  reuseAddress: true,
  // localPort: 20000,
  // interface: "wifi",
};

// Create socket
const client = TcpSocket.createConnection(options, () => {
    console.log("connection created")
});

client.on('data', function(data) {
  console.log('message was received', data);
});

client.on('error', function(error) {
  console.log(error);
});

client.on('close', function(){
  console.log('Connection closed!');
});