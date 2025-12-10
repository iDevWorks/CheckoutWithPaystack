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
                var instructions = new List<CodeValue>
                    {
                        new() { Code = "switch", Value = i % 2 == 0 },
                        //new() { Code = "countdown_1", Value = 1111 },     //setup multiple countdowns
                        //new() { Code = "relay_status", Value = "last" },  //power_on, power_off, last
                        //new() { Code = "light_mode", Value = "relay" },   //relay, pos, none, on
                        //new() { Code = "child_lock", Value = true },      //maintenance lock
                        //new() { Code = "cycle_time", Value = 33 },
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