package com.example.spotifychat.presentation.fragments

import android.os.Bundle
import android.widget.Button
import com.example.core.base.FragmentBase
import com.example.spotifychat.R
import androidx.fragment.app.Fragment
import com.example.spotifychat.databinding.FragmentStartBinding
import com.example.spotifychat.presentation.viewmodels.StartViewModel
import com.google.android.material.button.MaterialButton

class StartFragment : FragmentBase<FragmentStartBinding, StartViewModel>(R.id.mainFragmentContainer) {


    override fun setUpViews() {
        super.setUpViews()

        binding.btnRegistration.setOnClickListener {
            this.requireActivity().supportFragmentManager
                .beginTransaction()
                .replace(R.id.mainFragmentContainer, RegistrationFragment.newInstance())
                .addToBackStack(StartFragment::javaClass.name)
                .commit()
        }

        binding.btnSignin.setOnClickListener {
            this.requireActivity().supportFragmentManager
                .beginTransaction()
                .replace(R.id.mainFragmentContainer, AuthorizationFragment.newInstance())
                .commit()
        }
    }


    override fun getViewModelClass(): Class<StartViewModel> {
        return StartViewModel::class.java
    }

    override fun getViewBinding(): FragmentStartBinding {
        return FragmentStartBinding.inflate(layoutInflater)
    }

    companion object {
        @JvmStatic
        fun newInstance() = StartFragment()
    }

}