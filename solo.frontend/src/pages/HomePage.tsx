import { Box, Grid, Link, Typography } from "@material-ui/core";
import React from "react";
import { Link as RouterLink } from "react-router-dom";
import { buildUrl, routes } from "../routes/routes";

export default function HomePage() {
    return (
        <Grid container direction="column" justify="center" alignItems="center">
            <Typography variant="h3">Добро пожаловать!</Typography>
            <Grid container direction="row" justify="center">
                <Box margin="1em">
                    <Link style={{ fontSize: "2em" }} component={RouterLink} to={buildUrl(routes.parkList)}>
                        Открыть список парков
                    </Link>
                </Box>
                <Box margin="1em">
                    <Link
                        style={{ fontSize: "2em" }}
                        href="https://drive.google.com/file/d/1SwGVIXAMDw3lVQuDiJlutlIGl4GS_QmC/view?usp=sharing"
                        target="__blank"
                    >
                        Скачать приложение
                    </Link>
                </Box>
            </Grid>
        </Grid>
    );
}
