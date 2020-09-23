import React from "react";
import { ParkObject } from "../../api/ParkObject";
import { Park } from "../../api/Park";
import Grid from "@material-ui/core/Grid/Grid";
import { Box, Container, List, ListItem, ListItemIcon, ListItemText, Typography } from "@material-ui/core";
import { GeoPoint } from "../../api/GeoPoint";
import { Link } from "react-router-dom";
import { buildUrl, routes } from "../../routes/routes";
import AccountBalanceIcon from "@material-ui/icons/AccountBalance";
import AddIcon from "@material-ui/icons/Add";

interface Props {
    parkId: number;
    objects: ParkObject[];
}

export default function ParkObjectList(props: Props) {
    return (
        <Container>
            <Typography variant="h4">Объекты парка</Typography>
            <List component="nav">
                <ListItem button component={Link} to={buildUrl(routes.parkObjectDetails, props.parkId, "new")}>
                    <ListItemIcon>
                        <AddIcon />
                    </ListItemIcon>
                    <ListItemText primary="Добавить объект" />
                </ListItem>
                {props.objects.map((o) => (
                    <ListItem button component={Link} key={o.id} to={buildUrl(routes.parkObjectDetails, props.parkId, o.id)}>
                        <ListItemIcon>
                            <AccountBalanceIcon />
                        </ListItemIcon>
                        <ListItemText primary={o.name} secondary={o.publicDescriptionMarkdown} />
                    </ListItem>
                ))}
            </List>
        </Container>
    );
}
