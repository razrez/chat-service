import SendMessageForm from '../components/SendMessageForm';
import MessageContainer from '../components/MessageContainer';
import ConnectedUsers from "../components/ConnectedUsers";
import {Button} from "react-bootstrap";
import {FileUploader} from "../components/FileUploader";

const Chat = ({messages, sendMessage, closeConnection, users, history}) => <div>
    <div className='leave-room'>
        <Button variant='danger' onClick={() => closeConnection() }>Leave Room</Button>
    </div>
    <ConnectedUsers users={users} />
    <div className='chat'>
        <MessageContainer messages={messages} history={history}/>
        <SendMessageForm sendMessage={sendMessage} />
    </div>
    <FileUploader></FileUploader>
   </div>

export default Chat;