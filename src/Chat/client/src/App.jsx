import 'bootstrap/dist/css/bootstrap.css';
import Chat from "./pages/Chat.jsx";
import React from 'react';
import {Route, Routes} from "react-router-dom";
import Login from "./pages/Login.jsx";
import Registration from "./pages/Registration.jsx";
import ProtectedRoutes from "./pages/ProtectedRoutes.jsx";

function App() {

  return (
    <div className="App">
        <Chat/>
    </div>
  )
}

export default App
