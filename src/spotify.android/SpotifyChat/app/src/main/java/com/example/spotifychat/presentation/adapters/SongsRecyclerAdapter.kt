package com.example.spotifychat.presentation.adapters

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import com.example.data.SongsQuery
import com.example.spotifychat.R

class SongsRecyclerAdapter(private val songs: List<SongsQuery.Node>?) :
    RecyclerView.Adapter<SongsRecyclerAdapter.MyViewHolder>()  {

    // it's a container for all list's components, needs to optimize resources
    class MyViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView){
        private val artist: TextView = itemView.findViewById(R.id.artist)
        private val song: TextView = itemView.findViewById(R.id.song)

        fun bind(song: SongsQuery.Node){
            artist.text = song.user.username
            this.song.text = song.song
        }
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): MyViewHolder {
        val itemView =
            LayoutInflater.from(parent.context)
                .inflate(R.layout.item_song, parent, false)

        return MyViewHolder(itemView)
    }

    override fun onBindViewHolder(holder: MyViewHolder, position: Int) {
        val song = songs?.get(position)
        song?.let { holder.bind(it) }
    }

    // returns amount of collection's elements
    override fun getItemCount(): Int {

        if (songs == null)
            return 0

        return songs.size
    }

}