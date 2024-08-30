using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alman;
using Microsoft.EntityFrameworkCore;


namespace DbAccess.Models;

/// <summary>
/// Class that represents the database.
/// </summary>
public partial class AlmanContext : DbContext
{
    /// <summary>
    /// Path to the database file.
    /// </summary>
    public string DbPath { get; }

    /// <summary>
    /// ctor that accepts database file path.
    /// </summary>
    /// <param name="path"> Path of the database file </param>
    public AlmanContext(string path)
    {
        this.DbPath = path;
    }
}
