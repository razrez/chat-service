package com.example.core.http_clients

import android.util.Log
import com.rabbitmq.client.*
import java.nio.charset.StandardCharsets

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

    fun defaultExchangeAndQueue(): RabbitMqClient {
        val channel = rabbitmq?.createChannel()

        channel?.exchangeDeclare("exchange", "fanout", true)
        channel?.queueDeclare("queue", true, false, true, emptyMap())
        channel?.queueBind("queue", "exchange", "mykey")

        channel?.close()
        rabbitmq?.close()

        return this
    }

    fun consumeLogout(queueName: String, session: String, callback: (msg: String) -> Unit) {
        println("I'm ok")
        val channel = rabbitmq!!.createChannel()

        val deliverCallback = DeliverCallback { consumerTag: String?, delivery: Delivery ->
            val message = String(delivery.body, StandardCharsets.UTF_8)
            println("[$consumerTag] Received message: '$message'")
            if ( message == session) {
                callback(message)
                channel.basicAck(delivery.envelope.deliveryTag, false)
            }

        }
        val cancelCallback = CancelCallback { consumerTag: String? ->
            println("[$consumerTag] was canceled")
        }

        try {
            channel.basicConsume(queueName, true, "", deliverCallback, cancelCallback)
        }
        catch(e: Exception) {
            Log.e("errrrrrr", e.message.toString())
        }
    }

    fun consumeSome(callback: (msg: String) -> Unit)  {
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
            Log.e("errrrrrr", e.message.toString())
        }
    }

    fun close() {
        rabbitmq!!.abort()
    }
}