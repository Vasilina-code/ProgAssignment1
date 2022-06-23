using System.Collections.Generic;

using System;

using System.Linq;

using System;

namespace BankModule
{
        // Exception used when the user cancels an operation by pressing ENTER
        public class OperationCancel
            : Exception
        {
        }

        // 
        //     Represents a bank composed of a list of accounts.
        // 
        //     Attributes:
        //     _accountList : list -- the list of accounts managed by the bank
        //     
        public class Bank
        {

            public Bank()
            {
                this._accountList = new List<object>();
                this.DEFAULT_ACCT_NO_START = 100;
            }

            // Load the account data for all the accounts. The account data files are stored in a directory
            //         named BankingData located in the current directory, the directory used to run the application from
            //         
            public void loadAccountData()
            {
                var dataDirectory = os.path.join(os.getcwd(), "BankingData");
                if (os.path.exists(dataDirectory))
                {
                    //get the list of files in the directory
                    var acctFileList = os.listdir(dataDirectory);
                    //go through the list of files, create the appropriate accounts and load the file
                    foreach (var acctFileName in acctFileList)
                    {
                        var acctFile = open(os.path.join(dataDirectory, acctFileName));
                        try
                        {
                            //read the account type and create the correct account
                            var acctType = acctFile.readline().rstrip("\n");
                            if (acctType == "Account")
                            {
                                var acct = Account();
                            }
                            else if (acctType == "ChecquingAccount")
                            {
                                acct = ChecquingAccount();
                            }
                            else if (acctType == "SavingsAccount")
                            {
                                acct = SavingsAccount();
                            }
                            //load the data into the account object
                            acct.load(acctFile);
                            //add the account to the list of accounts
                            this._accountList.append(acct);
                        }
                        finally
                        {
                            //close the file regardless of whether an excetion occurrs or not the finally block will execute
                            //ensuring the file is closed. Alternatively the "with" statement could be used (see saveAccountData)
                            acctFile.close();
                        }
                    }
                }
                //if at this point the list of accounts is empty add the defaults accounts so the application is usable
                if (this._accountList.Count == 0)
                {
                    this.createDefaultAccounts();
                }
            }

            // Saves the data for all accounts in the data directory of the application. Each account is
            //  saved in a separate file which contains all the account information. The account data files are stored in a 
            //  directory named BankingData located in the current directory, the directory used to run the application from
            public void saveAccountData()
            {
                var dataDirectory = os.path.join(os.getcwd(), "BankingData");
                //make the directory if it does not exist
                if (!os.path.exists(dataDirectory))
                {
                    os.mkdir(dataDirectory);
                }
                //go through each account in the list of accounts and ask it to save itself into a corresponding file
                foreach (var acct in this._accountList)
                {
                    var acctType = type(acct).@__name__;
                    var prefix = acctType == "Account" ? "acct" : acctType == "ChecquingAccount" ? "chqacct" : "savacct";
                    var acctFileName = "{0}{1}.dat".format(prefix, acct.getAccountNumber());
                    //by using context manager for the file that will automatically close the file at the end of the with eblock
                    using (var acctFile = open(os.path.join(dataDirectory, acctFileName), "w"))
                    {
                        acctFile.write(acctType + "\n");
                        acct.save(acctFile);
                    }
                }
            }

            // Create 10 accounts with predefined IDs and balances. The default accounts are created only
            // if no account data files exist
            public void createDefaultAccounts()
            {
                foreach (var iAccount in Enumerable.Range(0, 10))
                {
                    //create the account with required properties
                    var newDefAcct = Account(this.DEFAULT_ACCT_NO_START + iAccount, "DefaultAccount{0}".format(iAccount));
                    newDefAcct.deposit(100);
                    newDefAcct.setAnnualIntrRate(2.5);
                    //add the account to the list
                    this._accountList.append(newDefAcct);
                }
            }

            // 
            //         Returns the account with the given account number or null if no account with that ID can be found
            //         Parameters:
            //             acctNo - the account number of the account to return
            //         Return:
            //             the account object with the given ID
            //         
            public int findAccount(object acctNo)
            {
                //go through all the accounts until one is found with the given account number
                foreach (var acct in this._accountList)
                {
                    if (acct.getAccountNumber() == acctNo)
                    {
                        return acct;
                    }
                }
                //if the program got here it means there was no account with the given account number
                return null;
            }

            // Determine the account number prompting the user until they enter the correct information
            //         
            //            The method will raise an AssertError if the user chooses to terminate.
            //         
            public int determineAccountNumber()
            {
                //pylint: disable=no-self-use
                while (true)
                {
                    try
                    {
                        //ask the user for input
                        var acctNoInput = input("Please enter the account number [100 - 1000] or press [ENTER] to cancel: ");
                        if (acctNoInput.Count == 0)
                        {
                            throw OperationCancel("User has selected to terminate the program after invalid input");
                        }
                        //check the input to ensure correctness and deal with incorrect input
                        var acctNo = Convert.ToInt32(acctNoInput);
                        if (acctNo < 100 || acctNo > 1000)
                        {
                            throw InvalidValue("The account number you have entered is not valid. Please enter a valid account number");
                        }
                        //check that the account number is not in use
                        foreach (var account in this._accountList)
                        {
                            if (acctNo == account.getAccountNumber())
                            {
                                throw InvalidValue("The account number you have entered already exists. Please enter a different account number");
                            }
                        }
                        //the account number has been generated successfully
                        return acctNo;
                    }
                    catch
                    {
                        Console.WriteLine(err, "\n");
                    }
                }
            }

            // Create and store an account objec with the required attributes
            public object setOpenAccount(string clientName, string acctType)
            {
                object newAccount;
                //prompt the user for an account number
                var acctNo = this.determineAccountNumber();
                //create and store an account object with the required attributes
                if (acctType == Account.ACCOUNT_TYPE_CHECQUING)
                {
                    newAccount = ChecquingAccount(acctNo, clientName);
                }
                else if (acctType == Account.ACCOUNT_TYPE_SAVINGS)
                {
                    newAccount = SavingsAccount(acctNo, clientName);
                }
                //add the new account to the list of the accounts
                this._accountList.append(newAccount);
                //return the account to the caller so other properties can be set
                return newAccount;
            }
        }
}