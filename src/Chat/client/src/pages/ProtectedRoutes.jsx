import React, {useContext} from 'react';
import { Outlet } from 'react-router'
import {observer} from "mobx-react-lite";
import Login from "../pages/Login";
import {Context} from "../main.jsx";

const ProtectedRoutes = () => {
    const {userStore} = useContext(Context);
    return (
        userStore.isAuthenticated ? <Outlet/> : <Chat/>
    );
};

export default observer(ProtectedRoutes);