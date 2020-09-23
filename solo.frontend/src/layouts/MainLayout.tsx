import { Box, Grid } from "@material-ui/core";
import React from "react";

export default function MainLayout(props: {
  children: React.ReactElement | React.ReactElement[] | null;
}) {
  return (
    <Grid container direction="row" justify="center">
      {props.children}
    </Grid>
  );
}
