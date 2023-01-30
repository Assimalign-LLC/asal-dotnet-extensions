


namespace Assimalign.Extensions.DependencyInjection.MockObjects
{
    public class ClassWithOptionalArgsCtor
    {
        public ClassWithOptionalArgsCtor(string whatever = "BLARGH")
        {
            Whatever = whatever;
        }

        public string Whatever { get; set; }
    }
}
