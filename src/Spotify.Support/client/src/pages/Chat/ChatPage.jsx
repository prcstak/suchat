import React, {useEffect, useState} from 'react';
import {HttpTransportType, HubConnectionBuilder} from "@microsoft/signalr";
import "./hideScroll.css"
import InputGroup from "./InputGroup.jsx";
import ChatHistory from "./ChatHistory.jsx";
import {useNavigate} from "react-router-dom";

function ChatPage({user}) {

    const [connection, setConnection] = useState(null);
    const [chatHistory, setChatHistory] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        if (user === "") navigate('/');
        const newConnection = new HubConnectionBuilder()
            .withUrl('http://localhost:5225/Chat', {
                withCredentials: false, skipNegotiation: true, transport: HttpTransportType.WebSockets
            })
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);
    }, []);


    return (<div>
            <ChatHistory
                chatHistory={chatHistory}
                setChatHistory={setChatHistory}
                user={user}
                connection={connection}/>
            <InputGroup
                user={user}
                connection={connection}/>
        </div>);
}

export default ChatPage;