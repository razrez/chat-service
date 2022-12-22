import Lobby from './pages/Lobby';
import './App.css';
import * as signalR from "@microsoft/signalr";
import Chat from "./pages/Chat";
import {useState} from "react";
import 'bootstrap/dist/css/bootstrap.min.css'
import axios from "axios";
import {HttpTransportType} from "@microsoft/signalr";
import React from "react";
import { v4 as uuid } from 'uuid';

const App = () => {
    const [connection, setConnection] = useState();
    const [messages, setMessages] = useState([]);
    const [users, setUsers] = useState([]);
    const [history, setHistory] = useState([]);
    const [metaMessages, setMetaMessages] = useState([]);
    const [metaHistory, setMetaHistory] = useState([]);
    const [needHelp, setNeedHelp] = useState(true);
    const [isAdmin, setIsAdmin] = useState(true);

    //user info
    const [userName, setUserName] = useState();
    const [roomName, setRoomName] = useState();


    const joinRoom = async (user, room, isAdmin) => {
        try {

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

            connection.on('AdminInfo', async (roomToConnect) => {
                room = roomToConnect;
                setRoomName(roomToConnect);

                if (roomToConnect === '') {
                    setConnection();
                    setNeedHelp(false);
                    alert("no need help");
                }
            });
            await connection.start();
            if (isAdmin) {
                await connection.invoke('JoinRoomByAdmin', user);

                if(room !== ""){
                    //загрузка истории
                    let getHistoryUrl = "http://localhost:5038/api/chat?room=";
                    const loadedHistory = await axios.get(`${getHistoryUrl}${room}`)
                        .then(response => response.data);

                    //загрузка метаданных в комнате
                    let getMetadataUrl = "http://localhost:5038/api/file-metadata/get-by-room?room=";
                    const loadedMetadataHistory = await axios.get(`${getMetadataUrl}${roomName}`)
                        .then(response => response.data);

                    setUserName(user);
                    //setRoomName(room);
                    setConnection(connection);
                    setHistory(loadedHistory);
                    setMetaHistory(loadedMetadataHistory);
                    setIsAdmin(isAdmin);
                }
            }
            else {
                await connection.invoke('JoinRoom', {user, room});
                //setUserName(user);
                //setConnection(connection);

                //загрузка истории
                let getHistoryUrl = "http://localhost:5038/api/chat?room=";
                const loadedHistory = await axios.get(`${getHistoryUrl}${room}`)
                    .then(response => response.data);

                //загрузка метаданных в комнате
                let getMetadataUrl = "http://localhost:5038/api/file-metadata/get-by-room?room=";
                const loadedMetadataHistory = await axios.get(`${getMetadataUrl}${room}`)
                    .then(response => response.data);

                setConnection(connection);
                setUserName(user);
                setRoomName(room);
                setHistory(loadedHistory);
                setMetaHistory(loadedMetadataHistory);
                setIsAdmin(isAdmin);
            }
        }

        catch (e) {
            console.log(e);
        }
    }

    const uploadFile = async (file) => {
        //загрузка файла
        const reqId = uuid().slice(0,8);
        const formData = new FormData();
        formData.append('file', file);
        const fileConfig = {
            Headers: {
                'Content-Type': 'multipart/form-data',
            },
            d: {
                "fileName": file.name,
                "contentType": file.type,
                "roomName": roomName,
                "user": userName,
                "requestId": reqId
            }   
        };
        let postFileUrl = 'http://localhost:5038/api/files/upload?';
        //отправка базовых метаданных
        let postMetaUrl = 'http://localhost:5038/api/file-metadata';
        
        await Promise.all(
            axios.post(
                `${postFileUrl}roomName=${roomName}&prefix=${userName}&reqId=${reqId}`,
                formData, fileConfig),
            axios.post(postMetaUrl,
                {
                    "fileName": file.name,
                    "contentType": file.type,
                    "roomName": roomName,
                    "user": userName,
                    "requestId": reqId
                },
                {
                    Headers: {
                        'Content-Type': 'application/json',
                    }
                })
        );
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

    function getToken() {
        let cookie = {};
        document.cookie.split(';').forEach(function(el) {
            let [key,value] = el.split('=');
            cookie[key.trim()] = value;
        })
        return cookie['.AspNetCore.Connection.Token'];
    }

    const decode = require('jwt-claims');
    const userClaims = decode(getToken());

    return <div className="app">
        <h2>Chat</h2>
        <hr className="line"></hr>
        {!connection
            ? <Lobby joinRoom={joinRoom} userClaims={userClaims} />
            : <Chat messages={messages} sendMessage={sendMessage} closeConnection={closeConnection} users={users}
                    history={history} metaMessages={metaMessages} metaHistory={metaHistory} connection={connection}/>}
    </div>
}

export default App;
