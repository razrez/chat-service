package com.example.domain.common

data class LoginData(
    val grant_type:String = "password",
    val username:String,
    val password:String
)
