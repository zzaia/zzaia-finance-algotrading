namespace Zzaia.Finance.Core.Models.EnumerationAggregate
{
    public class Continent : Enumeration
    {
        public static readonly Continent SA = new Continent(1, "South America");
        public static readonly Continent OC = new Continent(2, "Oceania");
        public static readonly Continent NA = new Continent(3, "North America");
        public static readonly Continent AN = new Continent(4, "Antarctica");
        public static readonly Continent AS = new Continent(5, "Asia");
        public static readonly Continent EU = new Continent(6, "Europe");
        public static readonly Continent AF = new Continent(7, "Africa");

        public Continent() { }
        private Continent(int value, string displayName) : base(value, displayName) { }
    }
}
