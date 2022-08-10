using AutoFixture.AutoMoq;
using AutoFixture;
using AutoFixture.Xunit2;

namespace CloudStorage.Tests.Base;

public class AutoDomainDataAttribute : AutoDataAttribute
{
    public AutoDomainDataAttribute()
        : base(() => new Fixture().Customize(new AutoMoqCustomization()))
    { }
}