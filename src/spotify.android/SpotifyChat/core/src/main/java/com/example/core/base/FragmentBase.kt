package com.example.core.base


import android.annotation.SuppressLint
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.fragment.app.FragmentTransaction
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.viewbinding.ViewBinding

/// fragmentContainerId - which contaniner will show the current fragment
///
abstract class FragmentBase<VBinding : ViewBinding, ViewModelClass : ViewModel>(
    private var fragmentContainerId: Int?
) : Fragment() {


    protected lateinit var viewModel: ViewModelClass
    protected abstract fun getViewModelClass(): Class<ViewModelClass>

    protected lateinit var binding: VBinding
    protected abstract fun getViewBinding(): VBinding


    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        init()
    }

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        setUpViews()
        observeData()
    }

    open fun setUpViews() {}

    @SuppressLint("FragmentLiveDataObserve", "HardwareIds")
    open fun observeData() {}

    private fun init() {
        binding = getViewBinding()
        viewModel = ViewModelProvider(this).get(getViewModelClass())
    }


    fun replaceFragment(newFragment: Fragment) {
        parentFragmentManager
            .apply {
                fragments.forEach {
                    beginTransaction().remove(it).commit()
                }
                beginTransaction()
                    .replace(fragmentContainerId!!, newFragment)
                    .commit()
            }
    }

    fun addFragment(newFragment: Fragment) {
        parentFragmentManager
            .beginTransaction()
            .add(fragmentContainerId!!, newFragment)
            .setTransition(FragmentTransaction.TRANSIT_FRAGMENT_OPEN)
            .addToBackStack(newFragment::class.java.name)
            .commit()
    }

    fun goBack() {
        activity?.supportFragmentManager?.popBackStack()
    }
}