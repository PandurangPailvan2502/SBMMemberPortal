﻿using Microsoft.Extensions.Logging;
using SBMMember.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBMMember.Data.DataFactory
{
    public class SubscriptionDataFactory: ISubscriptionDataFactory
    {
        private readonly SBMMemberDBContext dBContext;
        private readonly ILogger<SubscriptionDataFactory> logger;
        public SubscriptionDataFactory(SBMMemberDBContext memberDBContext, ILogger<SubscriptionDataFactory> _logger)
        {
            dBContext = memberDBContext;
            logger = _logger;
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
                        Message = "subscription details updated successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                logger.LogError($"Error occured while updating subcription details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Subscription charges update operation failed."
                };
            }

            return responseDTO;
        }

        public List<SBMSubscriptionCharges> Getsubscriptioncharges()
        {
            return dBContext.SubscriptionCharges.ToList();
        }
    }
    public interface ISubscriptionDataFactory
    {
        ResponseDTO UpdateSubscriptionDetails(SBMSubscriptionCharges charges);
        List<SBMSubscriptionCharges> Getsubscriptioncharges();
    }

}
