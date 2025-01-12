namespace Siasm
{
    [System.Serializable]
    public class BoolAndIntTuple
    {
        public bool BoolValue;
        public int IntValue;

        public BoolAndIntTuple(bool boolValue, int intValue)
        {
            BoolValue = boolValue;
            IntValue = intValue;
        }
    }
}
