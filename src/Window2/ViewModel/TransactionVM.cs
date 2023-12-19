﻿namespace Window2.ViewModel;

public class TransactionVM: ViewModelBase
{
    private readonly PageModel _pageModel;
    public decimal TransactionAmount
    {
        get { return _pageModel.TransactionValue; }
        set { _pageModel.TransactionValue = value; OnPropertyChanged(); }
    }

    public TransactionVM()
    {
        _pageModel = new PageModel();
        TransactionAmount = 5638;
    }
} 