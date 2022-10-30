import React, {useEffect, useRef, useState} from 'react';
import {Button, Card, Row, Form, Stack} from "react-bootstrap";
import Message from "./message.jsx";
import {HttpTransportType, HubConnectionBuilder} from "@microsoft/signalr";
import api from "../../utils/api.js";
import "./hideScroll.css"

function ChatPage({user}) {

    const [connection, setConnection] = useState(null);
    const [chat, setChat] = useState([]);
    const [offset, setOffset] = useState(0);
    const [limit, setLimit] = useState(10);

    const latestChat = useRef(null);
    const [input, setInput] = useState('');

    latestChat.current = chat;

    const bottom = useRef(null)

    const scrollToBottom = () => {
        bottom.current.scrollIntoView({behavior: "smooth"})
    }

    useEffect(() => {
        scrollToBottom()
    }, [chat]);

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl('http://localhost:5225/Chat', {
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

    useEffect(() => {
        const updatedChat = [...latestChat.current];
        api.post(`Message/history?offset=${offset}&limit=${limit}`).then((result) => {
            if (result.data.messages.length !== 0) {
                result.data['messages'].map((mes, index) => {
                    updatedChat.push({
                        message: mes.body,
                        timestamp: mes.created,
                        isMine: mes.username === user,
                        user: mes.username
                    });
                });
                setOffset(offset + 10);
                setLimit(limit + 10);
                setChat(updatedChat.reverse());
            }
        });
    }, [user])

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

    const handleScroll = event => {
        if (event.currentTarget.scrollTop === 0) {
            const updatedChat = [...latestChat.current];
            api.post(`Message/history?offset=${offset}&limit=${limit}`).then((result) => {
                if (result.data.messages.length !== 0) {
                    let count = result.data.messages.length;
                    result.data['messages'].map((mes, index) => {
                        updatedChat.unshift({
                            message: mes.body,
                            timestamp: mes.created,
                            isMine: mes.username === user,
                            user: mes.username
                        });
                    });
                    setOffset(offset + count);
                    setLimit(limit + count);
                    setChat(updatedChat);
                }
            })
        }
    };

    return (
        <div>
            <Card onScroll={handleScroll} className="hideScroll" style={{
                height: "92vh", overflowY: "auto", display: "flex",
            }}>
                {chat.reverse().map((mes, index) => {
                    return <Message
                        key={index}
                        message={mes.message}
                        isMine={mes.isMine}
                        user={mes.user}
                        timestamp={mes.timestamp}/>
                })}
                <div ref={bottom}/>
            </Card>
            <Row>
                <Stack style={{padding: 10}} direction="horizontal" gap={3}>
                    <Form.Control
                        onChange={(e) => setInput(e.target.value)}
                        value={input}
                        placeholder="Message"
                        aria-label="Message"
                        aria-describedby="basic-addon2"
                    />
                    <Button variant="info">Submit</Button>
                    <Button variant="success"
                            onClick={(e) => {
                                sendMessage(user, input);
                            }}>
                        Send
                    </Button>
                </Stack>
            </Row>
        </div>
    );
}

export default ChatPage;