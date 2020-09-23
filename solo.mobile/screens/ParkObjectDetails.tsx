import React from "react";
import { StackScreenProps } from "../routes";
import { StyleSheet, Text, ScrollView, Dimensions, View } from "react-native";
import { ParkObject } from "../api/ParkObject";
import Loader from "../components/Loader";
import { Image } from "react-native-elements";

interface Props extends StackScreenProps<"ParkObject"> {}

export default function ParkObjectDetails(props: Props) {
    return (
        <Loader<ParkObject>
            getUrl={`parks/${props.route.params.parkId}/parkObjects/${props.route.params.parkObjectId}`}
            render={(parkObject) => (
                <ScrollView>
                    {parkObject.imageUrl && (
                        <Image style={{ width: Dimensions.get("window").width, height: 200, resizeMode: "cover" }} source={{ uri: parkObject.imageUrl }} />
                    )}
                    <View style={{ padding: 5 }}>
                        <Text style={styles.titleText}>{parkObject.name}</Text>
                        <Text>{parkObject.publicDescriptionMarkdown}</Text>
                    </View>
                </ScrollView>
            )}
        />
    );
}

const styles = StyleSheet.create({
    titleText: {
        fontSize: 20,
        fontWeight: "bold",
    },
});
