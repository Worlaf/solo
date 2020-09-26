import React, { useEffect, useState } from "react";
import { StackScreenProps } from "../routes";
import { StyleSheet, Text, ScrollView, Dimensions, View, Button, Alert } from "react-native";
import { ParkObject } from "../api/ParkObject";
import Loader from "../components/Loader";
import { Image } from "react-native-elements";
import { ListModel } from "../api/ListModel";
import { Ticket } from "../api/Ticket";
import axios from "../api/axios";

interface Props extends StackScreenProps<"ParkObject"> {}

export default function ParkObjectDetails(props: Props) {
    const [tickets, setTickets] = useState<Ticket[]>([]);
    const [requestInProgress, setRequestInProgress] = useState(false);

    useEffect(() => {
        updateTickets();
    }, []);

    const updateTickets = () => {
        setRequestInProgress(true);
        axios.get<ListModel<Ticket>>(`tickets/my?parkObjectId=${props.route.params.parkObjectId}`).then((response) => {
            setTickets(response.data.items);
            setRequestInProgress(false);
        });
    };

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
                    {!requestInProgress &&
                        (tickets.length === 0 ? (
                            <BuyTicketsView onTicketsReceived={(tickets) => setTickets(tickets)} parkObjectId={props.route.params.parkObjectId} />
                        ) : (
                            <MyTicketsView tickets={tickets} onTicketClosed={(ticketId) => updateTickets()} />
                        ))}
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
    ticketSection: {
        marginBottom: 5,
        flexDirection: "row",
    },
    ticketCountButton: {
        marginLeft: 5,
        marginRight: 5,
    },
});

function BuyTicketsView(props: { parkObjectId: number; onTicketsReceived(tickets: Ticket[]): void }) {
    const [adultCount, setAdultCount] = useState(0);
    const [childCount, setChildCount] = useState(0);
    const [privilegedCount, setPrivilegedCount] = useState(0);
    const [transactionInProgress, setTransactionInProgress] = useState(false);

    return (
        <View style={{ flexDirection: "column", margin: 5 }}>
            <Text style={{ fontSize: 20, fontWeight: "bold" }}>Билеты</Text>
            <Text>Количество взрослых</Text>
            <View style={styles.ticketSection}>
                <Button onPress={() => setAdultCount(adultCount + 1)} title="+" />
                <Text>{adultCount}</Text>
                <Button
                    onPress={() => {
                        if (adultCount > 0) setAdultCount(adultCount - 1);
                    }}
                    title="-"
                />
            </View>

            <Text>Количество детей</Text>
            <View style={styles.ticketSection}>
                <Button onPress={() => setChildCount(childCount + 1)} title="+" />
                <Text>{childCount}</Text>
                <Button
                    onPress={() => {
                        if (childCount > 0) setChildCount(childCount - 1);
                    }}
                    title="-"
                />
            </View>

            <Text>Количество льготников</Text>
            <View style={styles.ticketSection}>
                <Button onPress={() => setPrivilegedCount(privilegedCount + 1)} title="+" />
                <Text>{privilegedCount}</Text>
                <Button
                    onPress={() => {
                        if (privilegedCount > 0) setPrivilegedCount(privilegedCount - 1);
                    }}
                    title="-"
                />
            </View>

            <Button
                disabled={transactionInProgress}
                title="Купить"
                onPress={() => {
                    setTransactionInProgress(true);
                    axios
                        .post<ListModel<Ticket>>("tickets/buy", {
                            parkObjectId: props.parkObjectId,
                            adultCount: adultCount,
                            childCount: childCount,
                            privilegedCount: privilegedCount,
                        })
                        .then((response) => {
                            setTransactionInProgress(false);
                            props.onTicketsReceived(response.data.items);
                        });
                }}
            />
        </View>
    );
}

function MyTicketsView(props: { tickets: Ticket[]; onTicketClosed(ticketId: number): void }) {
    const [requestInProgress, setRequestInProgress] = useState(false);
    const [zeroTicket, setZeroTicket] = useState<Ticket>();
    const queuePlacesStr = props.tickets
        .filter((t) => !t.closed)
        .sort((a, b) => a.queueNumber - b.queueNumber)
        .map((t) => t.queueNumber)
        .join(", ");

    useEffect(() => setZeroTicket(props.tickets.find((t) => !t.closed && t.queueNumber === 0)), [props.tickets]);

    return (
        <View style={{ flexDirection: "column" }}>
            <Text>{`Места в очереди: ${queuePlacesStr}`}</Text>
            {zeroTicket && (
                <Button
                    disabled={requestInProgress}
                    onPress={() => {
                        setRequestInProgress(true);
                        axios
                            .post(`tickets/close/${zeroTicket.id}`)
                            .then(() => {
                                props.onTicketClosed(zeroTicket.id);
                            })
                            .catch((e) => {
                                Alert.alert(`Failed to close ticket ${zeroTicket.id}`);
                            })
                            .finally(() => setRequestInProgress(false));
                    }}
                    title="Использовать билет"
                />
            )}
        </View>
    );
}
