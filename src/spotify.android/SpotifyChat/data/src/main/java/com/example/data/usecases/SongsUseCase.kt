package com.example.data.usecases

import com.example.data.datasource.DataSourceGraphql
import com.example.domain.datasource.usecases.ISongsUseCase

class SongsUseCase : ISongsUseCase {
    private val dataSourceGraphql = DataSourceGraphql()

    override suspend fun getSongs(): List<*>? {
        return dataSourceGraphql.getSongs()
    }
}