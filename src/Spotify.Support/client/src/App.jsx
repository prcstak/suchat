import 'bootstrap/dist/css/bootstrap.css';
import ChatPage from "./pages/Chat/ChatPage.jsx";
import React, {useState} from 'react';
import {Col, Container, Row} from "react-bootstrap";
import SimpleLoginPage from "./pages/Login/SimpleLoginPage.jsx";
import {BrowserRouter, Route, Routes} from "react-router-dom";
import Rooms from "./pages/Chat/Rooms.jsx";

function App() {
    const [user, setUser] = useState('');

    return (
            <Container fluid={true}  style={{backgroundColor: "#3b3b3b", height: "100vh"}}>
                <Row className="h-100 justify-content-md-center">
                    <Col xs={6} style={{padding: 0}}>
                        <BrowserRouter>
                            <Routes>
                                <Route path="/" element={<SimpleLoginPage setName={setUser}/>}/>
                                <Route path="/rooms" element={<Rooms user={user}/>}/>
                                <Route path="/chat" element={<ChatPage user={user}/>}/>
                            </Routes>
                        </BrowserRouter>
                    </Col>
                </Row>
            </Container>
    )
}

export default App
