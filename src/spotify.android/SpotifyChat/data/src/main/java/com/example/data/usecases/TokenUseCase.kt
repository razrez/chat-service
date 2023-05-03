package com.example.data.usecases

import com.example.data.datasource.DataSource
import com.example.domain.common.LoginData
import com.example.domain.common.Token
import com.example.domain.usecases.ITokenUseCase
import retrofit2.awaitResponse

class TokenUseCase : ITokenUseCase {

    override suspend fun logIn(loginData: LoginData): Token? {
        return DataSource
            .tokenService
            .logIn(loginData)
            .awaitResponse()
            .body()
    }

    override suspend fun signUp(id: Int): Token? {
        TODO("Not yet implemented")
    }
}