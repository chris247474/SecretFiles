using Foundation;
using System;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Acr.UserDialogs;

namespace SecretFiles
{
	public partial class OverviewController : PageForPageController
    {
		nfloat SecretViewHeight;
		nfloat SecretViewWidth;
		nfloat padding = 20;
		nfloat x = 0;
		bool SecretFilesAlreadyRendered;

		public OverviewController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			PopulateSecretFilesList();
		}
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			if (!Reachability.HasInternetConnection()) {
				UserDialogs.Instance.Alert("Please connect to the internet");
			}
		}
		void PopulateListIfConnected() {
			if (Reachability.HasInternetConnection())
			{
				PopulateSecretFilesList();
			}
		}
		async void PopulateSecretFilesList()
		{
			LoadingIndicatorView.StartAnimating();

			//fetch secret files from server
			var SecretFilesSource =
				await GlobalVars.CloudDB.GetGroupItemsAsync();
				//GenerateTestData();

			var background = "backgrounds/background.png";//for testing
			foreach (var secret in SecretFilesSource)
			{
				secret.groupImage = background;
			}

			LoadingIndicatorView.StopAnimating();

			//add SecretFile views into scrollview
			if (SecretFilesSource != null && SecretFilesSource.Count > 0)
			{
				SetupScrollViewLayout(SecretFilesSource.Count);
				CreateSecretFileScrollView(SecretFilesSource);
			}
		}

		List<GroupItem> GenerateTestData() { 
			var SecretFilesSource = new List<GroupItem>();
			SecretFilesSource.Add(new GroupItem { 
				groupName = "DLSU Secret Files", 
				groupDesc = "DLSU Secret Files' New Home", 
			});
			SecretFilesSource.Add(new GroupItem
			{
				groupName = "DLSU Secret Files",
				groupDesc = "DLSU Secret Files' New Home",
			});
			SecretFilesSource.Add(new GroupItem
			{
				groupName = "DLSU Secret Files",
				groupDesc = "DLSU Secret Files' New Home",
			});
			SecretFilesSource.Add(new GroupItem
			{
				groupName = "DLSU Secret Files",
				groupDesc = "DLSU Secret Files' New Home",
			});
			return SecretFilesSource;
		}

		void SetupScrollViewLayout(int numberOfSecretFiles) { 
			CurrentSecretsScrollView.Frame = new CGRect(CurrentSecretsScrollView.Frame.X, CurrentSecretsScrollView.Frame.Y,
														CurrentSecretsScrollView.Frame.Width,
														UIScreen.MainScreen.ApplicationFrame.Height - CurrentSecretsScrollView.Frame.Y);
			
			SecretViewHeight = ((CurrentSecretsScrollView.Frame.Height) - (padding * 2)) * 0.8f;
			SecretViewWidth = (UIScreen.MainScreen.ApplicationFrame.Width - (padding * 3)) / 2;

			CurrentSecretsScrollView.ContentSize = new CGSize(numberOfSecretFiles * (padding + SecretViewWidth), 
			                                                  0//SecretViewHeight + (padding*2)
			                                                 );
			CurrentSecretsScrollView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
		}

		void CreateSecretFileScrollView(List<GroupItem> Source) {
			Console.WriteLine("CreateSecretFileScrollView: {0} secret files", Source.Count);
			Console.WriteLine("Screen width is {0}", View.Frame.Width);//UIScreen.MainScreen.ApplicationFrame.Width);

			if (!SecretFilesAlreadyRendered) { //don't double render everytime view is shown to avoid duplicates
				var numberOfFiles = Source.Count;
				for (int c = 0; c < numberOfFiles; c++)
				{
					x += c < 1 ? padding : (padding + SecretViewWidth);
					var y = padding;
					var view = CreateSecretFileView(Source[c], x, y, SecretViewWidth, SecretViewHeight);
					CurrentSecretsScrollView.Add(view);
					CurrentSecretsScrollView.BringSubviewToFront(view);
				}
				SecretFilesAlreadyRendered = true;
			}
		}

		UIView CreateSecretFileView(GroupItem secret, nfloat ViewX, nfloat ViewY, nfloat ViewWidth, nfloat ViewHeight) {
			var labelSpacing = 10;
			var ViewFrame = new CGRect(ViewX, ViewY, ViewWidth, ViewHeight);
			var descY = ViewY + (ViewHeight * 0.6);
			var labelWidth = ViewWidth * 0.8;

			var titleLabel = new UILabel(new CGRect((ViewWidth * 0.5) - (labelWidth * 0.5), 
			                                        labelSpacing, labelWidth, ViewHeight * 0.1));//doesnt seem consistent for iPhone SE and 5s screen spacing 
			titleLabel.Lines = 2;
			titleLabel.TextAlignment = UITextAlignment.Center;
			titleLabel.LineBreakMode = UILineBreakMode.WordWrap;
			titleLabel.Font = UIFont.BoldSystemFontOfSize(15);
			titleLabel.TextColor = UIColor.White;
			titleLabel.Text = secret.groupName;
			titleLabel.TextAlignment = UITextAlignment.Left;

			var descriptionLabel = new UILabel(new CGRect((ViewWidth * 0.5) - (labelWidth * 0.5), descY, labelWidth, ViewHeight * 0.4));
			descriptionLabel.Lines = 4;
			descriptionLabel.TextAlignment = UITextAlignment.Center;
			descriptionLabel.LineBreakMode = UILineBreakMode.WordWrap;
			descriptionLabel.Font = UIFont.SystemFontOfSize(15);
			descriptionLabel.TextColor = UIColor.White;
			descriptionLabel.Text = secret.groupDesc;
			descriptionLabel.TextAlignment = UITextAlignment.Left;

			var imageView = new UIImageView(new CGRect(0, 0, ViewWidth, ViewHeight));
			imageView.ContentMode = UIViewContentMode.ScaleAspectFill;
			imageView.Image = UIImage.FromFile(secret.groupImage);

			var secretFileView = new UIView(ViewFrame);
			secretFileView.Add(imageView);
			secretFileView.Add(titleLabel);
			secretFileView.Add(descriptionLabel);

			var tapRecognizer = new UITapGestureRecognizer(() => {
				var ChatController = Storyboard.InstantiateViewController("SecretFileChatController") as SecretFileChatController;
				ChatController.SetSecretFileContent(secret);
				PresentViewController(ChatController, GlobalVars.CanAnimate, () => { });
			});
			secretFileView.AddGestureRecognizer(tapRecognizer);

			return secretFileView;
		}

		void AddSwipeNavigationGestures() { 
			var SwipeToCameraGesture = new UISwipeGestureRecognizer( () => 
			{ 
				//slide camera screen from left side
			});
			SwipeToCameraGesture.Direction = UISwipeGestureRecognizerDirection.Right;
			View.AddGestureRecognizer(SwipeToCameraGesture);
		}

		partial void NewSecretButton_TouchUpInside(UIButton sender)
		{
			var topVC = iOSNavigationHelper.GetTopUIViewController();
			var NewSecretController = topVC.Storyboard.InstantiateViewController("NewSecretFileController") as NewSecretFileController;
			this.PresentViewController(NewSecretController, GlobalVars.CanAnimate, () => { });
		}
	}
}