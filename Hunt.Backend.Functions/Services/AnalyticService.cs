﻿using System;

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.DataContracts;

namespace Hunt.Backend.Analytics
{
	public class AnalyticService : IDisposable
	{
		private readonly TelemetryClient _telemetryClient = new TelemetryClient(TelemetryConfiguration.Active);
		private readonly RequestTelemetry _requestTelemetry;

		private IOperationHolder<RequestTelemetry> _operation;

        public AnalyticService(RequestTelemetry requestTelemetry)
		{
            _telemetryClient.InstrumentationKey = Environment.GetEnvironmentVariable("APP_INSIGHTS_KEY");

            _requestTelemetry = requestTelemetry;

			// start tracking request operation
			_operation = _telemetryClient.StartOperation(_requestTelemetry);
		}

		public void TrackException(Exception e)
		{
			// track exceptions that occur
			_telemetryClient.TrackException(e);
		}

		public void Dispose()
		{
			// stop the operation (and track telemetry).
			_telemetryClient.StopOperation(_operation);
		}
	}
}