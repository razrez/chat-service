package com.example.spotifychat.presentation.fragments

import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.core.base.FragmentBase
import com.example.domain.common.Message
import com.example.domain.common.User
import com.example.spotifychat.R
import com.example.spotifychat.databinding.FragmentChatBinding
import com.example.spotifychat.presentation.adapters.ChatRecyclerAdapter
import com.example.spotifychat.presentation.adapters.SongsRecyclerAdapter
import com.example.spotifychat.presentation.viewmodels.ChatViewModel

class ChatFragment : FragmentBase<FragmentChatBinding, ChatViewModel>(R.id.mainFragmentContainer) {

    private lateinit var recyclerView: RecyclerView
    override fun setUpViews() {
        super.setUpViews()

        recyclerView = binding.recyclerGchat
        recyclerView.layoutManager = LinearLayoutManager(this.requireContext())
        recyclerView.adapter = ChatRecyclerAdapter(fillList())
        //viewModel.loadMessages()

        val messageInput = binding.editGchatMessage
        binding.buttonGchatSend.setOnClickListener{
            if (messageInput.text.toString() != ""){
                // add message to recycler
                val myMessage = Message(
                    message = messageInput.text.toString(),
                    sender = null,
                    createdAt = System.currentTimeMillis()
                )

                // add message to recycler and scroll to the bottom
                val newPosition = (recyclerView.adapter as ChatRecyclerAdapter).addMessage(myMessage)
                recyclerView.smoothScrollToPosition(newPosition)

                // post to the server
                viewModel.sendMessage(myMessage)

                // clear input
                messageInput.setText("")
            }
        }
    }

    override fun observeData() {
        super.observeData()

        // get all messages
        viewModel.messagesMutableList.observe(this){
            recyclerView.adapter = ChatRecyclerAdapter(it as MutableList<Message>?)
        }
    }

    override fun onResume() {
        super.onResume()
    }

    override fun getViewModelClass(): Class<ChatViewModel> {
        return ChatViewModel::class.java
    }

    override fun getViewBinding(): FragmentChatBinding {
        return FragmentChatBinding.inflate(layoutInflater)
    }

    companion object {
        @JvmStatic
        fun newInstance() = ChatFragment()
    }

    // Let's create a useless list of messages for now, which we will pass to the adapter.
    private fun fillList(): MutableList<Message> {
        val data = mutableListOf<Message>()
        (0..9).forEach {

            if (it % 2 != 0){
                data.add(
                    Message(
                        message = "no problem :)",
                        sender = User("support"),
                        createdAt = System.currentTimeMillis()
                    )
                )
            }

            else{
                data.add(
                    Message(
                        message = "can u help me?",
                        sender = null,
                        createdAt = System.currentTimeMillis()
                    )
                )
            }

        }
        return data
    }
}