using System;
using System.Collections.Generic;
using UIKit;

namespace SecretFiles
{
	public class PageControllerSource: UIPageViewControllerDataSource
	{
		public PageControllerSource(List<PageForPageController> pages)
		{
			Setup(pages);
		}

		void Setup(List<PageForPageController> pages) {
			for (int c = 0; c < pages.Count; c++)
			{
				var nextC = c + 1;
				var prevC = c - 1;

				if (nextC == pages.Count)
				{
					pages[c].Next = pages[0];
				}
				else pages[c].Next = pages[nextC];

				if (prevC < 0)
				{
					pages[c].Previous = pages[pages.Count - 1];
				}
				else pages[c].Previous = pages[prevC];
			}
		}

		public override UIViewController GetNextViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
		{
			var currentPage = referenceViewController as PageForPageController;
			if (currentPage.Next != null) {
				return currentPage.Next;
			}

			return null;
		}

		public override UIViewController GetPreviousViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
		{
			var currentPage = referenceViewController as PageForPageController;
			if (currentPage.Previous != null)
			{
				return currentPage.Previous;
			}

			return null;
		}
	}
}
