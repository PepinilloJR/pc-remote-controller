import {StyleSheet } from 'react-native';

export const botoneras = StyleSheet.create({
    container: {
        flex: 1,
        position: 'absolute',
        width: 300,
        height: 80,
        backgroundColor: '#fff',

        alignItems: 'center'
    },

    buttonRow: {
        flex: 1,
        width: '100%',
        flexDirection: "row",

    },

    
    buttonRowSeparated: {
        flex: 1,
        width: '110%',
        flexDirection: "row",
        justifyContent: 'space-between',

    },

    buttonSeparated: {
        width: '30%',
        backgroundColor: "#969696ff",
        borderCurve: "circular",
        borderRadius: 25,

        justifyContent: 'center',
    },

    button: {
        flex: 1,
        backgroundColor: "#969696ff",
        borderCurve: "circular",
        borderRadius: 25,
        justifyContent: 'center',
        margin: 2
    },


    buttonText: {
        width: "100%",
        textAlign: 'center'
    }
});