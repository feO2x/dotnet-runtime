// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Xunit;

namespace Microsoft.Extensions.DependencyInjection.Tests;

public static class OptionalDisposalTests
{
    [Fact]
    public static void AvoidDisposalOfInstanceImplementingIDisposable()
    {
        var instance = new Foo();
        var serviceProvider = new ServiceCollection().AddScoped(_ => instance, doNotDispose: true).BuildServiceProvider();
        using (var scope = serviceProvider.CreateScope())
        {
            scope.ServiceProvider.GetService<Foo>();
        }

        Assert.Equal(0, instance.DisposeCallCount);
    }

    private sealed class Foo : IDisposable
    {
        public int DisposeCallCount { get; private set; }
        public void Dispose() => DisposeCallCount++;
    }
}
