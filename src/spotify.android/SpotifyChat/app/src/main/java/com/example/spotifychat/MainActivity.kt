package com.example.spotifychat

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import com.example.spotifychat.databinding.ActivityMainBinding
import com.example.spotifychat.presentation.fragments.StartFragment

class MainActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        val binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(R.layout.activity_main)
        supportActionBar?.hide()

        supportFragmentManager
            .beginTransaction()
            .replace(R.id.mainFragmentContainer, StartFragment.newInstance())
            .commit()
    }
}