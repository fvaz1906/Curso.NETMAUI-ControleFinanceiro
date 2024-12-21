using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Platform;
using oovaz_financeiro.Libraries.Utils.FixBugs;
using oovaz_financeiro.Models;
using oovaz_financeiro.Repositories;
using System.Text;

namespace oovaz_financeiro.Views;

public partial class TransactionAdd : ContentPage
{

    private ITransactionRepository _transactionRepository;

	public TransactionAdd(ITransactionRepository transactionRepository)
	{
		InitializeComponent();

        _transactionRepository = transactionRepository;
    }

    private async void TapGestureRecognizerTapped_ToClose(object sender, TappedEventArgs e)
    {
        KeyboardFixBugs.hideKeyboard();
        await Navigation.PopModalAsync();
    }

    private void OnButtonClicked_Save(object sender, EventArgs e)
    {
        if (IsValidData() == false)
            return;

        SaveTransactionInDatabase();

        KeyboardFixBugs.hideKeyboard();
        Navigation.PopModalAsync();
        WeakReferenceMessenger.Default.Send<string>(string.Empty);
    }

    private void SaveTransactionInDatabase()
    {
        Transaction transaction = new Transaction()
        {
            Type = RadioIncome.IsChecked ? TransactionType.Income : TransactionType.Expenses,
            Name = EntryName.Text,
            Date = DatePickerDate.Date,
            Value = Math.Abs(double.Parse(EntryValue.Text))
        };

        _transactionRepository.Add(transaction);
    }

    private bool IsValidData()
    {
        double result;
        bool valid = true;
        StringBuilder sb = new StringBuilder();

        if (string.IsNullOrEmpty(EntryName.Text) || string.IsNullOrWhiteSpace(EntryName.Text))
        {
            sb.AppendLine("O Campo 'Nome' deve ser preenchido!");
            valid = false;
        }

        if (string.IsNullOrEmpty(EntryValue.Text) || string.IsNullOrWhiteSpace(EntryValue.Text))
        {
            sb.AppendLine("O Campo 'Valor' deve ser preenchido!");
            valid = false;
        }

        if (!string.IsNullOrEmpty(EntryValue.Text) && !double.TryParse(EntryValue.Text, out result))
        {
            sb.AppendLine("O Campo 'Valor' é inválido!");
            valid = false;
        }

        if (valid == false)
        {
            LabelError.Text = sb.ToString();
            LabelError.IsVisible = true;
        }

        return valid;
    }
}