using Foundation;
using System;
using UIKit;
using Acr.UserDialogs;
using CoreGraphics;

namespace SecretFiles
{
    public partial class SecretFileChatController : UIViewController
    {
		

		SecretFileItem content;
		NSObject keyboardShownNotif, keyboardHiddenNotif;
		const string placeholder = "Type a message here...";
		CGRect OriginalViewFrame, OriginalScrollViewFrame, OriginalImageViewFrame, OriginalTextViewFrame;
		TextViewDelegate TextView_Delegate;
		nfloat animationDuration = 0.25f;

        public SecretFileChatController (IntPtr handle) : base (handle)
        {
        }

		public void SetSecretFileContent(SecretFileItem content) {
			this.content = content;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			//hide keyboard on tap
			ChatScrollView.AddGestureRecognizer(new UITapGestureRecognizer(() => {
				TextField.ResignFirstResponder();
			}));

			//set placeholder text if textfield is blank
			TextView_Delegate = new TextViewDelegate(ChatScrollView, ChatBackground);
			TextField.Delegate = TextView_Delegate;
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			SetPlaceholderTextIfTextViewIsEmpty();

			TextField.Layer.BorderWidth = 1;
			TextField.Layer.BorderColor = UIColor.LightGray.CGColor;
		}
		void SetPlaceholderTextIfTextViewIsEmpty() { 
			if (string.IsNullOrWhiteSpace(TextField.Text))
			{
				TextField.TextColor = UIColor.LightGray;
				TextField.Text = placeholder;
			}
		}
		void ClearTextViewWhenStartingToType() {

			if (string.Equals(TextField.Text, placeholder))
			{
				TextField.Text = string.Empty;
				TextField.TextColor = UIColor.Black;
			}
		}
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			RegisterKeyboardEvents();
			SaveStartingFrames();
		}

		void SaveStartingFrames() { 
			OriginalViewFrame = View.Frame;
			OriginalScrollViewFrame = ChatScrollView.Frame;
			OriginalImageViewFrame = ChatBackground.Frame;
			OriginalTextViewFrame = TextField.Frame;
		}

		void RestoreStartingFrames_AdjustForChatTextContent() {
			var heightDiff = OriginalTextViewFrame.Y - TextField.Frame.Y;
			Console.WriteLine("Chat window content height displacement is {0}", heightDiff);
			if (heightDiff <= 0) heightDiff = 0;

			this.View.Frame = OriginalViewFrame;
			
			ChatScrollView.Frame = new CGRect(OriginalScrollViewFrame.X, OriginalScrollViewFrame.Y - heightDiff, 
			                                  OriginalScrollViewFrame.Width, OriginalScrollViewFrame.Height);
			
			ChatBackground.Frame = new CGRect(OriginalImageViewFrame.X, OriginalImageViewFrame.Y - heightDiff,
											  OriginalImageViewFrame.Width, OriginalImageViewFrame.Height);
		}

		public override void ViewWillUnload()
		{
			base.ViewWillUnload();
			keyboardShownNotif.Dispose();
			keyboardHiddenNotif.Dispose();
		}

		partial void GalleryButton_TouchUpInside(UIButton sender)
		{
			PhotoPickerService.ChoosePicture();
		}

		partial void SendButton_TouchUpInside(UIButton sender)
		{
			//send message to chat server

			ResetTextField();
		}

		void ResetTextField() { 
			TextField.Text = string.Empty;
			SetPlaceholderTextIfTextViewIsEmpty();
			TextView_Delegate.AdjustChatUIAsUserTypes(TextField);
			TextField.ResignFirstResponder();
		}

		void RegisterKeyboardEvents(){
			Console.WriteLine("Registering");

			keyboardShownNotif = UIKeyboard.Notifications.ObserveWillShow((sender, args) =>
			{
				Console.WriteLine("Notification: {0}", args.Notification);
				Console.WriteLine("FrameBegin {0}, {1}, {2}, {3}", args.FrameBegin.X, args.FrameBegin.Y, args.FrameBegin.Width, args.FrameBegin.Height);
				Console.WriteLine("FrameEnd {0}, {1}, {2}, {3}", args.FrameEnd.X, args.FrameEnd.Y, args.FrameEnd.Width, args.FrameEnd.Height);
				Console.WriteLine("AnimationDuration {0}", args.AnimationDuration);
				Console.WriteLine("AnimationCurve {0}", args.AnimationCurve);

				UIView.Animate(animationDuration, 0, UIViewAnimationOptions.CurveEaseInOut, () =>
				{
					ClearTextViewWhenStartingToType();
					View.Frame = new CGRect(View.Frame.X, View.Frame.Y - (args.FrameEnd.Height),
											View.Frame.Width, View.Frame.Height);

				}, null);
			});

			keyboardHiddenNotif = UIKeyboard.Notifications.ObserveWillHide((sender, args) =>
			{
				Console.WriteLine("keyboard hidden");
				UIView.Animate(animationDuration, 0, UIViewAnimationOptions.CurveEaseInOut, () =>
				{
					SetPlaceholderTextIfTextViewIsEmpty();
					RestoreStartingFrames_AdjustForChatTextContent();
				}, null);
			});
		}

    }
}