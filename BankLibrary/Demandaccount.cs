using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    class DemandAcount : Account
    {
        public DemandAcount(decimal sum, int percentage) : base(sum, percentage) { }

        protected internal override void Open()=>
            base.OnOpened(new AccountEventArgs($"Открыт новый счёт до востребования! Id счёта: {this.Id}", this.Sum));
    }
}
