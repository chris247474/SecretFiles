using Foundation;
using System;
using UIKit;
using Acr.UserDialogs;

namespace SecretFiles
{
    public partial class DoneCreatingController : UIViewController
    {
        public DoneCreatingController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			AddGestureRecognizerToDoneLabel();
		}

		void AddGestureRecognizerToDoneLabel()
		{
			DoneLabel.UserInteractionEnabled = true;
			DoneLabel.AddGestureRecognizer(new UITapGestureRecognizer(() =>
			{
				DismissViewController(GlobalVars.CanAnimate, () => { });
			}));
		}

		partial void ShareButton_TouchUpInside(UIButton sender)
		{
			DeviceUtil.Share("<invite link here>", this);
		}

		partial void CopyButton_TouchUpInside(UIButton sender)
		{
			DeviceUtil.CopyToClipboard("<invite link here>");
			UserDialogs.Instance.Alert("Invite link copied to clipboard!");
		}
	}
}