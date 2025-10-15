import { StatusBar } from 'expo-status-bar';
import { StyleSheet, Text, View, Button } from 'react-native';
import TcpSocket from 'react-native-tcp-socket';
import { MouseController } from './Components/MouseController';
import { KeyboardController } from './Components/KeyboardController';
import { CommonController } from './Components/CommonController';
import { VolumeController } from './Components/VolumeController';
import { useState } from 'react';
import { ConnectMenu } from './Services/ConnectService';
import { DangerousController } from './Components/DangerousController';

export default function App() {

  const [connected, setConnected] = useState(false)

  return (
    connected ? 
    <View style={styles.container}>
      <MouseController></MouseController>
      <KeyboardController></KeyboardController>
      <CommonController></CommonController>
      <VolumeController></VolumeController>
      <DangerousController setConnected={setConnected}></DangerousController>
      <StatusBar style="auto" />
    </View> : <ConnectMenu setConnected={setConnected}></ConnectMenu>
  );
}



const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
    alignItems: 'center',
    justifyContent: 'center',
  },
});
