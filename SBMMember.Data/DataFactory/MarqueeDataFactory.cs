using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SBMMember.Models;
namespace SBMMember.Data.DataFactory
{
   public class MarqueeDataFactory: IMarqueeDataFactory
    {
        private readonly SBMMemberDBContext dBContext;
        public MarqueeDataFactory(SBMMemberDBContext context)
        {
            dBContext = context;
        }

        public List<MarqueeText> GetMarquees()
        {
            return dBContext.MarqueeTexts.Where(x=>x.Status=="Active").ToList();
        }
    }

    public interface IMarqueeDataFactory
    {
        List<MarqueeText> GetMarquees();
    }
}
