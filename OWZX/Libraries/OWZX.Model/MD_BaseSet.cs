using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
   public class MD_BaseSet
    {
       
        private int autoid;
        public int Autoid
        {
            get { return autoid; }
            set { autoid = value; }
        }

        private string key;
        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string account;
        public string Account
        {
            get { return account; }
            set { account = value; }
        }

        private string bank;
        public string Bank
        {
            get { return bank; }
            set { bank = value; }
        }

        private string bankAddress;
        public string BankAddress
        {
            get { return bankAddress; }
            set { bankAddress = value; }
        }

        private string image;
        public string Image
        {
            get { return image; }
            set { image = value; }
        }

        public int TotalCount { get; set; }

    }
}
