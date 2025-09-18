import React, { createContext, useState, useEffect, useContext } from "react";
import * as Google from "expo-auth-session/providers/google";
import * as WebBrowser from "expo-web-browser";
import * as SecureStore from "expo-secure-store";
import { Alert } from "react-native";
import Api from "../helpers/api";

WebBrowser.maybeCompleteAuthSession();

const AuthContext = createContext({});

export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(null);
    const [token, setToken] = useState(null);
    const [loading, setLoading] = useState(true);

    // login com Google (IDs devem ser configurados no console do Google)
    const [request, response, promptAsync] = Google.useAuthRequest({
        expoClientId: "",
        iosClientId: "",
        androidClientId: "",
    });

    const handleAuthSuccess = async (data) => {
        const { username, token: JwtToken } = data;
        setToken(JwtToken);
        setUser({ username });

        Api.defaults.headers.common["Authorization"] = `Bearer ${JwtToken}`;
        await SecureStore.setItemAsync("user_token", JwtToken);
    };

    // --- Login com e-mail/senha ---
    const signIn = async (email, password) => {
        console.log("--- Iniciando login ---");
        console.log("Email:", email);
        console.log("API Base:", Api.defaults.baseURL);

        try {
            const response = await Api.post("/api/Account/login", { email, password });
            console.log("Login OK:", response.data);
            handleAuthSuccess(response.data);
        } catch (error) {
            console.error("--- ERRO no login ---");
            if (error.response) {
                console.error("Dados:", error.response.data);
                console.error("Status:", error.response.status);
            } else if (error.request) {
                console.error("Sem resposta do servidor:", error.request);
            } else {
                console.error("Erro:", error.message);
            }
            Alert.alert("Falha no Login", "Verifique o terminal para mais detalhes.");
        }
    };

    // --- Registro com e-mail/senha ---
    const signUp = async (name, username, email, password) => {
        console.log("--- Iniciando registro ---");
        try {
            const response = await Api.post("/api/Account/register", {
                name,
                username,
                email,
                password,
            });
            console.log("Registro OK:", response.data);
            handleAuthSuccess(response.data);
        } catch (error) {
            console.error("--- ERRO no registro ---");
            if (error.response) {
                console.error("Dados:", error.response.data);
                console.error("Status:", error.response.status);
            } else if (error.request) {
                console.error("Sem resposta do servidor:", error.request);
            } else {
                console.error("Erro:", error.message);
            }
            Alert.alert(
                "Falha no Registro",
                error.response?.data || "Não foi possível criar a conta."
            );
        }
    };

    // --- Login com Google ---
    const signInWithGoogle = async () => {
        try {
            const result = await promptAsync();
            if (result.type === "success") {
                const { authentication } = result;

                // Buscar dados do Google
                const userInfoResponse = await fetch(
                    "https://www.googleapis.com/oauth2/v1/userinfo",
                    {
                        headers: { Authorization: `Bearer ${authentication.accessToken}` },
                    }
                );
                const googleUserInfo = await userInfoResponse.json();

                // Enviar para backend
                const backendResponse = await Api.post("/api/Account/social-login", {
                    email: googleUserInfo.email,
                    name: googleUserInfo.name,
                    avatar: googleUserInfo.picture,
                });

                handleAuthSuccess(backendResponse.data);
            }
        } catch (error) {
            console.error("Erro no login com Google:", error.response?.data || error.message);
            Alert.alert("Falha no Login", "Não foi possível autenticar com o Google.");
        }
    };

    // --- Logout ---
    const signOut = async () => {
        setUser(null);
        setToken(null);
        delete Api.defaults.headers.common["Authorization"];
        await SecureStore.deleteItemAsync("user_token");
    };

    // --- Carregar token salvo ---
    useEffect(() => {
        async function loadStoredToken() {
            const storedToken = await SecureStore.getItemAsync("user_token");
            if (storedToken) {
                setToken(storedToken);
                Api.defaults.headers.common["Authorization"] = `Bearer ${storedToken}`;
            }
            setLoading(false);
        }
        loadStoredToken();
    }, []);

    return (
        <AuthContext.Provider
            value={{ user, token, loading, signIn, signUp, signInWithGoogle, signOut }}
        >
            {children}
        </AuthContext.Provider>
    );
};

export default AuthContext;
