import { useEffect, useRef, useState } from 'react';
import { Button, View, StyleSheet, Touchable, Pressable, Text, TextInput } from 'react-native';
import { sendMessage } from '../Services/ControllerService';
import { botoneras } from '../styles/styles';

export function CommonController() {
    // some basic computer buttons that cannot be mapped from the virtual keyboard

    const sendChar = (text) => {
        console.log(text)
        if (text.length === 1) {
            sendMessage(text)
        } else {
            sendMessage("special" + text)
        }
    }


    return <>

        <View style={{...botoneras.container, top: 560}}>
            <View style={{ ...botoneras.buttonRow }}>
                <Pressable style={botoneras.button} onPressIn={() => sendChar('Esc')}><Text style={botoneras.buttonText}>Esc</Text></Pressable>
                <Pressable style={{ ...botoneras.button, backgroundColor: 'green' }} onPressIn={() => sendChar('Enter')}><Text style={botoneras.buttonText}>Enter</Text></Pressable>
                <Pressable style={{ ...botoneras.button, backgroundColor: '#ff525a'  }} onPressIn={() => sendChar('Delete')}><Text style={botoneras.buttonText}>Del</Text></Pressable>
            </View>
        </View>

    </>

}
