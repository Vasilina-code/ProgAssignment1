using System;

namespace SavngsAccountModule
{


        // Represents a savings account. A saving account has specific business logic. It has
        //     a minimum interest rate and an additional deposit business rule
        //     
        public class SavingsAccount
            : Account
        {
                //The matching deposit ratio. For every dollar deposit this account will
                //automatically be credited with 0.5 dollars. Defined as a class variable and accessible
                //through the name of the class along with the DOT notation
               
               //The minimmum interest rate for savings accounts. Defined as a class variable and accessible through the name of the class along with the DOT notation 

            const float MATCHING_DEPOSIT_RATIO = 0.5;

            const float MIN_INTEREST_RATE = 3.0;

            public SavingsAccount(int acctNo = -1, string acctHolderName = "")
                : base(acctNo, acctHolderName)
            {
            }

            // 
            //         Change the annual interest rate on the account. Verify the annual interest rate is valid for a savings account
            // 
            //         Arguments:
            //             newAnnualIntrRatePercentage: float -- the annual interest as a percentage (e.g. 3%)
            //         
            public void setAnnualIntrRate(int newAnnualIntrRatePercentage)
            {
                //check to ensure the annual interest rate is valid for a checquing account
                if (newAnnualIntrRatePercentage < SavingsAccount.MIN_INTEREST_RATE)
                {
                    throw InvalidValue("A savings account cannot have an interest rate less than {0}".format(SavingsAccount.MIN_INTEREST_RATE));
                }
                //use the base class to set the annual interest rate
                Account.setAnnualIntrRate(this, newAnnualIntrRatePercentage);
            }

            // Deposit the given amount in the account and return the new balance. For every dollar deposited the
            //         account will be credited with 0.5 dollars with an automatic deposit
            //         Arguments:
            //             amount - the amount to be deposited
            //         Returns:
            //             the new account balance AFTER the amount was deposited to avoid a call to getBalance() if needed
            //         
            public void deposit(object amount)
            {
                Account.deposit(this, amount + amount * SavingsAccount.MATCHING_DEPOSIT_RATIO);
            }
        }
}