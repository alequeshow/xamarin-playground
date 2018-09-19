using System;
using System.Threading.Tasks;

namespace XamarinMvvm.View
{
	public interface INavigate
	{
		Task ToAsync(string view, params object[] parameters);
	}
}