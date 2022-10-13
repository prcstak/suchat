import React, {useEffect, useRef, useState} from 'react';
import {Button, Card, Col, Container, InputGroup, Row, Form} from "react-bootstrap";
import Message from "../widgets/message.jsx";
import {HttpTransportType, HubConnectionBuilder} from "@microsoft/signalr";
import api from "../utils/api.js";

function Chat({user}) {

    const [connection, setConnection] = useState(null);
    const [chat, setChat] = useState([]);
    const latestChat = useRef(null);
    const [input, setInput] = useState('');

    latestChat.current = chat;

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl('http://localhost:8000/Chat', {
                withCredentials: false,
                skipNegotiation: true,
                transport: HttpTransportType.WebSockets
            })
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);
    }, []);

    useEffect(() => {
        const updatedChat = [...latestChat.current];
        api.post('Message/history?offset=0&limit=100').then((result) => {
            result['messages'].map((mes, index) => {
                updatedChat.push({
                    message: mes.body,
                    timestamp: mes.created,
                    isMine: mes.username == user,
                    user: mes.username
                });
                setChat(updatedChat);
            });
        });
    }, [user])

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(_ => {
                    console.log('Connected!');
                    connection.on('ReceiveMessage', (u, m, t) => {
                        const updatedChat = [...latestChat.current];
                        updatedChat.push({message: m, timestamp: t, isMine: user === u, user: u});
                        setChat(updatedChat);
                    });
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);

    const sendMessage = async (u, m) => {
        connection.on('ReceiveMessage', (u, m, t) => {
            const updatedChat = [...latestChat.current];
            updatedChat.push({message: m, timestamp: t, isMine: user === u, user: u});
            setChat(updatedChat);
        });
        const chatMessage = {
            user: u,
            message: m
        };
        await connection.send('SendMessage', chatMessage.user, chatMessage.message);
    }

    return (
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
                                        user={mes.user}
                                        timestamp={mes.timestamp}/>
                    })
                }
            </div>
            <InputGroup style={{position: "absolute", bottom: 0}} className="mb-3">
                <Form.Control
                    onChange={(e) => setInput(e.target.value)}
                    value={input}
                    placeholder="Message"
                    aria-label="Message"
                    aria-describedby="basic-addon2"
                />
                <Button variant="outline-secondary" id="button-addon2"
                        onClick={(e) => {
                            sendMessage(user, input);
                        }}>
                    Send
                </Button>
            </InputGroup>
        </Col>);
}

export default Chat;