import React, {useState} from 'react';
import {Button} from "react-bootstrap";

const SimpleLogin = ({setName}) => {
    const [input, setInput] = useState('');

    return (
        <div>
            <input onChange={(e) => setInput(e.target.value)}
                    type={"text"}/>
            <Button onClick={() => setName(input)}>Join</Button>
        </div>
    );
};

export default SimpleLogin;