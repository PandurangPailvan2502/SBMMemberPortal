using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SBMMember.Models;

namespace SBMMember.Data.DataFactory
{
    public abstract class BaseMemberFactory<T>
    {
        public abstract ResponseDTO AddDetails(T t);
        public abstract ResponseDTO UpdateDetails(T t);
        public abstract T GetDetailsByMemberId(int MemberId);
    }


}
