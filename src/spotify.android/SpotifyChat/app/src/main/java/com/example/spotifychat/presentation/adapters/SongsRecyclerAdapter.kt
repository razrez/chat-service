package com.example.spotifychat.presentation.adapters

import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import com.example.data.SongsQuery
import com.example.domain.common.Song
import com.example.domain.common.SongStat
import com.example.spotifychat.R
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext

class SongsRecyclerAdapter(private val songs: List<Song>?) :
    RecyclerView.Adapter<SongsRecyclerAdapter.MyViewHolder>()  {

    // it's a container for all list's components, needs to optimize resources
    class MyViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView){
        private var id: Int = 0
        private val artist: TextView = itemView.findViewById(R.id.artist)
        private val song: TextView = itemView.findViewById(R.id.song)

        private var listens: TextView = itemView.findViewById(R.id.views)
        private var listensCounter = 0

        fun bind(song: Song){
            this.artist.text = song.user.username
            this.song.text = song.song
            this.id = song.id
            this.listens.text = song.listens.toString()
            this.listensCounter = song.listens
        }

        init {
            itemView.setOnClickListener {

            }
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

    fun updateStat (newStat: SongStat) {
        val songUpdate = songs?.find {
            s -> s.id == newStat.songId.toInt()
        }

        if (songUpdate != null){
            val positionUpdate = songs?.indexOf(songUpdate)
            songs?.get(positionUpdate!!)?.listens = newStat.listens

            notifyItemChanged(positionUpdate!!)
        }

    }
}