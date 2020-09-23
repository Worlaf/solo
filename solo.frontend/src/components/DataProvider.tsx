import { Box, Grid } from "@material-ui/core";
import CircularProgress from "@material-ui/core/CircularProgress/CircularProgress";
import React, { useEffect, useState } from "react";
import axios from "../api/axios";
import { ListModel } from "../api/ListModel";

interface DataProviderProps<TDataItem> {
  render(data: { items: TDataItem[] }): React.ReactElement | null;
  getUrl: string;
}

export default function DataProvider<TDataItem>(
  props: DataProviderProps<TDataItem>
) {
  const [data, setData] = useState<{ items: TDataItem[] }>();

  useEffect(() => {
    axios
      .get<ListModel<TDataItem>>(props.getUrl)
      .then((r) => setData(r.data))
      .handleAxiosError();
  }, [props.getUrl]);

  if (data !== undefined) {
    return props.render(data!);
  } else {
    return (
      <Grid container direction="column" justify="center" alignItems="center">
        <CircularProgress />
      </Grid>
    );
  }
}
