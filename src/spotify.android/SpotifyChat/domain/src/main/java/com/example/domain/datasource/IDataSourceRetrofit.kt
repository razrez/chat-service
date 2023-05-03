package com.example.domain.datasource

import com.example.domain.common.Token
import retrofit2.Call
import retrofit2.http.GET
import retrofit2.http.Path
import retrofit2.http.Query

interface IDataSourceRetrofit {

    @GET("posts/{id}")
    fun getUserData(@Path("id") id:String): Call<Token>
}