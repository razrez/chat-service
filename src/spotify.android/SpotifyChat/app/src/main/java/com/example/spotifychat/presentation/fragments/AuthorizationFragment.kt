package com.example.spotifychat.presentation.fragments

import android.annotation.SuppressLint
import android.widget.Toast
import com.example.core.base.FragmentBase
import com.example.domain.common.LoginData
import com.example.spotifychat.Prefs
import com.example.spotifychat.R
import com.example.spotifychat.databinding.FragmentAuthorizationBinding
import com.example.spotifychat.presentation.viewmodels.AuthViewModel

class AuthorizationFragment : FragmentBase<FragmentAuthorizationBinding, AuthViewModel>(R.id.mainFragmentContainer){

    private val prefs = Prefs(this.requireContext())
    override fun setUpViews() {
        super.setUpViews()

        binding.back.setOnClickListener {
            this.requireActivity().supportFragmentManager
                .beginTransaction()
                .replace(R.id.mainFragmentContainer, StartFragment.newInstance())
                .commit()
        }

        binding.btnSignIn.setOnClickListener {

            val loginData = LoginData(
                username = R.id.input_email.toString(),
                password = R.id.editText.toString()
            )
            viewModel.signIn(loginData)
        }


    }

    override fun observeData() {
        super.observeData()

        // dumb auth without token validation
        viewModel.tokenMutableData.observe(this){
            if (it != null){
                prefs.saveToken(true, it.access_token)

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