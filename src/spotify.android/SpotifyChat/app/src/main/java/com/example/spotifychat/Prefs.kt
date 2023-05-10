package com.example.spotifychat

import android.content.Context
import android.content.SharedPreferences
import android.util.Log
import com.example.core.UserClaims

data class SharedValues(val isAuth: Boolean, val token: String?)

class Prefs(val context: Context) {
    private val PREFS_NAME = "myprefs"
    val sharedPref:SharedPreferences = context.getSharedPreferences(PREFS_NAME, Context.MODE_PRIVATE)

    fun saveToken(token: String) {
        val editor: SharedPreferences.Editor = sharedPref.edit()
        editor.putBoolean("isAuth", true)
        editor.putString("token", token)
        editor.apply()
        Log.d("Prefs", "Saved token $token")
    }

    fun saveClaims(claims: UserClaims) {
        val editor: SharedPreferences.Editor = sharedPref.edit()
        editor.putString("userId", claims.id)
        editor.putString("userName", claims.name)
        editor.apply()
        Log.d("Prefs", "Saved claims ${claims.id} ${claims.name}")
    }

    fun getAllPrefs(): SharedValues{
        val isAuth = sharedPref.getBoolean("isAuth", false)
        val token = sharedPref.getString("token", "")
        return SharedValues(isAuth, token)
    }

    fun clearSharedPreference() {
        val editor: SharedPreferences.Editor = sharedPref.edit()
        editor.clear()
        editor.apply()

    }
}

