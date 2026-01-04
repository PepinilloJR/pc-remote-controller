import { Button, View, StyleSheet, Touchable, Pressable, Text } from 'react-native';
import { sendMessage, stopMessage } from '../Services/ControllerService';
import { useRef } from 'react';
import { botoneras } from '../styles/styles';
import { JoyStick } from './JoyStickControler';

export function MouseController() {

    const intervalRef = useRef(null)
    const clickIntervalRef = useRef(null)
    const clickStatus = useRef({})
    const joyStickPos = useRef({x: 0, y: 0})

    const sendMouseMovement = (message) => {

        var formatedMessage;

        // if type is string, then the legacy controller is being used
        if (typeof(message) != 'string') {
            console.log(message)
            formatedMessage = "joystick:" + message.x + ":" + message.y
        } else {
            formatedMessage = message
        }

        sendMessage(formatedMessage)

        // to avoid potential floating timer on user double click

        if (intervalRef.current) clearTimeout(intervalRef.current);


        intervalRef.current = setTimeout(() => { sendMouseMovement(message) }, 50);
    }

    const sendMouseClick = (message) => {

        const status = {...clickStatus.current}

        if (status[message] == "up" || !status[message]) {
            console.log("hice click")
            sendMessage(message)
            status[message] = "click"
        } else if (status[message] == "hold") {
            console.log("detecte hold y cambie el status a click")

            status[message] = "click"
        }

        clickStatus.current = { ...status }

        if (clickIntervalRef.current) {
            clearTimeout(clickIntervalRef.current)
        }

        clickIntervalRef.current = setTimeout(() => {
            const status_ = {...clickStatus.current}
            status_[message] = "hold"
            clickStatus.current = {...status_}
        }, 500)
    }

    const stopMouseClick = (message) => {
        const status = { ...clickStatus.current }
        console.log("estoy cerrando el click con status: " + status[message])
        if (status[message] !== "hold") {
            console.log("detecte click y cambie a up")
            clearTimeout(clickIntervalRef.current);
            sendMessage(message + "_up")
            status[message] = 'up'
        }
        clickStatus.current = { ...status }
    }


    const stopMouseMovement = () => {
        clearTimeout(intervalRef.current);
    }

    return <>
        <JoyStick JoyStickPos={joyStickPos} Sender={sendMouseMovement} Stopper={stopMouseMovement}></JoyStick>

        <View style={{ ...botoneras.container, top: 350 }}>


            <View style={{ ...botoneras.buttonRow }}>
                <Pressable style={botoneras.button} onPressIn={() => sendMouseClick('left_click')} onPressOut={() => { stopMouseClick('left_click') }}><Text style={botoneras.buttonText}>Click 0</Text></Pressable>
                <Pressable style={botoneras.button} onPressIn={() => sendMouseClick('right_click')} onPressOut={() => { stopMouseClick('right_click') }}><Text style={botoneras.buttonText}>Click 1</Text></Pressable>

            </View>

        </View>

        <View style={{ ...botoneras.container, top: 650, left: 20, height: 220, width: 60 }}>
            <View style={{ ...botoneras.buttonRow }}>
                <Pressable style={{ ...botoneras.buttonSeparated, height: 100, width: 60 }} onPressIn={() => sendMouseMovement('wheel_up')} onPressOut={stopMouseMovement}><Text style={botoneras.buttonText}>W_UP</Text></Pressable>
            </View>
            <View style={{ ...botoneras.buttonRow }}>
                <Pressable style={{ ...botoneras.buttonSeparated, height: 100, width: 60 }} onPressIn={() => sendMouseMovement('wheel_down') } onPressOut={stopMouseMovement}><Text style={botoneras.buttonText}>W_DOWN</Text></Pressable>
            </View>
        </View>

    </>

}

const styles2 = StyleSheet.create({
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


/* 
            <View style={{ ...botoneras.container, top: 80, height: 250 }}>


            <View style={{ ...botoneras.buttonRowSeparated, justifyContent: 'center' }}>
                <Pressable style={botoneras.buttonSeparated} onPressIn={() => sendMouseMovement('up')} onPressOut={stopMouseMovement}><Text style={botoneras.buttonText}>Up</Text></Pressable>
            </View>

            <View style={{ ...botoneras.buttonRowSeparated }}>
                <Pressable style={botoneras.buttonSeparated} onPressIn={() => sendMouseMovement('left')} onPressOut={stopMouseMovement}><Text style={botoneras.buttonText}>Left</Text></Pressable>
                <Pressable style={botoneras.buttonSeparated} onPressIn={() => sendMouseMovement('right')} onPressOut={stopMouseMovement}><Text style={botoneras.buttonText}>Right</Text></Pressable>
            </View>

            <View style={{ ...botoneras.buttonRowSeparated, justifyContent: 'center' }}>
                <Pressable style={botoneras.buttonSeparated} onPressIn={() => sendMouseMovement('down')} onPressOut={stopMouseMovement}><Text style={botoneras.buttonText}>Down</Text></Pressable>
            </View>
        </View>


*/