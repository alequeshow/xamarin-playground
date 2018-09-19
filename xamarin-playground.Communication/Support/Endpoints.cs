using System;

namespace Playground.Communication.Support
{
	public static class Endpoints
	{
		public const string BASE = @"http://iorquestrador.dev.cloud.ultrai.net/Integration/iMobile.svc/WebAPI/";

		public const string AUTENTICAR = BASE + "Autenticar";
		public const string CLIENTE = BASE + "ObterDadosCliente/{O}";
	}
}