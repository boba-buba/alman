using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace DbAccess.Models;
public partial class AlmanContext : DbContext
{
    public string DbPath { get; } = ".\\Database\\alman.db";
    public AlmanContext(string path)
    {
        this.DbPath = path;
    }
}
