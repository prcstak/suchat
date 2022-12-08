import React from "react";
import {Stack} from "react-bootstrap";
import dayOfWeek from "../../consts/DayOfWeek.js"

const Message = ({user, isMine, message, timestamp}) => {

    const formatTimestamp = (timestamp) => {
        let t = new Date(timestamp);
        let now = new Date();
        if (t.getDate() === now.getDate()) {
            return `${t.getHours()}:${(t.getMinutes()< 10 ? '0' : '')+t.getMinutes()}`;
        }
        return `${t.getDate()} ${dayOfWeek[t.getMonth()]} ${t.getHours()}:${t.getMinutes()}`;
    }

    return (
        <div style={{
            backgroundColor: isMine ? "lightblue" : "wheat",
            borderRadius: isMine ? "15px 15px 3px 15px" : "3px 15px 15px 15px",
            padding: 10,
            margin: isMine ? "2% 2% 5% 40%" : "2% 40% 5% 2%"
        }}>
            <Stack direction={"vertical"} gap={2}>
                <Stack direction={"horizontal"}>
                    <div style={{fontSize: 14, fontWeight: 700, opacity: "70%"}} className={"me-auto"}>{user}</div>
                    <div style={{fontSize: 12, fontWeight: 700, opacity: "70%"}}>{formatTimestamp(timestamp)}</div>
                </Stack>
                <div>
                    {message}
                </div>
            </Stack>
        </div>
    );
};

export default Message;