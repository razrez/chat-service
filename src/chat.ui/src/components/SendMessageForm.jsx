import {Form, Button, FormControl, InputGroup} from 'react-bootstrap';
import { useState } from 'react';
import React from "react";

const SendMessageForm = ({ sendMessage }) => {
    const [message, setMessage] = useState('');
    const [file, setFile] = useState();
    const [show, setShow] = useState(false);
    const [fileType, setFileType] = useState('');
    const [fileMeta, setFileMeta] = useState([]);

    //for test
    const [addInfo, setAddInfo] = useState('');
    const [author, setAuthor] = useState('');

    return <Form
        onSubmit={e => {
            e.preventDefault();
            sendMessage(message, file);
            setMessage('');
        }}>
        <InputGroup>
            <FormControl type="user" className="text-light bg-dark" placeholder="message..."
                onChange={e => setMessage(e.target.value)} value={message}/>

            <FormControl  type="file" className="text-light bg-dark" onChange={e => {
                setFile(e.target.files[0]);
                setShow(true);
                setFileType(e.target.files[0].type);
                console.log(fileType);
            }}/>

            <InputGroup>
                <Button variant="dark" type="submit"
                        disabled={(!message && !file)}>Send</Button>
            </InputGroup>

        </InputGroup>
        {show ? <Form className="form-control">
            <div className="modal-header">
                <h5 className="modal-title">Declare custom Metadata</h5>
            </div>
            <Form.Group>
                {fileType === "image/jpeg" ? (
                <Form.Control placeholder="author pic" onChange={e => setAuthor(e.target.value)}/>)
                : (<Form.Control placeholder="additional info for ur file" onChange={e => setAddInfo(e.target.value)}/>)}
            </Form.Group>
            <Button variant="success" type="submit" onClick={(e) => {
                e.preventDefault();
                setShow(false);
            }}>accept</Button>
        </Form>: null}

    </Form>
}

export default SendMessageForm;