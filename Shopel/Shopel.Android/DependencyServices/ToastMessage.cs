using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Shopel.Droid.DependencyServices;
using Shopel.ToastMessaging;
using Xamarin.Forms;

[assembly: Dependency(typeof(ToastMessage))]
namespace Shopel.Droid.DependencyServices
{
    public class ToastMessage : IToastMessage
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }
    }
}