import 'bootstrap/dist/css/bootstrap.css';
import Chat from "./pages/Chat.jsx";
import {Route, Routes} from "react-router-dom";
import Login from "./pages/Login.jsx";
import Registration from "./pages/Registration.jsx";
import ProtectedRoutes from "./pages/ProtectedRoutes.jsx";

function App() {

  return (
    <div className="App">
        <Routes>
            <Route element={<ProtectedRoutes/>}>
                <Route path="/" element={<Chat/>}/>
            </Route>
            <Route path="/login" element={<Login/>}/>
            <Route path="/signup" element={<Registration/>}/>
        </Routes>
    </div>
  )
}

export default App
