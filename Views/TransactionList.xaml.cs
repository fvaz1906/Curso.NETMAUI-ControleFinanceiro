using CommunityToolkit.Mvvm.Messaging;
using oovaz_financeiro.Models;
using oovaz_financeiro.Repositories;

namespace oovaz_financeiro.Views;

public partial class TransactionList : ContentPage
{

    private ITransactionRepository _transactionRepository;

    public TransactionList(ITransactionRepository transactionRepository)
	{
        _transactionRepository = transactionRepository;

        InitializeComponent();

        Reload();

        WeakReferenceMessenger.Default.Register<string>(this, (e, msg) => {
            Reload();
        });
    }

    private void Reload()
    {
        var items = _transactionRepository.GetAll();
        CollectionViewTransaction.ItemsSource = items;

        double income = items.Where(x => x.Type == Models.TransactionType.Income).Sum(y => y.Value);
        double expense = items.Where(x => x.Type == Models.TransactionType.Expenses).Sum(y => y.Value);
        double balance = income - expense;

        labelIcome.Text = income.ToString("C");
        LabelExpanse.Text = expense.ToString("C");
        LabelBalance.Text = balance.ToString("C");

    }

    private async void OnButtonClicked_To_TransactionAdd(object sender, EventArgs args)
    {

        var transactionAdd = Handler?.MauiContext?.Services.GetService<TransactionAdd>();
		await Navigation.PushModalAsync(transactionAdd);
    }

    private async void TapGestureRecognizerTapped_To_TransactionEdit(object sender, TappedEventArgs e)
    {
        var grid = (Grid)sender;
        var gesture = (TapGestureRecognizer)grid.GestureRecognizers[0];
        Transaction transaction = (Transaction)gesture.CommandParameter;

        var transactionEdit = Handler?.MauiContext?.Services.GetService<TransactionEdit>();
        transactionEdit.SetTransactionToEdit(transaction);

        await Navigation.PushModalAsync(transactionEdit);
    }

    private async void TapGestureRecognizerTapped_To_TransactionDelete(object sender, TappedEventArgs e)
    {
        await AnimationBorder((Border)sender, true);

        bool result = await Application.Current.MainPage.DisplayAlert("Excluir", "Tem certeza que deseja excluir?", "Sim", "Não");

        if (result)
        {
            Transaction transaction = (Transaction)e.Parameter;
            _transactionRepository.Delete(transaction);

            Reload();
        }
        else
        {
            await AnimationBorder((Border)sender, false);
        }
    }

    private Color _borderOriginalBackgroundColor;
    private String _labelOriginalText;

    private async Task AnimationBorder(Border border, bool IsDeleteAnimation)
    {
        var label = (Label)border.Content;

        if (IsDeleteAnimation)
        {
            _borderOriginalBackgroundColor = border.BackgroundColor;
            _labelOriginalText = label.Text;

            await border.RotateYTo(90, 500);
            
            border.BackgroundColor = Colors.Red;    
            label.TextColor = Colors.White;
            label.Text = "X";

            await border.RotateYTo(180, 500);
        }
        else
        {
            await border.RotateYTo(90, 500);
            
            border.BackgroundColor = _borderOriginalBackgroundColor;
            label.TextColor = Colors.Black;
            label.Text = _labelOriginalText;

            await border.RotateYTo(0, 500);
        }
    }
}
