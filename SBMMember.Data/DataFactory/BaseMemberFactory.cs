using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBMMember.Data.DataFactory
{
    public class BaseMemberFactory
    {
        public readonly SBMMemberDBContext memberDBContext;
        public BaseMemberFactory( SBMMemberDBContext dBContext)
        {
            memberDBContext = dBContext;
        }
    }
}
