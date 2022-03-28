using System.Collections.ObjectModel;

namespace Challenge4.Model
{
    public class AgeRange
    {
        public ObservableCollection<Member> Members { get; set; }
        public int Range { get; set; }

        public AgeRange(int range)
        {
            Range = range;
            Members = new ObservableCollection<Member>();
        }

        public void AddMember(Member member)
        {
            int i = 0;

            foreach (var mem in Members)
            {
                if (mem.Name.CompareTo(member.Name) > 0)
                    break;
                i++;
            }

            member.AgeRange = this;
            Members.Insert(i, member);
        }

        public void RemoveMember(Member member)
        {
            Members.Remove(member);
        }
    }
}
