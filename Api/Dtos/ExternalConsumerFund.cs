namespace Api.Dtos
{
    public class ExternalConsumerFund
    {
        public bool Active { get; set; }

        public decimal CurrentUnitPrice { get; set; }

        public string FundManager { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
    }
}
