using Amazon.Runtime.Internal;
using Amazon.S3;
using Amazon.S3.Model;

namespace FormsAPI.Services
{
    public class ImageService
    {
        private readonly string _bucketName;
        private readonly string _baseUrl;
        private readonly string _keyId;
        private readonly string _secretKey;
        private readonly AmazonS3Client _client;

        public ImageService()
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            _bucketName = configuration["ImageService:bucketName"]!;
            _baseUrl = configuration["ImageService:baseUrl"]!;
            _keyId = configuration["ImageService:keyId"]!;
            _secretKey = configuration["ImageService:secretKey"]!;

            var config = new AmazonS3Config { ServiceURL = _baseUrl, AuthenticationRegion="ru-central1", ForcePathStyle = true };
            _client = new AmazonS3Client(_keyId,_secretKey,config);
        }

        public async Task<string> UploadImage(Stream imageStream, string imageName)
        {
            var objectKey = $"images/{Guid.NewGuid()}{Path.GetExtension(imageName)}";
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = objectKey,
                InputStream = imageStream,
                CannedACL = S3CannedACL.PublicRead 
            };
            return await SendPutRequest(request, objectKey);
        }

        public async Task<string> UpdateImage(Stream imageStream, string oldImageUrl, string imageName)
        {
            var response = await DeleteImage(oldImageUrl);
            return await UploadImage(imageStream, imageName);            
        }

        public async Task<DeleteObjectResponse> DeleteImage(string oldImageUrl)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = ExtractObjectKey(oldImageUrl),
            };
            return await _client.DeleteObjectAsync(request);
        }

        private string ExtractObjectKey(string url)
        {
            return url.Replace($"https://{_bucketName}.storage.yandexcloud.net/", "");
        }

        private async Task<string> SendPutRequest(IAmazonWebServiceRequest request, string objectKey)
        {
            var response = await _client.PutObjectAsync((PutObjectRequest)request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return $"https://{_bucketName}.storage.yandexcloud.net/{objectKey}";
            }
            return string.Empty;
        }

    }
}
