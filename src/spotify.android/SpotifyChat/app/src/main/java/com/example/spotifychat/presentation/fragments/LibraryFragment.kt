package com.example.spotifychat.presentation.fragments

import android.annotation.SuppressLint
import android.graphics.Color
import androidx.lifecycle.ViewModelProvider
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Adapter
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import androidx.recyclerview.widget.RecyclerView.ViewHolder
import com.example.core.base.FragmentBase
import com.example.spotifychat.R
import com.example.spotifychat.databinding.FragmentLibraryBinding
import com.example.spotifychat.databinding.FragmentRegistrationEmailBinding
import com.example.spotifychat.presentation.adapters.CustomRecyclerAdapter
import com.example.spotifychat.presentation.adapters.SongsRecyclerAdapter
import com.example.spotifychat.presentation.viewmodels.EmailViewModel
import com.example.spotifychat.presentation.viewmodels.LibraryViewModel

class LibraryFragment : FragmentBase<FragmentLibraryBinding, LibraryViewModel>(R.id.mainFragmentContainer) {

    override fun setUpViews() {
        super.setUpViews()

        val recyclerView: RecyclerView = binding.recyclerSongs
        recyclerView.layoutManager = LinearLayoutManager(this.requireContext())
        recyclerView.adapter = SongsRecyclerAdapter(fillSongs())
    }

    override fun getViewModelClass(): Class<LibraryViewModel> {
        return LibraryViewModel::class.java
    }

    override fun getViewBinding(): FragmentLibraryBinding {
        return FragmentLibraryBinding.inflate(layoutInflater)
    }

    companion object {
        @JvmStatic
        fun newInstance() = LibraryFragment()
    }

    private fun fillSongs(): List<String> {
        val data = mutableListOf<String>()
        (0..30).forEach { i -> data.add("$i element") }
        return data
    }
}