using LiteDB;
using oovaz_financeiro.Models;

namespace oovaz_financeiro.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly LiteDatabase _liteDatabase;
        private readonly string collectionName = "transactions";
        public TransactionRepository(LiteDatabase liteDatabase)
        {
            _liteDatabase = liteDatabase;
        }

        public List<Transaction> GetAll()
        {
            return _liteDatabase
                .GetCollection<Transaction>(collectionName)
                .Query()
                .OrderByDescending(x => x.Date)
                .ToList();
        }

        public void Add(Transaction transaction)
        {
            var col = _liteDatabase.GetCollection<Transaction>(collectionName);
            col.Insert(transaction);
            col.EnsureIndex(x => x.Date);
        }

        public void Update(Transaction transaction)
        {
            var col = _liteDatabase.GetCollection<Transaction>(collectionName);
            col.Update(transaction);
        }

        public void Delete(Transaction transaction)
        {
            var col = _liteDatabase.GetCollection<Transaction>(collectionName);
            col.Delete(transaction.Id);
        }

    }
}
