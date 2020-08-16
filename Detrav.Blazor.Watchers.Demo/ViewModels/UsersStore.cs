using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Detrav.Blazor.Watchers.Demo.ViewModels
{
    public class UsersStore
    {
        public static UsersStore Instance { get; private set; }

        public UsersStore()
        {
            Instance = this;
            LoadMoreUsers();
            LoadMoreUsers();
            LoadMoreUsers();
            LoadMoreUsers();

            Observable.Interval(TimeSpan.FromSeconds(1))
                .Subscribe(_ =>
                {
                    Random r = new Random();
                    for(int i =0; i< Items.Count; i++)
                    {
                        if(r.NextDouble() < 0.1)
                        {
                            Items[i].LastLogin = DateTime.Now;
                        }
                    }
                });
        }

        
        public ObservableCollection<UserVM> Items { get; } = new ObservableCollection<UserVM>();

        [JSInvokable]
        public static void LoadMoreUsers()
        {
            for (int i = 0; i < 5; i++)
            {
                Instance.Items.Add(new UserVM()
                {
                    Name = RandomName(),
                    Email = RandomEmail(),
                    LastLogin = DateTime.Now
                });
            }
        }

        private static string RandomEmail()
        {
            return RandomName() + "@gmail.com";
        }

        private static string RandomName()
        {
            Random r = new Random();
            int len = r.Next(5, 15);
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string Name = "";
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }

            return Name;
        }
    }
}
