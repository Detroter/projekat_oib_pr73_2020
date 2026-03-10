public class Vehicle
{
    public Guid Id { get; set; }
    public string Registration { get; set; }
    public string Model { get; set; }
    public TypeOfVehicle Type { get; set; }
    public bool Serviced { get; set; }
    public Vehicle()
    {
        Id = Guid.NewGuid();
        Serviced = false;
    }
}