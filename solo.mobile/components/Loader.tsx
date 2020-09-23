import React, { useEffect, useState } from "react";
import { ActivityIndicator, View, StyleSheet } from "react-native";
import axios from "../api/axios";

export default function Loader<TData>(props: { getUrl: string; render(data: TData): React.ReactElement }) {
    const [data, setData] = useState<TData>();

    useEffect(() => {
        axios.get<TData>(props.getUrl).then((response) => setData(response.data));
    }, []);

    if (data === undefined)
        return (
            <View style={[styles.container, styles.horizontal]}>
                <ActivityIndicator size="large" color="#0000ff" />
            </View>
        );

    return props.render(data);
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        justifyContent: "center",
    },
    horizontal: {
        flexDirection: "row",
        justifyContent: "space-around",
        padding: 10,
    },
});
