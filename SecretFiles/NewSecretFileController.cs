using Foundation;
using System;
using UIKit;
using CoreGraphics;
using System.Threading.Tasks;
using Acr.UserDialogs;

namespace SecretFiles
{
    public partial class NewSecretFileController : UIViewController
    {
		const string descriptionPlaceholderText = "Enter a description here. What kind of people will this cater to? Most likely, will they talk about?";
		NSObject keyboardShownNotif, keyboardHiddenNotif;
		CGRect OriginalNextLabelFrame;

        public NewSecretFileController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			AddDismissGesture();
			AddTapGestureToNextLabel();
			AddDismissKeyboardGesture();
			RegisterKeyboardEvents();
		}

		public override void ViewDidUnload()
		{
			base.ViewDidUnload();
			keyboardShownNotif.Dispose();
			keyboardHiddenNotif.Dispose();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			SetPlaceholderTextIfTextViewIsEmpty();
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			OriginalNextLabelFrame = NextLabel.Frame;
		}

		partial void CloseButton_TouchUpInside(UIButton sender)
		{
			DismissViewController(GlobalVars.CanAnimate, null);
		}

		void SetPlaceholderTextIfTextViewIsEmpty(){
		
			if (string.IsNullOrWhiteSpace(DescriptionTextView.Text))
			{
				DescriptionTextView.TextColor = UIColor.LightGray;
				DescriptionTextView.Text = descriptionPlaceholderText;
			}
		}
		void ClearTextViewWhenStartingToType()
		{

			if (string.Equals(DescriptionTextView.Text, descriptionPlaceholderText))
			{
				DescriptionTextView.Text = string.Empty;
				DescriptionTextView.TextColor = UIColor.Black;
			}
		}

		void AddDismissKeyboardGesture() {
			this.View.AddGestureRecognizer(new UITapGestureRecognizer(() => {
				DescriptionTextView.ResignFirstResponder();
				SecretFileNameLabel.ResignFirstResponder();
			}));
			//when user presses "next" on lower right of keyboard
			SecretFileNameLabel.ShouldReturn += (textField) =>
			{
				((UITextField)textField).ResignFirstResponder();
				return true;
			};
		}
		void AddTapGestureToNextLabel()
		{
			NextLabel.UserInteractionEnabled = true;
			NextLabel.AddGestureRecognizer(new UITapGestureRecognizer(() =>
			{
				Console.WriteLine("NextLabel tapped");

				//check if fields are filled
				if (string.IsNullOrWhiteSpace(SecretFileNameLabel.Text) || string.IsNullOrWhiteSpace(DescriptionTextView.Text)
					|| string.Equals(SecretFileNameLabel.Text, SecretFileNameLabel.Placeholder) || string.Equals(DescriptionTextView.Text, descriptionPlaceholderText))
				{
					UserDialogs.Instance.Alert("Please fill in all fields");
				}
				else { 
					//create new secret file in DB

					var DoneController = Storyboard.InstantiateViewController("DoneCreatingController") as DoneCreatingController;
						DismissViewController(GlobalVars.CanAnimate, () =>
						{
							var nav = iOSNavigationHelper.GetTopUIViewController();
							nav.PresentViewController(DoneController, GlobalVars.CanAnimate, () => { });
						});
					}
			}));
		}

		void AddDismissGesture() { 
			var dismissGesture = new UISwipeGestureRecognizer(() => { 
				//swipe to change background
			});
			dismissGesture.Direction = UISwipeGestureRecognizerDirection.Down;
			View.AddGestureRecognizer(dismissGesture);
		}

		void RegisterKeyboardEvents()
		{
			Console.WriteLine("Registering");

			keyboardShownNotif = UIKeyboard.Notifications.ObserveWillShow((sender, args) =>
			{
				Console.WriteLine("Notification: {0}", args.Notification);
				Console.WriteLine("FrameBegin {0}, {1}, {2}, {3}", args.FrameBegin.X, args.FrameBegin.Y, args.FrameBegin.Width, args.FrameBegin.Height);
				Console.WriteLine("FrameEnd {0}, {1}, {2}, {3}", args.FrameEnd.X, args.FrameEnd.Y, args.FrameEnd.Width, args.FrameEnd.Height);
				Console.WriteLine("AnimationDuration {0}", args.AnimationDuration);
				Console.WriteLine("AnimationCurve {0}", args.AnimationCurve);

				if (DescriptionTextView.IsFirstResponder) { 
					UIView.Animate(0.5, () =>
						{
							ClearTextViewWhenStartingToType();
							NextLabel.Frame = new CGRect(NextLabel.Frame.X, NextLabel.Frame.Y - args.FrameEnd.Height,
														 NextLabel.Frame.Width, NextLabel.Frame.Height);
						}, null);
				}

			});

			keyboardHiddenNotif = UIKeyboard.Notifications.ObserveWillHide((sender, args) =>
			{
				Console.WriteLine("keyboard hidden");
				if (DescriptionTextView.IsFirstResponder) { 
					UIView.Animate(0.5, () =>
					{
						SetPlaceholderTextIfTextViewIsEmpty();
						NextLabel.Frame = OriginalNextLabelFrame;
					}, null);
				}
			});
		}
    }
}