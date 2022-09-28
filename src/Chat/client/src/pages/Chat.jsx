import React, {useState} from 'react';
import {Button, Card, Col, Container, InputGroup, Row, Form} from "react-bootstrap";
import Config from "bootstrap/js/src/util/config";
import Message from "../widgets/message.jsx";

function Chat(props) {

    const [messages, setMessages] = useState([{message: "lol", timestamp: "12:30", isMine: true}, {
        message: "lol",
        timestamp: "12:30",
        isMine: false
    }, {message: "lol", timestamp: "12:30", isMine: true}]);

    return (<div>
        <Container className="vh-100 d-flex flex-column ">
            <Row className="h-100">
                <Col/>
                <Col xs={6}>
                    <Card style={{padding: 0, height: "100%"}}>

                        <Col style={{height: "100px", position: "relative"}}>
                            <div style={{position: "absolute", right: 0, bottom: 0, paddingBottom: 70, width: "100%"}}>
                                {
                                    messages.map((mes, index) => {
                                        return <Message message={mes.message} isMine={mes.isMine}
                                                        timestamp={mes.timestamp}/>
                                    })
                                }
                            </div>
                            <InputGroup style={{position: "absolute", bottom: 0}} className="mb-3">
                                <Form.Control
                                    placeholder="Message"
                                    aria-label="Message"
                                    aria-describedby="basic-addon2"
                                />
                                <Button variant="outline-secondary" id="button-addon2">
                                    Send
                                </Button>
                            </InputGroup>
                        </Col>
                    </Card>
                </Col>
                <Col/>
            </Row>
        </Container>
    </div>);
}

export default Chat;