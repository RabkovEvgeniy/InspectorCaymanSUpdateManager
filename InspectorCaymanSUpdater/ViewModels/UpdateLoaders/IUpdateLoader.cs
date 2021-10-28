using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectorCaymanSUpdater
{
    interface IUpdateLoader
    {
        void LoadUpdate(string targetDirectoryName);
    }
}
