using CACIB.CREW.Api.Core.Route;
using CACIB.CREW.Api.Features.Simulation.Model;
using CREW.Core.Models.Response;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CACIB.CREW.IntegrationTests.Features.Simulation
{
    public class CreatePiSimulationAuthorizationTests(CrewWebApplicationFactory<Program> factory) : IClassFixture<CrewWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        [Fact]
        public async Task Create_PiSimulationAuthorization_Success_Response()
        {
            await new CreatePiSimulationClientTests(factory).Create_PiSimulationClient_Success_Response();
            // Arrange
            var request = new SaveAuthorizationRequest
            {
                SimulationId = 1,
                PiSimulationAuthorization = new SavePiSimulationAuthorization
                {
                    MainClientId = 1,
                    AuthorizedClients = "1",
                    MainProductId = "DEL100",
                    ProductRisks = "DEL100*",
                    BookingProductLineId = "CBT31",
                    RequestingEntityId = 10,
                    BookingEntities = "10",
                    InitialGlobalAmount = 1000,
                    InitialGlobalAmountCurrencyId = "EUR",
                    DurationValue = 9,
                    DurationUnit = "1",
                    StartDate = "01/14/2025",
                    EndDate = "01/23/2025",
                    IsAmortisable = false,
                    LineOrSpecificId = "L",
                    ConfirmationId = 3,
                    ProductTenor = 1,
                    ProductTenorUnitId = 1,
                    Status = "NEW",
                    IsInCalculation = false,
                    BiiPortfolioId = 11,
                    BiiSubPortfolioId = "DEI01",
                    DebtSeniorityId = "SEN",
                    ScheduleChanged = true,
                    Detaillines = new List<DetailLineItem> { new DetailLineItem
                    {
                        ClientId = 1,
                        ClientAmount = 1000,
                        ClientAmountCurrencyId = "EUR",
                        ClientAuthorizedCurrenciesCodes = "*DV",
                        BookingEntityId = 10,
                        EntityAmount = 1000,
                        EntityAmountCurrencyId = "EUR",
                        ProductId = "DEL100*",
                        ProductAmount = 1000,
                        ProductAmountCurrencyId = "EUR",
                        ProductTenor = 1,
                        ProductTenorUnitId = "1",
                        MaxAuthorizedAmount = 1000,
                        Amount = 1000,
                        AmountCurrencyId = "EUR",
                        DrawCcy = "*DV",
                        StatusId = 1,
                        StartDate = "01/14/2025",
                        EndDate = "01/23/2025",
                        AuthorizedCurrenciesCode = "*DV"
                    } }
                }
            };
            // Act
            var response = await _client.PostAsJsonAsync(ApiRouteConstants.SimulationRoutes.CreateAuthorization, request);
            string responseContent = await response.Content.ReadAsStringAsync();
            var respData = JsonConvert.DeserializeObject<SaveAuthorizationResponse>(responseContent);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(Convert.ToInt32(respData?.Data.Id) > 0);
        }

        [Fact]
        public async Task Create_PiSimulationAuthorization_In_Calculation_Yes_Success_Response()
        {
            await new CreatePiSimulationClientTests(factory).Create_PiSimulationClient_Success_Response();
            // Arrange
            var request = new SaveAuthorizationRequest
            {
                SimulationId = 1,
                PiSimulationAuthorization = new SavePiSimulationAuthorization
                {
                    MainClientId = 1,
                    AuthorizedClients = "1",
                    MainProductId = "TRE401",
                    ProductRisks = "TRE401*",
                    BookingProductLineId = "CBT31",
                    RequestingEntityId = 10,
                    BookingEntities = "10",
                    InitialGlobalAmount = 1000,
                    InitialGlobalAmountCurrencyId = "EUR",
                    DurationValue = 9,
                    DurationUnit = "1",
                    StartDate = "01/14/2025",
                    EndDate = "01/23/2025",
                    IsAmortisable = false,
                    LineOrSpecificId = "L",
                    ConfirmationId = 3,
                    ProductTenor = 1,
                    ProductTenorUnitId = 1,
                    Status = "NEW",
                    IsInCalculation = false,
                    BiiPortfolioId = 11,
                    BiiSubPortfolioId = "DEI01",
                    DebtSeniorityId = "SEN",
                    ScheduleChanged = true,
                    UtilizationForecastAmount = 10,
                    UtilizationForecastInPercentage = Convert.ToDecimal("0.01"),
                    Margin = 1,
                    CommitmentFees = 1,
                    Detaillines = new List<DetailLineItem> { new DetailLineItem
                    {
                        ClientId = 1,
                        ClientAmount = 1000,
                        ClientAmountCurrencyId = "EUR",
                        ClientAuthorizedCurrenciesCodes = "*DV",
                        BookingEntityId = 10,
                        EntityAmount = 1000,
                        EntityAmountCurrencyId = "EUR",
                        ProductId = "DEL100*",
                        ProductAmount = 1000,
                        ProductAmountCurrencyId = "EUR",
                        ProductTenor = 1,
                        ProductTenorUnitId = "1",
                        MaxAuthorizedAmount = 1000,
                        Amount = 1000,
                        AmountCurrencyId = "EUR",
                        DrawCcy = "*DV",
                        StatusId = 1,
                        StartDate = "01/14/2025",
                        EndDate = "01/23/2025",
                        AuthorizedCurrenciesCode = "*DV"
                    } }
                }
            };
            // Act
            var response = await _client.PostAsJsonAsync(ApiRouteConstants.SimulationRoutes.CreateAuthorization, request);
            string responseContent = await response.Content.ReadAsStringAsync();
            var respData = JsonConvert.DeserializeObject<SaveAuthorizationResponse>(responseContent);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(Convert.ToInt32(respData?.Data.Id) > 0);
        }

        [Fact]
        public async Task Create_PiSimulationAuthorization_MissingParameter_Validator_Failure()
        {
            // Arrange
            var request = new SaveAuthorizationRequest
            {
                SimulationId = 12345,
                PiSimulationAuthorization = new SavePiSimulationAuthorization
                {
                    MainClientId = 111,
                    AuthorizedClients = "111"
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(ApiRouteConstants.SimulationRoutes.CreateAuthorization, content);
            string responseContent = await response.Content.ReadAsStringAsync();
            var respError = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

            // Assert
            Assert.Equal(HttpStatusCode.UnprocessableContent, response.StatusCode);
            Assert.NotNull(respError);
            Assert.Equal("Validation Error", respError?.ErrorType);
        }

    }
}