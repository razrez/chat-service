package com.example.spotifychat.presentation.viewmodels

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.data.usecases.TokenUseCase
import com.example.domain.common.LoginData
import com.example.domain.common.ProfileData
import com.example.domain.common.Token
import com.example.core.UserClaims
import kotlinx.coroutines.launch

class RegistrationViewModel : ViewModel() {
    private val tokenUseCase = TokenUseCase()
    val tokenMutableData = MutableLiveData<Token?>()
    val claimsMutableData = MutableLiveData<UserClaims?>()

    fun signUp(loginData: LoginData, profileData: ProfileData) {
        viewModelScope.launch {
            val tokenData = tokenUseCase.signUp(loginData, profileData)
            val claims = tokenUseCase.validateToken(tokenData?.access_token)

            tokenMutableData.postValue(tokenData)
            claimsMutableData.postValue(claims)
        }
    }
}