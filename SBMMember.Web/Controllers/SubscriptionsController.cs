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
        private readonly IMatrimonySubscriptionDataFactory matrimonySubscriptionDataFactory;
        private readonly IToastNotification toastNotification;
        public SubscriptionsController(ISubscriptionDataFactory subscriptionDataFactory,IToastNotification toast,
            IMatrimonySubscriptionDataFactory matrimonySubscriptionData)
        {
            _subscriptionDataFactory = subscriptionDataFactory;
            toastNotification = toast;
            matrimonySubscriptionDataFactory = matrimonySubscriptionData;
        }
        public IActionResult MatrimonySubscriptionCharges()
        {

            List<SBMSubscriptionCharges> subscriptions = matrimonySubscriptionDataFactory.Getsubscriptioncharges();
            List<SubscriptionViewModel> subscriptionsViewModel = new List<SubscriptionViewModel>();
            foreach (var item in subscriptions)
            {
                SubscriptionViewModel viewModel = new SubscriptionViewModel()
                {
                    Id = item.Id,
                    Charges = item.SubscriptionCharges
                };
                subscriptionsViewModel.Add(viewModel);
            }
            SubscriptionViewModel model = new SubscriptionViewModel()
            {
                SubCharges = subscriptionsViewModel
            };


            return View(model);

        }

        public IActionResult MemberPortalSubscriptionCharges()
        {
            List<SBMSubscriptionCharges> subscriptions = _subscriptionDataFactory.Getsubscriptioncharges();
            List<SubscriptionViewModel> subscriptionsViewModel = new List<SubscriptionViewModel>();
            foreach (var item in subscriptions)
            {
                SubscriptionViewModel viewModel = new SubscriptionViewModel()
                {
                    Id = item.Id,
                    Charges = item.SubscriptionCharges
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
                Charges = sub.SubscriptionCharges
            };
            return View(subscriptionViewModel);
        }

        [HttpPost]
        public IActionResult EditMembersubscription(SubscriptionViewModel viewModel)
        {
            if(viewModel!=null)
            {
                SBMSubscriptionCharges subscription = new SBMSubscriptionCharges()
                {
                    Id = viewModel.Id,
                    SubscriptionCharges = viewModel.Charges
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
        public IActionResult EditMatrimonysubscription(int id)
        {
            var sub = matrimonySubscriptionDataFactory.Getsubscriptioncharges().Where(x => x.Id == id).FirstOrDefault();
            SubscriptionViewModel subscriptionViewModel = new SubscriptionViewModel()
            {
                Id = sub.Id,
                Charges = sub.SubscriptionCharges
            };
            return View(subscriptionViewModel);
        }

        [HttpPost]
        public IActionResult EditMatrimonysubscription(SubscriptionViewModel viewModel)
        {
            if (viewModel != null)
            {
                SBMSubscriptionCharges subscription = new SBMSubscriptionCharges()
                {
                    Id = viewModel.Id,
                    SubscriptionCharges = viewModel.Charges
                };
                ResponseDTO response = matrimonySubscriptionDataFactory.UpdateSubscriptionDetails(subscription);
                if (response != null && response.Result == "Success")
                {
                    toastNotification.AddSuccessToastMessage(response.Message);
                }
                else
                {
                    toastNotification.AddErrorToastMessage(response.Message);
                }
            }
            return RedirectToAction("MatrimonySubscriptionCharges");
        }
    }
}