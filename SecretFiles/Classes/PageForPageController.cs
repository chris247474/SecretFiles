using System;
using UIKit;

namespace SecretFiles
{
	public class PageForPageController:UIViewController
	{
		public int PageIndex;
		public PageForPageController Previous, Next;

		public PageForPageController(IntPtr ptr) : base(ptr){
			
		}
	}
}
