using System;
using System.IO; //to work with text file
// Defines the Account class and used by the BankingApplication module.
namespace AccountModule
{

        // Exception class used when an invalid trasaction is performed
        public class InvalidTransaction
            : Exception
        {
        }

        // Exception class used when an invalid value is detected
        public class InvalidValue
            : Exception
        {
        }
        //     Defines a bank account its associated attributes and operations.
        // 
        //     Attributes:
        //         _acctNo         : int   -- the account number, read-only attribute
        //         _acctHolderName : str   -- the name of the account holder, read-only attribute
        //         _balance        : float -- the account balance that gets affected by withdrawls and deposits
        //         _annualIntrRate : float -- the annual interest rate applicable on the balance
        //     
        public class Account
        {

            const int ACCOUNT_TYPE_CHECQUING = 1; // constant representing a checquing account type
            const int ACCOUNT_TYPE_SAVINGS = 2; // constant representing a savings account type
            public int acctNo = -1;
            public string acctHolderName = "";
            public float balance = 0;
            public float annualIntrRate = 0;
            public int _acctNo;
            public string _acctHolderName;
            public float _balance;
            public float _annualIntrRate;

            public void setAcctNo(int acctNo)
            {
                this._acctNo = acctNo;
            }

            public void setAcctHolderName(string acctHolderName)
            {
                this._acctHolderName = acctHolderName;
            }

            public void setBalance(float balance)
            {
                this._balance = balance;
            }
            public void setAnnualIntrRate(float annualIntrRate)
            {
                this._annualIntrRate = annualIntrRate;
            }
            public int getAccountNumber() // Return the account number
            {
                return _acctNo;
            }

            public string getAcctHolderName()  // Return the account holder's name
            {
                return _acctHolderName;
            }

            public float getBalance()     // Return the balance in the account
            {
                return _balance;
            }

            public float getAnnualIntrRate()    // Return the annuaul interest rate on the account
            {
                return _annualIntrRate;
            }

            //         Change the annual interest rate on the account
            //         Arguments:
            //         newAnnualIntrRatePercentage: float -- the annual interest as a percentage (e.g. 3%)

            public void setAnnualIntrRate(int newAnnualIntrRatePercentage)
            {
                this._annualIntrRate = newAnnualIntrRatePercentage / 100;
            }

            // Calculate and return the monthly interest rate on the account
            public float getMonthlyIntrRate()
            {
                return this._annualIntrRate / 12;
            }

            // Deposit the given amount in the account and return the new balance
            //         Arguments:
            //             amount - the amount to be deposited
            //         Returns:
            //             the new account balance AFTER the amount was deposited to avoid a call to getBalance() if needed
            //         
            public float deposit(float amount)
            {
                //check that the amount is positive
                if (amount < 0)
                {
                    throw InvalidTransaction("Invalid amount provided. Cannot deposit a negative amount.");
                }
                //change the balance
                var oldBalance = this._balance;
                this._balance += amount;
                //provide the new balance to the caller to avoid a getBalance() call
                return this._balance;
            }

            // 
            //         Withdraw the given amount from the account and return teh new balance
            //         Arguments:
            //             amount - the amount to be withdrawn, cannot be negative or greater than available balance             
            //         Returns:
            //             the new account balance AFTER the amount was deposited to avoid a call to getBalance() if needed
            //         
            public float withdraw(float amount)
            {
                //pylint: disable=no-self-use, unused-argument
                if (amount < 0)
                {
                    throw InvalidTransaction("Invalid amount provided. Cannot withdraw a negative amount.");
                }
                if (amount > this._balance)
                {
                    throw InvalidTransaction("Insufficient funds. Cannot withdraw the provided amount.");
                }
                //change the balance
                var oldBalance = this._balance;
                this._balance -= amount;
                //provide the new balance to the caller to avoid a getBalance() call
                return this._balance;
            }

            // Load the account information from the given file. The file is assumed opened
            //         Arguments:
            //             file - the file containing the account information
            //         
            public void load()
            {
                //read the account properties in the same order they were saved
                StreamReader sr = new StreamReader("file.txt");
                this._acctNo = Convert.ToInt32(sr.ReadLine());
                this._acctHolderName = sr.ReadLine();
                this._balance = float(sr.ReadLine());
                this._annualIntrRate = float(sr.ReadLine());
            }

            // Save the account information in the given file. The file is assumed opened
            //         Arguments:
            //             file - the file to contain the account information
            //         
            public void save()
            {
                //write the account properties, one per line
                StreamWriter sw = new StreamWriter("file.txt");
                sw.WriteLine(this._acctNo.ToString() + "\n");
                sw.WriteLine(this._acctHolderName.ToString() + "\n");
                sw.WriteLine(this._balance.ToString() + "\n");
                sw.WriteLine(this._annualIntrRate.ToString() + "\n");
            }
        }
}