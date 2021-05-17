using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;
using RazorPayWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorPayWeb.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult IntiatePayment()
        {
            return View();
        }
        [HttpPost]
        public IActionResult IntiatePayment(PaymentModel payModel)
        {
            Random randomObj = new Random();
            string transactionId = randomObj.Next(10000000, 100000000).ToString();
            RazorpayClient client = new RazorpayClient("rzp_test_50YDal8CuXyEdq", "6DQJmgVX5EKkKsjUfZpxhtHE");
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", payModel.amount * 100);  // Amount will in paise
            options.Add("receipt", transactionId);
            options.Add("currency", "INR");
            options.Add("payment_capture", "0"); // 1 - automatic  , 0 - manual
                                                 //options.Add("notes", "-- You can put any notes here --");
            Order orderResponse = client.Order.Create(options);
            string orderId = orderResponse["id"].ToString();

            // Create order model for return on view
            OrderModel orderModel = new OrderModel
            {
                orderId = orderResponse.Attributes["id"],
                razorpayKey = "rzp_test_50YDal8CuXyEdq",
                amount = payModel.amount * 100,
                currency = "INR",
                name = payModel.name,
                email = payModel.email,
                contactNumber = payModel.contactNumber,
                address = payModel.address,
                description = "Testing description"
            };

            // Return on PaymentPage with Order data
            return View("Payment", orderModel);
        }

        [HttpPost]
        public IActionResult CompletePayment(OrderModel ordModel)
        {            
          
           RazorpayClient client = new RazorpayClient("rzp_test_50YDal8CuXyEdq", "6DQJmgVX5EKkKsjUfZpxhtHE");
            Payment payment = client.Payment.Fetch(ordModel.rzp_paymentid);
            // This code is for capture the payment 
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", payment.Attributes["amount"]);
            Payment paymentCaptured = payment.Capture(options);
            string amt = paymentCaptured.Attributes["amount"];
            //// Check payment made successfully

            if (paymentCaptured.Attributes["status"] == "captured")
            {
                // Create these action method
                return RedirectToAction("Success");
            }
            else
            {
                return RedirectToAction("Failed");
            }
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Failed()
        {
            return View();
        }
    }
}
