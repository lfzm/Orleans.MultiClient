namespace Orleans.MultiClient
{
    public  interface IClusterClientBuilder
    {
        IClusterClient Build();
    }
}
