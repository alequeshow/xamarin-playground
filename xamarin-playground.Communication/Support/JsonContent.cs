using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Playground.Communication.Support
{
	internal class JsonContent : HttpContent
	{
		private readonly MemoryStream _stream;

		private JsonContent()
		{
			this._stream = new MemoryStream();

			this.Headers.ContentType = new MediaTypeHeaderValue("application/json");
		}

		public JsonContent(object content) : this()
		{
			var sw = new StreamWriter(this._stream);
			var jw = new JsonTextWriter(sw);
			var js = new JsonSerializer();
			js.Serialize(jw, content);
			jw.Flush();
			this._stream.Position = 0;
		}

		protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
		{
			this._stream.CopyTo(stream);
			var async = new TaskCompletionSource<object>();
			async.SetResult(null);
			return async.Task;
		}

		protected override bool TryComputeLength(out long length)
		{
			length = this._stream.Length;
			return true;
		}
	}
}