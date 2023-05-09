package com.example.spotifychat.presentation.fragments

import android.widget.Toast
import com.example.core.base.FragmentBase
import com.example.domain.common.LoginData
import com.example.domain.common.ProfileData
import com.example.spotifychat.Prefs
import com.example.spotifychat.R
import com.example.spotifychat.databinding.FragmentRegistrationEmailBinding
import com.example.spotifychat.presentation.viewmodels.RegistrationViewModel

class RegistrationFragment : FragmentBase<FragmentRegistrationEmailBinding, RegistrationViewModel>(R.id.mainFragmentContainer) {


    override fun setUpViews() {
        super.setUpViews()

        binding.back.setOnClickListener {
            this.requireActivity().supportFragmentManager
                .beginTransaction()
                .replace(R.id.mainFragmentContainer, StartFragment.newInstance())
                .commit()
        }

        val inputEmail = binding.inputEmail
        val inputPassword = binding.inputPassword

        binding.btnNextToPassword.setOnClickListener {

            val loginData = LoginData(
                username = inputEmail.text.toString(),
                password = inputPassword.text.toString()
            )
            val profileData = ProfileData()

            viewModel.signUp(loginData, profileData)

        }
    }

    override fun observeData() {
        super.observeData()
        val prefs = Prefs(this.requireActivity())

        viewModel.tokenMutableData.observe(this){
            if (it?.access_token != null){
                prefs.saveToken(it.access_token)
                viewModel.claimsMutableData.observe(this){
                    if(it != null)
                        prefs.saveClaims(it)
                }

                this.requireActivity().supportFragmentManager
                    .beginTransaction()
                    .replace(R.id.mainFragmentContainer, LibraryFragment.newInstance())
                    .addToBackStack(StartFragment::javaClass.name)
                    .commit()
            }

            else{
                val message = "Wrong credentials!"
                val duration = Toast.LENGTH_SHORT

                val toast = Toast.makeText(this.requireContext(), message, duration)
                toast.show()
            }
        }
    }

    override fun getViewModelClass(): Class<RegistrationViewModel> {
        return RegistrationViewModel::class.java
    }

    override fun getViewBinding(): FragmentRegistrationEmailBinding {
        return FragmentRegistrationEmailBinding.inflate(layoutInflater)
    }

    companion object {
        @JvmStatic
        fun newInstance() = RegistrationFragment()
    }

}