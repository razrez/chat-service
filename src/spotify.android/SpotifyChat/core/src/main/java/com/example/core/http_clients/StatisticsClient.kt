package com.example.core.http_clients

import okhttp3.OkHttpClient
import java.io.IOException

class StatisticsClient {

    companion object{
        val client: OkHttpClient = OkHttpClient()
    }
}