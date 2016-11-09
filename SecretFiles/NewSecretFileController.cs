using Foundation;
using System;
using UIKit;

namespace SecretFiles
{
    public partial class NewSecretFileController : UIViewController
    {
		const string descriptionPlaceholderText = "Enter a description here. What kind of people will this cater to? Most likely, will they talk about?";

        public NewSecretFileController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			AddDismissGesture();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			DescriptionLabel.TextColor = UIColor.LightGray;
		}

		void RemoveDescriptionPlaceholder() {
			if (DescriptionTextView.Text.Contains(descriptionPlaceholderText)) {
				DescriptionTextView.Text = "";
			}
		}

		void AddDismissGesture() { 
			var dismissGesture = new UISwipeGestureRecognizer(() => 
			                                                  DismissViewController(GlobalVars.CanAnimate, () => { })
			                                                 );
			dismissGesture.Direction = UISwipeGestureRecognizerDirection.Down;
			View.AddGestureRecognizer(dismissGesture);
		}
    }
}