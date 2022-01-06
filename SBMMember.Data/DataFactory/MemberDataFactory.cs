using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SBMMember.Models;
namespace SBMMember.Data.DataFactory
{
    public class MemberDataFactory: IMemberDataFactory
    {
        private readonly SBMMemberDBContext memberDBContext;
        private readonly ILogger<MemberDataFactory> Logger;
        public MemberDataFactory(SBMMemberDBContext dBContext,ILogger<MemberDataFactory> logger)
        {
            memberDBContext = dBContext;
            Logger = logger;
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
