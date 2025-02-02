﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace abc_bank
{
    public class Customer
    {
        private String name;
        private List<Account> accounts;

        public Customer(String name)
        {
            this.name = name;
            this.accounts = new List<Account>();
        }

        public String GetName()
        {
            return name;
        }

        public Customer OpenAccount(Account account)
        {
            accounts.Add(account);
            return this;
        }

        public int GetNumberOfAccounts()
        {
            return accounts.Count;
        }

        public decimal TotalInterestEarned() 
        {
            decimal total = 0m;
            foreach (Account a in accounts)
                total += a.InterestEarned();
            return total;
        }

        public String GetStatement() 
        {
            String statement = null;
            statement = "Statement for " + name + "\n";
            decimal total = 0.0m;
            foreach (Account a in accounts) 
            {
                statement += "\n" + statementForAccount(a) + "\n";
                total += a.sumTransactions();
            }
            statement += "\nTotal In All Accounts " + ToDollars(total);
            return statement;
        }

        private String statementForAccount(Account a) 
        {
            String s = "";

           //Translate to pretty account type
            switch(a.GetAccountType()){
                case Account.CHECKING:
                    s += "Checking Account\n";
                    break;
                case Account.SAVINGS:
                    s += "Savings Account\n";
                    break;
                case Account.MAXI_SAVINGS:
                    s += "Maxi Savings Account\n";
                    break;
            }

            //Now total up all the transactions
            decimal total = 0.0m;
            foreach (Transaction t in a.transactions) {
                s += "  " + (t.amount < 0m ? "withdrawal" : "deposit") + " " + ToDollars(t.amount) + "\n";
                total += t.amount;
            }
            s += "Total " + ToDollars(total);
            return s;
        }

        private String ToDollars(decimal d)
        {
            return String.Format("{0:C}", Math.Abs(d));
        }

        public Customer Transfer(Account to, Account from, decimal amount)
        {
            if (amount <= 0m)
                throw new ArgumentException("Amount must be greater than zero.");
            if (!this.accounts.Contains(to))
                throw new ArgumentException("Account being deposited into does not belong to customer");
            if (!this.accounts.Contains(from))
                throw new ArgumentException("Account being withdrawn from does not belong to customer");
            if (from.sumTransactions() < amount)
                throw new InvalidOperationException("Insufficient funds");
            from.Withdraw(amount);
            to.Deposit(amount);
            return this;
        }
    }
}
