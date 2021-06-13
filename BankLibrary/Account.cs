using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    public abstract class Account : IAccount
    {
        protected internal event AccountStateHandler Withdrawed;//сообщение при выводе денег
        protected internal event AccountStateHandler Added;//сообщение при добавление на счёт
        protected internal event AccountStateHandler Opened;//сообщение при открытии счёта
        protected internal event AccountStateHandler Closed;//сообщение при закрытии счёта
        protected internal event AccountStateHandler Calculated;//сообщение при начислении процентов
        static int counter = 0;
        protected int _days = 0;//время с момента открытия счёта
        public Account(decimal sum,int percentage)
        {
            Sum = sum;
            Percentage = percentage;
            Id = ++counter;
        }
        public decimal Sum { get;private set; }// текущая сумма на счету
        public int Percentage { get; private set; }// процент начисления
        public int  Id { get;private set; }// айди счёта

        //вызов событий
        private void CallEvent(AccountEventArgs e, AccountStateHandler handler)
        {
            if (e != null)
                handler?.Invoke(this, e);
        }
        //вызов отдельных событий. Для каждого события определяется свой виртуальный метод
        protected virtual void OnWithdrawed(AccountEventArgs e)=> CallEvent(e, Withdrawed);
        protected virtual void OnOpened(AccountEventArgs e) => CallEvent(e, Opened);
        protected virtual void OnAdded(AccountEventArgs e)=>CallEvent(e,Added);
        protected virtual void OnClosed(AccountEventArgs e) => CallEvent(e, Closed);
        protected virtual void OnCalculated(AccountEventArgs e) => CallEvent(e, Calculated);
        public virtual void Put(decimal sum)//метод поступления денег на счёт
        {
            Sum += sum;
            OnAdded(new AccountEventArgs("на счёт поступило"+sum,sum));
        }

        public virtual decimal Withdraw(decimal sum)
        {
            decimal result = 0;
            if (Sum>sum)
            {
                Sum = -sum;
                result = sum;
                OnWithdrawed(new AccountEventArgs($"сумма {sum}снята со счёта{Id}",sum));
            }
            else
                OnWithdrawed(new AccountEventArgs($"недостаток денег на счёте {Id}",0));
            return result;
        }
        //открытие счёта
        protected internal virtual void Open() => OnOpened(new AccountEventArgs($"Открыт новый счёт! Id счёта{Id}",Sum));
        protected internal virtual void Close() => OnClosed(new AccountEventArgs($"Счёт {Id} закрыт. Итоговая сумма: {Sum}", Sum));
        protected internal void IncrementDays() => _days++;
        protected internal virtual void Calculate()
        {
            decimal increment = Sum * Percentage / 100;
            Sum = Sum + increment;
            OnCalculated(new AccountEventArgs($"Начислены проценты в размере: {increment}",increment));
        }
    }
}
