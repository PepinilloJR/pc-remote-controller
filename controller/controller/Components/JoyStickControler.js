import { Button, View, StyleSheet, Touchable, Pressable, Text, PanResponder, Animated } from 'react-native';
import { botoneras, interfaz, joystick } from '../styles/styles';
import { useRef } from 'react';

export function JoyStick ({JoyStickPos, Sender, Stopper}) {

    const pan = useRef(new Animated.ValueXY()).current;

    const panResponder = useRef(PanResponder.create({
        onStartShouldSetPanResponder: () => false,
        onMoveShouldSetPanResponder: () => true,
        onPanResponderMove: (event, gestureState) => {
            // normalize vector and stuff
            const dx = gestureState.dx ** 2
            const dy = gestureState.dy ** 2
            const d = Math.sqrt(dx + dy)

            // 100 is the size of the stick
            var value
            if (d < 100) {
                value = {x: gestureState.dx, y: gestureState.dy }
                pan.setValue(value) 

            } else {
                const nx = gestureState.dx / d;
                const ny = gestureState.dy / d;
                value = {x: 100 * nx, y: 100 * ny}
                pan.setValue(value)
            }

            JoyStickPos.current = value

            Sender(JoyStickPos.current)
        },

        onPanResponderRelease: () => {
            pan.setValue({x: 0, y: 0})
            JoyStickPos.current = {x: pan.x, y: pan.y}
            Stopper()
        }
    })).current;

    return <>
        <View style={joystick.container}>
            <Animated.View style={{...joystick.stick, top: pan.y, left: pan.x }} {...panResponder.panHandlers} >

            </Animated.View>
        </View>
    
    </> 
}