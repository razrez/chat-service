package com.example.spotifychat.presentation.adapters

import android.service.autofill.UserData
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.recyclerview.widget.RecyclerView
import com.example.spotifychat.R

class SongsRecyclerAdapter(private val songs: List<String>?) :
    RecyclerView.Adapter<SongsRecyclerAdapter.MyViewHolder>()  {

    // it's a container for all list's components, needs to optimize resources
    class MyViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView){
        /*val largeTextView: TextView = itemView.findViewById(R.id.)
        val smallTextView: TextView = itemView.findViewById(R.id.textViewSmall)*/
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): MyViewHolder {
        val itemView =
            LayoutInflater.from(parent.context)
                .inflate(R.layout.item_song, parent, false)

        return MyViewHolder(itemView)
    }

    override fun onBindViewHolder(holder: MyViewHolder, position: Int) {
        /*holder.largeTextView.text = users?.get(position)?.id.toString()
        holder.smallTextView.text = users?.get(position)?.name*/
    }

    // returns amount of collection's elements
    override fun getItemCount(): Int {

        if (songs == null)
            return 0

        return songs.size
    }

}