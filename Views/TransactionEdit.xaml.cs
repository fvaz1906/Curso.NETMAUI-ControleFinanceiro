using CommunityToolkit.Mvvm.Messaging;
using oovaz_financeiro.Libraries.Utils.FixBugs;
using oovaz_financeiro.Models;
using oovaz_financeiro.Repositories;
using System.Text;

namespace oovaz_financeiro.Views;

public partial class TransactionEdit : ContentPage
{
    private ITransactionRepository _transactionRepository;
    private Transaction _transaction;

	public TransactionEdit(ITransactionRepository transactionRepository)
	{
		InitializeComponent();

        _transactionRepository = transactionRepository;
    }

    public void SetTransactionToEdit(Transaction transaction)
    {
        _transaction = transaction;

        if (transaction.Type == TransactionType.Income) 
            RadioIncome.IsChecked = true;
        else 
            RadioExpense.IsChecked = true;

        EntryName.Text = transaction.Name;
        DatePickerDate.Date = transaction.Date.Date;
        EntryValue.Text = transaction.Value.ToString();
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

        UpdateTransactionInDatabase();

        KeyboardFixBugs.hideKeyboard();
        Navigation.PopModalAsync();
        WeakReferenceMessenger.Default.Send<string>(string.Empty);
    }

    private void UpdateTransactionInDatabase()
    {
        Transaction transaction = new Transaction()
        {
            Id = _transaction.Id,
            Type = RadioIncome.IsChecked ? TransactionType.Income : TransactionType.Expenses,
            Name = EntryName.Text,
            Date = DatePickerDate.Date,
            Value = Math.Abs(double.Parse(EntryValue.Text))
        };

        _transactionRepository.Update(transaction);
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