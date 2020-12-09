# Track HTTP Request / Responses in Dependencies
One of the valuable items that Application Insights performs is logging of calls to services / tools your application is dependent upon.  Many dependencies are [supported](https://docs.microsoft.com/en-us/azure/azure-monitor/app/asp-net-dependencies), but the main one of interest is HTTP calls.  Details about the calls are logged in the _dependencies_ sink, but do not contain the data that was requested / returned.

Given specific circumstances, it may be beneficial to log the request / response data as a record of what was changed so that engineers can investigate actions performed and debug issues when they are reported.  This project shows how to capture this data using the built-in TelemetryInitializer extension point for Application Insights.

# Examples
## GraphApiContentInitializer
This [initializer](SampleFunction/GraphApiContentInitializer.cs) captures the HTTP call request / response body and places them into the matching dependency telemetry

# How to run
To run this example, the following steps must be taken
1. Pull down the repository / code
2. In the SampleFunction project, add a local.settings.json file with the following content:
```Javascript
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "APPINSIGHTS_INSTRUMENTATIONKEY": "--application insights instance to send logs to--"
  }
}
```
3. Using Visual Studio 2019 or later, ensure that your dev environment is properly authenticated.  This is done by selecting the menu items Tools > Options > Azure Service Authentication and ensuring an account is selected / authorized.
4. Run the SampleFunction project (each method is on a timer than runs every minute) and check for the logs in your Application Insights instance!

## Sample Application Insights Query
Use the following query on the Applications Insights instance specific in the local.settings.json to view the data
```
dependencies
| extend request_body = customDimensions["request-body"], response_body = customDimensions["response-body"]
| where request_body != "" or response_body != ""
| where timestamp > ago(30d)
| order by timestamp desc
```
