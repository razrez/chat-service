package com.example.domain.datasource

import com.example.domain.common.LoginData
import com.example.domain.common.ProfileData
import com.example.domain.common.Token
import retrofit2.Call
import retrofit2.http.Body
import retrofit2.http.Field
import retrofit2.http.FormUrlEncoded
import retrofit2.http.Headers
import retrofit2.http.POST

interface IDataSourceRetrofit {
    // отправляем в body LoginData и получаем Token
    @FormUrlEncoded
    @POST("api/auth/login")
    fun logIn(@Field("grant_type") grant_type:String,
              @Field("username") username:String,
              @Field("password") password:String ): Call<Token>

    @POST("api/auth/signup")
    fun signUp(@Body loginData:LoginData, @Body profileData: ProfileData): Call<Token>
}