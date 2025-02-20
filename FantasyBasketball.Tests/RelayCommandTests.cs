using NUnit.Framework;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Commands;

namespace FantasyBasketball.Tests;

[TestFixture]
public class RelayCommandTests
{
    [Test]
    public void RelayCommandTests_ShouldInvokeExecute_WhenCanExecuteIsTrue()
    {
        var executeCalled = false;
        Action execute = () => executeCalled = true;
        Func<bool> canExecute = () => true;
        var command = new RelayCommand(execute, canExecute);

        command.Execute(null);

        Assert.That(executeCalled, Is.True);
    }

    [Test]
    public async Task RelayCommandTests_ShouldInvokeExecuteAsync_WhenCanExecuteIsTrue()
    {
        var executeCalled = false;
        Func<Task> executeAsync = async () => { executeCalled = true; await Task.CompletedTask; };
        Func<bool> canExecute = () => true;
        var command = new RelayCommand(executeAsync, canExecute);

        command.Execute(null);

        // Give some time for async execution
        await Task.Delay(100); 

        Assert.That(executeCalled, Is.True);
    }

    [Test]
    public void RelayCommandTests_ShouldRaiseCanExecuteChanged()
    {
        var command = new RelayCommand(() => { });
        var eventRaised = false;
        command.CanExecuteChanged += (sender, args) => eventRaised = true;

        command.RaiseCanExecuteChanged();

        Assert.That(eventRaised, Is.True);
    }

    [Test]
    public void RelayCommandTests_CanExecute_ShouldReturnTrue_WhenCanExecuteIsNull()
    {
        var command = new RelayCommand(() => { });

        var result = command.CanExecute(null);

        Assert.That(result, Is.True);
    }

    [Test]
    public void RelayCommandTests_CanExecute_ShouldReturnFalse_WhenCanExecuteReturnsFalse()
    {
        var command = new RelayCommand(() => { }, () => false);

        var result = command.CanExecute(null);

        Assert.That(result, Is.False);
    }
}
