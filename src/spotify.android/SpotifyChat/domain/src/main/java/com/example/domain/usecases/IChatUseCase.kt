package com.example.domain.usecases

import com.example.domain.common.Message

interface IChatUseCase {
    suspend fun getChatHistory(username: String) : List<Message>?
}