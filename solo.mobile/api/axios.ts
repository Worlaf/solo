import axiosStatic from "axios";

const axios = axiosStatic.create({
    baseURL: "https://e5ca24861b23.ngrok.io",
    timeout: 1000,
});

export default axios;
