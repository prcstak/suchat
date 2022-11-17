import {Button, Form, Modal, Row, Stack} from "react-bootstrap";
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
    const [show, setShow] = useState(false);

    const [author, setAuthor] = useState();
    const [date, setDate] = useState();
    const [description, setDescription] = useState();

    const fileMeta = '';

    const handleClose = () => setShow(false);
    const handleShow = () => setShow(false);


    const send = async () => {
        type === inputType.text
            ? await sendMessage()
            : setShow(true);
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
            let metaJson = JSON.stringify({"author": author, "date": date, "description": description});
            let id = v4();
            let fileReq = new Promise(() => $api.post('/File', formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                },
                params: {"filename": fileId, "requestId": id}
            }));
            let metaRequest = new Promise(() => $api.post('/Meta', {metaJson},{
                params: {"metaJson": metaJson, "filename": fileId,"requestId": id, "author": user}
            }));

            Promise.all([metaRequest, fileReq]).then(() => {
                setFilesInput(null);
            })
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
        <Modal show={show}>
            <Modal.Header closeButton>
                <Modal.Title>Данные о файле</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Row>
                    Author
                    <input value={author} onChange={(e) => setAuthor(e.target.value)}/>
                    Date
                    <input value={date} onChange={(e) => setDate(e.target.value)}/>
                    Description
                    <input value={description} onChange={(e) => setDescription(e.target.value)}/>
                </Row>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={handleClose}>
                    Close
                </Button>
                <Button variant="primary" onClick={sendFile}>
                    Send
                </Button>
            </Modal.Footer>
        </Modal>
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