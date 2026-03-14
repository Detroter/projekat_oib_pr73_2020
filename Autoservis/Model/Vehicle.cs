namespace Autoservis.Model
{
    public class Vehicle
    {
        public Guid Id { get; set; }
        public string Registration { get; set; }
        public string Model { get; set; }
        public string Mark { get; set; }
        public TypeOfVehicle Type { get; set; }
        public double Price { get; set; }
        public bool Serviced { get; set; }
        public Vehicle(string registration, string model, string mark, TypeOfVehicle type, double price)
        {
            Id = Guid.NewGuid();
            Serviced = false;
            Price = price;
            Registration = registration;
            Model = model;
            Mark = mark;
            Type = type;
        }
    }
}