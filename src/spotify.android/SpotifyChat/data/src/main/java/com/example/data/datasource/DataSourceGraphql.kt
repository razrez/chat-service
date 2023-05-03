package com.example.data.datasource

import com.apollographql.apollo3.ApolloClient
import com.example.data.SongsQuery
import com.example.domain.common.Song
import com.example.domain.datasource.IDataSourceGraphql

class DataSourceGraphql : IDataSourceGraphql {
    companion object{
        val apolloClient: ApolloClient = ApolloClient.Builder()
            .serverUrl("https://localhost:7030/graphq")
            .build()
    }

    override suspend fun getSongs(): List<Song>? {
        val response = apolloClient.query(SongsQuery()).execute()
        return response.data?.songs?.nodes as List<Song>
    }
}