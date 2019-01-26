using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CronHosts.ConsoleApp
{
    public interface IProgram
    {
        Task<int> Run(string[] args);
    }
}