import { useState } from 'react';
import { Form, Button } from 'react-bootstrap';

const Lobby = ({ joinRoom }) => {
    const [user, setUser] = useState();
    const [room, setRoom] = useState();

    return <div className='app'>
        <h2>MyChat</h2>
        <hr className='line' />
        <Form className='lobby'
              onSubmit={e => {
                  e.preventDefault();
                  console.log(user, room);
                  joinRoom(user, room);
              }} >
            <Form.Group>
                <Form.Control placeholder="name" onChange={e => setUser(e.target.value)} />
                <Form.Control placeholder="room" onChange={e => setRoom(e.target.value)} />
            </Form.Group>
            <Button variant="success" type="submit" disabled={!user || !room}>Join</Button>
        </Form>
    </div>
}

export default Lobby;