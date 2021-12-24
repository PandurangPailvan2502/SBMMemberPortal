using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SBMMember.Models;
namespace SBMMember.Data.DataFactory
{
    public class MemberDataFactory: IMemberDataFactory
    {
        private readonly SBMMemberDBContext memberDBContext;
        public MemberDataFactory(SBMMemberDBContext dBContext)
        {
            memberDBContext = dBContext;
        }

        public Members AddMember(Members member)
        {
            var memberInfo = memberDBContext.Members.Add(member);
            var data = memberDBContext.SaveChanges();
           return memberInfo.Entity;
        }

        public List<Members> GetMembers()
        {
            return memberDBContext.Members.ToList();
        }
    }

    public interface IMemberDataFactory
    {
        Members AddMember(Members member);
        List<Members> GetMembers();
    }
}
