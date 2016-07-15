namespace Famoser.BeerCompanion.UnitTests.TestCases
{
    interface ITestCase
    {
        void Prepare();
        void Run();
        void Test();
        void Clean();
    }
}
