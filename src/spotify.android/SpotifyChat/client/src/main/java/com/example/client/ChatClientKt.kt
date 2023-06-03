package com.example.client

import chat.Chat.Message
import chat.ChatRoomGrpcKt
import chat.ChatRoomGrpcKt.ChatRoomCoroutineStub
import chat.message
import io.grpc.ClientCall
import io.grpc.ManagedChannel
import io.grpc.ManagedChannelBuilder
import kotlinx.coroutines.delay
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.flow
import kotlinx.coroutines.runBlocking
import java.awt.SystemColor.text
import java.io.Closeable
import java.util.concurrent.TimeUnit
import kotlin.random.Random

class ChatClientKt(private val channel: ManagedChannel) : Closeable {
    private val random = Random(314159)
    val stub = ChatRoomCoroutineStub(channel)

    override fun close() {
        channel.shutdown().awaitTermination(5, TimeUnit.SECONDS)
    }

    suspend fun join(){
        println("join")
        val requests: Flow<Message> = generateOutgoingMessages()
        val test = stub.join(requests).collect{message ->
            println("Got ${message.user}:${message.text}")
        }
        println("Finished")
    }

    suspend fun sendMessage(message: Flow<Message>){
        stub.join(message).collect{message ->
            println("Got ${message.user}:${message.text}")
        }
    }

    private fun generateOutgoingMessages(): Flow<Message> = flow {
        val messages = listOf(
            message {
                user = "user"
                room = "rus"
                text = "Hey! Can you help me1?"
            },
            message {
                user = "user"
                room = "rus"
                text = "Hey! Can you help me?2"
            }
        )
        for (message in messages) {
            val mes = message.user
            println("Sent ${message.user}:${message.text}")
            emit(message)
            delay(3000)
        }
    }

}

suspend fun main(){
    val channel = ManagedChannelBuilder
        .forTarget("localhost:5059")
        .usePlaintext()
        .build()

    ChatClientKt(channel).use {client ->
        client.join()
    }

}