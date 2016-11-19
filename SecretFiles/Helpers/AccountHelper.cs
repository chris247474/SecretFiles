using System;
using System.Threading.Tasks;

namespace SecretFiles
{
	public class AccountHelper
	{
		public static async Task LogInIfCloudDBInitialized() {
			while (!GlobalVars.CloudDB.IsInitialized) {
				await Task.Delay(100);
			}
			CheckIfLoggedInThenContinue();
		}
		public static async void CheckIfLoggedInThenContinue()
		{
			Console.WriteLine("Entered CheckIfLoggedIn");
			if (!Settings.IsLoggedIn)
			{
				Settings.UserId = await GlobalVars.CloudDB.CreateNewAccount();
				Settings.Username = AccountHelper.GenerateRandomUsername();
				if (string.IsNullOrWhiteSpace(Settings.UserId))
				{
					Console.WriteLine("ID was not saved into app settings");
				}
				else if (string.IsNullOrWhiteSpace(Settings.Username))
				{
					Console.WriteLine("Username was not saved into app settings");
				}
				{
					Console.WriteLine("new account {0} name {1}", Settings.UserId, Settings.Username);
				}
				//pull latest data from server:
				//Util.LoggedInNotif(this);
			}
			else {
				Console.WriteLine("User already logged in");
			}
			Console.WriteLine("Done CheckIfLoggedIn");
		}

		public static string GenerateRandomUsername()//enhance
		{
			string rv = "";

			char[] lowers = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'j', 'k', 'm', 'n', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
			char[] uppers = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
			char[] numbers = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

			int l = lowers.Length;
			int u = uppers.Length;
			int n = numbers.Length;

			Random random = new Random();

			rv += lowers[random.Next(0, l)].ToString();
			rv += lowers[random.Next(0, l)].ToString();
			rv += lowers[random.Next(0, l)].ToString();

			rv += uppers[random.Next(0, u)].ToString();
			rv += uppers[random.Next(0, u)].ToString();
			rv += uppers[random.Next(0, u)].ToString();

			rv += numbers[random.Next(0, n)].ToString();
			rv += numbers[random.Next(0, n)].ToString();
			rv += numbers[random.Next(0, n)].ToString();

			return rv;
		}
	}
}
