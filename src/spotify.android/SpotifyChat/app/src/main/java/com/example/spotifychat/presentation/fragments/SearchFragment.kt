package com.example.spotifychat.presentation.fragments

import android.widget.ArrayAdapter
import android.widget.ListView
import com.example.core.base.FragmentBase
import com.example.spotifychat.R
import com.example.spotifychat.databinding.FragmentSearchBinding
import com.example.spotifychat.presentation.viewmodels.SearchViewModel

class SearchFragment: FragmentBase<FragmentSearchBinding, SearchViewModel>(R.id.mainFragmentContainer)  {

    override fun setUpViews() {
        super.setUpViews()

        val listView: ListView = binding.listView
        val list = arrayOf("Pop", "Rock", "Indie", "Podcasts", "Charts")

        val adapter: ArrayAdapter<String> = ArrayAdapter(this.requireContext(),
            R.layout.fragment_genre, list)
        listView.adapter = adapter;
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