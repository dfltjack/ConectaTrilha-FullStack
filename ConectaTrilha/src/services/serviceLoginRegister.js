import axios from 'axios';

const Api = axios.create({
    baseURL: 'http://172.18.21.3:5130/api', // IP do PC + porta do backend
});

export default Api;

export async function login(email, password) {
    try {
        const response = await Api.post('/account/login', { email, password });
        return response.data;
    } catch (error) {
        console.log("Erro Axios:", error.message);
        if (error.response) console.log("Resposta da API:", error.response.data);
        throw error; // joga o erro pra cima
    }
}

export async function register(name, username, email, password) {
    try {
        const response = await Api.post('/account/register', { name, username, email, password });
        return response.data;
    } catch (error) {
        console.log("Erro Axios:", error.message);
        if (error.response) console.log("Resposta da API:", error.response.data);
        throw error;
    }
}
