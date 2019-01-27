using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class Team
    {
        public int Id { get; set; }
        public List<Person> TeamMembers { get; set; } = new List<Person>();
        public string TeamName { get; set; }
        public string TeamMembersString { get; set; }//For text connection
        public int Score { get; set; } = 0;

    }
}
