package com.example.spotifychat

import android.content.Context;
import android.content.SharedPreferences;

data class SharedValues(val isAuth: Boolean, val token: String?)

class Prefs(val context: Context) {
    private val PREFS_NAME = "myprefs"
    val sharedPref:SharedPreferences = context.getSharedPreferences(PREFS_NAME,Context.MODE_PRIVATE)

    fun saveToken(isAuth: Boolean, token: String) {
        val editor: SharedPreferences.Editor = sharedPref.edit()
        editor.putBoolean("isAuth", isAuth)
        editor.putString("token", token)
        editor.apply()
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