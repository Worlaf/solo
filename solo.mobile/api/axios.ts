import axiosStatic from "axios";

const axios = axiosStatic.create({
  baseURL: "https://deb6416f7452.ngrok.io",
  timeout: 1000,
});

export default axios;
