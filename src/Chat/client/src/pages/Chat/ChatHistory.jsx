import Message from "./message.jsx";
import {Card} from "react-bootstrap";
import api from "../../utils/api.js";
import React, {useEffect, useRef, useState} from 'react';

const ChatHistory = ({chatHistory, setChatHistory, user, connection}) => {
    const [offset, setOffset] = useState(0);
    const [limit, setLimit] = useState(10);
    const latestChat = useRef(null);
    const bottom = useRef(null);

    latestChat.current = chatHistory;

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(_ => {
                    console.log('Connected!');
                    onReceiveMessage();
                    onReceiveFile();
                })
                .catch(e => console.log('Connection failed: ', e));
        }
        getHistory();
    }, [connection]);

    useEffect(() => {
        bottom.current.scrollIntoView({behavior: "smooth"});
    }, [chatHistory]); //scroll to bottom on new message receive

    const handleScroll = event => {
        if (event.currentTarget.scrollTop === 0) {
            const updatedChat = [...latestChat.current];
            api.post(`Message/history?offset=${offset}&limit=${limit}`).then((result) => {
                if (result.data.messages.length !== 0) {
                    let count = result.data.messages.length;
                    result.data['messages'].map((mes) => {
                        updatedChat.unshift({
                            message: mes.body,
                            timestamp: mes.created,
                            isMine: mes.username === user,
                            user: mes.username
                        });
                    });
                    setOffset(offset + count);
                    setLimit(limit + count);
                    setChatHistory(updatedChat);
                }
            })
        }
    };

    const getHistory = () => {
        const updatedChat = [...latestChat.current];
        api.post(`Message/history?offset=${offset}&limit=${limit}`).then((result) => {
            if (result.data.messages.length !== 0) {
                result.data['messages'].map((mes) => {
                    console.log(mes.isFile)
                    if (!mes.isFile) {
                        updatedChat.push({
                            message: mes.body,
                            timestamp: mes.created,
                            isMine: mes.username === user,
                            user: mes.username
                        });
                    } else {
                        let link = "http://localhost:8000/files-persistent/" + mes.body
                        updatedChat.push({
                            message: <a href={link} target="_blank">{mes.body}</a>,
                            timestamp: mes.created,
                            isMine: mes.username === user,
                            user: mes.username
                        });
                    }
                });
                setOffset(offset + 10);
                setLimit(limit + 10);
                setChatHistory(updatedChat.reverse());
            }
        });
    }

    const onReceiveMessage = () => {
        connection.on('ReceiveMessage', (u, m, t) => {
            const updatedChat = [...latestChat.current];
            updatedChat.push({message: m, timestamp: t, isMine: user === u, user: u});
            setChatHistory(updatedChat);
        });
    }

    const onReceiveFile = () => {
        connection.on('ReceiveFile', (u, m, t) => {
            let link = "http://localhost:8000/files-persistent/" + m
            const updatedChat = [...latestChat.current];
            updatedChat.push({message: <a href={link} target="_blank">{m}</a>, timestamp: t, isMine: user === u, user: u});
            setChatHistory(updatedChat);
        })
    };

    return (
        <Card onScroll={handleScroll} className="hideScroll" style={{
            height: "92vh", overflowY: "auto", display: "flex",
        }}>
            {chatHistory.reverse().map((mes, index) => {
                return <Message
                    key={index}
                    message={mes.message}
                    isMine={mes.isMine}
                    user={mes.user}
                    timestamp={mes.timestamp}/>
            })}
            <div ref={bottom}/>
        </Card>
    );
};

export default ChatHistory;