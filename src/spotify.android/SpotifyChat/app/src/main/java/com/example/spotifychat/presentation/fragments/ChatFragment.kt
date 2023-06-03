package com.example.spotifychat.presentation.fragments

import android.app.Activity
import android.content.Intent
import android.content.pm.PackageManager
import android.graphics.Bitmap
import android.graphics.ImageDecoder
import android.net.Uri
import android.os.Build
import android.provider.MediaStore
import android.widget.Toast
import androidx.core.app.ActivityCompat
import androidx.core.content.ContextCompat
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.core.base.FragmentBase
import com.example.domain.common.Message
import com.example.domain.common.User
import com.example.spotifychat.Prefs
import com.example.spotifychat.R
import com.example.spotifychat.databinding.FragmentChatBinding
import com.example.spotifychat.presentation.adapters.ChatRecyclerAdapter
import com.example.spotifychat.presentation.viewmodels.ChatViewModel

class ChatFragment : FragmentBase<FragmentChatBinding, ChatViewModel>(R.id.mainFragmentContainer) {

    private lateinit var recyclerView: RecyclerView
    private var pickedPhoto: Uri? = null
    private var pickedBitMap: Bitmap? = null
    override fun setUpViews() {
        super.setUpViews()

        val prefs = Prefs(this.requireActivity())
        var username = prefs.getAllPrefs().username!!

        // recycler settings
        recyclerView = binding.recyclerGchat
        recyclerView.layoutManager = LinearLayoutManager(this.requireContext())
        recyclerView.adapter = ChatRecyclerAdapter(fillList())

        // load messages history
        // viewModel.loadHistory(username)

        // invoke chat bidirectional streaming
        viewModel.sendMessage(username, R.string.hello.toString())

        // send message
        val messageInput = binding.editGchatMessage
        binding.buttonGchatSend.setOnClickListener{
            if (messageInput.text.toString() != ""){
                // add message to recycler
                val myMessage = Message(
                    message = messageInput.text.toString(),
                    sender = null,
                    createdAt = System.currentTimeMillis(),
                    imageBitmap = pickedBitMap // convert to bytearray
                )

                // add message to recycler and scroll to the bottom
                val newPosition = (recyclerView.adapter as ChatRecyclerAdapter).addMessage(myMessage)
                recyclerView.smoothScrollToPosition(newPosition)

                // post to the server
                var username = prefs.getAllPrefs().username!!
                viewModel.sendMessage(username, messageInput.text.toString())

                // clear input and attachments
                messageInput.setText("")
                pickedPhoto = null
                pickedBitMap = null
            }

        }

        // attach photo
        binding.buttonAttachment.setOnClickListener{
            pickPhoto()

            // toast if file's attached
            if (pickedBitMap != null){
                val toast = Toast
                    .makeText(this.requireContext(), "Attached!", Toast.LENGTH_SHORT)
                    .show()
            }
        }

    }

    override fun observeData() {
        super.observeData()

        // get all message's history
        viewModel.messagesMutableList.observe(this){
            if (it != null){
                recyclerView.adapter = ChatRecyclerAdapter(it as MutableList<Message>?)
            }
        }

        // receive messages from support manager
        viewModel.messageMutableReceived.observe(this){ it ->
            // add message to recycler and scroll to the bottom
            val newPosition = (recyclerView.adapter as ChatRecyclerAdapter).addMessage(it)
            recyclerView.smoothScrollToPosition(newPosition)
        }

    }

    override fun onDestroy() {
        super.onDestroy()
        //viewModel.shutDownChannel()
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

    @Deprecated("Deprecated in Java")
    override fun onRequestPermissionsResult(
        requestCode: Int,
        permissions: Array<out String>,
        grantResults: IntArray
    ) {
        if (requestCode == 1) {
            if (grantResults.size > 0  && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                val galeriIntext = Intent(Intent.ACTION_PICK,MediaStore.Images.Media.EXTERNAL_CONTENT_URI)
                startActivityForResult(galeriIntext,2)
            }
        }
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
    }

    @Deprecated("Deprecated in Java")
    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        if (requestCode == 2 && resultCode == Activity.RESULT_OK && data != null) {
            pickedPhoto = data.data
            if (pickedPhoto != null) {
                if (Build.VERSION.SDK_INT >= 28) {
                    val source = ImageDecoder.createSource(this.requireActivity().contentResolver,pickedPhoto!!)
                    pickedBitMap = ImageDecoder.decodeBitmap(source)
                }
                else {
                    pickedBitMap = MediaStore.Images.Media.getBitmap(this.requireActivity().contentResolver,pickedPhoto)
                }
            }
        }
        super.onActivityResult(requestCode, resultCode, data)
    }

    private fun pickPhoto(){
        if (ContextCompat.checkSelfPermission(this.requireContext(),android.Manifest.permission.READ_EXTERNAL_STORAGE)
            != PackageManager.PERMISSION_GRANTED) {
            ActivityCompat.requestPermissions(this.requireActivity(),arrayOf(android.Manifest.permission.READ_EXTERNAL_STORAGE),
                1)
        } else {
            val galeriIntext = Intent(Intent.ACTION_PICK, MediaStore.Images.Media.EXTERNAL_CONTENT_URI)
            startActivityForResult(galeriIntext,2)
        }
    }

    // Let's create a useless list of messages for now, which we will pass to the adapter.
    private fun fillList(): MutableList<Message> {
        val data = mutableListOf<Message>()
        data.add(
            Message(
                message = "can u help me?",
                sender = null,
                createdAt = System.currentTimeMillis(),
                imageBitmap = pickedBitMap
            )
        )

        /*(0..9).forEach {

            if (it % 2 != 0){
                data.add(
                    Message(
                        message = "no problem :)",
                        sender = User("support"),
                        createdAt = System.currentTimeMillis(),
                        imageBitmap = pickedBitMap
                    )
                )
            }

            else{
                data.add(
                    Message(
                        message = "can u help me?",
                        sender = null,
                        createdAt = System.currentTimeMillis(),
                        imageBitmap = pickedBitMap
                    )
                )
            }

        }*/
        return data
    }
}