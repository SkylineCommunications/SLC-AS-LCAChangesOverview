
using Newtonsoft.Json;
using Skyline.DataMiner.Analytics.GenericInterface;
using System;
using System.Collections.Generic;
using System.IO;

namespace SLCGQIDSGetApplicationInfo
{
    [GQIMetaData(Name = "SLC-GQIDS-GetApplicationInfo")]
    public sealed class SLCGQIDSGetApplicationInfo : IGQIDataSource
    {
        private readonly GQIStringColumn applicationNameColumn =
            new GQIStringColumn("Application Name");

        private readonly GQIStringColumn changedByColumn =
            new GQIStringColumn("Changed By");

        private readonly GQIStringColumn changedAtColumn =
            new GQIStringColumn("Changed At");

        private readonly GQIStringColumn versionColumn = 
            new GQIStringColumn("Version");

        private readonly GQIStringColumn applicationIdColumn =
            new GQIStringColumn("Application ID");

        public GQIColumn[] GetColumns()
        {
            return new GQIColumn[]
            {
                applicationNameColumn,
                changedByColumn,
                changedAtColumn,
                versionColumn,
                applicationIdColumn,                
            };
        }

        public GQIPage GetNextPage(GetNextPageInputArgs args)
        {
            var rows = new List<GQIRow>();

            string applicationsRoot = @"C:\Skyline DataMiner\applications";

            if (!Directory.Exists(applicationsRoot))
            {
                return new GQIPage(rows.ToArray())
                {
                    HasNextPage = false,
                };
            }

            foreach (string applicationFolder in Directory.GetDirectories(applicationsRoot))
            {
                string applicationId = Path.GetFileName(applicationFolder);

                foreach (string versionFolder in Directory.GetDirectories(applicationFolder, "version_*"))
                {
                    string jsonFile = Path.Combine(versionFolder, "app.config.json");
                    string version = Path.GetFileName(versionFolder).Replace("version_", String.Empty);

                    if (!File.Exists(jsonFile))
                    {
                        continue;
                    }

                    try
                    {
                        string json = File.ReadAllText(jsonFile);

                        ApplicationInfo appInfo =
                            JsonConvert.DeserializeObject<ApplicationInfo>(json);

                        if (appInfo == null)
                        {
                            continue;
                        }

                        string applicationName = appInfo.Name ?? String.Empty;
                        string changedBy = appInfo.CreatedBy ?? String.Empty;
                        string changedAt = String.Empty;

                        if (appInfo.CreatedAt > 0)
                        {
                            changedAt = DateTimeOffset
                                .FromUnixTimeMilliseconds(appInfo.CreatedAt)
                                .LocalDateTime
                                .ToString("yyyy-MM-dd HH:mm:ss");
                        }

                        rows.Add(new GQIRow(new[]
                        {
                            new GQICell { Value = applicationName },
                            new GQICell { Value = changedBy },
                            new GQICell { Value = changedAt },
                            new GQICell { Value = version },
                            new GQICell { Value = applicationId },
                        }));
                    }
                    catch
                    {
                        // Ignore invalid JSON files
                    }
                }
            }

            return new GQIPage(rows.ToArray())
            {
                HasNextPage = false,
            };
        }
    }
}