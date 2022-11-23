import { useEffect, useRef } from "react";
import axios from "axios";

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

    
    const downloadFile = async (room, key) => {
        console.log("download logic goes here")
        //загрузка истории
        let getFileUrl = "http://localhost:5038/api/files/get-by-key?";
        window.open(`${getFileUrl}key=${room}/${key}`, '_blank', 'noopener,noreferrer');
        //console.log(room,key);
        /*await axios.get(`${getFileUrl}key=${room}/${key}`, {responseType: 'blob'})
            .then((response) => {
                console.log(response);
                
            });*/
    };

    return <div className='message-container' ref={messageRef}>

        {history.map((m, index) =>
            <div key={index} className='user-message'>
                <div className='message bg-primary'>{m.message}</div>
                <div className='from-user'>{m.user}</div>
            </div>
        )}

        {metaHistory.map((m, index) =>
            <div key={index} className="user-message user-file">
                <button className="message box" id="downloadBtn"
                        onClick={() => downloadFile(m.roomName, m.user + "/" + m.fileName)}
                        value="download">{m.fileName}
                </button>
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
                <button className="message box" id="downloadBtn"
                        onClick={() => downloadFile(m.roomName, m.user + "/" + m.fileName)}
                        value="download">{m.fileName}</button>
                <div className='from-user'>{m.user}</div>
            </div>
        )}


    </div>
}

export default MessageContainer;