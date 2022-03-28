using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Challenge4.Model
{
    public class Member : INotifyPropertyChanged, ICloneable
    {
        public delegate void BeforeAgeChangeHandler(Member member);

        public event PropertyChangedEventHandler PropertyChanged;
        public event BeforeAgeChangeHandler BeforeAgeChange;

        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        private string address;
        public string Address
        {
            get => address;
            set
            {
                address = value;
                OnPropertyChanged();
            }
        }

        private int age;
        public int Age
        {
            get => age;
            set
            {
                age = value;
                BeforeAgeChange?.Invoke(this);
                OnPropertyChanged();
            }
        }

        private string gender;
        public string Gender
        {
            get => gender;
            set
            {
                gender = value;
                OnPropertyChanged();
            }
        }

        private string phone;
        public string Phone
        {
            get => phone;
            set
            {
                phone = value;
                OnPropertyChanged();
            }
        }

        private string birthday;
        public string Birthday
        {
            get => birthday;
            set
            {
                birthday = value;
                OnPropertyChanged();
            }
        }

        private string note;
        public string Note
        {
            get => note;
            set
            {
                note = value;
                OnPropertyChanged();
            }
        }

        public AgeRange AgeRange { get; set; }

        protected void OnPropertyChanged([CallerMemberName] string caller = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }

        public object Clone()
        {
            var clone = new Member();
            clone.Name = Name;
            clone.Address = Address;
            clone.Age = Age;
            clone.Gender = Gender;
            clone.Phone = Phone;
            clone.Birthday = Birthday;
            clone.Note = Note;

            return clone;
        }
    }
}
