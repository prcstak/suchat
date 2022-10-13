import 'bootstrap/dist/css/bootstrap.css';
import Chat from "./pages/Chat.jsx";
import React, {useState} from 'react';
import {Card, Col, Container, Row} from "react-bootstrap";
import SimpleLogin from "./pages/SimpleLogin.jsx";

function App() {
    const [user, setUser] = useState('');

    return (
        <div>
            <Container className="vh-100 d-flex flex-column ">
                <Row className="h-100">
                    <Col/>
                    <Col xs={6}>
                        <Card style={{padding: 0, height: "100%"}}>
                            {
                                user === '' ?
                                    <SimpleLogin setName={() => setUser()}/> :
                                    <Chat user={user}/>
                            }
                        </Card>
                    </Col>
                    <Col/>
                </Row>
            </Container>
        </div>
    )
}

export default App
