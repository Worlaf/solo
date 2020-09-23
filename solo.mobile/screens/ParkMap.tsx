import React, { useEffect, useState } from "react";
import { Dimensions } from "react-native";
import { StackScreenProps } from "../routes";
import { StyleSheet, Text, View } from "react-native";
import MapView, { Marker } from "react-native-maps";
import * as Location from "expo-location";
import { ParkObject } from "../api/ParkObject";
import { ListModel } from "../api/ListModel";
import { Park } from "../api/Park";
import { getLatitudeDiff, getLongitudeDiff, getRegionCenter } from "../api/Region";
import Loader from "../components/Loader";
import ParkObjectMarker from "../components/ParkObjectMarker";

interface Props extends StackScreenProps<"Map"> {}

export default function ParkMap(props: Props) {
    const [, setLocation] = useState<Location.LocationObject>();
    const [errorMsg, setErrorMsg] = useState<string>();

    useEffect(() => {
        (async () => {
            let { status } = await Location.requestPermissionsAsync();
            if (status !== "granted") {
                setErrorMsg("Permission to access location was denied");
            }

            let location = await Location.getCurrentPositionAsync({});
            setLocation(location);
        })();
    }, []);

    return (
        <Loader<Park>
            getUrl={`parks/${props.route.params.parkId}`}
            render={(park) => (
                <Loader<ListModel<ParkObject>>
                    getUrl={`parks/${props.route.params.parkId}/parkObjects`}
                    render={(parkObjectList) => (
                        <View style={styles.container}>
                            <ParkMapView
                                park={park}
                                parkObjects={parkObjectList.items}
                                location={undefined}
                                onParkObjectClicked={(o) => props.navigation.navigate("ParkObject", { parkId: o.parkId, parkObjectId: o.id })}
                            />
                            {errorMsg && <Text>{errorMsg}</Text>}
                        </View>
                    )}
                />
            )}
        />
    );
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: "#fff",
        alignItems: "center",
        justifyContent: "center",
    },
    mapStyle: {
        width: Dimensions.get("window").width,
        height: Dimensions.get("window").height,
    },
    mapModal: {
        flex: 1,
        justifyContent: "flex-end",
        alignItems: "center",
        marginTop: 22,
        width: Dimensions.get("window").width,
    },
});

function ParkMapView(props: { park: Park; parkObjects: ParkObject[]; location?: Location.LocationObject; onParkObjectClicked(parkObject: ParkObject): void }) {
    const parkCenter = getRegionCenter(props.park.region);

    return (
        <View>
            <MapView
                style={styles.mapStyle}
                provider={"google"}
                showsPointsOfInterest={false}
                showsBuildings={false}
                showsIndoors={false}
                showsScale={true}
                initialRegion={{
                    latitude: parkCenter.latitude,
                    longitude: parkCenter.longitude,
                    latitudeDelta: getLatitudeDiff(props.park.region),
                    longitudeDelta: getLongitudeDiff(props.park.region),
                }}
                region={
                    (props.location && {
                        latitude: props.location.coords.latitude,
                        longitude: props.location.coords.longitude,
                        latitudeDelta: 0.0922,
                        longitudeDelta: 0.421,
                    }) ||
                    undefined
                }
            >
                {props.location && (
                    <Marker
                        coordinate={{
                            latitude: props.location.coords.latitude,
                            longitude: props.location.coords.longitude,
                        }}
                    />
                )}
                {props.parkObjects.map((po) => (
                    <Marker key={po.id} coordinate={po.location} title={po.name} onPress={() => props.onParkObjectClicked(po)}>
                        <ParkObjectMarker parkObjectType={po.type} />
                    </Marker>
                ))}
            </MapView>
        </View>
    );
}
