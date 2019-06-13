namespace Orleans.MultiClient
{
    public interface IClusterClientFactory
    {
        IGrainFactory Create<TGrainInterface>();
    }
}
