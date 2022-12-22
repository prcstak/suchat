import React, {useEffect, useState} from 'react';
import {HttpTransportType, HubConnectionBuilder} from "@microsoft/signalr";
import {Card} from "react-bootstrap";
import {Link, useNavigate} from "react-router-dom";

const Rooms = ({user}) => {
    const [connection, setConnection] = useState(null);
    const [rooms, setRooms] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        if (user === "") navigate('/');
        const newConnection = new HubConnectionBuilder()
            .withUrl('http://localhost:5225/Admin', {
                withCredentials: false, skipNegotiation: true, transport: HttpTransportType.WebSockets
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
                    connection.send("GetAllRooms");
                    connection.on('GetAllRooms', (list) => {
                        setRooms(JSON.parse(list));
                    });
                    connection.on('RoomAmountChanged', (list) => {
                        setRooms(JSON.parse(list));
                    });
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);

    return (<div>
        <div style={{
            paddingLeft: 10,
            fontSize: 26,
            backgroundColor: "#1DB954",
            borderRadius: 10,
            margin: 4
        }}>Активные чаты
        </div>
        <Card className="hideScroll" style={{
            height: "92vh", overflowY: "auto", display: "flex", padding: 10
        }}>
            {rooms?.map((roomName, index) => {
                return <Link key={index} to={`/chat?room=${roomName}`} style={{textDecoration: "none", fontSize: 20}}>
                    <span style={{paddingLeft: 10}}>{roomName}</span>
                    <hr/>
                </Link>
            })}
        </Card>
    </div>);
};

export default Rooms;