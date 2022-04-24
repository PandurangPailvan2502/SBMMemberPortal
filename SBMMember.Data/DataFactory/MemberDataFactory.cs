using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SBMMember.Models;
namespace SBMMember.Data.DataFactory
{
    public class MemberDataFactory : BaseMemberFactory<Members>, IMemberDataFactory
    {
        private readonly SBMMemberDBContext memberDBContext;
        private readonly ILogger<MemberDataFactory> Logger;
        public MemberDataFactory(SBMMemberDBContext dBContext, ILogger<MemberDataFactory> logger)
        {
            memberDBContext = dBContext;
            Logger = logger;
        }

        public override ResponseDTO AddDetails(Members member)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                var memberInfo = memberDBContext.Members.Add(member);
                int affectedrows = memberDBContext.SaveChanges();
                if (affectedrows > 0)
                    return responseDTO= new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Member Details added Successfully."
                    };
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while adding member details:{ex.Message}");
                return responseDTO= new ResponseDTO()
                {
                    Result="Failed",
                    Message= $"Error occured while adding member details:{ex.Message}"
                };
            }
            return responseDTO;
        }

        public Members AddMember(Members member)
        {
            var memberInfo = memberDBContext.Members.Add(member);
            var data = memberDBContext.SaveChanges();
            return memberInfo.Entity;
        }

        public override Members GetDetailsByMemberId(int MemberId)
        {
            Members members = new Members();
            try
            {
                members = memberDBContext.Members.Where(x => x.MemberId == MemberId).First();


            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while Get member details by memberId. Exception:{ex.Message}");

            }

            return members;
        }
        public  Members GetDetailsByMemberMobile(string mobile)
        {
            Members members = new Members();
            try
            {
                members = memberDBContext.Members.Where(x => x.Mobile == mobile).First();


            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while Get member details by memberId. Exception:{ex.Message}");

            }

            return members;
        }
        public List<Members> GetMembers()
        {
            return memberDBContext.Members.ToList();
        }

        public override ResponseDTO UpdateDetails(Members _member)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                Members member = memberDBContext.Members.Where(x => x.MemberId == _member.MemberId).First();
                member = _member;
                int affectedRows = 0;
                affectedRows = memberDBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Member  details updated successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while updating member details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = $"Member details update operation failed.{ex.Message}"
                };
            }

            return responseDTO;
        }
        public ResponseDTO UpdateMPIN(Members _member)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                Members member = memberDBContext.Members.Where(x => x.Mobile == _member.Mobile).FirstOrDefault();
                member.Mpin = _member.Mpin;
                int affectedRows = 0;
                affectedRows = memberDBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Member  MPIN updated successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while updating member MPIN. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = $"Member MPIN update operation failed.{ex.Message}"
                };
            }

            return responseDTO;
        }
        public void UpdateName(int MemberId,String FirstName,String MiddleName,string LastName)
        {
            try
            {
                Members memberData = memberDBContext.Members.Where(x => x.MemberId == MemberId).FirstOrDefault();
                memberData.FirstName = FirstName;
                memberData.MiddleName = MiddleName;
                memberData.LastName = LastName;

                int affectedRows = 0;
                affectedRows = memberDBContext.SaveChanges();
            }
            catch (Exception ex)
            {

                Logger.LogError($"Error occured while updating member Name details. Exception:{ex.Message}");
            }
        }
    }

    public interface IMemberDataFactory
    {
        Members AddMember(Members member);
        List<Members> GetMembers();
        ResponseDTO AddDetails(Members member);
        ResponseDTO UpdateDetails(Members _member);
        Members GetDetailsByMemberId(int MemberId);
        Members GetDetailsByMemberMobile(string mobile);
        void UpdateName(int MemberId, String FirstName, String MiddleName, string LastName);
        ResponseDTO UpdateMPIN(Members _member);
    }
}
