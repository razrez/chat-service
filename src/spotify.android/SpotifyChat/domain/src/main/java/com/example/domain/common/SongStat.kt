package com.example.domain.common

import kotlinx.serialization.Serializable

@Serializable
data class SongStat(val songId: String, val listens: Int)
