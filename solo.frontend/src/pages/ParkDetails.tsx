import { Grid, Typography } from "@material-ui/core";
import React from "react";
import { useParams } from "react-router-dom";
import { Park } from "../api/Park";
import { ParkObject } from "../api/ParkObject";
import DataProvider from "../components/DataProvider";
import ParkDetailsView from "./ParkDetails/ParkDetailsView";

export default function ParkDetails() {
  const routeParams = useParams<{ parkId: string }>();
  const parkId = parseInt(routeParams.parkId);

  return (
    <DataProvider<Park>
      getUrl="parks"
      render={(parksData) => {
        const park = parksData.items.filter((i) => i.id === parkId)[0];

        return (
          <DataProvider<ParkObject>
            getUrl={`parks/${parkId}/parkObjects`}
            render={(parkObjectsData) => (
              <ParkDetailsView objects={parkObjectsData.items} park={park} />
            )}
          />
        );
      }}
    />
  );
}
