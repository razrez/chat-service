package com.example.domain.datasource

import com.example.domain.common.Song

interface IDataSourceGraphql {
    suspend fun getSongs() : List<Song>?
}