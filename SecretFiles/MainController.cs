using Foundation;
using System;
using UIKit;
using System.Collections.Generic;

namespace SecretFiles
{
    public partial class MainController : UIViewController
    {
		bool AlreadySet;

        public MainController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();


		}

		//setting up in ViewDidAppear allows you to reference already existing View.Frame values. Just make sure not to setup more than once
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			if (!AlreadySet) 
				SetupScrollView();{
				AlreadySet = true;
			}
		}
		void SetupScrollView() { 
			var OverviewController = Storyboard.InstantiateViewController("OverviewController") as OverviewController;
			var CameraController = Storyboard.InstantiateViewController("CameraController") as CameraController;

			MainScrollView.ContentSize = new CoreGraphics.CGSize(View.Frame.Width * 2, View.Frame.Height);

			OverviewController.View.Frame = new CoreGraphics.CGRect(CameraController.View.Frame.Width, 0, View.Frame.Width, View.Frame.Height);
			CameraController.View.Frame = View.Frame;

			MainScrollView.Frame = View.Frame;
			MainScrollView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
			MainScrollView.AddSubview(OverviewController.View);
			MainScrollView.AddSubview(CameraController.View);
			View.BringSubviewToFront(MainScrollView);
		}
    }
}