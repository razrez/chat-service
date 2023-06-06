package com.example.spotifychat.presentation.viewmodels

import android.util.Log
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.core.http_clients.RabbitMqClient
import com.example.data.SongsQuery
import com.example.data.usecases.SongsUseCase
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext

class LibraryViewModel : ViewModel() {
    private val songsUseCase = SongsUseCase()
    val songsMutableList = MutableLiveData<List<SongsQuery.Node>?>()

    fun getSongs() {
        viewModelScope.launch {
            val songsData = songsUseCase.getSongs()
            songsMutableList.postValue(songsData as List<SongsQuery.Node>?)
        }
    }

    fun getStats() {
        viewModelScope.launch {
            withContext(Dispatchers.IO) {
                val rabbit = RabbitMqClient()

                rabbit.startListening()
            }
        }
    }
}