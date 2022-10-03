import Lobby from './components/Lobby';
import './App.css';
import * as signalR from "@microsoft/signalr";
import Chat from "./components/Chat";
import {useState} from "react";
import 'bootstrap/dist/css/bootstrap.min.css'
const App = () => {
    const [connection, setConnection] = useState();
    const [messages, setMessages] = useState([]);
    //const [users, setUsers] = useState([]);

    const joinRoom = async (user, room) => {
        try
        {
            const connection = new signalR.HubConnectionBuilder()
                .withUrl("https://localhost:7015/chat")
                .build();

            connection.on("ReceiveMessage", (user, message) => {
                setMessages(messages => [...messages, {user, message}]); //для отображения сообщений
            })

            await connection.start();

            await connection.invoke('JoinRoom', {user, room});

            setConnection(connection);
        }

        catch (e) {
            console.log(e);
        }
    }


    const sendMessage = async (message) => {
        try {
            await connection.invoke('SendMessage', message);
        } catch (e) {
            console.log(e);
        }
    }

    return <div className="app">
        <h2>Chat</h2>
        <hr className="line"></hr>
        {!connection
            ? <Lobby joinRoom={joinRoom} />
            : <Chat messages={messages} sendMessage={sendMessage}/>}
    </div>


}

export default App;
