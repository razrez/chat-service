package com.example.domain.datasource

import com.example.domain.common.LoginData
import com.example.domain.common.ProfileData
import com.example.domain.common.Token
import retrofit2.Call
import retrofit2.http.Body
import retrofit2.http.POST

interface IDataSourceRetrofit {
    // отправляем в body LoginData и получаем Token
    @POST("api/auth/login")
    fun logIn(@Body loginData:LoginData): Call<Token>

    @POST("api/auth/signup")
    fun signUp(@Body loginData:LoginData, @Body profileData: ProfileData): Call<Token>
}