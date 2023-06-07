package com.example.domain.common

import org.w3c.dom.Node

data class Song (
    val id: Int,
    val song: String,
    val user: User,
    val listens: Int
)
