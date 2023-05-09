package com.example.core

import kotlinx.serialization.Serializable

@Serializable
data class UserClaims(
    val id: String,
    val name: String,
    val role: String
)