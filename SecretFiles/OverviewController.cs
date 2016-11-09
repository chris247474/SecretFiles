using Foundation;
using System;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;

namespace SecretFiles
{
    public partial class OverviewController : UIViewController
    {
		nfloat SecretViewHeight;
		nfloat SecretViewWidth;
		nfloat padding = 20;

		public OverviewController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();



		}
		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			var SecretFilesSource = new List<SecretFileItem>();
			SecretFilesSource.Add(new SecretFileItem("DLSU Secret Files", "DLSU Secret Files' New Home", ""));
			SecretFilesSource.Add(new SecretFileItem("ADMU Secret Files", "DLSU Secret Files' New Home", ""));
			SecretFilesSource.Add(new SecretFileItem("UP Secret Files", "DLSU Secret Files' New Home", ""));
			SecretFilesSource.Add(new SecretFileItem("Zobel Secret Files", "DLSU Secret Files' New Home", ""));
			SecretFilesSource.Add(new SecretFileItem("GH Secret Files", "DLSU Secret Files' New Home", ""));

			SecretViewHeight = CurrentSecretsScrollView.Frame.Height * 0.7f;
			SecretViewWidth = (CurrentSecretsScrollView.Frame.Width - (padding * 3)) / 2;

			CurrentSecretsScrollView.ContentSize = new CGSize(SecretFilesSource.Count * (padding + SecretViewWidth), CurrentSecretsScrollView.Frame.Height);
			CurrentSecretsScrollView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;

			//add SecretFile templates into scrollvieww
			CreateSecretFileScrollView(SecretFilesSource);
		}
		void CreateSecretFileScrollView(List<SecretFileItem> Source) {
			Console.WriteLine("CreateSecretFileScrollView: {0} secret files", Source.Count);
			for (int c = 0; c < Source.Count; c++) { 
				var x = padding + ((padding + SecretViewWidth) * c);
				var y = padding;
				Console.WriteLine("SecretViews at y {0}, ScrollView at y {1}", y, CurrentSecretsScrollView.Frame.Y);
				Console.WriteLine("Secret files {0} positioned at {1}, {2}", c, x, y);;
				CurrentSecretsScrollView.Add(CreateSecretFileView(Source[c].Title, Source[c].Description, "",
				                                                  x, y, SecretViewWidth, SecretViewHeight));
			}
		}

		UIView CreateSecretFileView(string title, string description, string imageFile, nfloat ViewX, nfloat ViewY, nfloat ViewWidth, nfloat ViewHeight) {
			var labelSpacing = 2;
			var ViewFrame = new CGRect(ViewX, ViewY, ViewWidth, ViewHeight);

			var titleLabel = new UILabel(new CGRect(ViewX+labelSpacing, ViewY+labelSpacing, ViewWidth * 0.8, ViewHeight * 0.1));
			titleLabel.Lines = 3;
			titleLabel.LineBreakMode = UILineBreakMode.WordWrap;
			titleLabel.Font = UIFont.BoldSystemFontOfSize(15);
			titleLabel.TextColor = UIColor.White;
			titleLabel.Text = title;
			titleLabel.TextAlignment = UITextAlignment.Left;

			var descriptionLabel = new UILabel(new CGRect(ViewX + 5, ViewY + (ViewHeight * 0.6), ViewWidth * 0.8, ViewHeight * 0.4));
			descriptionLabel.Lines = 4;
			descriptionLabel.LineBreakMode = UILineBreakMode.WordWrap;
			descriptionLabel.Font = UIFont.SystemFontOfSize(15);
			descriptionLabel.TextColor = UIColor.White;
			descriptionLabel.Text = description;

			var imageView = new UIImageView(ViewFrame);
			imageView.Image = UIImage.FromFile("background.jpg");

			var secretFileView = new UIView(ViewFrame);
			secretFileView.Add(imageView);
			secretFileView.Add(titleLabel);
			secretFileView.Add(descriptionLabel);
			secretFileView.BackgroundColor = UIColor.Black;

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