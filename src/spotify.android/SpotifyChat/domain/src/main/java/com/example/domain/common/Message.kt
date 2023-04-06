package com.example.domain.common

import kotlinx.serialization.Serializable

@Serializable
data class Message(
    val message: String,
    val sender: User?,
    val createdAt: Long
)
