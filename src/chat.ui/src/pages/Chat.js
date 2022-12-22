import SendMessageForm from '../components/SendMessageForm';
import MessageContainer from '../components/MessageContainer';
import ConnectedUsers from "../components/ConnectedUsers";
import {Button} from "react-bootstrap";

const Chat = ({messages, sendMessage, closeConnection, users, history, metaMessages, metaHistory, connection}) => <div>
    {users.length === 1 ? <marquee direction="up" scrollamount="5" className="runningText">wait please...</marquee>:""}
    <div className='leave-room'>
        <Button variant='dark' onClick={() => closeConnection() }>Leave Room</Button>
    </div>
    <ConnectedUsers users={users} />
    <div className='chat'>
        <MessageContainer messages={messages} history={history} metaHistory={metaHistory}
                          metaMessages={metaMessages} connection={connection}/>

        <SendMessageForm sendMessage={sendMessage} />
    </div>
   </div>

export default Chat;