import { useEffect, useRef, useState } from 'react';
import { Button, View, StyleSheet, Touchable, Pressable, Text, TextInput } from 'react-native';
import { sendMessage } from '../Services/ControllerService';


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

        <View style={styles.container}>
            <View style={{ ...styles.buttonRow, width: "50%" }}>
                <Pressable style={styles.button} onPressIn={() => sendChar('Esc')}><Text style={styles.button}>Esc</Text></Pressable>
                <Pressable style={styles.button} onPressIn={() => sendChar('Enter')}><Text style={styles.button}>Enter</Text></Pressable>
                <Pressable style={styles.button} onPressIn={() => sendChar('Delete')}><Text style={styles.button}>Del</Text></Pressable>
            </View>
        </View>

    </>

}

const styles = StyleSheet.create({
    container: {
        position: 'absolute',
        top: 560,
        width: 300,
        height: 80,
        backgroundColor: '#fff',
        alignItems: 'center',
        justifyContent: 'center',
        borderBlockColor: "green",
        borderWidth: 2,
    },

    buttonRow: {
        flex: 1,
        flexDirection: "row",
        borderBlockColor: "red",
        borderWidth: 2,
        width: "100%",
        height: "100%",
        alignItems: 'stretch',
        justifyContent: 'center'
    },

    button: {
        backgroundColor: "#7E9BBD",
        margin: "2.5%",
        padding: "2.5%",
        width: "60%",
        borderBlockColor: "blue",
        borderWidth: 2,
        alignItems: 'center'
    },

    buttonText: {
        width: "100%",
        height: '100%'
    }
});