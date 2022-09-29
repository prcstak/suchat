import $api from "../utils/api";

export default class AuthService {
    static async login(login, password, rememberMe) {
        return $api.post(
            "/v1/authorization/login",
                {login, password, rememberMe},
                {headers: {"Content-Type": "multipart/form-data"}});
    }

    static async register(email, username, password) {
        return $api.post("/v1/authorization/registration",
            {email, username, password},
            {headers: {"Content-Type": "multipart/form-data"}});
    }
}