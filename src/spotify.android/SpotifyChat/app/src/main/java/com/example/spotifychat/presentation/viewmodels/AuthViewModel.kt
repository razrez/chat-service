package com.example.spotifychat.presentation.viewmodels

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.data.SongsQuery
import com.example.data.usecases.TokenUseCase
import com.example.domain.common.LoginData
import com.example.domain.common.Token
import com.example.spotifychat.Prefs
import kotlinx.coroutines.launch

class AuthViewModel : ViewModel() {

    private val tokenUseCase = TokenUseCase()
    val tokenMutableData = MutableLiveData<Token?>()

    fun signIn(loginData: LoginData) {
        viewModelScope.launch {
            val tokenData = tokenUseCase.logIn(loginData)

            // объясняю, отправляем полученный токен на обработку,
            // потом через метод observeData() в AuthorizationFragment полученное значение обрабатывается
            tokenMutableData.postValue(tokenData)
        }
    }
}