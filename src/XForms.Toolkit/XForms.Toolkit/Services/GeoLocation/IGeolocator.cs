using System;
using System.Threading.Tasks;
using System.Threading;

namespace XForms.Toolkit.Services.Geolocation
{
	/// <summary>
	/// Interface IGeolocator
	/// </summary>
	public interface IGeolocator
	{
		/// <summary>
		/// Occurs when [position error].
		/// </summary>
		event EventHandler<PositionErrorEventArgs> PositionError;

		/// <summary>
		/// Occurs when [position changed].
		/// </summary>
		event EventHandler<PositionEventArgs> PositionChanged;

		/// <summary>
		/// Gets or sets the desired accuracy.
		/// </summary>
		/// <value>The desired accuracy.</value>
		double DesiredAccuracy
		{
			get;
			set;
		}

		/// <summary>
		/// Gets a value indicating whether this instance is listening.
		/// </summary>
		/// <value><c>true</c> if this instance is listening; otherwise, <c>false</c>.</value>
		bool IsListening {
			get;
		}

		/// <summary>
		/// Gets a value indicating whether [supports heading].
		/// </summary>
		/// <value><c>true</c> if [supports heading]; otherwise, <c>false</c>.</value>
		bool SupportsHeading {
			get;
		}

		/// <summary>
		/// Gets a value indicating whether this instance is geolocation available.
		/// </summary>
		/// <value><c>true</c> if this instance is geolocation available; otherwise, <c>false</c>.</value>
		bool IsGeolocationAvailable {
			get;
		}

		/// <summary>
		/// Gets a value indicating whether this instance is geolocation enabled.
		/// </summary>
		/// <value><c>true</c> if this instance is geolocation enabled; otherwise, <c>false</c>.</value>
		bool IsGeolocationEnabled {
			get;
		}

		/// <summary>
		/// Gets the position asynchronous.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		Task<Position> GetPositionAsync (int timeout);

		/// <summary>
		/// Gets the position asynchronous.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <param name="includeHeading">if set to <c>true</c> [include heading].</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		Task<Position> GetPositionAsync (int timeout, bool includeHeading);

		/// <summary>
		/// Gets the position asynchronous.
		/// </summary>
		/// <param name="cancelToken">The cancel token.</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		Task<Position> GetPositionAsync (CancellationToken cancelToken);

		/// <summary>
		/// Gets the position asynchronous.
		/// </summary>
		/// <param name="cancelToken">The cancel token.</param>
		/// <param name="includeHeading">if set to <c>true</c> [include heading].</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		Task<Position> GetPositionAsync (CancellationToken cancelToken, bool includeHeading);

		/// <summary>
		/// Gets the position asynchronous.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <param name="cancelToken">The cancel token.</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		Task<Position> GetPositionAsync (int timeout, CancellationToken cancelToken);

		/// <summary>
		/// Gets the position asynchronous.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <param name="cancelToken">The cancel token.</param>
		/// <param name="includeHeading">if set to <c>true</c> [include heading].</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		Task<Position> GetPositionAsync (int timeout, CancellationToken cancelToken, bool includeHeading);

		/// <summary>
		/// Starts the listening.
		/// </summary>
		/// <param name="minTime">The minimum time.</param>
		/// <param name="minDistance">The minimum distance.</param>
		void StartListening (int minTime, double minDistance);

		/// <summary>
		/// Starts the listening.
		/// </summary>
		/// <param name="minTime">The minimum time.</param>
		/// <param name="minDistance">The minimum distance.</param>
		/// <param name="includeHeading">if set to <c>true</c> [include heading].</param>
		void StartListening (int minTime, double minDistance, bool includeHeading);

		/// <summary>
		/// Stops the listening.
		/// </summary>
		void StopListening ();
	}
}

