import { Form, Button } from 'react-bootstrap';

const Lobby = ({ joinRoom, userClaims }) => {
    let buttonValue = userClaims['role'] === 'User' ? 'ASK A QUESTION' : 'HELP THE USER'
    if (userClaims.name.substring(0,5) === 'admin') buttonValue = "HELP THE USER"

    return <Form className='lobby'
          onSubmit={e => {
              e.preventDefault();
              joinRoom(userClaims.name, userClaims.name);
          }} >
        <Button variant="success" type="submit">{buttonValue}</Button>
    </Form>
}

export default Lobby;