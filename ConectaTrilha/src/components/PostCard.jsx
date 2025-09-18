import React from "react";
import { View, Text, Image, TouchableOpacity, StyleSheet } from "react-native";

export default function PostCard({ item, onLike, onPostPress }) {
    return (
        <TouchableOpacity onPress={() => onPostPress(item)} style={styles.card}>
            <View style={styles.header}>
                <Image source={{ uri: item.author.avatar }} style={styles.avatar} />
                <Text style={styles.username}>{item.author.username}</Text>
            </View>
            <Text style={styles.description}>{item.description}</Text>
            {item.images.length > 0 && <Image source={{ uri: item.images[0] }} style={styles.postImage} />}
            <View style={styles.footer}>
                <TouchableOpacity onPress={() => onLike(item.id)}>
                    <Text style={{ color: item.isLiked ? 'red' : 'gray' }}>❤️ {item.likesCount}</Text>
                </TouchableOpacity>
                <Text>💬 {item.commentsCount}</Text>
            </View>
        </TouchableOpacity>
    );
}

const styles = StyleSheet.create({
    card: { padding: 10, backgroundColor: '#fff', marginHorizontal: 10, borderRadius: 8 },
    header: { flexDirection: 'row', alignItems: 'center', marginBottom: 5 },
    avatar: { width: 40, height: 40, borderRadius: 20, marginRight: 10 },
    username: { fontWeight: 'bold' },
    description: { marginBottom: 5 },
    postImage: { width: '100%', height: 200, borderRadius: 8, marginBottom: 5 },
    footer: { flexDirection: 'row', justifyContent: 'space-between' },
});
