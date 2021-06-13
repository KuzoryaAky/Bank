using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    public enum AccountType //тип счёта
    {
        Ordinary,
        Deposit
    }
    public class Bank<T> where T : Account
    {
        T [] accounts;
        public string Name { get;private set; }
        public Bank(string name) => this.Name = name;
        //метод создания счёта
        public void Open(AccountType accountType,decimal sum,
            AccountStateHandler addSumHandler,AccountStateHandler withdrawSumHandler,
            AccountStateHandler closeAccountHandler,AccountStateHandler openAccountHandler,
            AccountStateHandler calculationHandler)
        {
            T newAccount = null;
            switch (accountType)
            {
                case AccountType.Ordinary:
                    newAccount = new DemandAcount(sum, 1) as T;
                    break;
                case AccountType.Deposit:
                    newAccount = new DepositeAccount(sum, 40) as T;
                    break;
            }
            if (newAccount == null)
                throw new Exception("Ошибка создания счёта");
            //добавляем новый счёт в массив счетов
            if (accounts == null)
                accounts = new T[] { newAccount };
            else
            {
                T[] tempAccounts = new T[accounts.Length + 1];
                for (int i = 0; i < accounts.Length; i++)
                    tempAccounts[i] = accounts[i];
                tempAccounts[tempAccounts.Length - 1] = newAccount;
                accounts = tempAccounts;
            }
            //установка обработчиков событий счёта
            newAccount.Added += addSumHandler;
            newAccount.Withdrawed += withdrawSumHandler;
            newAccount.Closed += closeAccountHandler;
            newAccount.Opened += openAccountHandler;
            newAccount.Calculated += calculationHandler;

            newAccount.Open();
        }

        //добавление средств на счёт
        public void Put(decimal sum,int id)
        {
            T account = FindAccount(id);
            if (account == null)
                throw new Exception("Счёт не найден");
                account.Put(sum);
        }

        //вывод средств
        public void Withdraw (decimal sum,int id)
        {
            T account = FindAccount(id);
            if (account == null)
                throw new Exception("Счёт не найден");
            account.Withdraw(sum);
        }

        //закрытие счёта
        public void Close(int id)
        {
            int index;
            T account = FindAccount(id,out index);
            if (account == null)
                throw new Exception("Счёт не найден");
            account.Close();

            if (accounts.Length <= 1)
                accounts = null;
            else
            {
                //уменьшаем масив счетов,удаляя из него закрытый счёт
                T[] tempAccounts = new T[accounts.Length - 1];
                for (int i = 0, j = 0; i < accounts.Length; i++)
                    if (i != index)
                        tempAccounts[j++] = accounts[i];
                accounts = tempAccounts;
            }

        }
        


        //начисление процентов по счетам
        public void CalculatePercentage()
        {
            if (accounts == null)//если массив не создан выходим из метода
                return;
            for (int i = 0; i < accounts.Length; i++)
            {
                accounts[i].IncrementDays();
                accounts[i].Calculate();
            }
        }

        //поиск счёта по Id
        public T FindAccount(int id)
        {
            for (int i = 0; i < accounts.Length; i++)
                if (accounts[i].Id == id)
                    return accounts[i];
            return null;
        }
        //перегруженая версия поиска сетов
        public T FindAccount (int id,out int index)
        {
            for (int i = 0; i < accounts.Length; i++)
                if (accounts[i].Id == id)
                {
                    index = i;
                    return accounts[i];
                }
            index = -1;
            return null;
        }
    }
}
