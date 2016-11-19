using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.iOS;
using Xamarin.UITest.Queries;

namespace SecretFiles.UITests
{
	[TestFixture]
	public class Tests
	{
		iOSApp app;
		const string iPhone7PlusSimID_iOS10point1 = "7C19E7DF-9D4D-44A9-BD31-0AC481366753";

		[SetUp]
		public void BeforeEachTest()
		{
			app = ConfigureApp.iOS.
							  EnableLocalScreenshots().
							  PreferIdeSettings().
							  InstalledApp("com.secret.SecretFiles").
			                  DeviceIdentifier(iPhone7PlusSimID_iOS10point1).
			                  StartApp();
		}

		[Test]
		public void ViewIsDisplayed()
		{
			// Pause until the view controller has loaded
			AppResult[] results = app.WaitForElement(c => c.Button("NewSecretFileButton"));//.Marked("NewSecretFileButton"));
			app.Screenshot("First screen.");

			var result = results.Any();
			Assert.IsTrue(result);
		}

		/*[Test]
		public void NewSecretFileController_FieldMissing_DisplayErrorMessage()
		{
			app.WaitForElement(c => c.Class("UILabel").Marked("SecretFileNameTextField"));
			app.Tap(c => c.Marked("NextLabel").Class("UILabel"));
			var results = app.WaitForElement(c => c.Marked("Please fill in all fields").Class("UILabel"));
			Assert.IsTrue(results.Any());
		}*/

		static void ResetSimulator(string deviceId)
		{
			var shutdownProcess = Process.Start("xcrun", string.Format("simctl shutdown {0}", deviceId));
			shutdownProcess.WaitForExit();
			var eraseProcess = Process.Start("xcrun", string.Format("simctl erase {0}", deviceId));
			eraseProcess.WaitForExit();
		}
	}
}

