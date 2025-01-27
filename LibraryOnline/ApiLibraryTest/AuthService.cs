using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibraryTest
{
    public class AuthService
    {
        private readonly Dictionary<string, string> _users = new Dictionary<string, string>
        {
            { "user1", "password123" },
            { "user2", "password456" }
        };

        public bool Authenticate(string username, string password)
        {
            return _users.ContainsKey(username) && _users[username] == password;
        }
    }

}
