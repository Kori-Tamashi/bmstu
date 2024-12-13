using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    class FormManager : Manager
    {
        public FormManager() { }

        public void _execute(FormCommand command)
        {
            command._execute();
        }
    }
}
