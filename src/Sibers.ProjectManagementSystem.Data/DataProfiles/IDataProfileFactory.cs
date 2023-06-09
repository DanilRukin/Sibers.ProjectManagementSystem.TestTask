using Sibers.ProjectManagementSystem.Data.DataProfiles.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Data.DataProfiles
{
    public interface IDataProfileFactory
    {
        DataProfile CreateProfile();
    }
}
