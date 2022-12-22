import React, {useState} from 'react';
import {Button, Form, InputGroup} from "react-bootstrap";
import {useNavigate} from "react-router-dom";

const SimpleLoginPage = ({setName}) => {
    const [input, setInput] = useState('');
    let navigate = useNavigate();

    const Join = () => {
        if (input !== ''){
            setName(input);
            input === "admin" ? navigate("/rooms") : navigate(`/chat?room=${input}`);
        }

    }

    const onEnter = async (e) => {
        if (e.key === 'Enter') {
            Join();
        }
    }

    return (<div>
            <InputGroup className="mb-3">
                <Form.Control
                    value={input}
                    placeholder="Name"
                    aria-label="Name"
                    aria-describedby="basic-addon2"
                    onChange={e => setInput(e.target.value)}
                    onKeyPress={e => onEnter(e)}
                />
                <Button variant={"dark"} onClick={() => Join()}>Join</Button>
            </InputGroup>
        </div>);
};

export default SimpleLoginPage;