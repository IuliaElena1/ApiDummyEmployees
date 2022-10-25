using Newtonsoft.Json;

namespace DummyEndpoints.Reusable
{
    public class ReusableFunctions
    {

        internal static T GetJsonDeserialized<T>(string response)
        {
            return JsonConvert.DeserializeObject<T>(response);
        }

    }
}
