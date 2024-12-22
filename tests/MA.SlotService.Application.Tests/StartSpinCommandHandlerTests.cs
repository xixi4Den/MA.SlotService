using FluentAssertions;
using MA.SlotService.Application.Abstractions;
using MA.SlotService.Application.Features.StartSpin;
using MA.SlotService.Contracts;
using Moq;

namespace MA.SlotService.Application.Tests;

[TestClass]
public class StartSpinCommandHandlerTests
{
    private StartSpinCommandHandler _subject;
    
    private Mock<ISpinResultGenerator> _spinResultGeneratorMock;
    private Mock<ISpinsBalanceRepository> _spinsBalanceRepositoryMock;
    private Mock<IEventPublisher> _eventPublisherMock;
    private StartSpinCommand _command = new(666);

    [TestInitialize]
    public void Init()
    {
        _spinResultGeneratorMock = new Mock<ISpinResultGenerator>();
        _spinsBalanceRepositoryMock = new Mock<ISpinsBalanceRepository>();
        _eventPublisherMock = new Mock<IEventPublisher>();
        _subject = new StartSpinCommandHandler(_spinResultGeneratorMock.Object, _spinsBalanceRepositoryMock.Object, _eventPublisherMock.Object);

        MockResultGenerator([7, 7, 7]);
    }

    [TestMethod]
    public async Task InsufficientBalance_ShouldReturnErrorResult()
    {
        SetupDeductionResult(new SpinsBalanceDeductionResult(false, 0));
        
        var result = await _subject.Handle(_command, CancellationToken.None);

        result.IsSuccessful.Should().BeFalse();
        result.Error.Should().Be("Insufficient balance");
        result.Data.Should().BeNull();
    }
    
    [TestMethod]
    public async Task InsufficientBalance_ShouldNotPublishEvent()
    {
        SetupDeductionResult(new SpinsBalanceDeductionResult(false, 0));
        
        await _subject.Handle(_command, CancellationToken.None);

        _eventPublisherMock.Verify(x => x.PublishAsync(It.IsAny<SpinProcessedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [TestMethod]
    public async Task SufficientBalance_ShouldReturnSuccessfulResult()
    {
        SetupDeductionResult(new SpinsBalanceDeductionResult(true, 11));
        
        var result = await _subject.Handle(_command, CancellationToken.None);
        
        result.IsSuccessful.Should().BeTrue();
    }
    
    [TestMethod]
    public async Task SufficientBalance_ShouldReturnExpectedSpinResult()
    {
        SetupDeductionResult(new SpinsBalanceDeductionResult(true, 11));
        
        var result = await _subject.Handle(_command, CancellationToken.None);
        
        result.Data!.Result.Should().BeEquivalentTo([7, 7, 7]);
    }
    
    [TestMethod]
    public async Task SufficientBalance_ShouldReturnExpectedNewBalance()
    {
        SetupDeductionResult(new SpinsBalanceDeductionResult(true, 11));
        
        var result = await _subject.Handle(_command, CancellationToken.None);
        
        result.Balance.Should().Be(11);
    }
    
    [TestMethod]
    public async Task SufficientBalance_ShouldPublishEvent()
    {
        SetupDeductionResult(new SpinsBalanceDeductionResult(true, 11));
        
        await _subject.Handle(_command, CancellationToken.None);
        
        _eventPublisherMock.Verify(x => x.PublishAsync(It.IsAny<SpinProcessedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    private void SetupDeductionResult(SpinsBalanceDeductionResult deductionResult)
    {
        _spinsBalanceRepositoryMock.Setup(x => x.TryDeductAsync(It.IsAny<int>()))
            .ReturnsAsync(deductionResult);
    }
    
    private void MockResultGenerator(int[] result)
    {
        _spinResultGeneratorMock.Setup(x => x.Generate())
            .Returns(result);
    }
}