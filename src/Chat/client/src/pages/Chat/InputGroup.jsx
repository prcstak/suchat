import {Button, Form, Row, Stack} from "react-bootstrap";
import React, {useState} from 'react';
import {v4} from 'uuid';
import $api from "../../utils/api.js";

const inputType = {
    text: "text",
    file: "file",
}

function InputGroup({user, connection}) {
    const [textInput, setTextInput] = useState('');
    const [filesInput, setFilesInput] = useState([]);
    const [type, setType] = useState(inputType.text);

    const send = async () => {
        type === inputType.text
            ? await sendMessage()
            : await sendFile()
    }

    const sendMessage = async () => {
        const chatMessage = {
            user: user,
            message: textInput
        };
        await connection.send('SendMessage', chatMessage.user, chatMessage.message);
        setTextInput('');
    }

    const sendFile = async () => {
        if (filesInput.length === 0) {}
        else {
            let formData = new FormData();
            formData.append("file", filesInput[0]);
            let filename = filesInput[0].name;
            let fileId = `${Date.now().toString()}-${filename}`;
            let meta = "{}";
            let id = v4();
            await $api.post('/File', formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                },
                params: {"filename": fileId, "requestId": id}
            });
            await $api.post('/Meta', {},{
                params: {"metaJson": meta, "filename": fileId,"requestId": id, "author": user}
            });
            setFilesInput(null);
        }
    }

    const setInputType = () => {
        type === inputType.text
            ? setType(inputType.file)
            : setType(inputType.text);
    }

    const onEnter = async (e) => {
        if(e.key === 'Enter'){
            await send();
        }
    }

    return <Row>
        <Stack style={{padding: 10}} direction="horizontal" gap={3}>
            {
                type === inputType.text
                    ? <Form.Control
                        value={textInput}
                        placeholder="Message"
                        onChange={e => setTextInput(e.target.value)}
                        onKeyPress={e => onEnter(e)}
                    />
                    : <Form.Control
                        type="file"
                        onChange={e => setFilesInput(e.target.files)}
                    />
            }
            <Button variant="info" onClick={_ => setInputType()}>
                File/Message
            </Button>
            <Button variant="success"
                    onClick={_ => send()}
            >
                Send
            </Button>
        </Stack>
    </Row>;
}

export default InputGroup;