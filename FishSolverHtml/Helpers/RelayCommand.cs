using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishSolverHtml.Helpers
{
    public class RelayCommand 
    {
        Action action;

        public RelayCommand(Action action)
        {
            this.action = action;
        }

        public void Execute()
        {
            action.Invoke();
        }
    }
}
