import { useEffect, useRef } from "react";

const MessageContainer = ({ messages, history }) => {

    //ссылка на message-container
    const messageRef = useRef();
    useEffect(() => {
        if(messageRef && messageRef.current){

            const { scrollHeight, clientHeight } = messageRef.current;

            messageRef.current.scrollTo({
                left: 0, top: scrollHeight - clientHeight,
                behavior: 'smooth'
            });
        }
    }, [messages]); //крч если messages изменяется

    return <div className='message-container' ref={messageRef}>
        {history.map((m, index) =>
            <div key={index} className='user-message'>
                <div className='message bg-primary'>{m.message}</div>
                <div className='from-user'>{m.user}</div>
            </div>
        )}
        {messages.map((m, index) =>
            <div key={index} className='user-message'>
                <div className='message bg-primary'>{m.message}</div>
                <div className='from-user'>{m.user}</div>
            </div>
        )}
    </div>
}

export default MessageContainer;