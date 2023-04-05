package com.example.spotifychat.presentation.adapters

import android.service.autofill.UserData
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import com.example.spotifychat.R
import com.example.domain.common.Message

class CustomRecyclerAdapter(private val users: List<Message>?) :
    RecyclerView.Adapter<CustomRecyclerAdapter.MyViewHolder>()  {

    // it's a container for all list's components, needs to optimize resources
    class MyViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView){
        /*val largeTextView: TextView = itemView.findViewById(R.id.textViewLarge)
        val smallTextView: TextView = itemView.findViewById(R.id.textViewSmall)*/
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): MyViewHolder {
        TODO()
        /*val itemView =
            LayoutInflater.from(parent.context)
                .inflate(R.layout.recyclerview_item, parent, false)

        return MyViewHolder(itemView)*/
    }

    override fun onBindViewHolder(holder: MyViewHolder, position: Int) {
        /*holder.largeTextView.text = users?.get(position)?.id.toString()
        holder.smallTextView.text = users?.get(position)?.name*/
    }

    // returns amount of collection's elements
    override fun getItemCount(): Int {

        if (users == null)
            return 0

        return users.size
    }

}