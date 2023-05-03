package com.example.core.http_clients

import okhttp3.OkHttpClient
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import java.util.concurrent.TimeUnit

class RetrofitClient {

    companion object {

        private val client: OkHttpClient = OkHttpClient.Builder()
            .connectTimeout(10, TimeUnit.SECONDS)
            .callTimeout(10, TimeUnit.SECONDS)
            .build()

        val retrofitClient: Retrofit =  Retrofit.Builder()
            .baseUrl("https://localhost:7030")
            .addConverterFactory(GsonConverterFactory.create())
            .build()

    }

}