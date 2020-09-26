import React, { useCallback, useEffect, useState } from "react";
import { ParkObject } from "../../api/ParkObject";
import { Box, Button, CircularProgress, Container, createStyles, InputAdornment, makeStyles, TextField, Theme, Typography } from "@material-ui/core";
import nameof from "ts-nameof.macro";
import { GeoPoint } from "../../api/GeoPoint";

interface Props {
    object: ParkObject;
    onSave(object: ParkObject): void;
    markerLocation?: GeoPoint;
    isSaving?: boolean;
}

export default function ParkObjectDetails(props: Props) {
    const [object, setObject] = useState(props.object);
    const [propertyInEditMode, setPropertyInEditMode] = useState<string>();

    useEffect(() => {
        if (props.markerLocation && propertyInEditMode == nameof(object.location)) {
            setObject({ ...object, location: props.markerLocation });
        }
    }, [props.markerLocation]);

    const restoreProperty = (propertyName: string) => {
        setObject({
            ...object,
            [propertyName]: (props.object as any)[propertyName],
        });
    };

    useEffect(() => setObject(props.object), [props.object]);

    function renderProperty(title: string, propertyName: string, displayView: React.ReactElement, editView: React.ReactElement) {
        return (
            <ParkObjectField
                title={title}
                fieldKey={propertyName}
                onCancelClicked={(field) => {
                    restoreProperty(field);
                    setPropertyInEditMode(undefined);
                }}
                onEditClicked={(field) => setPropertyInEditMode(field)}
                onSaveClicked={() => setPropertyInEditMode(undefined)}
                isInEditMode={!props.isSaving && propertyInEditMode === propertyName}
                displayView={displayView}
                editView={editView}
            />
        );
    }

    function renderTextField(title: string, propertyName: string) {
        return renderProperty(
            title,
            propertyName,
            <Box>{(object as any)[propertyName]}</Box>,
            <TextField fullWidth value={(object as any)[propertyName]} onChange={(e) => setObject({ ...object, [propertyName]: e.target.value })}></TextField>
        );
    }

    function renderMultilineTextField(title: string, propertyName: string) {
        return renderProperty(
            title,
            propertyName,
            <Box
                dangerouslySetInnerHTML={{
                    __html: (object as any)[propertyName]?.replace(/\n/gm, "<br />"),
                }}
            ></Box>,
            <TextField
                fullWidth
                multiline
                rows={4}
                value={(object as any)[propertyName]}
                onChange={(e) => setObject({ ...object, [propertyName]: e.target.value })}
            ></TextField>
        );
    }

    return (
        <Box>
            <Container>
                <Typography variant="h4">Редактирование объекта</Typography>
                <hr />
            </Container>

            {renderTextField("Название", nameof(object.name))}
            {renderMultilineTextField("Служебная информация", nameof(object.administrationDescriptionMarkdown))}
            {renderMultilineTextField("Информация для посетителей", nameof(object.publicDescriptionMarkdown))}
            {renderProperty(
                "Координаты",
                nameof(object.location),
                <Box>
                    {object.location.latitude}, {object.location.longitude}
                </Box>,
                <Box>
                    <TextField
                        type="number"
                        value={object.location.latitude}
                        onChange={(e) =>
                            setObject({
                                ...object,
                                location: {
                                    latitude: parseFloat(e.target.value),
                                    longitude: object.location.longitude,
                                },
                            })
                        }
                        InputProps={{
                            startAdornment: <InputAdornment position="start">Lat</InputAdornment>,
                        }}
                    />
                    <TextField
                        type="number"
                        value={object.location.longitude}
                        onChange={(e) =>
                            setObject({
                                ...object,
                                location: {
                                    longitude: parseFloat(e.target.value),
                                    latitude: object.location.longitude,
                                },
                            })
                        }
                        InputProps={{
                            startAdornment: <InputAdornment position="start">Lon</InputAdornment>,
                        }}
                    />
                </Box>
            )}
            <Container style={{ marginTop: "10px" }}>
                <Button color="primary" variant="contained" onClick={() => props.onSave(object)} disabled={props.isSaving}>
                    {props.isSaving && <CircularProgress size="small" />}
                    Сохранить изменения и вернуться к списку
                </Button>
            </Container>

            <Container>
                <br />
                <Typography variant="h5">Статистика по билетам: </Typography>
                Всего билетов куплено: {object.ticketsTotal}
                <br />
                Билетов использовано: {object.ticketsClosed}
            </Container>
        </Box>
    );
}

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        containerWithSpacing: {
            "& > *": {
                margin: theme.spacing(1),
            },
            "& > *:first-child": {
                marginLeft: 0,
            },
            textAlign: "right",
            borderBottomColor: theme.palette.grey[200],
            borderBottomWidth: 1,
            borderBottomStyle: "solid",
        },
    })
);

function ParkObjectField(props: {
    fieldKey: string;
    title: string;
    onEditClicked(fieldKey: string): void;
    onSaveClicked(fieldKey: string): void;
    onCancelClicked(fieldKey: string): void;
    isInEditMode: boolean;
    displayView: React.ReactElement;
    editView: React.ReactElement;
}) {
    const classes = useStyles();

    return (
        <Container>
            <Box>
                <Typography variant="caption">{props.title}</Typography>
            </Box>
            {props.isInEditMode ? props.editView : props.displayView}
            {props.isInEditMode ? (
                <Box className={classes.containerWithSpacing}>
                    <Button color="primary" variant="contained" onClick={() => props.onSaveClicked(props.fieldKey)}>
                        Сохранить
                    </Button>
                    <Button color="secondary" variant="outlined" onClick={() => props.onCancelClicked(props.fieldKey)}>
                        Отменить
                    </Button>
                </Box>
            ) : (
                <Box className={classes.containerWithSpacing}>
                    <Button color="primary" variant="contained" onClick={() => props.onEditClicked(props.fieldKey)}>
                        Редактировать
                    </Button>
                </Box>
            )}
        </Container>
    );
}
