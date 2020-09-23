import "react-native-gesture-handler";
import React from "react";
import { NavigationContainer } from "@react-navigation/native";
import { createStackNavigator } from "@react-navigation/stack";
import { RootStackParamsList } from "./routes";
import Home from "./screens/Home";
import ParkMap from "./screens/ParkMap";
import ParkList from "./screens/ParkList";
import ParkObjectDetails from "./screens/ParkObjectDetails";

const RootStack = createStackNavigator<RootStackParamsList>();

export default function App() {
    return (
        <NavigationContainer>
            <RootStack.Navigator initialRouteName="ParkList">
                <RootStack.Screen name="Home" component={Home} options={{ title: "" }} />
                <RootStack.Screen name="Map" component={ParkMap} options={{ title: "Карта парка" }} />
                <RootStack.Screen name="ParkList" component={ParkList} options={{ title: "Доступные парки" }} />
                <RootStack.Screen name="ParkObject" component={ParkObjectDetails} options={{ title: "Карточка объекта" }} />
            </RootStack.Navigator>
        </NavigationContainer>
    );
}
