package com.example.spotifychat.presentation.fragments

import com.example.core.base.FragmentBase
import com.example.spotifychat.R
import com.example.spotifychat.databinding.FragmentRegistrationEmailBinding
import com.example.spotifychat.presentation.viewmodels.EmailViewModel

class RegistrationEmailFragment : FragmentBase<FragmentRegistrationEmailBinding, EmailViewModel>(R.id.mainFragmentContainer) {


    override fun setUpViews() {
        super.setUpViews()

        binding.back.setOnClickListener {
            this.requireActivity().supportFragmentManager
                .beginTransaction()
                .replace(R.id.mainFragmentContainer, StartFragment.newInstance())
                .commit()
        }

        binding.btnNextToPassword.setOnClickListener {
            this.requireActivity().supportFragmentManager
                .beginTransaction()
                .replace(R.id.mainFragmentContainer, LibraryFragment.newInstance())
                .addToBackStack(StartFragment::javaClass.name)
                .commit()
        }
    }


    override fun getViewModelClass(): Class<EmailViewModel> {
        return EmailViewModel::class.java
    }

    override fun getViewBinding(): FragmentRegistrationEmailBinding {
        return FragmentRegistrationEmailBinding.inflate(layoutInflater)
    }

    companion object {
        @JvmStatic
        fun newInstance() = RegistrationEmailFragment()
    }

}