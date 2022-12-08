import {makeAutoObservable} from "mobx";
import AuthService from "../services/AuthService";
import {deleteCookie, setCookie} from "../utils/cookies";

export default class UserStore {
    user = "";
    isAuthenticated = false;

    constructor() {
        makeAutoObservable(this);
    }

    setAuth(bool) {
        this.isAuthenticated = bool;
    }

    setUser(user) {
        this.user = user;
    }

    async login(login, password) {
        try {
            const response = await AuthService.login(login, password)
                .then(async res => {
                    setCookie(
                        "access_token",
                        res.data['access_token']);
                    this.setAuth(true);
                    this.setUser(login);
                });
        } catch (e) {
            console.log(e.response?.data?.message)
        }
    }

    async register(username, password) {
        try {
            await AuthService.register(username, password);
        } catch (e) {
            console.log(e.response?.data?.message)
        }
    }

    async logout() {
        try {
            deleteCookie("access_token");
            this.setAuth(false);
            this.setUser({});
        } catch (e) {
            console.log(e.response?.data?.message)
        }
    }
}