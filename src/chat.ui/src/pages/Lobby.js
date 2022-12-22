import { Form, Button } from 'react-bootstrap';

const Lobby = ({ joinRoom, userClaims }) => {
    let buttonValue = userClaims['role'] === 'User' ? 'ASK A QUESTION' : 'HELP THE USER'
    const isAdmin = userClaims.name.substring(0,5) === 'admin'
    if (isAdmin) {
        buttonValue = "HELP THE USER";
    }

    return <Form className='lobby'
          onSubmit={e => {
              e.preventDefault();
              joinRoom(userClaims.name, userClaims.name, isAdmin);
          }} >
        <Button variant="success" type="submit">{buttonValue}</Button>
    </Form>
}

export default Lobby;