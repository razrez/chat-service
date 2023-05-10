package com.example.spotifychat.presentation.adapters

import android.annotation.SuppressLint
import android.graphics.Bitmap
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageView
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import androidx.recyclerview.widget.RecyclerView.ViewHolder
import com.example.domain.common.Message
import com.example.spotifychat.R
import com.example.spotifychat.presentation.adapters.ChatRecyclerAdapter.ViewHolderConstants.VIEW_TYPE_MESSAGE_RECEIVED
import com.example.spotifychat.presentation.adapters.ChatRecyclerAdapter.ViewHolderConstants.VIEW_TYPE_MESSAGE_SENT
import com.example.spotifychat.presentation.adapters.ChatRecyclerAdapter.ViewHolderConstants.dateFormatter
import java.text.SimpleDateFormat

// https://medium.com/codex/how-to-build-a-messaging-ui-for-your-android-chat-app-883fad05f43a
class ChatRecyclerAdapter(private val messages: MutableList<Message>?) :
    RecyclerView.Adapter<ViewHolder>()  {

    object ViewHolderConstants {
        const val VIEW_TYPE_MESSAGE_SENT  = 1
        const val VIEW_TYPE_MESSAGE_RECEIVED  = 2

        @SuppressLint("SimpleDateFormat")
        var dateFormatter: SimpleDateFormat = SimpleDateFormat("hh:mm")
    }

    class ReceivedMessageHolder(itemView: View) : ViewHolder(itemView){
        private val messageText: TextView = itemView.findViewById(R.id.text_gchat_message_other)
        private val timeText: TextView = itemView.findViewById(R.id.text_gchat_timestamp_other)
        private val nameText: TextView = itemView.findViewById(R.id.text_gchat_user_other)
        private val imageAttach: ImageView = itemView.findViewById(R.id.image_attach)

        fun bind(message: Message){
            messageText.text = message.message
            nameText.text = message.sender!!.username
            timeText.text = dateFormatter.format(message.createdAt)
            if (message.imageBitmap != null){
                imageAttach.setImageBitmap(message.imageBitmap) //convert bytes to BitMap
            }
        }
    }

    class SentMessageHolder (itemView: View) : ViewHolder(itemView){

        private val messageText: TextView = itemView.findViewById(R.id.text_gchat_message_me)
        private val timeText: TextView = itemView.findViewById(R.id.text_gchat_timestamp_me)
        private val imageAttach: ImageView = itemView.findViewById(R.id.image_attach)

        fun bind(message: Message){
            messageText.text = message.message
            timeText.text = dateFormatter.format(message.createdAt)
            if (message.imageBitmap != null){
                imageAttach.setImageBitmap(message.imageBitmap)
            }
        }
    }

    // returns amount of collection's elements
    override fun getItemCount(): Int {

        if (messages == null)
            return 0

        return messages.size
    }

    // Determines the appropriate ViewType according to the sender of the message.
    override fun getItemViewType(position: Int): Int {
        val message = messages?.get(position)

        if(message?.sender?.username == null){
            return VIEW_TYPE_MESSAGE_SENT
        }

        return VIEW_TYPE_MESSAGE_RECEIVED
    }

    // Inflates the appropriate layout according to the ViewType.
    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ViewHolder {

        if (viewType == VIEW_TYPE_MESSAGE_SENT){
            val itemView = LayoutInflater.from(parent.context)
                .inflate(R.layout.item_chat_me, parent, false)

            return SentMessageHolder(itemView)
        }

        val itemView = LayoutInflater.from(parent.context)
            .inflate(R.layout.item_chat_other, parent, false)

        return ReceivedMessageHolder(itemView)

    }

    // Passes the message object to a ViewHolder so that the contents can be bound to UI.
    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        val message = messages?.get(position)

        when (holder.itemViewType){
            VIEW_TYPE_MESSAGE_SENT -> message?.let { (holder as SentMessageHolder).bind(it) }
            VIEW_TYPE_MESSAGE_RECEIVED -> message?.let { (holder as ReceivedMessageHolder).bind(it) }
        }
    }

    fun addMessage(message: Message) : Int{
        messages?.add(message)

        val newPosition = messages!!.size
        notifyItemInserted(newPosition)

        return newPosition
    }

}