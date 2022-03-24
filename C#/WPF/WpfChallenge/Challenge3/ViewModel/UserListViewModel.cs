using Challenge3.Model;

using System.Collections.ObjectModel;

namespace Challenge3.ViewModel
{
    class UserListViewModel
    {
        public ObservableCollection<User> Users { get;  private set; }

        public UserListViewModel()
        {
            Users = new ObservableCollection<User>();
        }

        public void AddUser(User user)
        {
            Users.Add(user);
        }

        public void RemoveUser(User user)
        {
            Users.Remove(user);
        }
    }
}