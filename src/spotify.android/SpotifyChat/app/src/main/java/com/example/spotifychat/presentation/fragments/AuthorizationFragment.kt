package com.example.spotifychat.presentation.fragments

import com.example.core.base.FragmentBase
import com.example.spotifychat.R
import com.example.spotifychat.databinding.FragmentAuthorizationBinding
import com.example.spotifychat.presentation.viewmodels.AuthViewModel

class AuthorizationFragment : FragmentBase<FragmentAuthorizationBinding, AuthViewModel>(R.id.mainFragmentContainer) {


    override fun setUpViews() {
        super.setUpViews()

        binding.back.setOnClickListener {
            this.requireActivity().supportFragmentManager
                .beginTransaction()
                .replace(R.id.mainFragmentContainer, StartFragment.newInstance())
                .commit()
        }

        binding.btnSignIn.setOnClickListener {
            this.requireActivity().supportFragmentManager
                .beginTransaction()
                .replace(R.id.mainFragmentContainer, ChatFragment.newInstance())
                .addToBackStack(StartFragment::javaClass.name)
                .commit()
        }
    }


    override fun getViewModelClass(): Class<AuthViewModel> {
        return AuthViewModel::class.java
    }

    override fun getViewBinding(): FragmentAuthorizationBinding {
        return FragmentAuthorizationBinding.inflate(layoutInflater)
    }

    companion object {
        @JvmStatic
        fun newInstance() = AuthorizationFragment()
    }

}