using Microsoft.Extensions.Logging;
using SBMMember.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SBMMember.Data.DataFactory
{
    public class MatrimonySubscriptionDataFactory: IMatrimonySubscriptionDataFactory
    {

        private readonly MatrimonyDBContext dBContext;
        private readonly ILogger<MatrimonySubscriptionDataFactory> logger;
        public MatrimonySubscriptionDataFactory(MatrimonyDBContext matrimonyDBContext,ILogger<MatrimonySubscriptionDataFactory> _logger)
        {
            dBContext=matrimonyDBContext;
            logger=_logger;

        }

        public ResponseDTO UpdateSubscriptionDetails(SBMSubscriptionCharges charges)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                SBMSubscriptionCharges subscription = dBContext.SubscriptionCharges.Where(x => x.Id == charges.Id).FirstOrDefault();
                subscription.SubscriptionCharges = charges.SubscriptionCharges;


                int affectedRows = 0;

                affectedRows = dBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Matrimony subscription details updated successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                logger.LogError($"Error occured while updating Matrimony subcription details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Matrimony Subscription charges update operation failed."
                };
            }

            return responseDTO;
        }

        public List<SBMSubscriptionCharges> Getsubscriptioncharges()
        {
            return dBContext.SubscriptionCharges.ToList();
        }
    }

    public interface IMatrimonySubscriptionDataFactory
    {
        ResponseDTO UpdateSubscriptionDetails(SBMSubscriptionCharges charges);
        List<SBMSubscriptionCharges> Getsubscriptioncharges();
    }

}
