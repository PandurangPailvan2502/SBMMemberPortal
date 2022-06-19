using Microsoft.AspNetCore.Mvc;
using SBMMember.Data.DataFactory;
using System.Collections.Generic;
using SBMMember.Models;
using SBMMember.Web.Models;
using System.Linq;
using NToastNotify;
namespace SBMMember.Web.Controllers
{
    public class SubscriptionsController : Controller
    {

        private readonly ISubscriptionDataFactory _subscriptionDataFactory;
        private readonly IToastNotification toastNotification;
        public SubscriptionsController(ISubscriptionDataFactory subscriptionDataFactory,IToastNotification toast)
        {
            _subscriptionDataFactory = subscriptionDataFactory;
            toastNotification = toast;
        }
        public IActionResult MatrimonySubscriptionCharges()
        {

            return View();
        }

        public IActionResult MemberPortalSubscriptionCharges()
        {
            List<SubscriptionCharges> subscriptions = _subscriptionDataFactory.Getsubscriptioncharges();
            List<SubscriptionViewModel> subscriptionsViewModel = new List<SubscriptionViewModel>();
            foreach (var item in subscriptions)
            {
                SubscriptionViewModel viewModel = new SubscriptionViewModel()
                {
                    Id = item.Id,
                    Charges = item.SubscribeCharges
                };
                subscriptionsViewModel.Add(viewModel);
            }
            SubscriptionViewModel model = new SubscriptionViewModel()
            {
                SubCharges = subscriptionsViewModel
            };


            return View(model);
        }

        public IActionResult EditMembersubscription(int id)
        {
            var sub = _subscriptionDataFactory.Getsubscriptioncharges().Where(x => x.Id == id).FirstOrDefault();
            SubscriptionViewModel subscriptionViewModel = new SubscriptionViewModel()
            {
                Id = sub.Id,
                Charges = sub.SubscribeCharges
            };
            return View(subscriptionViewModel);
        }

        [HttpPost]
        public IActionResult EditMembersubscription(SubscriptionViewModel viewModel)
        {
            if(viewModel!=null)
            {
                SubscriptionCharges subscription = new SubscriptionCharges()
                {
                    Id = viewModel.Id,
                    SubscribeCharges = viewModel.Charges
                };
                ResponseDTO response=_subscriptionDataFactory.UpdateSubscriptionDetails(subscription);
                if(response!=null && response.Result=="Success")
                {
                    toastNotification.AddSuccessToastMessage(response.Message);
                }
                else
                {
                    toastNotification.AddErrorToastMessage(response.Message);
                }
            }
            return RedirectToAction("MemberPortalSubscriptionCharges");
        }

    }
}