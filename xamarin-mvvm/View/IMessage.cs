using System;
using System.Threading.Tasks;

namespace XamarinMvvm.View
{
	public interface IMessage
	{
		Task<string> ShowAsync(string title, string message, params string[] buttons);
	}
}