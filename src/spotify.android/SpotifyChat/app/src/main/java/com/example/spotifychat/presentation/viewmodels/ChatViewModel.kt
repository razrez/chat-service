package com.example.spotifychat.presentation.viewmodels

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.data.usecases.ChatUseCase
import com.example.domain.common.Message
import kotlinx.coroutines.launch

class ChatViewModel : ViewModel() {
    private val chatUseCase = ChatUseCase()
    val messagesMutableList = MutableLiveData<List<Message>>()

    fun loadHistory(username: String){
        viewModelScope.launch{
            val messages = chatUseCase.getChatHistory(username)

            messagesMutableList.postValue(messages!!);
        }
    }

    fun sendMessage(message: Message){
        println("message sent to the server")
    }
}