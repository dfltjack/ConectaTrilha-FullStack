import axios from 'axios';
import * as SecureStore from 'expo-secure-store';

//const API_URL = 'https://localhost:7049/api'; // quando for testar no emulador
// const API_URL = 'http://172.28.8.97:8081'; // ip da rede para dispositivo físico
const API_URL = 'http://172.18.21.3:8081'; // ip da rede faculdade

const Api = axios.create({
    baseUrl: API_URL,
    timeout: 20000, //20 segundos
});
console.log(`[API Service] Conectando à base URL: ${API_URL}`)

Api.interceptors.request.use(config => {
    const token = SecureStore.getItem('token');
    if (token) {
        config.headers.Authorization = `Bearer $(token)`;
    }
    return config;
}, error => {
    console.log(error);
    return Promise.reject(error);
});

export default Api;
