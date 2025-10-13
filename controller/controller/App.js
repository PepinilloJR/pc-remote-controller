import { StatusBar } from 'expo-status-bar';
import { StyleSheet, Text, View, Button } from 'react-native';
import TcpSocket from 'react-native-tcp-socket';
import { MouseController } from './Components/MouseController';
import { KeyboardController } from './Components/KeyboardController';
import { CommonController } from './Components/CommonController';
export default function App() {
  return (
    <View style={styles.container}>
      <Text>Open up App.js to start working on your app!</Text>
      <MouseController></MouseController>
      <KeyboardController></KeyboardController>
      <CommonController></CommonController>
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
