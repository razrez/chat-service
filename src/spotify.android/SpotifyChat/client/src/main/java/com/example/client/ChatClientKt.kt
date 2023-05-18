package com.example.client

import chat.Chat.Message
import chat.ChatRoomGrpcKt.ChatRoomCoroutineStub
import chat.message
import io.grpc.ManagedChannel
import io.grpc.ManagedChannelBuilder
import io.grpc.ManagedChannelProvider
import kotlinx.coroutines.delay
import java.io.Closeable
import java.util.concurrent.TimeUnit
import kotlin.random.Random
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.flow

class ChatClientKt(private val channel: ManagedChannel) : Closeable {
    private val random = Random(314159)
    private val stub = ChatRoomCoroutineStub(channel)

    override fun close() {
        channel.shutdown().awaitTermination(5, TimeUnit.SECONDS)
    }

    suspend fun join(){
        println("join")
        val requests: Flow<Message> = generateOutgoingMessages()
        stub.join(requests).collect{message ->
            println("${message.user}:${message.text}")
        }
    }

    private fun generateOutgoingMessages(): Flow<Message> = flow {
        val messages = listOf(
            message {
                user = "rus"
                room = "rus"
                text = "salam"
            },
            message {
                user = "rus"
                room = "rus"
                text = "salam2"
            },
        )
        for (message in messages) {
            val mes = message.user
            println("${message.user}:${message.text}")
            emit(message)
            delay(500)
        }
    }

}

suspend fun main(){
    val channel = ManagedChannelBuilder
        .forAddress("10.0.2.2", 5059)
        .usePlaintext()
        .build()

    ChatClientKt(channel).use {client ->
        client.join()
    }
}