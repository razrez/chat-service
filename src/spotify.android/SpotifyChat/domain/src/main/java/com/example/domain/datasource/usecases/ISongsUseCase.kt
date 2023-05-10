package com.example.domain.datasource.usecases

interface ISongsUseCase {
    suspend fun getSongs() : List<*>?
}