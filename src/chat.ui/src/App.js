import Lobby from './pages/Lobby';
import './App.css';
import * as signalR from "@microsoft/signalr";
import Chat from "./pages/Chat";
import {useState} from "react";
import 'bootstrap/dist/css/bootstrap.min.css'
import axios from "axios";
import {HttpTransportType} from "@microsoft/signalr";

const App = () => {
    const [connection, setConnection] = useState();
    const [messages, setMessages] = useState([]);
    const [users, setUsers] = useState([]);
    const [history, setHistory] = useState([]);
    const [metaMessages, setMetaMessages] = useState([]);
    const [metaHistory, setMetaHistory] = useState([]);

    //user info
    const [userName, setUserName] = useState();
    const [roomName, setRoomName] = useState();

    const joinRoom = async (user, room) => {
        try
        {
            const connection = new signalR.HubConnectionBuilder()
                .withUrl("http://localhost:5038/chat", {
                    withCredentials: false,
                    skipNegotiation: true,
                    transport: HttpTransportType.WebSockets
                })
                .withAutomaticReconnect()
                .build();

            connection.on("ReceiveMessage", (user, message) => {
                setMessages(messages => [...messages, {user, message}]); //для отображения рилтайм сообщений
            });

            // стейт сбрасывается и перекидывает в лобби компонент
            connection.onclose(() => {
                setConnection();
                setMessages([]);
                setUsers([]);
            });

            // вывод юзеров в чате
            connection.on('UsersInRoom', (users) => {
                setUsers(users);
            });

            // вывод только что отправленных метаданных
            connection.on('ReceiveMeta', (newMeta) => {
                setMetaMessages(current => [...current, newMeta]);
            });

            await connection.start();

            //загрузка истории
            let getHistoryUrl = "http://localhost:5038/api/chat?room=";
            const loadedHistory = await axios.get(`${getHistoryUrl}${room}`)
                .then(response => response.data);

            //загрузка метаданных в комнате
            let getMetadataUrl = "http://localhost:5038/api/file-metadata/get-by-room?room=";
            const loadedMetadataHistory = await axios.get(`${getMetadataUrl}${room}`)
                .then(response => response.data);

            await connection.invoke('JoinRoom', {user, room});

            setConnection(connection);
            setUserName(user);
            setRoomName(room);
            setHistory(loadedHistory);
            setMetaHistory(loadedMetadataHistory);
        }

        catch (e) {
            console.log(e);
        }
    }

    const uploadFile = async (file) => {
        //загрузка файла
        const formData = new FormData();
        formData.append('file', file);
        const config = {
            Headers: {
                'Content-Type': 'multipart/form-data',
            },
        };
        let postFileUrl = 'http://localhost:5038/api/files/upload?';
        const response = await axios.post(
            `${postFileUrl}bucketName=${roomName}&prefix=${userName}`,
            formData, config)
            .then(response => response.data);

        //отрисовка метаданных у всей комнаты для возможности скачивания
        await connection.invoke('SendMetadata', response);
    }

    const sendMessage = async (message, file) => {
        try {

            if (message.trim() !== '' ){
                await connection.invoke('SendMessage', message);
                if (file !== undefined){
                    await uploadFile(file);
                }

            }

            else if(file !== undefined){
                await uploadFile(file);
            }

            else console.log("attempt to send an empty string")
        } catch (e) {
            console.log(e);
        }
    }

    const closeConnection = async () => {
        try {
            await connection.stop();
        }
        catch (e) {
            console.log(e);
        }
    }

    return <div className="app">
        <h2>Chat</h2>
        <hr className="line"></hr>
        {!connection
            ? <Lobby joinRoom={joinRoom} />
            : <Chat messages={messages} sendMessage={sendMessage} closeConnection={closeConnection} users={users}
                    history={history} metaMessages={metaMessages} metaHistory={metaHistory} connection={connection}/>}
    </div>
}

export default App;
