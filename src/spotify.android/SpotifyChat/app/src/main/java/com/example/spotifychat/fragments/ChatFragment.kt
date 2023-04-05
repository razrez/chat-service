package com.example.spotifychat.fragments

import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.core.http_clients.FragmentBase
import com.example.spotifychat.R
import com.example.spotifychat.databinding.FragmentChatBinding
import com.example.spotifychat.viewmodels.ChatViewModel

class ChatFragment : FragmentBase<FragmentChatBinding, ChatViewModel>(R.id.mainFragmentContainer) {

    override fun setUpViews() {
        super.setUpViews()

        val recycleView: RecyclerView = binding.recyclerGchat
        recycleView.layoutManager = LinearLayoutManager(this.requireContext())
    }

    override fun getViewModelClass(): Class<ChatViewModel> {
        return ChatViewModel::class.java
    }

    override fun getViewBinding(): FragmentChatBinding {
        return FragmentChatBinding.inflate(layoutInflater)
    }
}