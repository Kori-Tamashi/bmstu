using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    class TransformationManager : Manager
    {
        public TransformationManager() { }

        public void _execute(TransformationCommand command)
        {
            command._execute();
        }
    }
}
