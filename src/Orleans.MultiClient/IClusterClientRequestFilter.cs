using System.Threading.Tasks;

namespace Orleans.MultiClient
{
    /// <summary>
    /// Filter before calling ClusterClient request
    /// </summary>
    public interface IClusterClientRequestFilter
    {
        IGrainFactory Filter(IGrainFactory grainFactory);
    }
}
