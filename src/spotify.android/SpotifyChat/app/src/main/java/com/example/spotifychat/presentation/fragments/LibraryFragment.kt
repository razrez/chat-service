package com.example.spotifychat.presentation.fragments

import android.content.res.ColorStateList
import android.graphics.Color
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.core.base.FragmentBase
import com.example.spotifychat.R
import com.example.spotifychat.databinding.FragmentLibraryBinding
import com.example.spotifychat.presentation.adapters.SongsRecyclerAdapter
import com.example.spotifychat.presentation.viewmodels.LibraryViewModel

class LibraryFragment : FragmentBase<FragmentLibraryBinding, LibraryViewModel>(R.id.mainFragmentContainer) {

    override fun setUpViews() {
        super.setUpViews()

        val recyclerView: RecyclerView = binding.recyclerSongs
        recyclerView.layoutManager = LinearLayoutManager(this.requireContext())
        viewModel.getSongs()
        //viewModel.getStats()

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