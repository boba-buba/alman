using Alman.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI.Models;

public class UserUI : IUserBase
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Password { get; set; } = "";

    public int Permissions { get; set; }
}
