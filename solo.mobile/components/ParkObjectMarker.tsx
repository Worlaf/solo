import React, { ReactNode, useEffect, useState } from "react";
import { StyleSheet } from "react-native";
import { ParkObjectType } from "../api/ParkObject";
import { FontAwesome } from "@expo/vector-icons";
import { MaterialCommunityIcons } from "@expo/vector-icons";
import { MaterialIcons } from "@expo/vector-icons";
import { Entypo } from "@expo/vector-icons";

export default function ParkObjectMarker(props: { parkObjectType: ParkObjectType }) {
    if (props.parkObjectType === ParkObjectType.Attraction)
        return <MaterialCommunityIcons name="ferris-wheel" size={24} color="black" style={[styles.greenIcon, styles.markerBig]} />;
    else if (props.parkObjectType === ParkObjectType.Sight)
        return <MaterialIcons name="account-balance" size={24} color="black" style={[styles.greenIcon, styles.markerBig]} />;
    else if (props.parkObjectType === ParkObjectType.Shop) return <Entypo name="shop" size={24} color="black" style={[styles.yellowIcon, styles.markerBig]} />;
    else if (props.parkObjectType === ParkObjectType.Food)
        return <MaterialCommunityIcons name="food" size={24} color="black" style={[styles.greenIcon, styles.markerBig]} />;
    else return <FontAwesome name="question" size={24} color="black" style={[styles.greenIcon, styles.markerBig]} />;
}

const styles = StyleSheet.create({
    markerBig: {
        width: 36,
        height: 36,
        borderWidth: 2,
        borderRadius: 30,
        padding: 5,
        textAlignVertical: "center",
        textAlign: "center",
        borderColor: "white",
    },
    greenIcon: {
        backgroundColor: "#bbd444",
    },
    yellowIcon: {
        backgroundColor: "#fcd744",
    },
    orangeIcon: {
        backgroundColor: "#fa7b53",
    },
});
