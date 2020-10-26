using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerReader.Model
{
    public class Address
    {
        public String StreetAddress;
        public String City;
        public String State;
        public String ZipCode;
    }

    public class Customer : Address
    {
        public String FirstName;
        public String LastName;
        public String Email;
        public String Phone;
    }
}
