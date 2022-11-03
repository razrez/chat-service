import { useEffect, useRef } from "react";

const MessageContainer = ({ messages, history, metaMessages, metaHistory, connection}) => {

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
    }, [messages, metaMessages]); //крч если messages изменяется

    useEffect(() => {
        }, [metaMessages]);
    //то, что уже сохранено в монго
    console.log(metaHistory);
    const downloadFile = () => {
        console.log("download logic goes here")
        console.log(metaMessages)
    }

    return <div className='message-container' ref={messageRef}>

        {history.map((m, index) =>
            <div key={index} className='user-message'>
                <div className='message bg-primary'>{m.message}</div>
                <div className='from-user'>{m.user}</div>
            </div>
        )}

        {metaHistory.map((m, index) =>
            <div key={index} className="user-message user-file">
                <button className="message box" id="downloadBtn" onClick={downloadFile} value="download">{m.fileName}</button>
                <div className='from-user'>{m.user}</div>
            </div>
        )}

        {messages.map((m, index) =>
            <div key={index} className='user-message'>
                <div className='message bg-primary'>{m.message}</div>
                <div className='from-user'>{m.user}</div>
            </div>
        )}

        {metaMessages.map((m, index) =>
            <div key={index} className="user-message user-file">
                <button className="message box" id="downloadBtn" onClick={downloadFile} value="download">{m.fileName}</button>
                <div className='from-user'>{m.user}</div>
            </div>
        )}


    </div>
}

export default MessageContainer;