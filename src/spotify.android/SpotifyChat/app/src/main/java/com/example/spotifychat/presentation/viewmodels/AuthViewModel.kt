package com.example.spotifychat.presentation.viewmodels

import androidx.lifecycle.ViewModel
import com.example.data.usecases.TokenUseCase
import com.example.domain.common.LoginData
import com.example.domain.common.Token

class AuthViewModel : ViewModel() {

    val tokenUseCase = TokenUseCase()

    suspend fun signIn(loginData: LoginData): Token? {
        return tokenUseCase.logIn(loginData)
    }
}