import 'bootstrap/dist/css/bootstrap.css';
import Chat from "./pages/Chat.jsx";
import React, {useState} from 'react';
import {Card, Col, Container, Row} from "react-bootstrap";
import SimpleLogin from "./pages/SimpleLogin.jsx";

function App() {
    const [user, setUser] = useState('');

    return (
            <Container className="vh-100">
                <Row className="h-100 justify-content-md-center">
                    <Col xs={6}>
                        <Card style={{height: "100%"}}>
                            {
                                user === '' ?
                                    <Col md={{span: 6, offset: 3}} style={{paddingTop: 300}}>
                                        <SimpleLogin setName={() => setUser()}/>
                                    </Col> :
                                    <Chat user={user}/>
                            }
                        </Card>
                    </Col>
                </Row>
            </Container>
    )
}

export default App
