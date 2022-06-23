using System;

namespace ATMApplicationModule
{
        public class ATMApplication
        {

            public void run()
            {
                //use exception handling to ensure the application does not crash
                try
                {
                    //create a bank for a more real-life like implementation
                    var bank = Bank();
                    bank.loadAccountData();
                    //create ATM nad link it with the bank
                    var atm = Atm(bank);
                    //start the ATM machine
                    atm.start();
                }
                catch (Exception)
                {
                    Console.WriteLine("An error occurred with the following message: ", e);
                }
            }
        }
}