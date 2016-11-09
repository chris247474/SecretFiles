using System;
using AddressBook;
using Foundation;
using Acr.UserDialogs;
using UIKit;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SecretFiles
{
    class DeviceUtil
	{
		public static bool CanAnimate
		{
			get{
				return !UIAccessibility.IsReduceMotionEnabled;
			}
		}

		public DeviceUtil(){
			Console.WriteLine ("Storing all contacts in iOS memory");
		}
		public static void OpeniOSSettings() { 
			UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
		}

		public static UIColor UIColorFromHex(string hexValue, float alpha = 1.0f)
		{
			var colorString = hexValue.Replace("#", "");
			if (alpha > 1.0f)
			{
				alpha = 1.0f;
			}
			else if (alpha < 0.0f)
			{
				alpha = 0.0f;
			}

			float red, green, blue;

			switch (colorString.Length)
			{
				case 3: // #RGB
					{
						red = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(0, 1)), 16) / 255f;
						green = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(1, 1)), 16) / 255f;
						blue = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(2, 1)), 16) / 255f;
						return UIColor.FromRGBA(red, green, blue, alpha);
					}
				case 6: // #RRGGBB
					{
						red = Convert.ToInt32(colorString.Substring(0, 2), 16) / 255f;
						green = Convert.ToInt32(colorString.Substring(2, 2), 16) / 255f;
						blue = Convert.ToInt32(colorString.Substring(4, 2), 16) / 255f;

						return UIColor.FromRGBA(red, green, blue, alpha);
					}

				default:
					throw new ArgumentOutOfRangeException(string.Format("Invalid color value {0} is invalid. It should be a hex value of the form #RBG, #RRGGBB", hexValue));

			}
		}
		// Displays a UIAlertView and returns the index of the button pressed.
		public static Task<int> ShowAlertReturnIndexOfButtonPressed(string title, string message, params string[] buttons)
		{
			var tcs = new TaskCompletionSource<int>();
			var alert = new UIAlertView
			{
				Title = title,
				Message = message
			};
			foreach (var button in buttons)
				alert.AddButton(button);
			alert.Clicked += (s, e) =>
			{
			};
			alert.Show();
			return tcs.Task;
		}
		public static void Share(string message, UIViewController vcForUIThread, bool showInterstitial = false)
		{
			var messagecontent = message;
			var msg = UIActivity.FromObject(messagecontent);

			var item = NSObject.FromObject(msg);
			var activityItems = new[] { item };
			var activityController = new UIActivityViewController(activityItems, null);

			var topController = UIApplication.SharedApplication.KeyWindow.RootViewController;

			while (topController.PresentedViewController != null)
			{
				topController = topController.PresentedViewController;
			}

			if (vcForUIThread == null)
			{
				topController.PresentViewController(activityController, DeviceUtil.CanAnimate, () => { });
			}
			else 
			{
				vcForUIThread.InvokeOnMainThread(() =>
						topController.PresentViewController(activityController, DeviceUtil.CanAnimate, () => { }));				
			}
		}

		public static void CopyToClipboard(String text)
		{
			UIPasteboard.General.String = text;
		}

		public static void SaveImageToPhotosApp(UIImage someImage, string filename)
		{
			try
			{
				someImage.SaveToPhotosAlbum((image, error) =>
				{
					var o = image as UIImage;
				});
			}
			catch (Exception e)
			{
				Console.WriteLine("error saving processed image: {0}", e.Message);
			}
		}

		public static void SendSMS (string number, UIViewController MainThreadVC = null){
			var smsTo = NSUrl.FromString("sms:"+number);
			if(MainThreadVC != null) MainThreadVC.InvokeOnMainThread(() => UIApplication.SharedApplication.OpenUrl(smsTo));
			else UIApplication.SharedApplication.OpenUrl(smsTo);
		}

		/*public static async Task<bool> SendSMS(string number, string message, string name,
		            string ConfirmOrBOM, bool AutoCall = false, string TodayOrTomorrow = null)
		{
			//var notifier = new iOSReminderService();
			try
			{
				var vc = UIApplication.SharedApplication.KeyWindow.RootViewController;

				while (vc.PresentedViewController != null)
				{
					vc = vc.PresentedViewController;
				}
				if (MFMessageComposeViewController.CanSendText)
				{
					Console.WriteLine("SMS available");

					var messageController =
						new MessageComposeController();

					messageController.Finished +=  
						//async 
						(sender, e) =>
					{
						Console.WriteLine("sms sent: {0}", messageController.Body);
						AutoCaller.Calling = false;
						if (string.Equals(ConfirmOrBOM, Values.BOM))
						{
							CalendarService.IsConfirmingAppointments = false;

							//set notification time to confirm appointment
						}
						else {
							CalendarService.IsConfirmingAppointments = true;

						}
						messageController.DismissViewController(DeviceUtil.CanAnimate, null);
					};

					messageController.Body = message;
					messageController.Recipients = new string[] { number };
					vc.PresentModalViewController(messageController, false);
				}
				else {
					Console.WriteLine("Can't send text");
				}
				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine("PhoneContacts.SendSMS() error: {0}", e.Message);
				if (string.Equals(ConfirmOrBOM, Values.BOM))
				{
					//notifier.Remind(DateTime.Now.AddMilliseconds(0), "BOM Confirmation failed sending to " + name, "Text Confirmation Failed");
				}
				else {

					if (string.Equals(TodayOrTomorrow, Values.TODAY))
					{
						//notifier.Remind(DateTime.Now.AddMilliseconds(0), "SMS failed to send", "Couldn't confirm " + name + " for later's meeting");
					}
					else {
						//notifier.Remind(DateTime.Now.AddMilliseconds(0), "SMS failed to send", "Couldn't confirm " + name + " for tomorrow's meeting");
					}
				}
			}
			return false;
		}*/

		/*string SaveDefaultImage(ContactData contact){
			string filename = System.IO.Path.Combine (Environment.GetFolderPath 
				(Environment.SpecialFolder.Personal), 
				"placeholder-contact-male.png");

			Console.WriteLine("Assigned default image to {0} {1}. Saving it as {1}", 
				contact.FirstName, contact.LastName, filename);
			
			return filename;
		}
		string SaveImageThenGetPath(ContactData contact, NSData image, ABPersonImageFormat format){
			string filename = "";

			try{
				if(format == ABPersonImageFormat.Thumbnail){
					filename = System.IO.Path.Combine (Environment.GetFolderPath
						(Environment.SpecialFolder.Personal), 
						string.Format("{0}.jpg", contact.ID)); 
				}else{
					filename = System.IO.Path.Combine (Environment.GetFolderPath
						(Environment.SpecialFolder.Personal), 
						string.Format("{0}-large.jpg", contact.ID));
				}

				image.Save (filename, true);

				Console.WriteLine("Found {0} {1}'s image. Saving it as {2}", 
					contact.FirstName, contact.LastName, filename);
				
				return filename;
			}catch(Exception e){
				Console.WriteLine ("Error in SaveImageThenGetPath(): {0}", e.Message);
			}
			return string.Empty;
		}*/
        public bool SaveContactToDevice(string firstName, string lastName, string phone, string aff)
        {
            try {
                ABAddressBook ab = new ABAddressBook();
                ABPerson p = new ABPerson();

                p.FirstName = firstName;
                p.LastName = lastName;
                p.Organization = aff;
				//p.GetImage(ABPersonImageFormat.Thumbnail).

                ABMutableMultiValue<string> phones = new ABMutableStringMultiValue();
                phones.Add(phone, ABPersonPhoneLabel.Mobile);

                p.SetPhones(phones);

                ab.Add(p);
                ab.Save();

				UserDialogs.Instance.ShowSuccess("Contact saved: " + firstName + " " + lastName, 2000);

                return true;
            } catch (Exception e) {
                System.Console.WriteLine("[iOS.PhoneContacts] Couldn't save contact: {0} {1}, {2}", firstName, lastName, e.Message);
				UserDialogs.Instance.ShowError("Failed to save contact: "+ firstName + " " + lastName + ". Pls try again.", 2000);
			}
            return false;
        }
		public byte[] ToByte (NSData data)
		{
			byte[] result = new byte[data.Length];
			Marshal.Copy (data.Bytes, result, 0, (int) data.Length);
			return result;
		}
		public string ToBase64String (NSData data)
		{
			return Convert.ToBase64String (ToByte (data));
		}
    }
}

