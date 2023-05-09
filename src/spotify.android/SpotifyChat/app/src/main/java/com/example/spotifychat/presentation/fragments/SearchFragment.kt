package com.example.spotifychat.presentation.fragments

import android.content.res.ColorStateList
import android.graphics.Color
import android.graphics.drawable.Drawable
import android.widget.ArrayAdapter
import android.widget.ListView
import androidx.core.view.iterator
import com.example.core.base.FragmentBase
import com.example.spotifychat.R
import com.example.spotifychat.databinding.FragmentSearchBinding
import com.example.spotifychat.presentation.viewmodels.SearchViewModel

class SearchFragment: FragmentBase<FragmentSearchBinding, SearchViewModel>(R.id.mainFragmentContainer)  {

    override fun setUpViews() {
        super.setUpViews()

        val listView: ListView = binding.listView
        val list = arrayOf("Rock", "Jazz", "Techno", "Electro", "Country", "Pop")

        val adapter: ArrayAdapter<String> = ArrayAdapter(this.requireContext(),
            R.layout.fragment_genre, list)

        listView.adapter = adapter;

        // active color
        binding.navFooterContainer.searchText.setTextColor(Color.WHITE)
        binding.navFooterContainer.buttonNavSearch.backgroundTintList = ColorStateList.valueOf(Color.WHITE)

        binding.navFooterContainer.buttonNavLibrary.setOnClickListener{
            this.requireActivity().supportFragmentManager
                .beginTransaction()
                .replace(R.id.mainFragmentContainer, LibraryFragment.newInstance())
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

    override fun getViewModelClass(): Class<SearchViewModel> {
        return SearchViewModel::class.java
    }

    override fun getViewBinding(): FragmentSearchBinding {
        return FragmentSearchBinding.inflate(layoutInflater)
    }

    companion object {
        @JvmStatic
        fun newInstance() = SearchFragment()
    }
}