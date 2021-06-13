using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    interface IAccount
    {
        void Put(decimal sum); //положить на счёт
        decimal Withdraw(decimal sum);//взять со счета
    }
}
