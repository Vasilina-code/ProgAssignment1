
using System;

namespace ATMModule
{ 
        //     The Atm class representing an ATM machine. The class displays and performs the the account management functions
        //     on a given bank account: checking balance, withdrawing and depositing money
        public class Atm
        {

            public Atm(object bank)
            {
                //the bank this ATM object is working with
                this._bank = bank;
                //create the MAIN MENU options
                this.SELECT_ACCOUNT_OPTION = 1;
                this.CREATE_ACCOUNT_OPTION = 2;
                this.EXIT_ATM_APPLICATION_OPTION = 3;
                //create ACCOUNT MENU option
                this.CHECK_BALANCE_OPTION = 1;
                this.WITHDRAW_OPTION = 2;
                this.DEPOSIT_OPTION = 3;
                this.EXIT_ACCOUNT_OPTION = 4;
            }

            // 
            //         Starts the ATM program by displaying the required user options. 
            //         User navigates the menus managing their accounts
            //         
            public void start()
            {
                //keep displaying the menu until the user chooses to exit the application
                while (true)
                {
                    //display the main menu and perform the main actions depending on the user's choice
                    var selectedOption = this.showMainMenu();
                    if (selectedOption == this.SELECT_ACCOUNT_OPTION)
                    {
                        var acct = this.selectAccount();
                        if (acct != null)
                        {
                            this.manageAccount(acct);
                        }
                    }
                    else if (selectedOption == this.CREATE_ACCOUNT_OPTION)
                    {
                        this.onCreateAccount();
                    }
                    else if (selectedOption == this.EXIT_ATM_APPLICATION_OPTION)
                    {
                        //the application is shutting down, save all account information
                        this._bank.saveAccountData();
                        return;
                    }
                    else
                    {
                        //go again when the user choose 3 instead of 1 or 2
                        Console.WriteLine("Please enter a valid menu option", "\n");
                    }
                }
            }

            // 
            //         Displays the main ATM menu and ensure the user picks an option. Handles invalid input but doesn't check
            //         that the menu option is one of the displayed ones.
            //         Returns:
            //             the option selected by the user
            // 
            //         
            public virtual object showMainMenu()
            {
                while (true)
                {
                    try
                    {
                        return Convert.ToInt32(input("\nMain Menu\n\n1: Select Account\n2: Create Account\n3: Exit\n\nEnter a choice: "));
                    }
                    catch (ValueError)
                    {
                        //if the user enters "abc" instead of a number
                        Console.WriteLine("Please enter a valid menu option.", "\n");
                    }
                }
            }

            // 
            //         Displays the ACCOUNT menu that allows the user to perform account operations. Handles invalid input but doesn't check
            //         that the menu option is one of the displayed ones.
            //         Returns:
            //             the option selected by the user
            //         
            public string showAccountMenu()
            {
                while (true)
                {
                    try
                    {
                        return Convert.ToInt32(input("\nAccount Menu\n\n1: Check Balance\n2: Withdraw\n3: Deposit\n4: Exit\n\nEnter a choice: "));
                    }
                    catch (ValueError)
                    {
                        //if the user enters "abc" instead of a number
                        Console.WriteLine("Please enter a valid menu option.", "\n");
                    }
                }
            }

            // Create and open an account. The user is prompted for all account information including the type of account to open.
            //         Create the account object and add it to the bank
            //         
            public void onCreateAccount()
            {
                while (true)
                {
                    try
                    {
                        //get the name of the account holder from the user
                        var clientName = this.promptForClientName();
                        //get the initial deposit from the user
                        var initDepositAmount = this.promptForDepositAmount();
                        //get the annual interest rate from the user
                        var annIntrRate = this.promptForAnnualIntrRate();
                        //get the account type from the user
                        var acctType = this.promptForAccountType();
                        //open the account
                        var newAccount = this._bank.openAccount(clientName, acctType);
                        //set the other account propertites
                        newAccount.deposit(initDepositAmount);
                        newAccount.setAnnualIntrRate(annIntrRate);
                        //now the the account has been successfully created and added to the bank the method is done
                        return;
                    }
                    catch (InvalidValue)
                    {
                        Console.WriteLine("err", "\n");
                    }
                    catch (OperationCancel)
                    {
                        Console.WriteLine("err", "\n");
                        return;
                    }
                }
            }

            // Select an account by prompting the user for an account number and remembering which account was selected.
            //         Prompt the user for performing account information such deposit and withdrawals
            //         
            public void selectAccount()
            {
                while (true)
                {
                    try
                    {
                        var acctNoInput = input("Please enter your account ID or press [ENTER] to cancel: ");
                        //check to see if the user gave up and is canceling the operation                
                        if (acctNoInput.Count == 0)
                        {
                            return null;
                        }
                        //the user entered an account number get the actual number
                        var acctNo = Convert.ToInt32(acctNoInput);
                        //obtain the account required by the user from the bank
                        var acct = this._bank.findAccount(acctNo);
                        if (acct != null)
                        {
                            return acct;
                        }
                        else
                        {
                            Console.WriteLine("The account was not found. Please select another account.");
                        }
                    }
                    catch (ValueError)
                    {
                        //The user entered an invalid (e.g. abc) account ID
                        Console.WriteLine("Please enter a valid account number (e.g. 100)", "\n");
                    }
                }
            }

            // Manage the account by allowing the user to execute operation on the given account
            //         Arguments:
            //             account - the account to be managed
            //         
            public void manageAccount(object account)
            {
                while (true)
                {
                    var selAcctMenuOpt = this.showAccountMenu();
                    if (selAcctMenuOpt == this.CHECK_BALANCE_OPTION)
                    {
                        this.onCheckBalance(account);
                    }
                    else if (selAcctMenuOpt == this.WITHDRAW_OPTION)
                    {
                        this.onWithdraw(account);
                    }
                    else if (selAcctMenuOpt == this.DEPOSIT_OPTION)
                    {
                        this.onDeposit(account);
                    }
                    else if (selAcctMenuOpt == this.EXIT_ACCOUNT_OPTION)
                    {
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid menu option");
                    }
                }
            }

            // Prompts the user to enter the name of the client and allows the user to cancel by pressing ENTER
            public string promptForClientName()
            {
                var clientName = input("Please enter the client name or press [ENTER] to cancel: ");
                if (clientName.Count == 0)
                {
                    //the user has canceled the creation of the account
                    throw OperationCancel("The user has selected to cancel the current operation");
                }
                return clientName;
            }

            // Prompts the user to enter an account balance and performs basic error checking
            public float promptForDepositAmount()
            {
                while (true)
                {
                    try
                    {
                        var initAmount = float(input("Please enter your initial account balance: "));
                        if (initAmount >= 0)
                        {
                            return initAmount;
                        }
                        else
                        {
                            //NOTE the difference between this error which is handled with an if statement
                            //and the ValueError exception. In this case an if statement is more suitable because
                            //it makes the code more readable than a try/catch.
                            Console.WriteLine("Cannot create an account with a negative initial balance. Please enter a valid amount");
                        }
                    }
                    catch (ValueError)
                    {
                        Console.WriteLine("err", "\n");
                    }
                }
            }

            // Prompts the user to enter the annual interest rate for an account
            public float promptForAnnualIntrRate()
            {
                while (true)
                {
                    try
                    {
                        var intrRate = float(input("Please enter the interest rate for this account: "));
                        //perform basic sanity checking of the input. Note that the business rules for checking are implemented
                        //in the account classes not here so that they are together with the rest of the account business logic
                        if (intrRate >= 0)
                        {
                            return intrRate;
                        }
                        else
                        {
                            //NOTE the difference between this error which is handled with an if statement
                            //and the ValueError exception. In this case an if statement is more suitable because
                            //it makes the code more readable than a try/catch.
                            Console.WriteLine("Cannot create an account with a negative interest rate.");
                        }
                    }
                    catch (ValueError)
                    {
                        Console.WriteLine(err, "\n");
                    }
                }
            }

            // Prompts the user to enter an account type
            //         Returns:
            //             - the account type as a constant
            //         
            public string promptForAccountType()
            {
                while (true)
                {
                    var acctTypeInput = input("Please enter the account type [c/s: chequing / savings]: ").upper();
                    if (acctTypeInput == "C" || acctTypeInput == "CHECQUING" || acctTypeInput == "CHECKING")
                    {
                        return Account.ACCOUNT_TYPE_CHECQUING;
                    }
                    else if (acctTypeInput == "S" || acctTypeInput == "SAVINGS" || acctTypeInput == "SAVING")
                    {
                        return Account.ACCOUNT_TYPE_SAVINGS;
                    }
                    else
                    {
                        Console.WriteLine("Answer not supported. Please enter one of the supported answers.");
                    }
                }
            }

            // 
            //         Prints the balance in the given account
            //         Arguments:
            //             account - the account for which the balance is printed  
            //         
            public void onCheckBalance(int account)
            {
                Console.WriteLine("The balance is {0}\n".format(account.getBalance()));
            }

            // 
            //         Prompts the user for an amount and performs the deposit. Handles any errors related to incorrect amounts
            //         Arguments:
            //             account - the account in which the amount is to be deposited
            //             
            //         
            public void onDeposit(int account)
            {
                while (true)
                {
                    try
                    {
                        var inputAmount = input("Please enter an amount to deposit or type [ENTER] to exit: ");
                        //test for empty input in case the user pressed [ENTER] because they wanted to give up on depositing money
                        if (inputAmount.Count > 0)
                        {
                            var amount = float(inputAmount);
                            //the account itself is responsible for checking the amount and raising any errors if the deposit
                            //is not possible like negative amounts 
                            account.deposit(amount);
                        }
                        //the deposit was done or user entered nothing so break from the infinite loop
                        return;
                    }
                    catch (ValueError)
                    {
                        //the user must have entered and invalid (e.g. "abc") amount
                        Console.WriteLine("Invalid entry. Please enter a number for your amount.", "\n");
                    }
                    catch (InvalidTransaction)
                    {
                        //the account must have refused to deposit the entered amount. The reason is in the exception object
                        Console.WriteLine("err", "\n");
                    }
                }
            }

            // 
            //         Prompts the user for an amount and performs the withdrawal. Handles any errors related to incorrect amounts
            //         Arguments:
            //             account - the account in which the amount is to be withdrawn
            //             
            //         
            public void onWithdraw(int account)
            {
                while (true)
                {
                    try
                    {
                        var inputAmount = input("Please enter an amount to withdraw or type [ENTER] to exit: ");
                        //test for empty input in case the user pressed [ENTER] because they wanted to give up on withdrawing money
                        if (inputAmount.Count > 0)
                        {
                            var amount = float(inputAmount);
                            //the account itself is responsible for checking the amount and raising any errors if the withdraw
                            //is not possible like negative amounts and balance overruns
                            account.withdraw(amount);
                        }
                        //the deposit was done or user entered nothing so break from the infinite loop
                        return;
                    }
                    catch (ValueError)
                    {
                        //the user must have entered and invalid (e.g. "abc") amount
                        Console.WriteLine("Invalid entry. Please enter a number for your amount.", "\n");
                    }
                    catch (InvalidTransaction)
                    {
                        //the account must have refused to withdraw the entered amount. The reason is in the exception object
                        Console.WriteLine("err", "\n");
                    }
                }
            }
        }
}