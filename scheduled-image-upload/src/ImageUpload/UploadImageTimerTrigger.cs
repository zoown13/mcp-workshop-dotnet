using Azure.Storage.Blobs;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ImageUpload;

/// <summary>
/// This represents the Timer Trigger function entity to upload images periodically.
/// </summary>
public class UploadImageTimerTrigger
{
    private readonly static Random random = new Random();
    private readonly static HttpClient httpClient = new HttpClient();

    private readonly BlobContainerClient _client;
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UploadImageTimerTrigger"/> class.
    /// </summary>
    /// <param name="client"><see cref="BlobContainerClient"/> instance.</param>
    /// <param name="loggerFactory"><see cref="ILoggerFactory"/> instance.</param>
    public UploadImageTimerTrigger(BlobContainerClient client, ILoggerFactory loggerFactory)
    {
        this._client = client ?? throw new ArgumentNullException(nameof(client));
        this._logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger<UploadImageTimerTrigger>();
    }

    /// <summary>
    /// Invokes the Timer Trigger function.
    /// </summary>
    /// <param name="timer"><see cref="TimerInfo"/> instance.</param>
    [Function(nameof(UploadImageTimerTrigger))]
    [FixedDelayRetry(5, "00:00:05")]
    public async Task Run([TimerTrigger("%RunSchedule%")] TimerInfo timer)
    {
        this._logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        this._logger.LogInformation($"Next timer schedule at: {timer.ScheduleStatus.Next}");

        var image = await this.GetRandomImageAsync();
        var blobName = $"{DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss")}.png";
        await this.UploadImageAsync(blobName, image);
    }

    private async Task<byte[]> GetRandomImageAsync()
    {
        var animalType = ((AnimalType)random.Next(0, 2)).ToString().ToLowerInvariant();
        var sequence = random.Next(0, 3);
        var imageUrl = $"https://raw.githubusercontent.com/Azure-Samples/azure-functions-isloated-worker-sample/main/scheduled-image-upload/images/{animalType}{sequence}.png";
        var image = await httpClient.GetByteArrayAsync(imageUrl);

        return image;
    }

    private async Task UploadImageAsync(string blobName, byte[] image)
    {
        using var stream = new MemoryStream(image);
        await this._client.UploadBlobAsync(blobName, stream).ConfigureAwait(false);
    }
}
