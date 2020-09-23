import React, { useEffect, useState } from "react";

export interface IGeolocationData {
    position?: Position;
}

interface IGeolocationProps {
    renderFunc(data: IGeolocationData): React.ReactElement | null;
}

export default function Geolocation(props: IGeolocationProps){
    const [geoData, setGeoData] = useState<IGeolocationData>({});

    useEffect(() => {
        const geoWatchHandle = navigator.geolocation.watchPosition(pos => setGeoData({position: pos}));

        return () => navigator.geolocation.clearWatch(geoWatchHandle);
    })

    return props.renderFunc(geoData);    
}