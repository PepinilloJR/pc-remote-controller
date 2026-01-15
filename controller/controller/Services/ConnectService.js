import { StyleSheet, Text, View, Button, TextInput, TouchableHighlight, TouchableOpacity } from 'react-native';
import { startClient } from './ControllerService';
import { useRef, useState, useEffect } from 'react';
import { interfaz } from '../styles/styles';
import AsyncStorage from '@react-native-async-storage/async-storage';
export function ConnectMenu({ setConnected }) {

    const refTextInput = useRef('')
    const [errorText, setErrorText] = useState();
    const [servers, setServers] = useState();

    useEffect( () => {
        getServers()
    }, [])
    
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
                AsyncStorage.setItem(options.host + ":" + options.port, options.host + ":" + options.port)
            } catch (error) {
                setErrorText(error.message)

            }
        }
    }

    const getServers = async () => {
        var serverList = await AsyncStorage.getAllKeys()

        if (serverList.length > 3) {
            await AsyncStorage.removeItem(serverList[0])
            serverList = await AsyncStorage.getAllKeys()
        }
        setServers(serverList)
    }   

    const clearServers = async () => {
        var serverList = await AsyncStorage.getAllKeys()
        await AsyncStorage.multiRemove(serverList)
        setServers([])
    }

    return <>
        <View style={interfaz.container}>
            <View style= {interfaz.textoView}><Text style= {interfaz.texto}>Welcome!!!!!</Text></View>
            <View style= {interfaz.textoView}><Text style= {interfaz.texto}>Type the server's ip in here</Text></View>

            <TextInput style= {interfaz.input} ref={refTextInput} onChangeText={(e) => { refTextInput.current.value = e }} onSubmitEditing={() => { handleConnect(refTextInput.current.value) }} placeholder="ie: 127.0.0.1:8080 ..." placeholderTextColor="#888"></TextInput>
            <TouchableOpacity style={interfaz.button} onPress={() => { handleConnect(refTextInput.current.value) }}>
                <Text style= {interfaz.texto}>Connect</Text>
            </TouchableOpacity>

            {errorText ? <Text>Error trying to connect: {errorText}</Text> : <></>}
            
            <View style= {interfaz.textoView}><Text style= {{...interfaz.texto, marginTop: 50}}>Servers recently used</Text></View>

            {servers?.map((keyName,key) => {
                return <TouchableOpacity key={key} style={{...interfaz.button, margin: 20 }} onPress={async () => 
                { 
                    const item = await AsyncStorage.getItem(keyName)
                    handleConnect(item) 

                }
                
                }>
                        <Text style= {interfaz.texto}>{keyName}</Text>
                    </TouchableOpacity>
            })}
            <TouchableOpacity style={{...interfaz.button, margin: 20 }} onPress={async () => 
                { 
                    clearServers()
                }
                
                }>
                    <Text style= {interfaz.texto}>Clear</Text>
                </TouchableOpacity>
        </View>
    </>
}