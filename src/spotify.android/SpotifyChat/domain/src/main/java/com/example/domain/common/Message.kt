package com.example.domain.common

data class Message(
    val message: String,
    val sender: User,
    val createdAt: Long
)
