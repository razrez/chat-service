package com.example.spotifychat.presentation.viewmodels

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.data.SongsQuery
import com.example.data.usecases.SongsUseCase
import kotlinx.coroutines.launch

class LibraryViewModel : ViewModel() {
    private val songsUseCase = SongsUseCase()
    val songsMutableList = MutableLiveData<List<SongsQuery.Node>?>()

    fun getSongs() {
        viewModelScope.launch {
            val songsData = songsUseCase.getSongs()
            songsMutableList.postValue(songsData as List<SongsQuery.Node>?)
        }
    }
}