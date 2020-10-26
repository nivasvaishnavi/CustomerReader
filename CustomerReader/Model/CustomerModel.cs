using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerReader.Model
{
    /// <summary>
    /// Address model class.
    /// </summary>
    public class Address
    {
        public String StreetAddress;
        public String City;
        public String State;
        public String ZipCode;
    }

    /// <summary>
    /// Customer model class with Address model class' properties inherited.
    /// </summary>
    public class Customer : Address
    {
        public String FirstName;
        public String LastName;
        public String Email;
        public String Phone;
    }
}
