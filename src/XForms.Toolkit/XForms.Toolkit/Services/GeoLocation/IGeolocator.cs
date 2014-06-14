using System;
using System.Threading.Tasks;
using System.Threading;

namespace XForms.Toolkit.Services.Geolocation
{
	public interface IGeolocator
	{
		event EventHandler<PositionErrorEventArgs> PositionError;

		event EventHandler<PositionEventArgs> PositionChanged;

		double DesiredAccuracy
		{
			get;
			set;
		}

		bool IsListening {
			get;
		}

		bool SupportsHeading {
			get;
		}

		bool IsGeolocationAvailable {
			get;
		}

		bool IsGeolocationEnabled {
			get;
		}

		Task<Position> GetPositionAsync (int timeout);

		Task<Position> GetPositionAsync (int timeout, bool includeHeading);

		Task<Position> GetPositionAsync (CancellationToken cancelToken);

		Task<Position> GetPositionAsync (CancellationToken cancelToken, bool includeHeading);

		Task<Position> GetPositionAsync (int timeout, CancellationToken cancelToken);

		Task<Position> GetPositionAsync (int timeout, CancellationToken cancelToken, bool includeHeading);

		void StartListening (int minTime, double minDistance);

		void StartListening (int minTime, double minDistance, bool includeHeading);

		void StopListening ();
	}
}

