namespace XLabs.Platform.WP8.Extensions
{
	using System.Threading.Tasks;

	public static class DeviceExtensions
    {
        public static Task<bool> DriveTo(this IDevice device, Position position)
        {
            return device.LaunchUriAsync(position.DriveToLink());
        }
    }
}
