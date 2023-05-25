package com.example.spotifychat.presentation.viewmodels

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import chat.Chat
import chat.message
import chat.Chat.Message as chatMessage
import com.example.client.ChatClientKt
import com.example.data.usecases.ChatUseCase
import com.example.domain.common.Message
import com.example.domain.common.User
import kotlinx.coroutines.launch
import io.grpc.ManagedChannelBuilder
import io.ktor.util.Identity.decode
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.collect
import kotlinx.coroutines.flow.flow
import kotlinx.coroutines.flow.flowOf
import java.util.concurrent.TimeUnit

class ChatViewModel : ViewModel() {
    private val chatUseCase = ChatUseCase()
    val messagesMutableList = MutableLiveData<List<Message>>()


    // gRPC Chat Client connection
    private val channel = ManagedChannelBuilder
        .forAddress("10.0.2.2", 5059)
        .usePlaintext()
        .build()
    private val chatClient = ChatClientKt(channel)

    fun loadHistory(username: String){
        viewModelScope.launch{
            val messages = chatUseCase.getChatHistory(username)
            messagesMutableList.postValue(messages!!);
        }
    }

    fun sendMessage(username: String, messageText: String){
        val message : Flow<chatMessage> = flow{
            emit(message {
                user = username  //"user01@gmail.com"
                room = username
                text = messageText
            })
        }

        viewModelScope.launch {
            chatClient.stub.join(message).collect{ messageReceived ->
                val messageReceived = Message(
                    message = messageReceived.text,
                    sender = User(messageReceived.user),
                    createdAt = System.currentTimeMillis(),
                    imageBitmap = null
                )

                println(messageReceived.message)
                //messagesMutableList.postValue(listOf(messageReceived!!))
            }
        }
    }

    private suspend fun join(){

        chatClient.use { client ->
            client.join()
        }
    }

    fun joinChatMessaging(){
        viewModelScope.launch {
            join()
        }
    }

}