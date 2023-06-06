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
        factory.host = "10.0.2.2"
        factory.port = 5672
        rabbitmq = factory.newConnection()
    }

    fun defaultExchangeAndQueue(): RabbitMqClient {
        val channel = rabbitmq?.createChannel()

        channel?.exchangeDeclare("logs", "fanout", false)
        channel?.queueDeclare("stats-queue", false, false, false, emptyMap())
        //channel?.queueBind("queue", "exchange", "mykey")

        channel?.close()
        rabbitmq?.close()

        return this
    }

    fun startListening() {
        val channel = rabbitmq?.createChannel()

        /*channel?.exchangeDeclare("logs", "fanout", false)
        channel?.queueDeclare("stats-queue", false, false, false, emptyMap())*/

        val deliverCallback = DeliverCallback{ consumerTag: String?, message: Delivery? ->
            Log.d("RES","Consume message: ${String(message!!.body)}")
        }
        val cancelCallback = CancelCallback { consumerTag: String? -> println("Cancelled... $consumerTag") }

        channel?.basicConsume("stats-queue", false, deliverCallback, cancelCallback)
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

        channel.basicConsume("stats-queue", true, "", deliverCallback, cancelCallback)
        /*try {
            Log.e("test", "aye robitt")
            channel.basicConsume("stats-queue", true, "", deliverCallback, cancelCallback)
        }
        catch(e: Exception) {
            Log.e("errrrrrr", e.message.toString())
        }*/
    }

    fun close() {
        rabbitmq!!.abort()
    }
}