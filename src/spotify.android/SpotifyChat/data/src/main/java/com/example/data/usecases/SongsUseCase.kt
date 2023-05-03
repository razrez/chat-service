package com.example.data.usecases

import com.example.data.datasource.DataSourceGraphql
import com.example.domain.common.Song
import com.example.domain.usecases.ISongsUseCase

class SongsUseCase : ISongsUseCase {
    private val dataSourceGraphql = DataSourceGraphql()

    override suspend fun getSongs(): List<Song>? {
        return dataSourceGraphql.getSongs()
    }
}