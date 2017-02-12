using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Core
{
    public class Ware
    {
        private int wareid;
        public int WareID
        {
            get { return wareid; }
            set { wareid = value; }
        }
        private int specid;
        public int SpecID
        {
            get { return specid; }
            set { specid = value; }
        }
        private string warecode;
        public string WareCode
        {
            get { return warecode; }
            set { warecode = value; }
        }
        private string warename;
        public string WareName
        {
            get { return warename; }
            set { warename = value; }
        }
        private string speccode;
        public string SpecCode
        {
            get { return speccode; }
            set { speccode = value; }
        }
        private string specname;
        public string SpecName
        {
            get { return specname; }
            set { specname = value; }
        }
        private decimal price;
        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }

        private int status;
        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        private int usernum;
        public int UserNum
        {
            get { return usernum; }
            set { usernum = value; }
        }
        private string imgsrc;
        public string ImgSrc
        {
            get { return imgsrc; }
            set { imgsrc = value; }
        }
        private int type;
        public int Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}
