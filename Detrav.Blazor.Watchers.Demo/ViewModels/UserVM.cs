using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detrav.Blazor.Watchers.Demo.ViewModels
{
    public class UserVM : ViewModelBase
    {
        private string name;
        private DateTime lastLogin;
        private string email;

        public string Name
        {
            get => name;
            set => SetField(ref name, value);
        }
        public DateTime LastLogin
        {
            get => lastLogin;
            set => SetField(ref lastLogin, value);
        }
        public string Email
        {
            get => email;
            set => SetField(ref email, value);
        }
    }
}
