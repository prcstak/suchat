import axios from "axios";
import {getCookie} from "./cookies";

export const API_URL = "http://localhost:5225/api/"

const $api = axios.create({
    baseURL: API_URL // TODO: "process.env.WEB_API",
});

/*$api.interceptors.request.use((config) => {
    const token = getCookie("access_token");
    if (token) {
        config.headers.common["Authorization"] = `Bearer ${token}`;
    }
    return config;
});*/

export default $api;