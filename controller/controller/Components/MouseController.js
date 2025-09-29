import { Button, View, StyleSheet, Touchable, Pressable, Text } from 'react-native';
import { sendMessage, stopMessage } from '../Services/ControllerService';
import { useRef } from 'react';


export function MouseController() {

    const intervalRef = useRef(null)

    const send = (message) => {
        sendMessage(message)

        // to avoid potential floating timer on user double click


        if (intervalRef.current) clearTimeout(intervalRef.current);


        intervalRef.current = setTimeout(() => {send(message)}, 50);
    }

    const stopTimer = () => {
        clearTimeout(intervalRef.current);
    }



    return <>

        <View style={styles.container}>


            <View style={{ ...styles.buttonRow, width: "50%" }}>
                <Pressable style={styles.button} onPressIn={() => send('up')} onPressOut={stopTimer}><Text style={styles.button}>Up</Text></Pressable>
            </View>

            <View style={{ ...styles.buttonRow }}>
                <Pressable style={styles.button} onPressIn={() => send('left')} onPressOut={stopTimer}><Text style={styles.button}>Left</Text></Pressable>
                <Pressable style={styles.button} onPressIn={() => send('right')} onPressOut={stopTimer}><Text style={styles.button}>Right</Text></Pressable>
            </View>

            <View style={{ ...styles.buttonRow, width: "50%" }}>
                <Pressable style={styles.button} onPressIn={() => send('down')} onPressOut={stopTimer}><Text style={styles.button}>Down</Text></Pressable>
            </View>
        </View>

        <View style={{...styles.container, top: 450, height: "10%"}}>


            <View style={{ ...styles.buttonRow, width: "50%"}}>
                <Pressable style={styles.button} onPressIn={() => send('left_click')} onPressOut={stopTimer}><Text style={styles.button}>Click 0</Text></Pressable>
                <Pressable style={styles.button} onPressIn={() => send('right_click')} onPressOut={stopTimer}><Text style={styles.button}>Click 1</Text></Pressable>

            </View>

        </View>

    </>

}

const styles = StyleSheet.create({
    container: {
        position: 'absolute',
        top: 80,
        width: 300,
        height: 300,
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