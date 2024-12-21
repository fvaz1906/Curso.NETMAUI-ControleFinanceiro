using oovaz_financeiro.Models;

namespace oovaz_financeiro.Repositories
{
    public interface ITransactionRepository
    {
        List<Transaction> GetAll();
        void Add(Transaction transaction);
        void Delete(Transaction transaction);
        void Update(Transaction transaction);
    }
}