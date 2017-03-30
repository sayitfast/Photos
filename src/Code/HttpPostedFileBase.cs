using System;

namespace Code.Models.AlbumVIewModels
{
	public class HttpPostedFileBase
	{
		public int ContentLength { get; internal set; }
		public string FileName { get; internal set; }

		internal void SaveAs(string path)
		{
			throw new NotImplementedException();
		}
	}
}