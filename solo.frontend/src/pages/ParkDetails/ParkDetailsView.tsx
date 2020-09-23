import React, { createRef, useEffect, useState } from "react";
import { ParkObject, ParkObjectType } from "../../api/ParkObject";
import { Park } from "../../api/Park";
import Grid from "@material-ui/core/Grid/Grid";
import { Box, Typography } from "@material-ui/core";
import { GeoPoint } from "../../api/GeoPoint";
import { getBottomRight, getTopLeft } from "../../api/Region";
import { YMaps, Map, Placemark, TypeSelector } from "react-yandex-maps";
import { Route, Switch, useHistory, useRouteMatch } from "react-router-dom";
import { buildUrl, routes } from "../../routes/routes";
import ParkObjectList from "./ParkObjectList";
import ParkObjectDetails from "./ParkObjectDetails";
import axios from "../../api/axios";

interface Props {
    objects: ParkObject[];
    park: Park;
}

const mapWidth = window.innerWidth - 800;
const mapHeight = 800;

export default function ParkDetailsView(props: Props) {
    const [objects, setObjects] = useState<ParkObject[]>(props.objects);
    const [isMapEventAdded, setIsMapEventAdded] = useState(false);
    const [isSaving, setIsSaving] = useState(false);
    const [lastMapClickLocation, setLastMapClickLocation] = useState<GeoPoint>();

    const history = useHistory();
    const route = useRouteMatch();

    const bounds = [getTopLeft(props.park.region), getBottomRight(props.park.region)];

    useEffect(() => setLastMapClickLocation(undefined), [route.url]);

    const onMapClicked = (point: GeoPoint) => {
        setLastMapClickLocation(point);
    };

    const onPlacemarkClicked = (object?: ParkObject) => {
        if (object) {
            setLastMapClickLocation(undefined);
            history.push(buildUrl(routes.parkObjectDetails, props.park.id, object.id));
        }
    };

    return (
        <YMaps>
            <Grid container direction="column" justify="flex-start" alignContent="stretch">
                <Typography variant="h3">{props.park.name}</Typography>
                <Grid container direction="row" justify="flex-start" alignContent="stretch">
                    <Map
                        width={mapWidth}
                        height={mapHeight}
                        defaultState={{
                            bounds: [
                                [bounds[0].latitude, bounds[0].longitude],
                                [bounds[1].latitude, bounds[1].longitude],
                            ],
                        }}
                        instanceRef={(inst) => {
                            if (inst === null || isMapEventAdded) return;
                            (inst as any).events.add("click", function (event: any) {
                                var coords = event.get("coords") as [number, number];
                                onMapClicked({ latitude: coords[0], longitude: coords[1] });
                            });
                            setIsMapEventAdded(true);
                        }}
                    >
                        {lastMapClickLocation && <Placemark geometry={[lastMapClickLocation.latitude, lastMapClickLocation.longitude]} />}
                        {objects.map((o) => (
                            <Placemark
                                key={o.id}
                                geometry={[o.location.latitude, o.location.longitude]}
                                options={{
                                    preset: "islands#greenStretchyIcon",
                                }}
                                properties={{
                                    iconContent: o.name,
                                }}
                                instanceRef={(inst: any) => {
                                    if (inst === null) return;
                                    (inst as any).events.add("click", function (event: any) {
                                        onPlacemarkClicked(o);
                                    });
                                }}
                            />
                        ))}
                        <TypeSelector />
                    </Map>
                    <Box flex={1}>
                        <Switch>
                            <Route
                                exact
                                path={routes.parkObjectDetails}
                                render={(p) => {
                                    var objectId = p.match.params.objectId;

                                    return (
                                        <ParkObjectDetails
                                            isSaving={isSaving}
                                            object={
                                                objectId !== "new"
                                                    ? objects.find((o) => o.id == objectId)!
                                                    : {
                                                          id: 0,
                                                          administrationDescriptionMarkdown: "",
                                                          location: { latitude: 0, longitude: 0 },
                                                          name: "",
                                                          parkId: props.park.id,
                                                          priceForAdults: 0,
                                                          priceForChildren: 0,
                                                          publicDescriptionMarkdown: "",
                                                          workScheduleJson: "",
                                                          imageUrl: "",
                                                          type: ParkObjectType.None,
                                                      }
                                            }
                                            onSave={(o) => {
                                                setIsSaving(true);
                                                function finishSavingAndRedirectToList() {
                                                    history.push(buildUrl(routes.parkDetails, props.park.id));
                                                    setIsSaving(false);
                                                    setLastMapClickLocation(undefined);
                                                }
                                                if (o.id === 0) {
                                                    axios
                                                        .post<ParkObject>(`parks/${props.park.id}/parkObjects/`, o)
                                                        .then((result) => {
                                                            setObjects([...objects, result.data]);
                                                            finishSavingAndRedirectToList();
                                                        })
                                                        .handleAxiosError();
                                                } else {
                                                    axios
                                                        .put<ParkObject>(`parks/${props.park.id}/parkObjects/${o.id}`, o)
                                                        .then((result) => {
                                                            setObjects(objects.map((oo) => (oo.id === result.data.id ? result.data : oo)));
                                                            finishSavingAndRedirectToList();
                                                        })
                                                        .handleAxiosError();
                                                }
                                            }}
                                            markerLocation={lastMapClickLocation}
                                        />
                                    );
                                }}
                            />
                            <Route>
                                <ParkObjectList objects={objects} parkId={props.park.id} />
                            </Route>
                        </Switch>
                    </Box>
                </Grid>
            </Grid>
        </YMaps>
    );
}
