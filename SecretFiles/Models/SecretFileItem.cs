using System;
namespace SecretFiles
{
	public class SecretFileItem
	{
		public string Title { get; set; }
		public string Description { get; set; }

		string _imagefile;
		public string ImageFile { 
			get {
				/*if (!string.IsNullOrWhiteSpace(_imagefile)) { 
					return 
				}*/
				return _imagefile;
			} set {
				_imagefile = value;
			} 
		}

		public SecretFileItem(string title, string description, string imageFile) {
			Title = title;
			Description = description;
			ImageFile = imageFile;
		}
	}
}
