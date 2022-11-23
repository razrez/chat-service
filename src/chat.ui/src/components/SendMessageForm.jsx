import {Form, Button, FormControl, InputGroup} from 'react-bootstrap';
import { useState } from 'react';
import React from "react";
import Modal from '../modal/Modal'

const SendMessageForm = ({ sendMessage }) => {
    const [message, setMessage] = useState('');
    const [file, setFile] = useState();
    const [show, setShow] = useState(false);

    return <Form
        onSubmit={e => {
            e.preventDefault();
            sendMessage(message, file);
            setMessage('');
        }}>
        <InputGroup>
            <FormControl type="user" placeholder="message..."
                onChange={e => setMessage(e.target.value)} value={message}/>

            <FormControl type="file" onChange={e => setFile(e.target.files[0])}/>

            <InputGroup>
                <Button variant="primary" type="submit"
                        disabled={(!message && !file)}>Send</Button>
            </InputGroup>

        </InputGroup>
        <Modal onClose={() => setShow(false)} show={show} />

    </Form>
}

export default SendMessageForm;