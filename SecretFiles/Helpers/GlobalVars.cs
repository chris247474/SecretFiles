using System;
using UIKit;

namespace SecretFiles
{
	public class GlobalVars
	{
		public static bool CanAnimate = !UIAccessibility.IsReduceMotionEnabled;
		public static SecretFileChatController CurrentChat { get; set; }

		static AzureDataService _clouddb;
		public static AzureDataService CloudDB { 
			get {
				if (_clouddb == null) {
					_clouddb = new AzureDataService();
				}
				return _clouddb;
			}
		}
	}
}
