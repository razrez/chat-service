package com.example.core.http_clients

import android.annotation.SuppressLint
import android.util.Log
import com.rabbitmq.client.*
import java.nio.charset.StandardCharsets

@SuppressLint("AuthLeak")
class RabbitMqClient {
    private var rabbitmq: Connection? = null;

    init {
        val factory = ConnectionFactory()
        factory.username = "guest"
        factory.password = "guest"
        factory.virtualHost = "/"
        factory.host = "10.0.2.2"
        factory.port = 5672
        rabbitmq = factory.newConnection()
    }

    fun consumeStatistics(callback: (msg: String) -> Unit)  {
        val channel = rabbitmq!!.createChannel()

        val deliverCallback = DeliverCallback { consumerTag: String?, delivery: Delivery ->
            val message = String(delivery.body, StandardCharsets.UTF_8)
            println("[$consumerTag] Received message: '$message'")
            callback(message)

        }
        val cancelCallback = CancelCallback { consumerTag: String? ->
            println("[$consumerTag] was canceled")
        }



        try {
            channel.basicConsume("stats-queue", true, "", deliverCallback, cancelCallback)
        }
        catch(e: Exception) {
            Log.e("error naxui", e.message.toString())
        }
    }

    fun close() {
        rabbitmq!!.abort()
    }
}