import React, { useState, useEffect, useCallback } from "react";
import { View, Text, StyleSheet, FlatList, Alert, ActivityIndicator } from "react-native";
import PostCard from "../components/PostCard.jsx";

// --- DADOS MOCKADOS PARA TESTE ---
const mockPosts = [
    {
        id: '1',
        description: 'A vista do cume é simplesmente espetacular! Trilha difícil mas a recompensa é enorme.',
        images: ['https://s2.glbimg.com/Q21x-mKei-6y6V4G-j06sTJx0G8=/e.glbimg.com/og/ed/f/original/2019/07/26/parque-nacional-do-itatiaia-pico-das-agulhas-negras.jpg'],
        createdAt: new Date().toISOString(),
        author: { id: 'user1', username: 'joaosilva', avatar: 'https://i.pravatar.cc/50?u=joao' },
        trail: { id: 'trail1', name: 'Pico das Agulhas Negras', city: 'Itatiaia', state: 'RJ', difficulty: 'Difícil' },
        likesCount: 127,
        commentsCount: 23,
        isLiked: false,
        isBookmarked: false,
    },
    {
        id: '2',
        description: 'Dia perfeito para relaxar e curtir a natureza. A água estava gelada, mas valeu a pena!',
        images: ['https://www.qualviagem.com.br/wp-content/uploads/2021/01/Cachoeira-veu-da-Noiva-RJ-serra-da-bocaina.jpg'],
        createdAt: new Date().toISOString(),
        author: { id: 'user2', username: 'mariasouza', avatar: 'https://i.pravatar.cc/50?u=maria' },
        trail: { id: 'trail2', name: 'Cachoeira Véu da Noiva', city: 'Serra da Bocaina', state: 'RJ', difficulty: 'Fácil' },
        likesCount: 254,
        commentsCount: 42,
        isLiked: true,
        isBookmarked: true,
    }
];
// --- FIM DOS DADOS MOCKADOS ---

export default function HomeScreen() {
    const [posts, setPosts] = useState([]);
    const [loading, setLoading] = useState(false);
    const [refreshing, setRefreshing] = useState(false);

    const fetchPosts = useCallback((isRefreshing = false) => {
        if (loading && !isRefreshing) return;
        setLoading(true);
        if (isRefreshing) setRefreshing(true);

        // Simula chamada de rede
        setTimeout(() => {
            try {
                setPosts(mockPosts);
            } catch (error) {
                Alert.alert("Erro", "Não foi possível carregar o feed.");
            } finally {
                setLoading(false);
                if (isRefreshing) setRefreshing(false);
            }
        }, 1000);
    }, [loading]);

    useEffect(() => {
        fetchPosts(true); // carga inicial
    }, []);

    const handleLike = useCallback((postId) => {
        setPosts(prevPosts =>
            prevPosts.map(p =>
                p.id === postId ? { ...p, isLiked: !p.isLiked, likesCount: p.isLiked ? p.likesCount - 1 : p.likesCount + 1 } : p
            )
        );
    }, []);

    const handleRefresh = () => {
        fetchPosts(true);
    };

    return (
        <View style={styles.container}>
            <FlatList
                data={posts}
                keyExtractor={item => item.id}
                renderItem={({ item }) => (
                    <PostCard
                        item={item}
                        onLike={handleLike}
                        onPostPress={() => Alert.alert("Navegação", `Ir para detalhes do post ${item.id}`)}
                    />
                )}
                refreshing={refreshing}
                onRefresh={handleRefresh}
                ItemSeparatorComponent={() => <View style={styles.separator} />}
                ListEmptyComponent={() => !loading && <Text style={styles.emptyText}>Nenhum post encontrado.</Text>}
                ListHeaderComponent={() => loading && posts.length === 0 ? <ActivityIndicator style={{ marginTop: 20 }} size="large" /> : null}
            />
        </View>
    );
}

const styles = StyleSheet.create({
    container: { flex: 1, backgroundColor: '#f0f2f5' },
    separator: { height: 8, backgroundColor: '#e0e0e0' },
    emptyText: { textAlign: 'center', marginTop: 50, fontSize: 16, color: 'gray' },
});
