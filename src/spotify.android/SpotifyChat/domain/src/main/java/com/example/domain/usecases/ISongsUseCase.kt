package com.example.domain.usecases

import com.example.domain.common.Song

interface ISongsUseCase {
    suspend fun getSongs() : List<Song>?
}