namespace SBMMember.Web.Models
{
    public class MemberCardViewModel
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string MemberProfileImage { get; set; }
        public int? MembershipId { get; set; }

        public string hashstring { get; set; }
    }
}
