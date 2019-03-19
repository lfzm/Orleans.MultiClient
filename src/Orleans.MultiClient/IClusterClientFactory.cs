namespace Orleans.MultiClient
{
    public interface IClusterClientFactory
    {
        IClusterClient Create<TGrainInterface>();
    }
}
