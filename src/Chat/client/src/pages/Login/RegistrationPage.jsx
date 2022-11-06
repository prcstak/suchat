import React, {useContext, useState} from 'react';
import {Context} from "../../main.jsx";
import {Navigate} from "react-router-dom";

const RegistrationPage = () => {
    const {userStore} = useContext(Context);
    const [user, setUser] = useState("");
    const [pwd, setPwd] = useState("");

    async function handleSubmit(e) {
        e.preventDefault();

        await userStore.register(user, pwd)
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
                    type="text"
                    id="password"
                    onChange={(e) => setPwd(e.target.value)}
                    value={pwd}
                    required
                />
            </form>
        </div>
    );
};

export default RegistrationPage;