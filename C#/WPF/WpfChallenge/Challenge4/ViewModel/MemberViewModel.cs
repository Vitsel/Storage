using Challenge4.Command;
using Challenge4.Model;
using Challenge4.State;

using System.Windows.Input;
using System.Collections.ObjectModel;

namespace Challenge4.ViewModel
{
    class MemberViewModel : BaseViewModel
    {
        public ObservableCollection<AgeRange> Ages { get; set; }

        private PageState pageState;
        public PageState PageState
        {
            get => pageState;
            set
            {
                pageState = value;
                OnPropertyChanged();
            }
        }

        private Member newMember;
        public Member NewMember
        {
            get => newMember;
            set
            {
                newMember = value;
                OnPropertyChanged();
            }
        }

        private Member selectedMember;
        public Member SelectedMember
        {
            get => selectedMember;
            set
            {
                selectedMember = value;
                OnPropertyChanged();
            }
        }

        public ICommand RemoveMemberCommand { get; set; }
        public ICommand RegistMemberCommand { get; set; }
        public ICommand ResetMemberCommand { get; set; }
        public ICommand ChangePageStateCommand { get; set; }

        public MemberViewModel()
        {
            NewMember = new Member();
            Ages = new ObservableCollection<AgeRange>();

            RemoveMemberCommand = new RelayCommand<Member>(ExecuteRemoveMember);
            ChangePageStateCommand = new RelayCommand<PageState>(ExecuteChangePageState);
            RegistMemberCommand = new RelayCommand<Member>(ExecuteAddMember);
            ResetMemberCommand = new RelayCommand(ExecuteResetMember);
        }

        public void ExecuteAddMember(Member member)
        {
            AddMember(member);

            NewMember = new Member();
            SelectedMember = member;
            PageState = PageState.MemberList;
        }

        public void ExecuteRemoveMember(Member member)
        {
            RemoveMember(member);

            SelectedMember = null;
        }

        public void ExecuteResetMember()
        {
            NewMember = new Member();
        }

        public void UpdateMemberAge(Member member)
        {
            if (member.AgeRange.Range == member.Age / 10 * 10)
                return;

            RemoveMember(member);
            AddMember(member);
        }

        public void ExecuteChangePageState(PageState state)
        {
            PageState = state;
        }

        public void SetSelectedMember(Member member)
        {
            SelectedMember = member;
        }

        private void RemoveMember(Member member)
        {
            if (member == null)
                return;

            var range = member.AgeRange;

            range.RemoveMember(member);

            if (range.Members.Count == 0)
                Ages.Remove(range);
        }

        private void AddMember(Member member)
        {
            member.BeforeAgeChange += UpdateMemberAge;

            int i = 0;
            foreach (var age in Ages)
            {
                if (age.Range > member.Age / 10 * 10)
                    break;

                if (age.Range == member.Age / 10 * 10)
                {
                    age.AddMember(member);
                    NewMember = new Member();
                    return;
                }

                i++;
            }

            var range = new AgeRange(member.Age / 10 * 10);
            range.AddMember(member);
            Ages.Insert(i, range);
        }
    }
}