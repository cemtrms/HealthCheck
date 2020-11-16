using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleProject.Services
{
    public interface INotifacationService
    {
        void SendeMail(string message,string userEmail);
    }
}
