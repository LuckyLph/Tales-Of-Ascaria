using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace TalesOfAscaria
{
    [CustomEditor(typeof(SqLiteConnectionFactory), true)]
    public class SqLiteConnectionFactoryInspector : GameScriptInspector
    {
        private SqLiteConnectionFactory sqLiteConnectionFactory;

        private new void Awake()
        {
            base.Awake();
            sqLiteConnectionFactory = target as SqLiteConnectionFactory;
        }

        private new void OnDestroy()
        {
            base.OnDestroy();
            sqLiteConnectionFactory = null;
        }

        protected override void OnDraw()
        {
            base.OnDraw();

            BeginTable("Database utils");
            BeginTableRow();
            DrawTitleLabel("Database utils");
            if (sqLiteConnectionFactory.IsSourceDatabaseExists())
            {
                DrawLabel("Open the source database file of the project.");
                DrawButton("Open Source Database", OpenSourceDatabase);
                DrawLabel("Open the current database file of the user.");
                DrawButton("Open Current Database", OpenCurrentDatabase);
                DrawLabel("Reset the current database file of the user.");
                DrawButton("Reset Current Database", ResetCurrentDatabase);
            }
            else
            {
                DrawErrorBox("Database doesn't exists. Make sure there's a database in the \"StreamingAssets\" folder.");
            }
            EndTableRow();
            EndTable();
        }

        private void OpenSourceDatabase()
        {
            OpenSqLiteDbBrowser(sqLiteConnectionFactory.GetSourceDatabaseFilePath());
        }

        private void OpenCurrentDatabase()
        {
            sqLiteConnectionFactory.CreateDatabaseIfDoesntExits();

            OpenSqLiteDbBrowser(sqLiteConnectionFactory.GetCurrentDatabaseFilePath());
        }

        private void ResetCurrentDatabase()
        {
            sqLiteConnectionFactory.ResetDatabase();
        }

        private void OpenSqLiteDbBrowser(string databasePath)
        {
            string pathToCodeGenerator = Path.GetFullPath(Path.Combine(Application.dataPath, "../Tools/SQLiteDBBrowser/SQLiteDBBrowser.exe"));
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = pathToCodeGenerator,
                Arguments = "\"" + databasePath + "\""
            };
            Process.Start(processStartInfo);
        }
    }
}