import { useEffect, useRef, useState } from 'react';
import { Button, View, StyleSheet, Touchable, Pressable, Text, TextInput } from 'react-native';
import { sendMessage } from '../Services/ControllerService';


export function KeyboardController () {

    const [currentText, setCurrentText] = useState("")


    useEffect(() => {
        sendChar(currentText)
    }, [currentText]) 

    const sendChar = (text) => {
        console.log(text[text.length - 1])
        sendMessage(text[text.length - 1])
    }

    return <View style={styles.container}>
        <TextInput onChangeText={setCurrentText}></TextInput>
    </View>


}

const styles = StyleSheet.create({
    container: {
        width: 300,
        top: 250,
        height: 'auto',
        backgroundColor: '#fff',
        alignItems: 'center',
        justifyContent: 'center',
        borderBlockColor: "green",
        borderWidth: 2,
    }, })