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
var timeStamp = 0;

export let client;

export async function stopClient(setConnected) {
  client.destroy()
  setConnected(false)

}

export async function startClient(options, setConnected) {
  // Create socket
  client = TcpSocket.createConnection(options, () => {

    console.log("connection created")
  });

  
  client.on('connect', function () {
    setConnected(true)
    const intervalSender = setInterval(() => {
      const m = messages.pop()
      if (m) {
        
        if (timeStamp === Number.MAX_SAFE_INTEGER) {
          timeStamp = 0;
        }
        timeStamp += 1
        
        client.write('|' + m + "#" + timeStamp) 
        
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

    client.on('data', function (data) {
      console.log('message was received', data);

    });


    client.on('error', function (error) {
      console.log(error);
      setConnected(false)
    });

    client.on('close', function () {
      console.log('Connection closed!');
      clearTimeout(timeoutAlive)
      clearInterval(intervalSender)
      setConnected(false)
    });
  })






}




