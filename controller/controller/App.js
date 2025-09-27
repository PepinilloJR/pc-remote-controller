import { StatusBar } from 'expo-status-bar';
import { StyleSheet, Text, View } from 'react-native';
import TcpSocket from 'react-native-tcp-socket';
export default function App() {
  return (
    <View style={styles.container}>
      <Text>Open up App.js to start working on your app!</Text>
      <StatusBar style="auto" />
    </View>
  );
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
  // Write on the socket
  // Close socket
  
  client.write("hola")

  client.destroy()
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


const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
    alignItems: 'center',
    justifyContent: 'center',
  },
});
