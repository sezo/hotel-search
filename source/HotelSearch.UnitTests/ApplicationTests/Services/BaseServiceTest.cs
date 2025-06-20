namespace HotelSearch.UnitTests.ApplicationTests.Services;

abstract class BaseServiceTest<TService>: BaseTest
{
    protected abstract void InstantiateDependencies();
    protected abstract TService GetService();
}