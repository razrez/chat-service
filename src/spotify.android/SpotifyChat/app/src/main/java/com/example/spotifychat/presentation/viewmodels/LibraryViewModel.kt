package com.example.spotifychat.presentation.viewmodels

import android.util.Log
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.core.http_clients.RabbitMqClient
import com.example.data.SongsQuery
import com.example.data.usecases.SongsUseCase
import com.example.domain.common.Song
import com.example.domain.common.SongStat
import com.example.domain.common.User
import com.google.gson.Gson
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import java.util.stream.Stream

class LibraryViewModel : ViewModel() {
    private val songsUseCase = SongsUseCase()
    val songsMutableList = MutableLiveData<List<Song>?>() // будет List<Song>
    val newStatMutable = MutableLiveData<SongStat>()
    var gson = Gson()

    fun getSongs() {
        viewModelScope.launch {
            val songsData = songsUseCase.getSongs() as List<SongsQuery.Node>?
            val songsStat = songsUseCase.getAllStats()
            val songs = mapSongs(songsData, songsStat)

            //Log.d("songs with statistics", songs.toString())
            songsMutableList.postValue(songs)
        }

    }

    fun consumeStatistics(){
        viewModelScope.launch {
            withContext(Dispatchers.IO) {
                val rabbit = RabbitMqClient();

                rabbit.consumeStatistics { message ->
                    val newStat = gson.fromJson(message, SongStat::class.java)
                    //Log.d("GETSTAT", "Got ${newStat.songId} ${newStat.listens}")
                    newStatMutable.postValue(newStat)
                }
            }
        }
    }

    private fun mapSongs(songsData:List<SongsQuery.Node>?, songsStat:List<SongStat>): List<Song>?{

        return songsData?.zip(songsStat)
        { node, stat ->
            Song(
                id = node.id,
                song = node.song,
                user = User(username = node.user.username!!),
                listens = stat.listens
            )
        }

    }


}