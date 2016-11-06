using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager
{
    class Program
    {
        static void Main(string[] args)
        {
            ClassicAccount a = new ClassicAccount(5000, "Matej");
            AccountEditor edit = new AccountEditor(a);

            edit.DoEdit();
            //Console.ReadLine();
        }
    }

    interface IAccount
    {
        bool AddMoney(float inMoney);
        bool DrawMoney(float inMoney);
        float GetBalance();
        bool SetName(string inName);
        string GetName();
    }

    class ClassicAccount : IAccount
    {
        private float money;
        private string name;

        public ClassicAccount(float inMoney, string inName)
        {
            this.money = inMoney;
            this.name = inName;
        }
        public ClassicAccount() : this(0, "No name") { }
        public ClassicAccount(float inMoney) : this(inMoney, "No name") { }
        public ClassicAccount(string inName) : this(0, inName) { }

        public override string ToString()
        {
            return string.Format("Balance: {0}, Name: {1}", this.money, this.name);
        }

        public bool AddMoney(float inMoney)
        {
            if (inMoney < 0)
                return false;

            money += inMoney;
            return true;
        }

        public bool DrawMoney(float inMoney)
        {
            if (inMoney < 0)
                return false;
            if (this.money - inMoney < 0)
                return false;

            this.money -= inMoney;
            return true;
        }

        public float GetBalance()
        {
            return this.money;
        }

        public bool SetName(string inName)
        {
            ValidateName(inName);
            this.name = inName;
            return true;
        }

        public string GetName()
        {
            return this.name;
        }

        //PRIVATE SECTION
        private void ValidateName(string inName)
        {
            if (inName == null)
                throw new BankException("Invalid name: Name set to null");
            string trimmedName = inName.Trim();
            if (trimmedName.Length == 0)
                throw new BankException("Invalid name: No text in name");
        }
    }

    class AccountEditor
    {
        IAccount account;

        public AccountEditor(IAccount account)
        {
            this.account = account;
        }

        public void DoEdit()
        {
            string selector;
            bool mainLoopBool = true;
            Console.WriteLine("Welcome " + this.account.GetName() + ", you are editing your account.");

            while (mainLoopBool)
            {
                Console.WriteLine("\nPlease, choose (and type accordingly) what you want to edit or view: \nEdit: name  /  money   \nView: accstatus");
                selector = Console.ReadLine();
                selector.Trim();
                selector.ToLower();

                switch (selector)
                {
                    case ("name"):
                        EditName();
                        break;

                    case ("money"):
                        EditMoney();
                        break;

                    case ("accstatus"):
                        ViewAccountStatus();
                        break;

                    default:
                        Console.WriteLine("Oops, you typed something wrong. Try again.");
                        break;
                }

                while (true)
                {
                    Console.WriteLine("Do you want to keep editing? Y/n");
                    selector = Console.ReadLine().ToLower();

                    if (selector == "y")
                        break;
                    else
                    {
                        mainLoopBool = false;
                        break;
                    }
                }
                    
            }
        }

        public void EditName()
        {
            string newName;
            Console.WriteLine("Editing name");

            while (true)
            {
                Console.Write("Please, enter new name: ");
                newName = Console.ReadLine();

                try
                {
                    this.account.SetName(newName);
                }
                catch(BankException bankE)
                {
                    Console.WriteLine("ERROR: " + bankE.Message);
                    continue;
                }
                //when no exception is thrown, jumping out of the while loop
                break;
            }
        }

        public void EditMoney()
        {
            bool whileBool = true;
            Console.WriteLine("Editing balance \nType which action you want to perform: \nadd / withdraw");

            while (whileBool)
            {
                string selector = Console.ReadLine();
                selector.Trim();
                selector.ToLower();

                switch (selector)
                {
                    case ("add"):
                        EditAddMoney();
                        whileBool = false;
                        break;

                    case ("withdraw"):
                        EditDrawMoney();
                        whileBool = false;
                        break;

                    default:
                        Console.WriteLine("You typed something wrong. Type again which action you want to perform: \nadd / withdraw");
                        break;         
                }
            }   
        }

        private void EditAddMoney()
        {
            float editMoney;
            Console.WriteLine("Adding money");

            while (true)
            {
                Console.Write("Please, enter amount of money to add: ");
                try
                {
                    editMoney = float.Parse(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR: " + e.Message);
                    continue;
                }

                if (this.account.AddMoney(editMoney))
                    break;

                Console.WriteLine("Oops, make sure that you didn't type a negative number.");
            }
        }

        private void EditDrawMoney()
        {
            float editMoney;
            Console.WriteLine("Withdrawing money");

            while (true)
            {
                Console.Write("Please, enter amount of money to withdraw: ");
                try
                {
                    editMoney = float.Parse(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR: " + e.Message);
                    continue;
                }

                if (this.account.DrawMoney(editMoney))
                    break;

                Console.WriteLine("Oops, make sure that you didn't type a negative number or that the balance after withdrawal is not negative.");
            }
        }

        public void ViewAccountStatus()
        {
            Console.WriteLine(this.account);
        }
    }

    class BankException : Exception
    {
        public BankException (string message) : base(message) { }
    }
}
