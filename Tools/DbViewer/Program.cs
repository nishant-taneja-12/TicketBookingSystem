using System;
using System.IO;
using Microsoft.Data.Sqlite;

class Program
{
    static int Main(string[] args)
    {
        var baseDir = AppContext.BaseDirectory; // where built binaries live when run via dotnet

        var candidatePaths = new[] {
            Path.Combine(Environment.CurrentDirectory, "BookingHub.API", "bookings.db"),
            Path.Combine(Environment.CurrentDirectory, "BookingHub.API", "bin", "Debug", "net8.0", "bookings.db"),
            Path.Combine(baseDir, "bookings.db"),
            Path.Combine(Environment.CurrentDirectory, "bookings.db")
        };

        string? dbPath = null;
        foreach (var p in candidatePaths)
        {
            if (File.Exists(p))
            {
                dbPath = p;
                break;
            }
        }

        if (dbPath == null)
        {
            Console.WriteLine("No bookings.db file found in common locations:");
            foreach (var p in candidatePaths) Console.WriteLine("  " + p);
            return 2;
        }

        Console.WriteLine($"Using DB file: {dbPath}");

        var connectionString = $"Data Source={dbPath}";
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='Bookings';";
        var exists = cmd.ExecuteScalar();
        if (exists == null)
        {
            Console.WriteLine("Table 'Bookings' not found in the database.");
            return 0;
        }

        // Select destination columns if present
        cmd.CommandText = "PRAGMA table_info('Bookings');";
        using var infoReader = cmd.ExecuteReader();
        var hasDestinationName = false;
        var hasDestinationAddress = false;
        while (infoReader.Read())
        {
            var colName = infoReader.GetString(1);
            if (string.Equals(colName, "Destination_Name", StringComparison.OrdinalIgnoreCase)) hasDestinationName = true;
            if (string.Equals(colName, "Destination_Address", StringComparison.OrdinalIgnoreCase)) hasDestinationAddress = true;
        }
        infoReader.Close();

        var selectSql = "SELECT Id, BookingDate, NumberOfSeats";
        if (hasDestinationAddress) selectSql += ", Destination_Address";
        if (hasDestinationName) selectSql += ", Destination_Name";
        selectSql += " FROM Bookings;";

        cmd.CommandText = selectSql;
        using var reader = cmd.ExecuteReader();
        var hasRows = false;
        while (reader.Read())
        {
            hasRows = true;
            var id = reader.IsDBNull(0) ? "(null)" : reader.GetString(0);
            var bookingDate = reader.IsDBNull(1) ? "(null)" : reader.GetString(1);
            var seats = reader.IsDBNull(2) ? "(null)" : reader.GetInt32(2).ToString();
            string destAddress = "(null)";
            string destName = "(null)";
            var colIndex = 3;
            if (hasDestinationAddress)
            {
                destAddress = reader.IsDBNull(colIndex) ? "(null)" : reader.GetString(colIndex);
                colIndex++;
            }
            if (hasDestinationName)
            {
                destName = reader.IsDBNull(colIndex) ? "(null)" : reader.GetString(colIndex);
                colIndex++;
            }

            if (hasDestinationAddress || hasDestinationName)
            {
                Console.WriteLine($"{id} | {bookingDate} | Seats: {seats} | Destination: {destName} | Address: {destAddress}");
            }
            else
            {
                Console.WriteLine($"{id} | {bookingDate} | Seats: {seats}");
            }
        }

        if (!hasRows) Console.WriteLine("No rows in Bookings table.");

        return 0;
    }
}
