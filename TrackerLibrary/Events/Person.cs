using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string CellphoeNumber { get; set; }

        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        public Person()
        {

        }

        public Person(string firstName, string lastName, string emailValue, string cellphoneValue)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailValue;
            CellphoeNumber = cellphoneValue;
        }
    }
}
