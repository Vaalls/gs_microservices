using GlobalS_2.Controllers;
using GlobalS_2.Models;
using Moq;

namespace GlobalS_2.Tests.Controller
{
    public class ConsumoControllerTests
    {
        private readonly ConsumoController _controller;
        private readonly Mock<IConsumoRepository> _mockRepo;

        public ConsumoControllerTests()
        {
            _mockRepo = new Mock<IConsumoRepository>();
            _controller = new ConsumoController(_mockRepo.Object);
        }

        [Fact]
        public async Task RegisterConsumo_ShouldReturnCreatedResult_WhenValidDataIsProvided()
        {
            var consumo = new Consumo { Data = "2024-11-21", Valor = 150.5 };
            _mockRepo.Setup(r => r.AddConsumoAsync(consumo)).ReturnsAsync(consumo);

            var result = await _controller.RegisterConsumo(consumo);

            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<Consumo>(actionResult.Value);
            Assert.Equal(consumo.Data, returnValue.Data);
            Assert.Equal(consumo.Valor, returnValue.Valor);
        }

        [Fact]
        public async Task GetConsumos_ShouldReturnOkResult_WhenDataIsAvailable()
        {
            var consumos = new List<Consumo>
            {
                new Consumo { Data = "2024-11-21", Valor = 150.5 }
            };

            _mockRepo.Setup(r => r.GetConsumosAsync(0, 10)).ReturnsAsync(consumos);

            var result = await _controller.GetConsumos(1, 10);

            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Consumo>>(actionResult.Value);
            Assert.Single(returnValue);
            Assert.Equal("2024-11-21", returnValue[0].Data);
        }

        [Fact]
        public async Task GetConsumos_ShouldReturnNotFoundResult_WhenNoDataIsAvailable()
        {
            _mockRepo.Setup(r => r.GetConsumosAsync(0, 10)).ReturnsAsync(new List<Consumo>());

            var result = await _controller.GetConsumos(1, 10);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task RegisterConsumo_ShouldReturnBadRequest_WhenDataIsInvalid()
        {
            var consumo = new Consumo { Data = "", Valor = -10 };

            var result = await _controller.RegisterConsumo(consumo);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
