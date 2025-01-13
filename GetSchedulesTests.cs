using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Features.Authorizations.Model;
using CREW.Core.Models.Response;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CACIB.CREW.IntegrationTests.Features.Authorization
{
    public class GetSchedulesTests(CrewWebApplicationFactory<Program> factory) : IClassFixture<CrewWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        [Fact]
        public async Task Get_Schedules_Empty_Response()
        {
            var response = await _client.GetAsync(ApiRouteConstants.AuthorizationRoutes.Schedules + "?authorizationId=-1&assignmentId=-1&isInitialLoad=false");
            string responseContent = await response.Content.ReadAsStringAsync();
            var respData = JsonConvert.DeserializeObject<GetSchedulesResponse>(responseContent);
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(0, respData?.RecordCount);
            Assert.Equal(0, respData?.Data.Count());
        }

        [Fact]
        public async Task Get_Schedules_Validator_Failure()
        {
            var response = await _client.GetAsync(ApiRouteConstants.AuthorizationRoutes.Schedules + "?authorizationId=0&assignmentId=-1&isInitialLoad=false");
            string responseContent = await response.Content.ReadAsStringAsync();
            var respError = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
            // Assert
            Assert.Equal(HttpStatusCode.UnprocessableContent, response.StatusCode);
            Assert.Equal("Validation Error", respError?.ErrorType);
        }
    }
}
