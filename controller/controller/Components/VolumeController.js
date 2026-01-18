import { useEffect, useRef, useState } from 'react';
import { Button, View, StyleSheet, Touchable, Pressable, Text, TextInput } from 'react-native';
import { sendMessage } from '../Services/ControllerService';
import { botoneras } from '../styles/styles';
import MaterialCommunityIcons from '@expo/vector-icons/MaterialCommunityIcons';
export function VolumeController() {
    // some basic computer volume controlling

    const sendChar = (text) => {
        console.log(text)
        sendMessage(text)
    }


    return <>

        <View style={{ ...botoneras.container, top: 650, left: 320, height: 220, width: 60 }}>
            <View style={{ ...botoneras.buttonRow}}>
                <Pressable style={{...botoneras.buttonSeparated, height: 100, width: 60}} onPressIn={() => sendChar('vol_up')}>
                    <MaterialCommunityIcons name="volume-plus" size={50} color="black" />
                   
                </Pressable>
            </View>
            <View style={{ ...botoneras.buttonRow }}>
                <Pressable style={{...botoneras.buttonSeparated, height: 100, width: 60}} onPressIn={() => sendChar('vol_down')}>
                    <MaterialCommunityIcons name="volume-minus" size={50} color="black" />

                </Pressable>
            </View>
        </View>

    </>

}