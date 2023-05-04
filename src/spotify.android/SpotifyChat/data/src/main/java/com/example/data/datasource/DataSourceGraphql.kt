package com.example.data.datasource

import com.apollographql.apollo3.ApolloClient
import com.example.data.SongsQuery
import com.example.domain.common.Song
import com.example.domain.datasource.IDataSourceGraphql

class DataSourceGraphql : IDataSourceGraphql {

    private val apolloClient: ApolloClient = ApolloClient.Builder()
        .serverUrl("http://10.0.2.2:7030/graphql")
        .build()

    override suspend fun getSongs(): List<*>? {
        val response = apolloClient.query(SongsQuery()).execute()
        return response.data?.songs?.nodes
    }
}