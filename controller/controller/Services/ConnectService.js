import { StyleSheet, Text, View, Button, TextInput, TouchableHighlight, TouchableOpacity } from 'react-native';
import { startClient } from './ControllerService';
import { useRef, useState } from 'react';
import { interfaz } from '../styles/styles';

export function ConnectMenu({ setConnected }) {

    const refTextInput = useRef('')
    const [errorText, setErrorText] = useState();

    const options = {
        port: 8080,
        host: '127.0.0.1',
        reuseAddress: true,

    };

    const handleConnect = (text) => {
        if (text) {
            console.log(text)
            const ip = text.split(':')[0]
            const port = text.split(':')[1]

            options.port = port
            options.host = ip

            try {
                startClient(options, setConnected)
                //setConnected(true)
            } catch (error) {
                setErrorText(error.message)
            }
        }
    }

    return <>
        <View style={interfaz.container}>
            <View style= {interfaz.textoView}><Text style= {interfaz.texto}>Welcome!!!!!</Text></View>
            <View style= {interfaz.textoView}><Text style= {interfaz.texto}>Type the server's ip in here</Text></View>

            <TextInput style= {interfaz.input} ref={refTextInput} onChangeText={(e) => { refTextInput.current.value = e }} onSubmitEditing={() => { handleConnect(refTextInput.current.value) }} placeholder="ie: 127.0.0.1:8080 ..."></TextInput>
            <TouchableOpacity style={interfaz.button} onPress={() => { handleConnect(refTextInput.current.value) }}><Text style= {interfaz.texto}>Connect</Text></TouchableOpacity>

            {errorText ? <Text>Error trying to connect: {errorText}</Text> : <></>}
        </View>
    </>
}