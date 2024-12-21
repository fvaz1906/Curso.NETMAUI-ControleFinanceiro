using oovaz_financeiro.Views;

namespace oovaz_financeiro
{
    public partial class App : Application
    {
        public App(TransactionList listPage)
        {
            InitializeComponent();

            MainPage = new NavigationPage(listPage);
        }
    }
}
