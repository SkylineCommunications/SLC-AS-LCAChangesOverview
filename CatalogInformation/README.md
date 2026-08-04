# SLC-GQIDS-GetApplicationInfo

## About

GetApplicationInfo is a GQI data source that gives you visibility into the **Low-Code
Apps (LCA)** deployed on a DataMiner Agent. Instead of manually browsing the application
folders on disk to find out what changed, you can query this information directly from a
low-code app or dashboard: which Low-Code App versions exist, who last changed each one,
and when that change happened.

## Key Features

- **Track version history**: List every published version of each Low-Code App found on
  the DataMiner Agent.
- **Identify authors**: Show which user created or last changed each Low-Code App version.
- **Monitor change timestamps**: Surface exactly when each version was produced.
- **Integrate with dashboards**: Expose the data through GQI so it can be combined with
  other widgets in low-code apps or dashboards.

## Prerequisites

- DataMiner 10.4.0.0/14003 or higher.
- One or more Low-Code Apps deployed on the DataMiner Agent (the data source reads their
  version metadata from the Agent's local `applications` folder).