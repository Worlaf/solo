import React from "react";
import { StackScreenProps } from "../routes";
import { StatusBar } from "expo-status-bar";
import { StyleSheet, Text, View } from "react-native";
import { Button } from "react-native-elements";
import Icon from "react-native-vector-icons/FontAwesome";

interface Props extends StackScreenProps<"Home"> {}

export default function Home(props: Props) {
  return (
    <View style={styles.container}>
      <Text>Hello dear friend!</Text>
      <Button
        title="Button!"
        type="outline"
        icon={<Icon name="arrow-right" size={15} color="white" />}
        onPress={() => props.navigation.navigate("Map")}
      />
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#fff",
    alignItems: "center",
    justifyContent: "center",
  },
});
