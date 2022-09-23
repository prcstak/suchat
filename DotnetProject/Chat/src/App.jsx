import React, {useState} from 'react';
import * as signalR from "@microsoft/signalr";
import {HttpTransportType} from "@microsoft/signalr";

const App = async () => {
    let [messageList, setList] = useState(['kek']);
    let [message, setMessage] = useState("");

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:5262\n/Chat",
            {
                skipNegotiation: true,
                transport: HttpTransportType.WebSockets,
            })
        .configureLogging(signalR.LogLevel.Information)
        .build();

    async function start() {
        try {
            await connection.start();
        } catch (err) {
            console.log(err);
        }
    };

    async function send(){
        try {
            await connection.invoke("SendMessage", message);
        } catch (err) {
            console.error(err);
        }
    }

    connection.on("ReceiveMessage", (message) => {
/*        setList([...messageList,message]);*/
    });

    connection.onclose(async () => {
        await start();
    });

// Start the connection.
    start();
    return (
        <div style={{backgroundColor: "red"}}>
{/*            {messageList ? messageList.map((message, index)=>{
                return <span id={index}>{message}</span>
            }) : ""}*/}{/*            {messageList ? messageList.map((message, index)=>{
                return <span id={index}>{message}</span>
            }) : ""}*/}
            <input type={"text"} onChange={(e)=>setMessage(e.target.value)}/>
            <input type={"submit"} onClick={()=>{send()}}/>
        </div>
    );
};

export default App;