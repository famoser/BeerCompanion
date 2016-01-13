using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
