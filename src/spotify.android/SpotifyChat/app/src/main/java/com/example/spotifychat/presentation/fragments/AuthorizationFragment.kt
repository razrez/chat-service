package com.example.spotifychat.presentation.fragments

import com.example.core.base.FragmentBase
import com.example.domain.common.LoginData
import com.example.spotifychat.Prefs
import com.example.spotifychat.R
import com.example.spotifychat.databinding.FragmentAuthorizationBinding
import com.example.spotifychat.presentation.viewmodels.AuthViewModel
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.Job
import kotlinx.coroutines.launch
import kotlin.coroutines.CoroutineContext

class AuthorizationFragment : FragmentBase<FragmentAuthorizationBinding, AuthViewModel>(R.id.mainFragmentContainer),
    CoroutineScope {

    private var job: Job = Job()
    val prefs = Prefs(this.requireActivity()) //ваще хз, правильно ли так
    override val coroutineContext: CoroutineContext
        get() = Dispatchers.Main + job

    override fun onDestroy() {
        super.onDestroy()
        job.cancel()
    }

    override fun setUpViews() {
        super.setUpViews()

        binding.back.setOnClickListener {
            this.requireActivity().supportFragmentManager
                .beginTransaction()
                .replace(R.id.mainFragmentContainer, StartFragment.newInstance())
                .commit()
        }

        binding.btnSignIn.setOnClickListener {

            launch {
                val loginData = LoginData(username = R.id.input_email.toString(), password = R.id.editText.toString()) //хз, как получить данные из edit text
                val token = viewModel.signIn(loginData)

                if (token != null) {
                    prefs.saveToken(true, token.access_token)
                }
            }

            this.requireActivity().supportFragmentManager
                .beginTransaction()
                .replace(R.id.mainFragmentContainer, LibraryFragment.newInstance())
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