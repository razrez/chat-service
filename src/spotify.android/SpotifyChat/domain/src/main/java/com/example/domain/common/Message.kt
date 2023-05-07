package com.example.domain.common

import android.graphics.Bitmap
import kotlinx.serialization.Serializable

data class Message(
    val message: String,
    val sender: User?,
    val createdAt: Long,
    val imageBitmap: Bitmap?
)
