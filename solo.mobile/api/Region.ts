import { GeoPoint } from "./GeoPoint";

export interface Region {
    points: GeoPoint[];
}

export function getRegionCenter(region: Region): GeoPoint {
    var latSum = 0;
    var lonSum = 0;

    region.points.forEach((p) => {
        latSum += p.latitude;
        lonSum += p.longitude;
    });

    var point: GeoPoint = {
        latitude: latSum / region.points.length,
        longitude: lonSum / region.points.length,
    };

    debugger;

    return point;
}

export function getTopLeft(region: Region): GeoPoint {
    var tl: GeoPoint = region.points[0];

    region.points.forEach((p) => {
        if (tl.latitude > p.latitude && tl.longitude > p.longitude) tl = p;
    });

    console.log("top left", region, tl);

    return tl;
}

export function getBottomRight(region: Region): GeoPoint {
    var br: GeoPoint = region.points[region.points.length - 1];

    region.points.forEach((p) => {
        if (br.latitude < p.latitude && br.longitude < p.longitude) br = p;
    });

    console.log("bottom right", region, br);

    return br;
}

export function getLongitudeDiff(region: Region) {
    return Math.abs(getTopLeft(region).longitude - getBottomRight(region).longitude);
}

export function getLatitudeDiff(region: Region) {
    return Math.abs(getTopLeft(region).latitude - getBottomRight(region).latitude);
}
