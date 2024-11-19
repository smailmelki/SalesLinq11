using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesLinq.Class
{
    public static class MasterFinance
    {
        /// <summary>
        /// دالة لجلب الرصيد الحالي لطرف التعامل
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public static AccountBalance GetAccountBalance(int accountID)
        {
            using (var db = new DAL.DBDataContext())
            {
                var query = db.Journals.Where(x => x.AccountID == accountID).Select(x => new { x.Debit, x.Credit });
                var totalCredit = query.Sum(x => (double?)x.Credit) ?? 0;
                var totalDebit = query.Sum(x => (double?)x.Debit) ?? 0;
                var account = db.Accounts.Single(x => x.ID == accountID);
                
                return new AccountBalance(accountID, account.Name,Math.Abs(totalCredit - totalDebit),
                    (totalCredit > totalDebit) ? AccountBalance.BalanceTypes.Credit : AccountBalance.BalanceTypes.Debit);
            }
        }

        public static AccountBalance GetAccountBalance(this DAL.Account account,int accountID)
        {
            return GetAccountBalance(account.ID);
        }

        /// <summary>
        /// كلاص نوعه رصيد
        /// </summary>
        public class AccountBalance
        {
            public AccountBalance(int id,string name,double amount, BalanceTypes balanceType)
            {
                ID = id;
                Name = name;
                BalanceAmount = amount;
                BalanceType = balanceType;
            }
            public int ID { get; set; }
            public string Name { get; set; }
            public double BalanceAmount { get; set; }
            public BalanceTypes BalanceType { get; set; }
            public string Balance { get
                {
                    return BalanceAmount.ToString() + " " + ((BalanceType == BalanceTypes.Credit) ? "دائن" : "مدين");
                }
            }
            public enum BalanceTypes
            {
                Credit = 1,
                Debit
            }  
        }
    }
}
