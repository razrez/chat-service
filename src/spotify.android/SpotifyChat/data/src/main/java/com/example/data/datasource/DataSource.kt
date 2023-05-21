package com.example.data.datasource

import com.example.core.http_clients.RetrofitClient.Companion.retrofitChatClient
import com.example.core.http_clients.RetrofitClient.Companion.retrofitSpotifyClient
import com.example.domain.datasource.IDataSourceRetrofit

class DataSource {
    companion object{
        val tokenService: IDataSourceRetrofit = retrofitSpotifyClient.create(IDataSourceRetrofit::class.java)
        val chatService: IDataSourceRetrofit = retrofitChatClient.create(IDataSourceRetrofit::class.java)
    }
}