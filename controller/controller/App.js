import { StatusBar } from 'expo-status-bar';
import { StyleSheet, Text, View, Button } from 'react-native';
import TcpSocket from 'react-native-tcp-socket';
import { MouseController } from './Components/MouseController';
import { KeyboardController } from './Components/KeyboardController';
import { CommonController } from './Components/CommonController';
import { VolumeController } from './Components/VolumeController';
export default function App() {
  return (
    <View style={styles.container}>
      <MouseController></MouseController>
      <KeyboardController></KeyboardController>
      <CommonController></CommonController>
      <VolumeController></VolumeController>
      <StatusBar style="auto" />
    </View>
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
