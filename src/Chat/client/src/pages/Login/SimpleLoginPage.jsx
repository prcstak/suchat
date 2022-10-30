import React, {useState} from 'react';
import {Button, Form, InputGroup} from "react-bootstrap";

const SimpleLoginPage = ({setName}) => {
    const [input, setInput] = useState('');

    return (
        <div>
            <InputGroup className="mb-3">
                <Form.Control
                    onChange={(e) => setInput(e.target.value)}
                    value={input}
                    placeholder="Name"
                    aria-label="Name"
                    aria-describedby="basic-addon2"
                />

                <Button onClick={() => setName(input)}>Join</Button>
            </InputGroup>
        </div>
    );
};

export default SimpleLoginPage;