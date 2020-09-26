import { useNavigation } from "@react-navigation/native";
import React, { ReactElement } from "react";
import { AuthenticationContext } from "../AuthenticationContext";

export default function Auth(props: { children: ReactElement | ReactElement[] }) {
    const navigation = useNavigation();

    return (
        <AuthenticationContext.Consumer>
            {(ctx) => {
                if (ctx.email === null) {
                    navigation.navigate("SignUp");
                }

                return props.children;
            }}
        </AuthenticationContext.Consumer>
    );
}
