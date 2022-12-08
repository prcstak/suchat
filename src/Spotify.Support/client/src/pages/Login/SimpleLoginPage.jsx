import React, {useState} from 'react';
import {Button, Form, InputGroup} from "react-bootstrap";

const SimpleLoginPage = ({setName}) => {
    const [input, setInput] = useState('');

    const onEnter = async (e) => {
        if(e.key === 'Enter'){
            await setName(input);
        }
    }

    return (
        <div>
            <InputGroup className="mb-3">
                <Form.Control
                    value={input}
                    placeholder="Name"
                    aria-label="Name"
                    aria-describedby="basic-addon2"
                    onChange={e=> setInput(e.target.value)}
                    onKeyPress={e => onEnter(e)}
                />

                <Button onClick={() => setName(input)}>Join</Button>
            </InputGroup>
        </div>
    );
};

export default SimpleLoginPage;