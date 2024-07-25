using System.Threading.Tasks;
using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Timers;

namespace CSharp_Kafka_threading
{
    public class ProgramTests
    {
        [Fact]
        public void test_RunConsumers()
        {
            // Arrange
            //var mockServiceA = new Mock<ServiceA>();
            //var mockServiceB = new Mock<ServiceB>();
            //var mockServiceC = new Mock<ServiceC>();

            //mockServiceA.Setup(s => s.DoWork(It.IsAny<CancellationToken>())).Callback(() => Task.Delay(1000).Wait());
            //mockServiceB.Setup(s => s.DoWork(It.IsAny<CancellationToken>())).Callback(() => Task.Delay(1000).Wait());
            //mockServiceC.Setup(s => s.DoWork(It.IsAny<CancellationToken>())).Callback(() => Task.Delay(1000).Wait());

            //// Act
            //Program.RunConsumers();

            //// Assert
            //// Verify that methods are called or perform other assertions as needed
            //mockServiceA.Verify(s => s.DoWork(It.IsAny<CancellationToken>()), Times.Once);
            //mockServiceB.Verify(s => s.DoWork(It.IsAny<CancellationToken>()), Times.Once);
            //mockServiceC.Verify(s => s.DoWork(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
