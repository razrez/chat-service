package com.example.spotifychat.presentation.viewmodels

import android.util.Log
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.core.http_clients.RabbitMqClient
import com.example.data.SongsQuery
import com.example.data.usecases.SongsUseCase
import com.example.domain.common.SongStat
import com.google.gson.Gson
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext

class LibraryViewModel : ViewModel() {
    private val songsUseCase = SongsUseCase()
    val songsMutableList = MutableLiveData<List<SongsQuery.Node>?>() // будет List<Song>
    val newStatMutable = MutableLiveData<SongStat>()
    var gson = Gson()

    fun getSongs() {
        viewModelScope.launch {
            val songsData = songsUseCase.getSongs() as List<SongsQuery.Node>?
            //val songsStat = songsUseCase.getAllStats() //List<SongStat>

            // слеиваем songsData и songsStat по id в один тип songs:List<Song>
            //songsMutableList.postValue(songs)

            songsMutableList.postValue(songsData)
        }

        viewModelScope.launch {
            withContext(Dispatchers.IO) {
                val rabbit = RabbitMqClient();

                rabbit.consumeStatistics { message ->
                    val newStat = gson.fromJson(message, SongStat::class.java)
                    Log.d("GETSTAT", "Got ${newStat.songId} ${newStat.listens}")
                    newStatMutable.postValue(newStat)
                }
            }
        }
    }

    fun getAllStat(){

    }

}