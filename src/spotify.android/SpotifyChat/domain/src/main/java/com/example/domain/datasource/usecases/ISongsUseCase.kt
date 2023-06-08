package com.example.domain.datasource.usecases

import com.example.domain.common.SongStat

interface ISongsUseCase {
    suspend fun getSongs() : List<*>?
    suspend fun getAllStats() : List<SongStat>
}

