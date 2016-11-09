using System;
using UIKit;

namespace SecretFiles
{
	public class GlobalVars
	{
		public static bool CanAnimate = !UIAccessibility.IsReduceMotionEnabled;
	}
}
