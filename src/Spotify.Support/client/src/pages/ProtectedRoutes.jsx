import React, {useContext} from 'react';
import { Outlet } from 'react-router'
import {observer} from "mobx-react-lite";
import LoginPage from "./Login/LoginPage.jsx";
import {Context} from "../main.jsx";

const ProtectedRoutes = () => {
    const {userStore} = useContext(Context);
    return (
        userStore.isAuthenticated ? <Outlet/> : <Chat/>
    );
};

export default observer(ProtectedRoutes);