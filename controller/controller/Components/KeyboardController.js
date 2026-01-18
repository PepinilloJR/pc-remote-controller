import { useEffect, useRef, useState } from 'react';
import { Button, View, StyleSheet, Touchable, Pressable, Text, TextInput, TouchableOpacity } from 'react-native';
import { sendMessage } from '../Services/ControllerService';
import Entypo from '@expo/vector-icons/Entypo';

export function KeyboardController() {

    const [currentText, setCurrentText] = useState("")
    const refTextInput = useRef(null)
    const [key, setKey] = useState()
    const [autofocus, setAutofocus] = useState(false)
    useEffect(() => {
        sendChar(currentText)
    }, [currentText])

    const sendChar = (text) => {
        console.log(text)
        if (text.length === 1) {
            sendMessage(text)
        } else {
            sendMessage("special" + text)
        }
    }
    //onChangeText={setCurrentText} 
    return <TouchableOpacity onPress={() => {
        //refTextInput.current.focus()
        setAutofocus(true);

        // the only way i managed to make this to work is getting the textInput component to be remounted
        setKey(Math.random())

    }} style={styles.container}>
        <TextInput key={key} ref={refTextInput} style={{ width: 0, height: 0 }} 
        autoFocus={autofocus}
        onKeyPress={({ nativeEvent }) => { sendChar(nativeEvent.key) }} 
        onSubmitEditing={() => { sendChar('Enter') }} 
        >

        </TextInput>
        <Entypo name="keyboard" size={80
            
        } color="black" />
    </TouchableOpacity>




}

const styles = StyleSheet.create({
    container: {
        position: 'absolute',
        width: 300,
        top: 450,
        height: 'auto',
        backgroundColor: '#fff',
        alignItems: 'center',
        justifyContent: 'center',

    },
})