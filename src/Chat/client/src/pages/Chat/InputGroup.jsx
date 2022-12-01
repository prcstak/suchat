import {Button, Form, Modal, Row, Stack} from "react-bootstrap";
import React, {useState} from 'react';
import {v4} from 'uuid';
import $api from "../../utils/api.js";
import FileExtensions from "../../consts/fileExtensions.js";
import MetaTemplates from "../../utils/metaTemplates.js";

const inputType = {
    text: "text",
    file: "file",
}

function InputGroup({user, connection}) {
    const [textInput, setTextInput] = useState('');
    const [filesInput, setFilesInput] = useState([]);
    const [type, setType] = useState(inputType.text);
    const [show, setShow] = useState(false);
    const [metadata, setMetadata] = useState({});
    const [formFields, setFormFields] = useState([]);

    const handleClose = async ()  => {
        setShow(false);
        await sendFile();
    };
    const handleShow = () => setShow(true);

    const send = async () => {
        type === inputType.text
            ? await sendMessage()
            : showModal();//await sendFile()
    }

    const showModal = () => {
        if (filesInput.length === 0) {
        } else {
            let extension = filesInput[0].name.split('.').pop();
            console.log(extension)
            let template = [];
            if (FileExtensions.image.includes(extension)) {
                template = MetaTemplates.ImageMeta;
            }
            else if (FileExtensions.video.includes(extension)) {
                template = MetaTemplates.VideoMeta;
            }
            else if (FileExtensions.music.includes(extension)) {
                template = MetaTemplates.MusicMeta;
            }
            else if (FileExtensions.document.includes(extension)) {
                template = MetaTemplates.DocumentMeta;
            }
            else if (FileExtensions.pdf.includes(extension)) {
                template = MetaTemplates.PdfMeta;
            }

            setFormFields(template);
            let meta = {};
            template.map(field => {

                meta[field] = ''
            });
            setMetadata(meta);
            handleShow();
        }
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
            let formData = new FormData();
            formData.append("file", filesInput[0]);
            let filename = filesInput[0].name;
            let fileId = `${Date.now().toString()}-${filename}`;
            let id = v4();
            let filePost = $api.post('/File', formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                },
                params: {"filename": fileId, "requestId": id}
            });
            let metaPost =  $api.post('/Meta', {},{
                params: {"metaJson": metadata, "filename": fileId,"requestId": id, "author": user}
            });

            await Promise.all([filePost, metaPost]);

            setFilesInput(null);
    }

    const setInputType = () => {
        type === inputType.text
            ? setType(inputType.file)
            : setType(inputType.text);
    }

    const onEnter = async (e) => {
        if (e.key === 'Enter') {
            await send();
        }
    }

    return <Row>
        <Modal show={show} onHide={handleClose}>
            <Modal.Header closeButton>
                <Modal.Title>File metadata</Modal.Title>
            </Modal.Header>
            {

                    formFields.map(field => {
                        return <>
                            <Modal.Body key={field}>
                                <input
                                    name={field}
                                    style={{padding: 10}}
                                    placeholder={field}
                                    onChange={e => {
                                        let meta = metadata;
                                        meta[e.target.name] = e.target.value;
                                        setMetadata(meta)
                                    }}
                                />
                            </Modal.Body>
                        </>
                    })
            }
            <Modal.Footer>
                <Button variant="primary" onClick={handleClose}>
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