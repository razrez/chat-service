package com.example.domain.common

import android.graphics.Bitmap
import kotlinx.serialization.Serializable

// TODO("нужен маппер, потому что у сообщений с бека немного другие поля")
data class Message(
    val message: String,
    val sender: User?,
    val createdAt: Long,
    val imageBitmap: Bitmap?
)
