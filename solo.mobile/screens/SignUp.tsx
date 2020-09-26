import { useNavigation } from "@react-navigation/native";
import React, { useState } from "react";
import { Button, View } from "react-native";
import { Input } from "react-native-elements";
import { AuthenticationContext } from "../AuthenticationContext";

export default function SignUp() {
    const [email, setEmail] = useState<string>("");
    const navigation = useNavigation();

    return (
        <AuthenticationContext.Consumer>
            {(auth) => (
                <View>
                    <Input value={email} onChangeText={(text) => setEmail(text)} />
                    <Button
                        title="Sign up"
                        onPress={() => {
                            auth.setEmail(email);
                            navigation.goBack();
                        }}
                    />
                </View>
            )}
        </AuthenticationContext.Consumer>
    );
}
