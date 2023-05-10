package com.example.data.usecases

import com.example.data.datasource.DataSource
import com.example.domain.common.Message
import com.example.domain.usecases.IChatUseCase
import retrofit2.awaitResponse

class ChatUseCase : IChatUseCase {
    override suspend fun getChatHistory(usename: String): List<Message>? {
        return DataSource
            .chatService.getChatHistory(usename)
            .awaitResponse()
            .body()
    }
}