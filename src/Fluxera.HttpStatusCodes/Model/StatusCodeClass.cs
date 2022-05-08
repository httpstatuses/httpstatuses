namespace Fluxera.HttpStatusCodes.Model
{
	using System.Text.Json.Serialization;

	public class StatusCodeClass
	{
		[JsonPropertyName("title")]
		public string Title { get; set; }
	}
}
