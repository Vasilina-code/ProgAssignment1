using System;
namespace ChecquingAccountModule
{

        // Represents a chequing account that has an overdraft limit and a maximum interest rate
        public class ChecquingAccount
            : Account
        {

                //The amount of overdraft is constant. Defined as a class variable and accessible through the name of the class along with the DOT notation
                //The maximum interest rate for checquing accounts. Defined as a class variable and accessible through the name of the class along with the DOT notation

            const int OVERDRAFT_LIMIT = 500;

            const int MAX_INTEREST_RATE = 1.0;

            public void setChecquingAccount(int acctNo = -1, string acctHolderName = "")
                : base(acctNo, acctHolderName)
            {
            }

            // 
            //         Change the annual interest rate on the account. Verify the annual interest rate is valid for a checquing account
            // 
            //         Arguments:
            //             newAnnualIntrRatePercentage: float -- the annual interest as a percentage (e.g. 3%)
            //         
            public void setAnnualIntrRate(object newAnnualIntrRatePercentage)
            {
                //check to ensure the annual interest rate is valid for a checquing account
                if (newAnnualIntrRatePercentage > ChecquingAccount.MAX_INTEREST_RATE)
                {
                    throw InvalidValue("A checquing account cannot have an interest rate greater than {0}".format(ChecquingAccount.MAX_INTEREST_RATE));
                }
                //use the base class to set the annual interest rate
                Account.setAnnualIntrRate(this, newAnnualIntrRatePercentage);
            }

            // 
            //         Withdraw the given amount from the account and return teh new balance
            //         Arguments:
            //             amount - the amount to be withdrawn, cannot be negative or greater than balance and overdraft combined            
            //         Returns:
            //             the new account balance AFTER the amount was deposited to avoid a call to getBalance() if needed
            //         
            public float withdraw(object amount)
            {
                //pylint: disable=no-self-use, unused-argument
                if (amount < 0)
                {
                    throw InvalidTransaction("Invalid amount provided. Cannot withdraw a negative amount.");
                }
                //check the overdraft on top of the actual balance
                if (amount > this._balance + ChecquingAccount.OVERDRAFT_LIMIT)
                {
                    throw InvalidTransaction("Insufficient funds. Cannot withdraw the provided amount.");
                }
                //change the balance
                var oldBalance = this._balance;
                this._balance -= amount;
                //provide the new balance to the caller to avoid a getBalance() call
                return this._balance;
            }
        }
}