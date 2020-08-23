using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DealerTrack.Web.Models;
using DealerTrack.Web.Utilities;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace DealerTrack.Test.IntegrationTests
{
    public class VehicleSaleControllerTest : IClassFixture<WebApplicationFactory<Web.Startup>>
    {

        private readonly WebApplicationFactory<Web.Startup> _factory;

        public VehicleSaleControllerTest(WebApplicationFactory<Web.Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task VehicleSale_UploadFile_GetErrorOnOverSizeFile()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var content = await GetContent("sample_oversize.csv");
            var response = await client.PostAsync("/VehicleSale/UploadFile", content);


            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("The file is too large.", responseString);

        }
        
        [Fact]
        public async Task VehicleSale_UploadFile_GetErrorOnInvalidFileType()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            // Act
            var content = await GetContent("sample_invalidType.txt");
            var response = await client.PostAsync("/VehicleSale/UploadFile", content);


            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("The file type is invalid.", responseString);

        }
        
        [Fact]
        public async Task VehicleSale_UploadFile_Success()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            // Act
            var content = await GetContent("sample_ok.csv");
            var response = await client.PostAsync("/VehicleSale/UploadFile", content);


            // Assert
            Assert.True(response.IsSuccessStatusCode);

            var responseString = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<ResponseModel<VehicleSaleResponseModel>>(responseString);
            
            Assert.True(model.IsSucceeded);
            Assert.NotNull(model.Result.List);
            Assert.NotNull(model.Result.List);
            Assert.True(model.Result.List.Count == 13);
            Assert.True(model.Result.MostOftenSoldVehicle == "2018 Jeep Grand Cherokee Trackhawk");
            Assert.True(model.Result.List.Last().Price == 135500M);
   }

        private async Task<MultipartFormDataContent> GetContent(string fileName)
        {
            
            var filePath = Path.Combine("files", fileName);
            var bytes = await File.ReadAllBytesAsync(filePath);
            var fileContent = new StreamContent(new MemoryStream(bytes));
            var form = new MultipartFormDataContent { { fileContent, "file", fileName } };
            return form;
        }
    }
}
