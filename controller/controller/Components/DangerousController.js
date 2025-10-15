
import { useEffect, useRef, useState } from 'react';
import { Button, View, StyleSheet, Touchable, Pressable, Text, TextInput } from 'react-native';
import { sendMessage, stopClient } from '../Services/ControllerService';
import { botoneras } from '../styles/styles';
import { AntDesign } from '@expo/vector-icons';
export function DangerousController({setConnected}) {
    const sendChar = (text) => {
        //console.log(text)
        //if (text.length === 1) {
        //    sendMessage(text)
        //} else {
        //    sendMessage("special" + text)
        //}
        stopClient(setConnected)
    }


    return <>

        <View style={{ ...botoneras.container, width: 100, top: 700 }}>
            <View style={{ ...botoneras.buttonRow }}>
                <Pressable style={{ ...botoneras.button, backgroundColor: 'red' }} onPressIn={() => sendChar('Enter')}><Text style={botoneras.buttonText}><AntDesign name="poweroff" size={30} color="black" /></Text></Pressable>
            </View>
        </View>

    </>

}