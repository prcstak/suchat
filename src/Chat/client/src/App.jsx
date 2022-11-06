import 'bootstrap/dist/css/bootstrap.css';
import ChatPage from "./pages/Chat/ChatPage.jsx";
import React, {useState} from 'react';
import {Card, Col, Container, Row} from "react-bootstrap";
import SimpleLoginPage from "./pages/Login/SimpleLoginPage.jsx";

function App() {
    const [user, setUser] = useState('');

    return (
            <Container fluid={true}  style={{backgroundColor: "lightseagreen", height: "100vh"}}>
                <Row className="h-100 justify-content-md-center">
                    <Col xs={6} style={{padding: 0}}>
                    {
                        user === '' ?
                            <Col md={{span: 6, offset: 3}} style={{paddingTop: 300}}>
                                <SimpleLoginPage setName={setUser}/>
                            </Col> :
                            <ChatPage user={user}/>
                    }
                    </Col>
                </Row>
            </Container>
    )
}

export default App
