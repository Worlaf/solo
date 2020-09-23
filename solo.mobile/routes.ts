import { StackScreenProps as ReactNavigationStackScreenProps } from "@react-navigation/stack";

export type RootStackParamsList = {
    Home: undefined;
    ParkList: undefined;
    Map: { parkId: number };
    ParkObject: { parkId: number; parkObjectId: number };
};

export type StackScreenProps<T extends keyof RootStackParamsList> = ReactNavigationStackScreenProps<RootStackParamsList, T>;
