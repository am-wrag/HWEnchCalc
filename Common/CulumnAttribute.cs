namespace HWEnchCalc.Titan
{
    public class ColumnAttribute : System.Attribute
    {
        public ColumnAttribute(string name = "", bool hideThisColomn = false)
        {
            Name = name;
            IsHide = hideThisColomn;
        }

        public string Name { get; set; }
        public bool IsHide { get; set; }
    }
}