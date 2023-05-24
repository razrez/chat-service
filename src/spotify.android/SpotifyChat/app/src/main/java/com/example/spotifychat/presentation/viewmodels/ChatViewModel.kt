package com.example.spotifychat.presentation.viewmodels

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.client.ChatClientKt
import com.example.data.usecases.ChatUseCase
import com.example.domain.common.Message
import kotlinx.coroutines.launch
import io.grpc.ManagedChannel
import io.grpc.ManagedChannelBuilder

class ChatViewModel : ViewModel() {
    private val chatUseCase = ChatUseCase()
    val messagesMutableList = MutableLiveData<List<Message>>()
    val channel = ManagedChannelBuilder
        .forTarget("localhost:5059")
        .usePlaintext()
        .build()

    fun loadHistory(username: String){
        viewModelScope.launch{
            val messages = chatUseCase.getChatHistory(username)

            messagesMutableList.postValue(messages!!);
        }
    }

    fun sendMessage(message: Message){
        println("message sent to the server")
    }

    suspend fun join(){
        viewModelScope.launch {
            ChatClientKt(channel).use { client ->
                client.join()
            }
        }
    }
}