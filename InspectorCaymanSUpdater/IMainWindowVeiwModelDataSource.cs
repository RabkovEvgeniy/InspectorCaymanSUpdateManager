using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectorCaymanSUpdater
{
    internal interface IMainWindowVeiwModelDataSource
    {
        string GetLastSoftwereUpdateDate();
        string GetLastDbUpdateDate();
    }
}
