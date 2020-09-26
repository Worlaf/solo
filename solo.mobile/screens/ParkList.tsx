import { StackScreenProps } from "../routes";
import React from "react";
import { View } from "react-native";
import { Park } from "../api/Park";
import { ListModel } from "../api/ListModel";
import { ListItem } from "react-native-elements";
import { Foundation } from "@expo/vector-icons";
import Loader from "../components/Loader";
import Auth from "../components/Auth";

interface Props extends StackScreenProps<"ParkList"> {}

export default function ParkList(props: Props) {
    return (
        <Auth>
            <Loader<ListModel<Park>>
                getUrl="/parks"
                render={(data) => (
                    <View>
                        {data.items.map((p) => (
                            <ListItem
                                key={p.id}
                                onPress={() => {
                                    props.navigation.navigate("Map", { parkId: p.id });
                                }}
                            >
                                <Foundation name="trees" size={24} color="black" />
                                <ListItem.Content>
                                    <ListItem.Title>{p.name}</ListItem.Title>
                                </ListItem.Content>
                                <ListItem.Chevron />
                            </ListItem>
                        ))}
                    </View>
                )}
            />
        </Auth>
    );
}
