package com.example.spotifychat.presentation.fragments

import android.content.res.ColorStateList
import android.graphics.Color
import android.util.Log
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.core.base.FragmentBase
import com.example.spotifychat.R
import com.example.spotifychat.databinding.FragmentLibraryBinding
import com.example.spotifychat.presentation.adapters.SongsRecyclerAdapter
import com.example.spotifychat.presentation.viewmodels.LibraryViewModel

class LibraryFragment : FragmentBase<FragmentLibraryBinding, LibraryViewModel>(R.id.mainFragmentContainer) {

    private lateinit var recyclerView: RecyclerView
    override fun setUpViews() {
        super.setUpViews()

        recyclerView = binding.recyclerSongs
        recyclerView.layoutManager = LinearLayoutManager(this.requireContext())
        viewModel.getSongs()
        viewModel.consumeStatistics()

        // active color
        binding.navFooterContainer.yourLibraryText.setTextColor(Color.WHITE)
        binding.navFooterContainer.buttonNavLibrary.backgroundTintList = ColorStateList.valueOf(Color.WHITE)

        binding.navFooterContainer.buttonNavSearch.setOnClickListener{
            this.requireActivity().supportFragmentManager
                .beginTransaction()
                .replace(R.id.mainFragmentContainer, SearchFragment.newInstance())
                .addToBackStack(this::javaClass.name)
                .commit()
        }

        binding.navFooterContainer.buttonNavChat.setOnClickListener {
            this.requireActivity().supportFragmentManager
                .beginTransaction()
                .replace(R.id.mainFragmentContainer, ChatFragment.newInstance())
                .addToBackStack(this::javaClass.name)
                .commit()
        }
    }

    override fun observeData() {
        super.observeData()

        viewModel.songsMutableList.observe(this){
            binding.recyclerSongs.adapter = SongsRecyclerAdapter(it)
            Log.d("songs with statistics", it.toString())
        }

        viewModel.newStatMutable.observe(this){
            (recyclerView.adapter as SongsRecyclerAdapter).updateStat(it)
            Log.d("GETSTAT", "Got ${it.songId} ${it.listens}")
        }
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
}