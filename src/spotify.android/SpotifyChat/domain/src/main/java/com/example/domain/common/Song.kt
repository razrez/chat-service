package com.example.domain.common

import kotlinx.serialization.Serializable

@Serializable
data class Song(
    val song: String,
    val user: User
)
