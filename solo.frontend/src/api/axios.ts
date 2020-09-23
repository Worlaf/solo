import React from "react";
import axiosGlobal, { AxiosError } from "axios";

const axios = axiosGlobal.create();
const baseURI = process.env.REACT_APP_API_URL;
axios.defaults.baseURL = baseURI;

console.log("AXIOS DEFAULT URL", baseURI);

export default axios;

declare module "axios" {
    interface AxiosRequestConfig {
        successMessage?: string;
        showSuccessAlert?: boolean;
        dontShowErrorAlert?: boolean;
    }
}

declare global {
    interface Promise<T> {
        handleAxiosError(): Promise<void>;
    }
}

Promise.prototype.handleAxiosError = function <T>(this: Promise<T>) {
    return this.catch((error: AxiosError) => {
        return error.request || error.response ? Promise.resolve() : Promise.reject(error);
    });
};
