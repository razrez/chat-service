package com.example.domain.datasource

import com.example.domain.common.UserData

// used in Ktor
interface IUserDataSource {
    suspend fun getUser(id: Int): UserData
}