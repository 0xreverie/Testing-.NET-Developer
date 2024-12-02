using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    class Vehicle
    {
        public string RegistrationNumber { get; set; }
        public string Color { get; set; }
        public string Type { get; set; }
        public DateTime CheckInTime { get; set; }
    }

    class ParkingLot
    {
        private int TotalSlots;
        private Dictionary<int, Vehicle> Slots;

        public ParkingLot(int totalSlots)
        {
            TotalSlots = totalSlots;
            Slots = new Dictionary<int, Vehicle>();
            for (int i = 1; i <= TotalSlots; i++)
            {
                Slots[i] = null;
            }
            Console.WriteLine($"Created a parking lot with {TotalSlots} slots");
        }

        public void Park(string registrationNumber, string color, string type)
        {
            if (type != "Mobil" && type != "Motor")
            {
                Console.WriteLine("Only Mobil and Motor are allowed.");
                return;
            }

            int slot = Slots.FirstOrDefault(s => s.Value == null).Key;

            if (slot == 0)
            {
                Console.WriteLine("Sorry, parking lot is full.");
                return;
            }

            Slots[slot] = new Vehicle
            {
                RegistrationNumber = registrationNumber,
                Color = color,
                Type = type,
                CheckInTime = DateTime.Now
            };

            Console.WriteLine($"Allocated slot number: {slot}");
        }

        public void Leave(int slotNumber)
        {
            if (!Slots.ContainsKey(slotNumber) || Slots[slotNumber] == null)
            {
                Console.WriteLine($"Slot number {slotNumber} is already free or invalid.");
                return;
            }

            Slots[slotNumber] = null;
            Console.WriteLine($"Slot number {slotNumber} is free");
        }

        public void Report()
        {
            Console.WriteLine("\nReport:");
            Console.WriteLine("Slot\tNo.\t\tType\tRegistration No\tColour");

            foreach (var slot in Slots)
            {
                if (slot.Value != null)
                {
                    Console.WriteLine($"{slot.Key}\t{slot.Value.RegistrationNumber}\t{slot.Value.Type}\t{slot.Value.RegistrationNumber}\t{slot.Value.Color}");
                }
            }

            int occupied = Slots.Count(s => s.Value != null);
            int available = TotalSlots - occupied;

            Console.WriteLine($"\nTotal Slots: {TotalSlots}, Occupied: {occupied}, Available: {available}");

            var oddEven = Slots.Values
                .Where(v => v != null)
                .GroupBy(v => int.Parse(v.RegistrationNumber.Split('-')[1]) % 2 == 0 ? "Even" : "Odd")
                .Select(g => new { Type = g.Key, Count = g.Count() });

            foreach (var group in oddEven)
            {
                Console.WriteLine($"Number of {group.Type}-numbered vehicles: {group.Count}");
            }

            var byType = Slots.Values
                .Where(v => v != null)
                .GroupBy(v => v.Type)
                .Select(g => new { Type = g.Key, Count = g.Count() });

            foreach (var group in byType)
            {
                Console.WriteLine($"Number of {group.Type}: {group.Count}");
            }

            var byColor = Slots.Values
                .Where(v => v != null)
                .GroupBy(v => v.Color)
                .Select(g => new { Color = g.Key, Count = g.Count() });

            foreach (var group in byColor)
            {
                Console.WriteLine($"Number of {group.Color}-colored vehicles: {group.Count}");
            }
        }
    }

    static void Main(string[] args)
    {
        ParkingLot parkingLot = null;

        while (true)
        {
            string input = Console.ReadLine();
            var commands = input.Split(' ');

            switch (commands[0])
            {
                case "create_parking_lot":
                    parkingLot = new ParkingLot(int.Parse(commands[1]));
                    break;

                case "park":
                    parkingLot?.Park(commands[1], commands[2], commands[3]);
                    break;

                case "leave":
                    parkingLot?.Leave(int.Parse(commands[1]));
                    break;

                case "report":
                    parkingLot?.Report();
                    break;

                case "exit":
                    return;

                default:
                    Console.WriteLine("Invalid command");
                    break;
            }
        }
    }
}
