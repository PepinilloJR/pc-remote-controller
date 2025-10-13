import { useEffect, useRef, useState } from 'react';
import { Button, View, StyleSheet, Touchable, Pressable, Text, TextInput } from 'react-native';
import { sendMessage } from '../Services/ControllerService';


export function KeyboardController () {

    const [currentText, setCurrentText] = useState("")


    useEffect(() => {
        sendChar(currentText)
    }, [currentText]) 

    const sendChar = (text) => {
        console.log(text)
        if (text.length === 1) {
            sendMessage(text)
        } else {
            sendMessage("special"+text)
        }
    } 
    //onChangeText={setCurrentText} 
    return <View style={styles.container}>
        <TextInput onKeyPress={({nativeEvent}) => {sendChar(nativeEvent.key)}} onSubmitEditing={() => {sendChar('Enter')}} ></TextInput>
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