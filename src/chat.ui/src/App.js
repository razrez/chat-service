import Lobby from './components/Lobby';
import './App.css';
import * as signalR from "@microsoft/signalr";

const App = () => {
    /*onst [connection, setConnection] = useState();
    const [messages, setMessages] = useState([]);
    const [users, setUsers] = useState([]);*/
    const joinRoom = (user, room) => {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:7229/chat")
            .build();

        connection.on("ReceiveMessage", (message) =>
        {
            console.log(message)
        });

        connection.start()
            .then( () => {
                connection.invoke('JoinRoom', {user, room})
            })
    }

    /*const sendMessage = async (message) => {
        try {
            await connection.invoke("SendMessage", message);
        } catch (e) {
            console.log(e);
        }
    }*/

    return <div className='app'>
        <h2>MyChat</h2>
        <hr className='line' />
        <Lobby joinRoom={joinRoom}></Lobby>
    </div>
}

export default App;
