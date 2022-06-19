using Microsoft.Extensions.Logging;
using SBMMember.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBMMember.Data.DataFactory
{
    public class BannerAdsDataFactory: IBannerAdsDataFactory
    {
        private readonly SBMMemberDBContext dBContext;
        private readonly ILogger<BannerAdsDataFactory> logger;

        public BannerAdsDataFactory(SBMMemberDBContext memberDBContext, ILogger<BannerAdsDataFactory> _logger)
        {
            dBContext = memberDBContext;
            logger = _logger;
        }
        public ResponseDTO AddBannerAdsDetails(BannerAds bannerAds)
        {

            bannerAds.CreateDate = DateTime.Now;
            bannerAds.Status = "Active";
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                int affectedRows = 0;
                dBContext.BannerAdvts.Add(bannerAds);
                affectedRows = dBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Banner Ads details saved successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                logger.LogError($"Error occured while adding Banner Ads details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Banner Ads details save operation failed."
                };
            }

            return responseDTO;
        }

        public ResponseDTO UpdateBannerAdsDetails(BannerAds bannerAds)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                BannerAds banner = dBContext.BannerAdvts.Where(x => x.Id == bannerAds.Id && x.Status =="Active").First();
                banner.Heading=bannerAds.Heading;
                banner.ImageURL=bannerAds.ImageURL;             


                int affectedRows = 0;
                
                affectedRows = dBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Banner Ads details updated successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                logger.LogError($"Error occured while updating Banner Ads details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Banner Ads update operation failed."
                };
            }

            return responseDTO;
        }
        public ResponseDTO DeleteBannerAdsDetails(int Id)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                BannerAds banner = dBContext.BannerAdvts.Where(x => x.Id == Id && x.Status =="Active").FirstOrDefault();
                banner.Status = "InActive";


                int affectedRows = 0;
                
                affectedRows = dBContext.SaveChanges();
                if (affectedRows > 0)
                {
                    responseDTO = new ResponseDTO()
                    {
                        Result = "Success",
                        Message = "Banner Ads details deleted successfully."
                    };
                }
            }
            catch (Exception ex)
            {

                logger.LogError($"Error occured while deleting Banner Ads details. Exception:{ex.Message}");
                responseDTO = new ResponseDTO()
                {
                    Result = "Failed",
                    Message = "Banner Ads delete operation failed."
                };
            }

            return responseDTO;
        }

        public List<BannerAds> GetBannerAds()
        {
            return dBContext.BannerAdvts.Where(x => x.Status =="Active").ToList();
        }
        public BannerAds GetBanerAdsById(int Id)
        {
            return dBContext.BannerAdvts.Where(x => x.Status == "Active" && x.Id == Id).FirstOrDefault();
        }
    }
    public interface IBannerAdsDataFactory
    {
        ResponseDTO AddBannerAdsDetails(BannerAds bannerAds);
        ResponseDTO UpdateBannerAdsDetails(BannerAds bannerAds);
        ResponseDTO DeleteBannerAdsDetails(int Id);
        List<BannerAds> GetBannerAds();
        BannerAds GetBanerAdsById(int Id);
    }
}
