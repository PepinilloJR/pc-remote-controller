import { useRef } from 'react';
import TcpSocket from 'react-native-tcp-socket';


export const sendMessage = (message) => {
  try {
    //client.write(message)
    messages.push(message)

  } catch (error) {
    console.error(error)
  }
}

// messages buffer
const messages = [];

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

client.on('data', function (data) {
  console.log('message was received', data);
  //if (messages.length == 0) {
  //  messages.push('alive')
  //}
});




client.on('connect', function () {
  setInterval(() => {
    const m = messages.pop()
    if (m) {
      client.write('|' + m)
      clearTimeout(timeoutAlive)
      
      timeoutAlive = setTimeout(() => {
        if (messages.length == 0) {
          messages.push('alive')
        }
      }, 5000)

      console.log(m);
    }

  }, 1)

  var timeoutAlive = setTimeout(() => {
    if (messages.length == 0) {
      messages.push('alive')
    }
  }, 5000)
})

/*
*/

client.on('error', function (error) {
  console.log(error);
});

client.on('close', function () {
  console.log('Connection closed!');
});

