package com.example.spotifychat.presentation.fragments

import com.example.core.base.FragmentBase
import com.example.spotifychat.R
import com.example.spotifychat.databinding.FragmentRegistrationPasswordBinding
import com.example.spotifychat.presentation.viewmodels.PasswordViewModel

class RegistrationPasswordFragment : FragmentBase<FragmentRegistrationPasswordBinding, PasswordViewModel>(
    R.id.mainFragmentContainer) {


    override fun setUpViews() {
        super.setUpViews()

        binding.back.setOnClickListener {
            this.requireActivity().supportFragmentManager
                .beginTransaction()
                .replace(R.id.mainFragmentContainer, RegistrationEmailFragment.newInstance())
                .commit()
        }

        binding.btnCreateAccount.setOnClickListener {
            this.activity?.supportFragmentManager
                ?.beginTransaction()
                ?.replace(R.id.mainFragmentContainer, LibraryFragment.newInstance())
                ?.addToBackStack(StartFragment::javaClass.name)
                ?.commit()
        }
    }


    override fun getViewModelClass(): Class<PasswordViewModel> {
        return PasswordViewModel::class.java
    }

    override fun getViewBinding(): FragmentRegistrationPasswordBinding {
        return FragmentRegistrationPasswordBinding.inflate(layoutInflater)
    }

    companion object {
        @JvmStatic
        fun newInstance() = RegistrationPasswordFragment()
    }

}