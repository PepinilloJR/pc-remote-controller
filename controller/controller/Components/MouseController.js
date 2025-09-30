import { Button, View, StyleSheet, Touchable, Pressable, Text } from 'react-native';
import { sendMessage, stopMessage } from '../Services/ControllerService';
import { useRef } from 'react';


export function MouseController() {

    const intervalRef = useRef(null)
    const clickStatus = useRef({})

    const sendMouseMovement = (message) => {
        sendMessage(message)

        // to avoid potential floating timer on user double click


        if (intervalRef.current) clearTimeout(intervalRef.current);


        intervalRef.current = setTimeout(() => {sendMouseMovement(message)}, 50);
    }

    const sendMouseClick = (message) => {

        const status = {...clickStatus.current}

        if (status[message] !== "hold") {
            sendMessage(message)
            status[message] = "click"
        } else {
            sendMessage(message + "_up")
            status[message] = "up"
        }

        clickStatus.current = {...status}

        // to avoid potential floating timer on user double click
        if (intervalRef.current) clearTimeout(intervalRef.current);


        intervalRef.current = setTimeout(() => {
            const status_ = {...clickStatus.current}
            status_[message] = "hold"
            clickStatus.current = {...status_}
            

        }, 500);
    }

    const stopMouseMovement = () => {
        clearTimeout(intervalRef.current);
    }

    const stopMouseClick = (message) => {
        const status = {...clickStatus.current}
        
        if (status[message] !== "hold") {
            sendMessage(message + "_up")
            status[message] = 'up'
            clearTimeout(intervalRef.current);
        } 
        clickStatus.current = {...status}
    }



    return <>

        <View style={styles.container}>


            <View style={{ ...styles.buttonRow, width: "50%" }}>
                <Pressable style={styles.button} onPressIn={() => sendMouseMovement('up')} onPressOut={stopMouseMovement}><Text style={styles.button}>Up</Text></Pressable>
            </View>

            <View style={{ ...styles.buttonRow }}>
                <Pressable style={styles.button} onPressIn={() => sendMouseMovement('left')} onPressOut={stopMouseMovement}><Text style={styles.button}>Left</Text></Pressable>
                <Pressable style={styles.button} onPressIn={() => sendMouseMovement('right')} onPressOut={stopMouseMovement}><Text style={styles.button}>Right</Text></Pressable>
            </View>

            <View style={{ ...styles.buttonRow, width: "50%" }}>
                <Pressable style={styles.button} onPressIn={() => sendMouseMovement('down')} onPressOut={stopMouseMovement}><Text style={styles.button}>Down</Text></Pressable>
            </View>
        </View>

        <View style={{...styles.container, top: 450, height: "10%"}}>


            <View style={{ ...styles.buttonRow, width: "50%"}}>
                <Pressable style={styles.button} onPressIn={() => sendMouseClick('left_click')} onPressOut={() => {stopMouseClick('left_click')}}><Text style={styles.button}>Click 0</Text></Pressable>
                <Pressable style={styles.button} onPressIn={() => sendMouseClick('right_click')} onPressOut={() => {stopMouseClick('right_click')}}><Text style={styles.button}>Click 1</Text></Pressable>

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