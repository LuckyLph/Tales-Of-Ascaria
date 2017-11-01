using System;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  [AddComponentMenu("Game/Data/SqLiteConnectionFactory")]
  public class SqLiteConnectionFactory : GameScript, IDbConnectionFactory
  {
    private const string SqliteConnectionTemplate = "URI=file:{0}";

    [SerializeField]
    private string databaseFileName = "Database.db";

    private string connexionString;

    private void Awake()
    {
      CreateDatabaseIfDoesntExits();

      connexionString = GetConnexionString();
    }

    public DbConnection GetConnection()
    {
      return new SQLiteConnection(connexionString);
    }

    public string GetCurrentDatabaseFilePath()
    {
      return Path.Combine(ApplicationExtensions.PersistentDataPath, databaseFileName);
    }

    public string GetSourceDatabaseFilePath()
    {
      return Path.Combine(ApplicationExtensions.ApplicationDataPath, databaseFileName);
    }

    public void CreateDatabaseIfDoesntExits()
    {
      if (IsCurrentDatabaseDoesntExists())
      {
        File.Copy(GetSourceDatabaseFilePath(), GetCurrentDatabaseFilePath(), true);
      }
    }

    public void ResetDatabase()
    {
      if (IsCurrentDatabaseExists())
      {
        File.Delete(GetCurrentDatabaseFilePath());
      }
    }

    public bool IsCurrentDatabaseExists()
    {
      return File.Exists(GetCurrentDatabaseFilePath());
    }

    public bool IsCurrentDatabaseDoesntExists()
    {
      return !IsCurrentDatabaseExists();
    }

    public bool IsSourceDatabaseExists()
    {
      return File.Exists(GetSourceDatabaseFilePath());
    }

    public bool IsSourceDatabaseDoesntExists()
    {
      return !IsSourceDatabaseExists();
    }

    private string GetConnexionString()
    {
      return String.Format(SqliteConnectionTemplate, GetCurrentDatabaseFilePath());
    }
  }
}