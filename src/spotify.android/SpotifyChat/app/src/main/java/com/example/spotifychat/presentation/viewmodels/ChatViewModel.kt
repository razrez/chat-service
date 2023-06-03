package com.example.spotifychat.presentation.viewmodels

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.asFlow
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
import okhttp3.internal.wait
import org.checkerframework.checker.units.qual.m
import java.util.concurrent.TimeUnit

class ChatViewModel : ViewModel() {
    private val chatUseCase = ChatUseCase()
    val messagesMutableList = MutableLiveData<List<Message>>()
    val messageOutGoing = MutableLiveData<chatMessage>()
    val messageMutableReceived = MutableLiveData<Message>()


    // gRPC Chat Client connection
    private lateinit var chatClient : ChatClientKt

    init {
        openChannel()
        viewModelScope.launch {
            chatClient.stub.join(messageOutGoing.asFlow()).collect{  messageReceived ->
                val messageReceived = Message(
                    message = messageReceived.text,
                    sender = User(messageReceived.user),
                    createdAt = System.currentTimeMillis(),
                    imageBitmap = null
                )

                messageMutableReceived.postValue(messageReceived)
                println(messageReceived.message)
            }
        }
    }
    fun sendMessage(username: String, messageText: String){

        messageOutGoing.postValue(message {
            user = username  //"user01@gmail.com"
            room = username
            text = messageText
        })

    }

    fun shutDownChannel(){
        chatClient.close()
    }
    fun openChannel(){
        var channel = ManagedChannelBuilder
            .forAddress("10.0.2.2", 5059)
            .usePlaintext()
            .build()

        chatClient = ChatClientKt(channel)
    }

    fun loadHistory(username: String){
        viewModelScope.launch{
            val messages = chatUseCase.getChatHistory(username)
            messagesMutableList.postValue(messages!!);
        }
    }
}
