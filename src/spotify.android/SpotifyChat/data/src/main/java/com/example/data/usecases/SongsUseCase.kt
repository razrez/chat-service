package com.example.data.usecases

import com.example.data.datasource.DataSource
import com.example.data.datasource.DataSourceGraphql
import com.example.domain.common.SongStat
import com.example.domain.datasource.usecases.ISongsUseCase
import retrofit2.awaitResponse

class SongsUseCase : ISongsUseCase {
    private val dataSourceGraphql = DataSourceGraphql()

    override suspend fun getSongs(): List<*>? {
        return dataSourceGraphql.getSongs()
    }

    override suspend fun getAllStats(): List<SongStat> {
        return DataSource.chatService.getAllStats()
            .awaitResponse()
            .body()!!
    }
}