using iDevWorks.Tuya;

partial class Program
{
    private static readonly string ClientSecret = "afee85a9f10743b19a5d33377971d9d2";
    private static readonly string ClientId = "4tfq8hgy4wnjw7kajxha";
    private static readonly string DeviceId = "bf5300fec5558ab660x653";

    static async Task Main(string[] args)
    {
        try
        {
            var tuya = new Client(Region.CentralEurope, ClientId, ClientSecret, new HttpClient());

            for (int i = 0; i < 3; i++)
            {
                var instructions = new Dictionary<string, object>
                    {
                        { "switch", i % 2 == 0 },
                        //{ "countdown_1", 1111 },        // setup multiple countdowns
                        //{ "relay_status", "last" },     // power_on, power_off, last
                        //{ "light_mode", "relay" },      // relay, pos, none, on
                        //{ "child_lock", true },         // maintenance lock
                        //{ "cycle_time", 33 },
                    };

                var result = await tuya.SendInstructions(DeviceId, instructions);
                Console.WriteLine(result);

                await Task.Delay(30000);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}