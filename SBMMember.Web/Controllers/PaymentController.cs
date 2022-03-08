﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using SBMMember.Web.Models;
using Razorpay.Api;
using System.Text;
using System.Security.Cryptography;
using SBMMember.Data.DataFactory;
using SBMMember.Models;
namespace SBMMember.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly string _Key;
        private readonly string _secret;
        private readonly IMemberPaymentsDataFactory paymentsDataFactory;
        public PaymentController(IConfiguration configuration, IMemberPaymentsDataFactory dataFactory)
        {
            Configuration = configuration;
            _Key = configuration.GetSection("PGKey").Value;
            _secret = configuration.GetSection("PGSecret").Value;
            paymentsDataFactory = dataFactory;
        }
        public ViewResult AcceptMemberPayment(MemberPaymentViewModel model)
        {
            MemberPaymentViewModel model1 = new MemberPaymentViewModel()
            {
                MemberName = "Rajesh Raut",
                Email = "rajesh@gmail.com",
                Mobile = "9172293692",
                Amount = 100,
                MemberId=3030
            };
            return View(model1);
        }

        public ViewResult MemberPayment(MemberPaymentViewModel model)
        {

            OrderModel orderModel = new OrderModel()
            {
                orderAmount = model.Amount,
                Currency = "INR",
                Payment_Capture = 1,
                Notes = new Dictionary<string, string>()
            {
                {"Notes1","Test Notes" }
            }
            };
            string orderId = CreateOrder(orderModel);

            RazorPayOptionsModel razorPayOptions = new RazorPayOptionsModel()
            {
                MemberId = model.MemberId,
                Key = _Key,
                AmountInSubUnits = orderModel.orderAmountInSubUnits,
                Currency = orderModel.Currency,
                Name = "Samata Bhratru Mandal",
                Description = "Member Registration Fees.",
                LogoImageUrl = "",
                OrderId = orderId,
                ProfileName = model.MemberName,
                ProfileEmail = model.Email,
                ProfileConatct = model.Mobile,
                Notes = new Dictionary<string, string>()
                {
                    {"Notes1","Test for PP" }
                }

            };
            return View(razorPayOptions);
        }

        private string CreateOrder(OrderModel orderModel)
        {
            try
            {
                RazorpayClient client = new RazorpayClient(_Key, _secret);
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", orderModel.orderAmountInSubUnits); // amount in the smallest currency unit
                options.Add("payment_capture", orderModel.Payment_Capture);
                options.Add("currency", orderModel.Currency);
                options.Add("notes", orderModel.Notes);

                Order orderResponse = client.Order.Create(options);
                var orderId = orderResponse.Attributes["id"].ToString();
                return orderId;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public ViewResult AfterPayment()
        {
            var memberId = Convert.ToInt32(Request.Form["MemberId"]);
            var paymentStatus = Request.Form["paymentstatus"].ToString();
            var orderId = Request.Form["orderid"].ToString();
            var paymentId = Request.Form["paymentid"].ToString();
            var signature = Request.Form["signature"].ToString();

            if (paymentStatus == "Fail")
                return View("PaymentFail");

            var validSignature = CompareSignatures(orderId, paymentId, signature);
            if (validSignature)
            {
                Member_PaymentsAndReciepts member_Payments = new Member_PaymentsAndReciepts()
                {
                    MemberId = memberId,
                    PaymentMode = "Online",
                    PaymentStatus = "Success",
                    ChagesPaid = Convert.ToInt32(Configuration.GetSection("SubscriptionCharges").Value.ToString()),
                    Createdate = DateTime.Now,
                    RegistrationDate = DateTime.Now,
                    Status = "Active",
                    TransactionDate=DateTime.Now,
                    TransactionNo=paymentId,
                    UpdateDate=DateTime.Now

                };
                paymentsDataFactory.AddDetails(member_Payments);
                return View("PaymentSuccess");
            }
            else
                return View("PaymentFail");


        }
        private bool CompareSignatures(string orderId, string paymentId, string razorPaySignature)
        {
            var text = orderId + "|" + paymentId;
            var secret = _secret;
            var generatedSignature = CalculateSHA256(text, secret);
            if (generatedSignature == razorPaySignature)
                return true;
            else
                return false;
        }
        private string CalculateSHA256(string text, string secret)
        {
            string result = "";
            var enc = Encoding.Default;
            byte[] baText2BeHashed = enc.GetBytes(text),
                basalt = enc.GetBytes(secret);
            HMACSHA256 hasher = new HMACSHA256(basalt);
            byte[] baHashedText = hasher.ComputeHash(baText2BeHashed);
            result = string.Join("", baHashedText.ToList().Select(b => b.ToString("x2")).ToArray());
            return result;

        }
    }
}