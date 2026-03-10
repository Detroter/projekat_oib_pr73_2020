public class Receipt
{
    public Guid Id { get; set; }
    public string MechanicName { get; set; }
    public DateTime Date { get; set; }
    public double Total { get; set; }

    public Receipt(string mechanicName, DateTime date, double total)
    {
        MechanicName = mechanicName;
        Date = date;
        Total = total;
    }
}