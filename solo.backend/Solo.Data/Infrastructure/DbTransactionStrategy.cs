using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Solo.Data.Infrastructure
{
    public class DbTransactionStrategy
    {
        private readonly TransactionScopeOption _transactionScopeOption;

        public DbTransactionStrategy(TransactionScopeOption transactionScopeOption)
        {
            _transactionScopeOption = transactionScopeOption;
        }

        public void Perform(Action func)
        {
            Perform(() =>
            {
                func();
                return 0;
            });
        }

        public TResult Perform<TResult>(Func<TResult> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            using var transactionScope = CreateTransaction();
            var result = func();
            transactionScope.Complete();

            return result;
        }

        public async Task PerformAsync(Func<Task> func)
        {
            await PerformAsync(async () =>
            {
                await func();
                return 0;
            });
        }

        public async Task<TResult> PerformAsync<TResult>(Func<Task<TResult>> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            using var transactionScope = CreateTransaction();
            var result = await func();
            transactionScope.Complete();

            return result;
        }

        private TransactionScope CreateTransaction()
        {
            return new TransactionScope(_transactionScopeOption,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromMinutes(10) },
                TransactionScopeAsyncFlowOption.Enabled);
        }
    }
}
