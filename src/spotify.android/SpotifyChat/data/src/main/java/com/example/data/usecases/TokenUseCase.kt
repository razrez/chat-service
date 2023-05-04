package com.example.data.usecases

import com.example.data.datasource.DataSource
import com.example.domain.common.LoginData
import com.example.domain.common.ProfileData
import com.example.domain.common.Token
import com.example.domain.usecases.ITokenUseCase
import retrofit2.awaitResponse

class TokenUseCase : ITokenUseCase {

    override suspend fun logIn(loginData: LoginData): Token? {
        return DataSource
            .tokenService
            .logIn(loginData.grant_type, loginData.username, loginData.password)
            .awaitResponse()
            .body()
    }

    override suspend fun signUp(loginData: LoginData, profileData: ProfileData): Token? {
        return DataSource
            .tokenService
            .signUp(loginData.grant_type,
                    loginData.username,
                    loginData.password,
                    profileData.Name,
                    profileData.BirthYear,
                    profileData.BirthMonth,
                    profileData.BirthDay,
                    profileData.Country,
                    profileData.ProfileImg,)
            .awaitResponse()
            .body()
    }
}