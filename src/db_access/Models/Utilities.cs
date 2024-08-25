using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alman.SharedModels;
using DbAccess;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Models;

public class User : IUserBase, IDeleteDependable
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Password { get; set; } = "";

    public int Permissions { get; set; }
    public void DeleteDependable(DbContext dbContext)
    {

    }
}


public class YearResult : IYearResultBase, IDeleteDependable
{
    public int Id { get; set; }
    public int Year { get; set; }
    public int YearRemainder { get; set; }
    public void DeleteDependable(DbContext dbContext)
    {

    }
}