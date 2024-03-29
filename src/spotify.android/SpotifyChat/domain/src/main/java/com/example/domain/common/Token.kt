package com.example.domain.common
import kotlinx.serialization.Serializable

@Serializable
data class Token(
    val access_token: String,
    val token_type: String,
    val expires_in: Int,
    val scope: String,
    val refresh_token: String,
)
