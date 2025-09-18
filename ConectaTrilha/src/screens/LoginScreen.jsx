// import React, { useState, useEffect } from 'react';
// import {
//     View,
//     Text,
//     StyleSheet,
//     Image,
//     TextInput,
//     Button,
//     TouchableOpacity,
//     Alert,
//     ActivityIndicator,
//     KeyboardAvoidingView,
//     Platform
// } from 'react-native';
// import axios from 'axios';
// import { login, register } from '../services/serviceLoginRegister';
// import * as Network from 'expo-network';

// export default function LoginScreen({ navigation }) {
//     const [isRegistering, setIsRegistering] = useState(false);
//     const [email, setEmail] = useState('');
//     const [password, setPassword] = useState('');
//     const [name, setName] = useState('');
//     const [username, setUsername] = useState('');
//     const [isLoading, setIsLoading] = useState(false);
//     const [apiUrl, setApiUrl] = useState('');

//     // Detecta o IP local da máquina
//     useEffect(() => {
//         async function fetchIp() {
//             try {
//                 //const ip = await Network.getIpAddressAsync();
//                 setApiUrl(`http://172.28.8.97:5130/api/account`);
//             } catch (error) {
//                 console.error('Erro ao obter IP local:', error);
//             }
//         }
//         fetchIp();
//     }, []);

//     const handleAuthAction = async () => {
//         if (!apiUrl) {
//             Alert.alert("Erro", "API não está configurada ainda.");
//             return;
//         }
//         setIsLoading(true);
//         try {
//             if (isRegistering) {
//                 if (!name || !username || !email || !password) {
//                     Alert.alert("Erro", "Preencha todos os campos para se registrar.");
//                     setIsLoading(false);
//                     return;
//                 }
//                 const response = await axios.post(`${apiUrl}/register`, {
//                     name,
//                     username,
//                     email,
//                     password
//                 });
//                 Alert.alert("Sucesso", "Conta criada com sucesso!");
//                 console.log("Token recebido:", response.data.token);
//             } else {
//                 const response = await axios.post(`${apiUrl}/login`, {
//                     email,
//                     password
//                 });

//                 console.log("Token recebido:", response.data.token);
//                 Alert.alert("Sucesso", "Login realizado com sucesso!");
//             }
//         } catch (error) {
//             console.error(error);
//             Alert.alert("Erro", error.response?.data || "Não foi possível realizar a operação.");
//         } finally {
//             setIsLoading(false);
//         }
//     };

//     return (
//         <KeyboardAvoidingView
//             behavior={Platform.OS === "ios" ? "padding" : "height"}
//             style={styles.container}
//         >
//             {isLoading &&
//                 <View style={styles.loadingOverlay}>
//                     <ActivityIndicator size="large" color="#FFFFFF" />
//                 </View>
//             }

//             <Image source={require('../../assets/ConectaTrilha.png')} style={styles.logo} resizeMode="contain" />
//             <Text style={styles.title}>{isRegistering ? 'Crie sua Conta' : 'Login'}</Text>

//             {isRegistering && (
//                 <>
//                     <TextInput style={styles.input} placeholder="Nome Completo" value={name} onChangeText={setName} />
//                     <TextInput style={styles.input} placeholder="Nome de Usuário" value={username} onChangeText={setUsername} autoCapitalize="none" />
//                 </>
//             )}

//             <TextInput style={styles.input} placeholder="Email" value={email} onChangeText={setEmail} keyboardType="email-address" autoCapitalize="none" />
//             <TextInput style={styles.input} placeholder="Senha" value={password} onChangeText={setPassword} secureTextEntry />

//             <View style={styles.buttonContainer}>
//                 <Button title={isRegistering ? 'Registrar' : 'Entrar'} onPress={handleAuthAction} color="#055705ff" disabled={isLoading || !apiUrl} />
//             </View>

//             <TouchableOpacity onPress={() => setIsRegistering(!isRegistering)} disabled={isLoading}>
//                 <Text style={styles.toggleText}>
//                     {isRegistering ? 'Já tem uma conta? Faça Login' : 'Não tem uma conta? Registre-se'}
//                 </Text>
//             </TouchableOpacity>
//         </KeyboardAvoidingView>
//     );
// }

// const styles = StyleSheet.create({
//     container: { flex: 1, justifyContent: 'center', padding: 20, backgroundColor: '#f5f5f5' },
//     logo: { width: 120, height: 120, alignSelf: 'center', marginBottom: 20, marginTop: 20 },
//     title: { fontSize: 28, fontWeight: 'bold', textAlign: 'center', marginBottom: 24, color: '#333' },
//     input: { height: 50, borderColor: '#ddd', borderWidth: 1, borderRadius: 8, paddingHorizontal: 15, marginBottom: 12, backgroundColor: '#fff' },
//     buttonContainer: { marginVertical: 8, width: '100%' },
//     toggleText: { color: '#055705ff', textAlign: 'center', marginTop: 20, fontWeight: 'bold' },
//     loadingOverlay: {
//         ...StyleSheet.absoluteFillObject,
//         backgroundColor: 'rgba(0,0,0,0.5)',
//         justifyContent: 'center',
//         alignItems: 'center',
//         zIndex: 10
//     }
// });

import React, { useState } from 'react';
import {
    View,
    Text,
    StyleSheet,
    Image,
    TextInput,
    Button,
    TouchableOpacity,
    Alert,
    ActivityIndicator,
    KeyboardAvoidingView,
    Platform
} from 'react-native';
import { login, register } from '../services/serviceLoginRegister';

export default function LoginScreen() {
    const [isRegistering, setIsRegistering] = useState(false);
    const [name, setName] = useState('');
    const [username, setUsername] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [isLoading, setIsLoading] = useState(false);

    const handleAuthAction = async () => {
        setIsLoading(true);
        try {
            if (isRegistering) {
                if (!name || !username || !email || !password) {
                    Alert.alert("Erro", "Preencha todos os campos para se registrar.");
                    setIsLoading(false);
                    return;
                }
                const response = await register(name, username, email, password);
                console.log("Register response:", response);
                Alert.alert("Sucesso", "Conta criada com sucesso!");
            } else {
                if (!email || !password) {
                    Alert.alert("Erro", "Preencha email e senha!");
                    setIsLoading(false);
                    return;
                }
                const response = await login(email, password);
                console.log("Login response:", response);
                Alert.alert("Sucesso", "Login realizado com sucesso!");
            }
        } catch (error) {
            Alert.alert("Erro", "Não foi possível realizar a operação." && error.response.data);
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <KeyboardAvoidingView
            behavior={Platform.OS === "ios" ? "padding" : "height"}
            style={styles.container}
        >
            {isLoading &&
                <View style={styles.loadingOverlay}>
                    <ActivityIndicator size="large" color="#FFFFFF" />
                </View>
            }

            <Image source={require('../../assets/ConectaTrilha.png')} style={styles.logo} resizeMode="contain" />
            <Text style={styles.title}>{isRegistering ? 'Crie sua Conta' : 'Login'}</Text>

            {isRegistering && (
                <>
                    <TextInput
                        style={styles.input}
                        placeholder="Nome Completo"
                        value={name}
                        onChangeText={setName}
                    />
                    <TextInput
                        style={styles.input}
                        placeholder="Nome de Usuário"
                        value={username}
                        onChangeText={setUsername}
                        autoCapitalize="none"
                    />
                </>
            )}

            <TextInput
                style={styles.input}
                placeholder="Email"
                value={email}
                onChangeText={setEmail}
                keyboardType="email-address"
                autoCapitalize="none"
            />
            <TextInput
                style={styles.input}
                placeholder="Senha"
                value={password}
                onChangeText={setPassword}
                secureTextEntry
            />

            <View style={styles.buttonContainer}>
                <Button
                    title={isRegistering ? 'Registrar' : 'Entrar'}
                    onPress={handleAuthAction}
                    color="#055705ff"
                    disabled={isLoading}
                />
            </View>

            <TouchableOpacity onPress={() => setIsRegistering(!isRegistering)} disabled={isLoading}>
                <Text style={styles.toggleText}>
                    {isRegistering ? 'Já tem uma conta? Faça Login' : 'Não tem uma conta? Registre-se'}
                </Text>
            </TouchableOpacity>
        </KeyboardAvoidingView>
    );
}

const styles = StyleSheet.create({
    container: { flex: 1, justifyContent: 'center', padding: 20, backgroundColor: '#f5f5f5' },
    logo: { width: 120, height: 120, alignSelf: 'center', marginBottom: 20, marginTop: 20 },
    title: { fontSize: 28, fontWeight: 'bold', textAlign: 'center', marginBottom: 24, color: '#333' },
    input: { height: 50, borderColor: '#ddd', borderWidth: 1, borderRadius: 8, paddingHorizontal: 15, marginBottom: 12, backgroundColor: '#fff' },
    buttonContainer: { marginVertical: 8, width: '100%' },
    toggleText: { color: '#055705ff', textAlign: 'center', marginTop: 20, fontWeight: 'bold' },
    loadingOverlay: {
        ...StyleSheet.absoluteFillObject,
        backgroundColor: 'rgba(0,0,0,0.5)',
        justifyContent: 'center',
        alignItems: 'center',
        zIndex: 10
    }
});
