import { GeoPoint } from "./GeoPoint";

export interface ParkObject {
    id: number;
    parkId: number;
    name: string;
    publicDescriptionMarkdown: string;
    administrationDescriptionMarkdown: string;
    location: GeoPoint;
    priceForAdults: number;
    priceForChildren: number;
    workScheduleJson: string;
    imageUrl: string;
    type: ParkObjectType;
    ticketsTotal: number;
    ticketsClosed: number;
}

export enum ParkObjectType {
    None,
    Sight,
    Attraction,
    Shop,
    Food,
}
