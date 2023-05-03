package com.example.domain.usecases

import com.example.domain.common.LoginData
import com.example.domain.common.Token

interface ITokenUseCase {
    suspend fun logIn(loginData : LoginData) : Token?
    suspend fun signUp(id: Int) : Token?
}