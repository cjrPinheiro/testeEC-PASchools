import { Coordinate } from "./coordinate";

export interface Address {
street: string;
number: number;
district: string;
city: string;
latitude: number;
longitude: number;
state: string;
coordinates: Coordinate;
}
