package com.example.spotifychat.presentation.viewmodels

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.domain.common.Message
import kotlinx.coroutines.launch

class ChatViewModel : ViewModel() {
    //private val messagesUseCase = MessagesUseCase()
    val messagesMutableList = MutableLiveData<List<Message>>()

    fun loadMessages(){
        viewModelScope.launch{
            // fetch history through usecase, then post data to observe in fragment
        }
    }

    fun sendMessage(message: Message){
        println("message sent to the server")
    }
}