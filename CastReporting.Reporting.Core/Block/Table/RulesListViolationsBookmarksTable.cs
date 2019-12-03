﻿using System.Collections.Generic;
using System.Linq;
using Cast.Util.Log;
using Cast.Util.Version;
using CastReporting.BLL.Computing;
using CastReporting.Reporting.Atrributes;
using CastReporting.Reporting.Builder.BlockProcessing;
using CastReporting.Reporting.ReportingModel;
using CastReporting.Domain;
using CastReporting.Reporting.Helper;
using CastReporting.Reporting.Core.Languages;

namespace CastReporting.Reporting.Block.Table
{
    [Block("LIST_RULES_VIOLATIONS_BOOKMARKS_TABLE")]
    public class RulesListViolationsBookmarksTable : TableBlock
    {
        public override TableDefinition Content(ReportData reportData, Dictionary<string, string> options)
        {
            List<string> rowData = new List<string>();
            var headers = new HeaderDefinition();

            List<string> metrics = options.GetOption("METRICS").Trim().Split('|').ToList();
            bool critical;
            if (options == null || !options.ContainsKey("CRITICAL") )
            {
                critical = false;
            }
            else
            {
                critical = options.GetOption("CRITICAL").Equals("true");
            }

            if (!VersionUtil.Is111Compatible(reportData.ServerVersion))
            {
                LogHelper.LogError("Bad version of RestAPI. Should be 1.11 at least for component LIST_RULES_VIOLATIONS_BOOKMARKS");
                return emptyTable(Labels.NoData);
            }

            headers.Append(Labels.RuleName);
            headers.Append(Labels.ObjectName);
            headers.Append(Labels.IFPUG_ObjectType);
            headers.Append(Labels.Status);
            headers.Append(Labels.AssociatedValue);
            headers.Append(Labels.FilePath);
            headers.Append(Labels.StartLine);
            headers.Append(Labels.EndLine);

            List<string> qualityRules = MetricsUtility.BuildRulesList(reportData, metrics,critical);
            if (qualityRules.Count > 0)
            {
                const string bcId = "60017";
                int nbLimitTop = options.GetIntOption("COUNT", 5);

                foreach (string _metric in qualityRules)
                {
                    if (!int.TryParse(_metric, out int metricId)) continue;
                    List<Violation> results = reportData.SnapshotExplorer.GetViolationsListIDbyBC(reportData.CurrentSnapshot.Href, _metric, bcId, nbLimitTop, "$all").ToList();
                    if (results == null || results.Count < 1) continue;
                    rowData.AddRange(MetricsUtility.PopulateViolationsBookmarksRow(reportData, results, headers, _metric));
                }

                if (rowData.Count <= 1)
                {
                    return emptyTable(Labels.NoViolation);
                }
            }
            else
            {
                return emptyTable(Labels.NoItem);
            }

            rowData.InsertRange(0, headers.Labels);
            var table = new TableDefinition
            {
                HasRowHeaders = false,
                HasColumnHeaders = true,
                NbColumns = headers.Count,
                NbRows = rowData.Count / headers.Count,
                Data = rowData,
            };

            return table;
        }

        private static TableDefinition emptyTable(string reason)
        {
            var emptyHeaders = new HeaderDefinition();
            emptyHeaders.Append(Labels.Violations);
            var _row = emptyHeaders.CreateDataRow();
            _row.Set(Labels.Violations, reason);
            var _data = new List<string>();
            _data.AddRange(_row);
            _data.InsertRange(0, emptyHeaders.Labels);
            return new TableDefinition
            {
                HasRowHeaders = false,
                HasColumnHeaders = true,
                NbRows = 2,
                NbColumns = 1,
                Data = _data
            };
        }
    }
}
