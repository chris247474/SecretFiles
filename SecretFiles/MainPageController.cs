using Foundation;
using System;
using UIKit;
using System.Collections.Generic;

namespace SecretFiles
{
    public partial class MainPageController : UIPageViewController
    {
        public MainPageController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var OverviewController = Storyboard.InstantiateViewController("OverviewController") as OverviewController;
			var CameraController = Storyboard.InstantiateViewController("CameraController") as CameraController;
			var pages = new List<PageForPageController>();
			pages.Add(CameraController);
			pages.Add(OverviewController);

			this.DataSource = new PageControllerSource(pages);
			SetViewControllers(new PageForPageController[] { pages[0] },
											UIPageViewControllerNavigationDirection.Forward,
			                   				GlobalVars.CanAnimate, s => { });
		}
    }
}