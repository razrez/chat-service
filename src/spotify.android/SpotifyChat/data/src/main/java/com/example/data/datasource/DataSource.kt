package com.example.data.datasource

import com.example.core.http_clients.RetrofitClient.Companion.retrofitClient
import com.example.domain.datasource.IDataSourceRetrofit

class DataSource {
    companion object{
        val tokenService: IDataSourceRetrofit = retrofitClient.create(IDataSourceRetrofit::class.java)
    }
}