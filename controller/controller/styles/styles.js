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

export const interfaz = StyleSheet.create({
    container: {
        flex: 1,
        width: '100%',
        height: '100%',
        backgroundColor: '#fff',

        alignItems: 'center',
        justifyContent: 'center'
    },
    textoView: {

        backgroundColor: '#fff',
        width: '100%',
        justifyContent: 'center'
    },

    texto: {
        textAlign: 'center',
        fontSize: 25
    },  

    input: {
        borderColor: 'gray',
        borderWidth: 1,
        width: '80%',
        height: 50,
        margin: 20,
        padding: 10
    },

    button: {
        width: '60%',
        backgroundColor: '#568cffff',
    }

});