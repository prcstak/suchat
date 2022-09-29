import React, {useEffect, useRef, useState} from 'react';
import {Button, Card, Col, Container, InputGroup, Row, Form} from "react-bootstrap";
import Message from "../widgets/message.jsx";
import {HttpTransportType, HubConnectionBuilder} from "@microsoft/signalr";

function Chat(props) {

    const [connection, setConnection] = useState(null);
    const [chat, setChat] = useState([]);
    const [message, setMessage] = useState("");
    const latestChat = useRef(null);
    const [user, setUser] = useState('');
    const [input, setInput] = useState('');

    latestChat.current = chat;

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl('https://localhost:7225/Chat', {
                withCredentials: false,
                skipNegotiation: true,
                transport: HttpTransportType.WebSockets
            })
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);
    }, []);

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(result => {
                    console.log('Connected!');

                    connection.on('ReceiveMessage', (userR, message) => {
                        const updatedChat = [...latestChat.current];
                        updatedChat.push({message: message, timestamp: "12:30", isMine: userR == user}); //TODO:isMine: userId == myId
                        setChat(updatedChat);
                    });
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);

    const sendMessage = async (user, message) => {
        const chatMessage = {
            user: user,
            message: message
        };
        await connection.send('SendMessage', chatMessage.message, chatMessage.message + "");
    }

    return (<div>
        <Container className="vh-100 d-flex flex-column ">
            <Row className="h-100">
                <Col/>
                <Col xs={6}>
                    <Card style={{padding: 0, height: "100%"}}>
                        {
                            user == '' ?
                                <div><input onChange={(e)=>setInput(e.target.value)}
                                            type={"text"}/>
                                    <Button onClick={()=>{setUser(input)}}>Join</Button>
                                </div> :
                                <Col style={{height: "100px", position: "relative"}}>
                                    <div style={{
                                        position: "absolute",
                                        right: 0,
                                        bottom: 0,
                                        paddingBottom: 70,
                                        width: "100%"
                                    }}>
                                        {
                                            chat.map((mes, index) => {
                                                return <Message key={index} message={mes.message} isMine={mes.isMine}
                                                                timestamp={mes.timestamp}/>
                                            })
                                        }
                                    </div>
                                    <InputGroup style={{position: "absolute", bottom: 0}} className="mb-3">
                                        <Form.Control
                                            onChange={(e) => setMessage(e.target.value)}
                                            placeholder="Message"
                                            aria-label="Message"
                                            aria-describedby="basic-addon2"
                                        />
                                        <Button variant="outline-secondary" id="button-addon2"
                                                onClick={(e) => {
                                                    sendMessage(user, message)
                                                }}>
                                            Send
                                        </Button>
                                    </InputGroup>
                                </Col>
                        }
                    </Card>
                </Col>
                <Col/>
            </Row>
        </Container>
    </div>);
}

export default Chat;