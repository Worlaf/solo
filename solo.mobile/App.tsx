import "react-native-gesture-handler";
import React, { useState } from "react";
import { NavigationContainer } from "@react-navigation/native";
import { createStackNavigator } from "@react-navigation/stack";
import { RootStackParamsList } from "./routes";
import Home from "./screens/Home";
import ParkMap from "./screens/ParkMap";
import ParkList from "./screens/ParkList";
import ParkObjectDetails from "./screens/ParkObjectDetails";
import Auth from "./components/Auth";
import SignUp from "./screens/SignUp";
import { AuthenticationContext } from "./AuthenticationContext";
import axios from "./api/axios";

const RootStack = createStackNavigator<RootStackParamsList>();

export default function App() {
    const [userEmail, setUserEmail] = useState<string | null>(null);

    return (
        <NavigationContainer>
            <AuthenticationContext.Provider
                value={{
                    email: userEmail,
                    setEmail: (email) => {
                        setUserEmail(userEmail);
                        axios.defaults.headers["Authorization"] = email;
                    },
                }}
            >
                <RootStack.Navigator initialRouteName="ParkList">
                    <RootStack.Screen name="Home" component={Home} options={{ title: "" }} />
                    <RootStack.Screen name="Map" component={ParkMap} options={{ title: "Карта парка" }} />
                    <RootStack.Screen name="ParkList" component={ParkList} options={{ title: "Доступные парки" }} />
                    <RootStack.Screen name="ParkObject" component={ParkObjectDetails} options={{ title: "Карточка объекта" }} />
                    <RootStack.Screen name="SignUp" component={SignUp} options={{ title: "Вход" }} />
                </RootStack.Navigator>
            </AuthenticationContext.Provider>
        </NavigationContainer>
    );
}
