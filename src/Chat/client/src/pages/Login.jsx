import React, {useContext, useState} from 'react';
import {Link, Navigate} from "react-router-dom";
import {Context} from "../main.jsx";

const Login = () => {
    const {userStore} = useContext(Context);
    const [user, setUser] = useState("");
    const [pwd, setPwd] = useState("");


    async function handleSubmit(e) {
        e.preventDefault();
        await userStore.login(user, pwd)
            .then(() => {
                setUser("");
                setPwd("");
            });
    }

    return (
        userStore.isAuthenticated ? <Navigate to="/"/> :
        <div>
            <form onSubmit={handleSubmit}>
                <label htmlFor="username">username</label>
                <input
                    type="text"
                    id="username"
                    onChange={(e) => setUser(e.target.value)}
                    value={user}
                    required
                />
                <label htmlFor="password">password</label>
                <input
                    type="password"
                    id="password"
                    onChange={(e) => setUser(e.target.value)}
                    value={pwd}
                    required
                />
                <button type="button">
                    sign-in
                </button>
            </form>
        </div>
    );
};

export default Login;