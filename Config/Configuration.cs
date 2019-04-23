namespace HWEnchCalc.Config
{
    public class Configuration
    {
        public ConnectionInfo ConnectionInfo { get; set; } = new ConnectionInfo();
        public WindowProperties WindowProperties { get; set; } = new WindowProperties();
        public CalculatorInfo CalculatorInfo { get; set; } = new CalculatorInfo();
        public GameInfo GameInfo { get; set; } = new GameInfo();
        
    }
}