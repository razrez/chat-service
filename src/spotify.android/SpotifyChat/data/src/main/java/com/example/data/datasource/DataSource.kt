package com.example.data.datasource

import com.apollographql.apollo3.ApolloClient
import com.example.core.http_clients.RetrofitClient.Companion.retrofitClient
import com.example.domain.datasource.IDataSourceRetrofit

class DataSource {
    companion object{
        val tokenService: IDataSourceRetrofit = retrofitClient.create(IDataSourceRetrofit::class.java)
        val chatService: IDataSourceRetrofit = retrofitClient.create(IDataSourceRetrofit::class.java) //по-моему хуйня какая-то
    }
}