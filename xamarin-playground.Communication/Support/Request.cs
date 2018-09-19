using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Playground.Communication.Support
{
	internal static class Request
	{
		public static async Task<TResult> GetAsync<TResult>(string url, CancellationToken? cancellationToken = null)
		{
			using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
			{
				var result = await client.GetAsync(url, cancellationToken ?? CancellationToken.None);
				ValidateStatusCode(result.StatusCode, result.ReasonPhrase);
				var jsonString = await result.Content.ReadAsStringAsync();

				return JsonConvert.DeserializeObject<TResult>(jsonString);
			}
		}

		public static async Task<TResult> PostAsync<TResult, TContent>(string url, TContent content, CancellationToken? cancellationToken = null)
		{
			using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
			{
				try {
					var jsonContent = new JsonContent(content);
					var result = await client.PostAsync(url, jsonContent, cancellationToken ?? CancellationToken.None);
					var jsonString = await result.Content.ReadAsStringAsync();
					var json = JsonConvert.DeserializeObject<TResult>(jsonString);

					ValidateStatusCode(result.StatusCode, result.ReasonPhrase);

					return json;
				} catch (Exception ex) {
					var a = 1;
				}
				return default(TResult);
			}
		}

		public static async Task<TResult> PostAsync<TResult>(string url, CancellationToken? cancellationToken = null)
		{
			return await PostAsync<TResult, object>(url, null, cancellationToken);
		}

		public static async Task<TResult> PutAsync<TResult, TContent>(string url, TContent content, CancellationToken? cancellationToken = null)
		{
			using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
			{
				var jsonContent = new JsonContent(content);
				var result = await client.PutAsync(url, jsonContent, cancellationToken ?? CancellationToken.None);
				var jsonString = await result.Content.ReadAsStringAsync();
				var json = JsonConvert.DeserializeObject<TResult>(jsonString);

				ValidateStatusCode(result.StatusCode, result.ReasonPhrase);

				return json;
			}
		}

		public static async Task<TResult> PutAsync<TResult>(string url, CancellationToken? cancellationToken = null)
		{
			return await PutAsync<TResult, object>(url, null, cancellationToken);
		}

		public static async Task<TResult> DeleteAsync<TResult>(string url, CancellationToken? cancellationToken = null)
		{
			using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
			{
				var result = await client.DeleteAsync(url, cancellationToken ?? CancellationToken.None);
				var jsonString = await result.Content.ReadAsStringAsync();
				var json = JsonConvert.DeserializeObject<TResult>(jsonString);

				ValidateStatusCode(result.StatusCode, result.ReasonPhrase);

				return json;
			}
		}

		private static void ValidateStatusCode(HttpStatusCode status, string reasonPhrase)
		{
			if (status != HttpStatusCode.OK)
			{
				var ex = new Exception(reasonPhrase);
				ex.Data.Add("StatusCode", status);
				ex.Data.Add("ReasonPhrase", reasonPhrase);
				throw ex;
			}
		}
	}
}