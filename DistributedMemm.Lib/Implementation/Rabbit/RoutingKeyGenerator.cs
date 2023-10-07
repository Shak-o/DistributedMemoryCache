namespace DistributedMemm.Lib.Implementation.Rabbit
{
    public class RoutingKeyGenerator
    {
        public string Key { get; init; }

        private RoutingKeyGenerator()
        {
            Key = "rk";
        }

        private static RoutingKeyGenerator _instance;
        public static RoutingKeyGenerator Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new RoutingKeyGenerator();
                return _instance;
            }
        }
    }
}
