package com.example.domain.datasource

import com.example.domain.common.LoginData
import com.example.domain.common.Token
import retrofit2.Call
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.POST
import retrofit2.http.Path
import retrofit2.http.Query

interface IDataSourceRetrofit {
    // отправляем в body LoginData и получаем Token
    @POST("api/auth/login")
    fun logIn(@Body loginData:LoginData): Call<Token>

}