using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    public delegate void AccountStateHandler(object senders, AccountEventArgs e);
    
    public class AccountEventArgs
    {
        public string Message { get; private set; }// Сообщение
        public decimal Sum { get; private set; }//Суммаб на которую изменился счёт

        public AccountEventArgs(string _mes,decimal _sum)
        {
            Message = _mes;
            Sum = _sum;
        }
    }
}
