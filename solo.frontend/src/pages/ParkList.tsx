import { Box, Grid, Link, Typography } from "@material-ui/core";
import React from "react";
import { Park } from "../api/Park";
import DataProvider from "../components/DataProvider";
import { Link as RouterLink } from "react-router-dom";
import { buildUrl, routes } from "../routes/routes";

export default function ParkList() {
    return (
        <DataProvider<Park>
            getUrl="parks"
            render={(data) => (
                <Grid container direction="column" justify="center" alignItems="center">
                    <Typography variant="h3">Выберите парк для перехода к странице администрирования:</Typography>
                    <Grid container direction="row" justify="center">
                        {data.items.map((i) => (
                            <Box margin="1em">
                                <Link style={{ fontSize: "2em" }} key={i.id} component={RouterLink} to={buildUrl(routes.parkDetails, i.id)}>
                                    {i.name}
                                </Link>
                            </Box>
                        ))}
                    </Grid>
                </Grid>
            )}
        />
    );
}
