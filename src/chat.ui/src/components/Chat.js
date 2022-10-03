import SendMessageForm from './SendMessageForm';
import MessageContainer from './MessageContainer';

const Chat = ({messages, sendMessage}) => <div>
    <div className='chat'>
        <MessageContainer messages={messages} />
        <SendMessageForm sendMessage={sendMessage} />
    </div>
</div>

export default Chat;