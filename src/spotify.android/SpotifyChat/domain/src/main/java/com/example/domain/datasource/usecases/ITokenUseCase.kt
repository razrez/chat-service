package com.example.domain.datasource.usecases

import com.example.domain.common.LoginData
import com.example.domain.common.ProfileData
import com.example.domain.common.Token
import com.example.core.UserClaims

interface ITokenUseCase {
    suspend fun logIn(loginData : LoginData) : Token?
    suspend fun signUp(loginData: LoginData, profileData: ProfileData) : Token?
    suspend fun validateToken(token: String?): UserClaims?
}