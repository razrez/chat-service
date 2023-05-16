package com.example.core.http_clients

import okhttp3.OkHttpClient
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import java.util.concurrent.TimeUnit

class RetrofitClient {

    companion object {

        private val client: OkHttpClient = OkHttpClient.Builder()
            .connectTimeout(40, TimeUnit.SECONDS)
            .callTimeout(40, TimeUnit.SECONDS)
            .build()

        val retrofitSpotifyClient: Retrofit =  Retrofit.Builder()
            .baseUrl("http://10.0.2.2:7030")
            .addConverterFactory(GsonConverterFactory.create())
            .build()

        val retrofitChatClient: Retrofit =  Retrofit.Builder()
            .baseUrl("http://10.0.2.2:5038")
            .addConverterFactory(GsonConverterFactory.create())
            .build()

    }

}