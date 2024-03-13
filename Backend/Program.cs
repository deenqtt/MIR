using System.IO;
using System.Text.Json; // Add this line
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// Configure services
builder.Services.AddDbContext<FullStackContext>(options =>
    options.UseSqlite("Datasource=./data.db"));

builder.Services.AddCors(o =>
{
    // Enable CORS
    o.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});


var app = builder.Build();

// Auto migration logic
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FullStackContext>();
    dbContext.Database.Migrate();
}
app.UseCors();

//Endpoint User for method Get all data, Get data use ID, Create or add data user, Update data User, Delete data user
// Endpoint for user login
// Endpoint for user login
app.MapPost("/login", async (HttpContext context, FullStackContext db) =>
{
    var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
    var login = JsonSerializer.Deserialize<User>(requestBody, new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    });

    var trimmedUsername = login.Username?.Trim().ToLower();

    var user = await db.Users.FirstOrDefaultAsync(u => u.Username != null && u.Username.Trim().ToLower() == trimmedUsername);

    if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
    {
        context.Response.StatusCode = 401; // Unauthorized
        await context.Response.WriteAsync("Invalid username or password");
        return;
    }

    // Generate JWT token
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes("your_secret_key_here"); // Change this to your secret key
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Assuming user has an Id property
            // You can add more claims as needed
        }),
        Expires = DateTime.UtcNow.AddHours(1), // Token expiration time
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    var tokenString = tokenHandler.WriteToken(token);

    // Serialize user data and token to send back to the client
    var responseData = new { user = user, token = tokenString };
    await context.Response.WriteAsync(JsonSerializer.Serialize(responseData));
});

// app.MapGet("/users", async (FullStackContext db) =>
app.MapGet("/users", async (FullStackContext db) => //Field in the name backend context with your name Context (FullStackContext)
    await db.Users.ToListAsync());

// Endpoint to get a single user by id
app.MapGet("/users/{id}", async (int id, FullStackContext db) =>
    await db.Users.FindAsync(id) is User user ? Results.Ok(user) : Results.NotFound());

// Endpoint to create a new user
// Endpoint to create a new user
app.MapPost("/users", async (User user, FullStackContext db) =>
{
    // Hash the password before storing it
    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

    // Add the user to the database
    db.Users.Add(user);

    // Save changes to the database
    await db.SaveChangesAsync();

    // Return the created user with the appropriate status code and location header
    return Results.Created($"/users/{user.Id}", user);
});


// Endpoint to update a user
app.MapPut("/users/{id}", async (int id, User updatedUser, FullStackContext db) =>
{
    var user = await db.Users.FindAsync(id);
    if (user is null) return Results.NotFound();

    user.Username = updatedUser.Username;
    user.Password = updatedUser.Password;
    user.Phone = updatedUser.Phone;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Endpoint to delete a user
app.MapDelete("/users/{id}", async (int id, FullStackContext db) =>
{
    if (await db.Users.FindAsync(id) is User user)
    {
        db.Users.Remove(user);
        await db.SaveChangesAsync();
        return Results.Ok(user);
    }

    return Results.NotFound();
});
// Endpoint to get all robots
app.MapGet("/robots", async (FullStackContext db) =>
    await db.Robots.ToListAsync());

// Endpoint to get a single robot by id
app.MapGet("/robots/{id}", async (int id, FullStackContext db) =>
    await db.Robots.FindAsync(id) is Robot robot ? Results.Ok(robot) : Results.NotFound());

// Endpoint to create a new robot
app.MapPost("/robots", async (Robot robot, FullStackContext db) =>
{
    // Add the robot to the database
    db.Robots.Add(robot);

    // Save changes to the database
    await db.SaveChangesAsync();

    // Return the created robot with the appropriate status code and location header
    return Results.Created($"/robots/{robot.Id}", robot);
});

// Endpoint to update a robot
app.MapPut("/robots/{id}", async (int id, Robot updatedRobot, FullStackContext db) =>
{
    var robot = await db.Robots.FindAsync(id);
    if (robot is null) return Results.NotFound();

    robot.Name = updatedRobot.Name;
    robot.Serialnumber = updatedRobot.Serialnumber;
    robot.Ip = updatedRobot.Ip;
    robot.DomainId = updatedRobot.DomainId;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/robots/{id}", async (int id, FullStackContext db) =>
{
    if (await db.Robots.FindAsync(id) is Robot robot)
    {
        // Hapus robot dari tabel Robot
        db.Robots.Remove(robot);
  // Hapus semua misi yang terkait dengan robot yang dihapus
        var mapsToDelete = await db.Maps.Where(m => m.Creator == robot.Name).ToListAsync();
        db.Maps.RemoveRange(mapsToDelete);
        // Hapus semua misi yang terkait dengan robot yang dihapus
        var missionsToDelete = await db.Missions.Where(m => m.Robot == robot.Name).ToListAsync();
        db.Missions.RemoveRange(missionsToDelete);

        // Hapus semua jejak kaki yang terkait dengan robot yang dihapus
        var footprintsToDelete = await db.Footprints.Where(f => f.Robotname == robot.Name).ToListAsync();
        db.Footprints.RemoveRange(footprintsToDelete);

        // Hapus semua kesalahan yang terkait dengan robot yang dihapus
        var errorsToDelete = await db.Errors.Where(e => e.Robotname == robot.Name).ToListAsync();
        db.Errors.RemoveRange(errorsToDelete);

        // Hapus semua aktivitas yang terkait dengan robot yang dihapus
        var activitiesToDelete = await db.Activities.Where(a => a.Robotname == robot.Name).ToListAsync();
        db.Activities.RemoveRange(activitiesToDelete);

        // Simpan perubahan ke database
        await db.SaveChangesAsync();

        // Mengembalikan respons dengan robot yang dihapus
        return Results.Ok(robot);
    }

    return Results.NotFound();
});


// Endpoint to get all maps
app.MapGet("/maps", async (FullStackContext db) =>
    await db.Maps.ToListAsync());

// Endpoint to get a single map by id
app.MapGet("/maps/{id}", async (int id, FullStackContext db) =>
    await db.Maps.FindAsync(id) is Map map ? Results.Ok(map) : Results.NotFound());

// Endpoint untuk membuat map baru
app.MapPost("/maps", async (Map map, FullStackContext db) =>
{
    // Tambahkan map ke database
    db.Maps.Add(map);

    // Simpan perubahan ke database
    await db.SaveChangesAsync();

    // Tambahkan entri ke tabel Activities dengan data yang sesuai
    var activity = new Activitie
    {
        Robotname =  map.Creator,
        Date = DateTime.Now,
        Activity = $"Added 1 new map: { map.Name}"
    };

    // Set expiry date 24 jam setelah waktu input
    activity.ExpiryTime = activity.Date.AddHours(24);

    // Tambahkan aktivitas ke database
    db.Activities.Add(activity);

    // Simpan perubahan ke database
    await db.SaveChangesAsync();

    // Kembalikan map yang dibuat dengan kode status dan header lokasi yang sesuai
    return Results.Created($"/maps/{map.Id}", map);
});


// Endpoint to update a map
app.MapPut("/maps/{id}", async (int id, Map updatedMap, FullStackContext db) =>
{
    var map = await db.Maps.FindAsync(id);
    if (map is null) return Results.NotFound();

    map.Name = updatedMap.Name;
    map.SiteName = updatedMap.SiteName;
    map.Creator = updatedMap.Creator;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Endpoint to delete a map
app.MapDelete("/maps/{id}", async (int id, FullStackContext db) =>
{
    if (await db.Maps.FindAsync(id) is Map map)
    {
        db.Maps.Remove(map);
        await db.SaveChangesAsync();
        return Results.Ok(map);
    }

    return Results.NotFound();
});// Endpoint to get all missions or filtered missions by robot
app.MapGet("/missions", async (FullStackContext db, [FromQuery] string robot) =>
{
    IQueryable<Mission> missions = db.Missions;

    if (!string.IsNullOrEmpty(robot))
    {
        missions = missions.Where(m => m.Robot == robot);
    }

    return await missions.ToListAsync();
});

// Endpoint to get all missions regardless of the robot parameter
app.MapGet("/missions/all", async (FullStackContext db) =>
{
    IQueryable<Mission> missions = db.Missions;

    return await missions.ToListAsync();
});

// Endpoint to get a single mission by id
app.MapGet("/missions/{id}", async (int id, FullStackContext db) =>
    await db.Missions.FindAsync(id) is Mission mission ? Results.Ok(mission) : Results.NotFound());

// Endpoint untuk membuat mission baru
app.MapPost("/missions", async (Mission mission, FullStackContext db) =>
{
    // Tambahkan mission ke database
    db.Missions.Add(mission);

    // Simpan perubahan ke database
    await db.SaveChangesAsync();

    // Tambahkan entri ke tabel Activities dengan data yang sesuai
    var activity = new Activitie
    {
        Robotname = mission.Robot,
        Date = DateTime.Now,
        Activity = $"Added 1 new mission: {mission.Name}"
    };

    // Set expiry date 24 jam setelah waktu input
    activity.ExpiryTime = activity.Date.AddHours(24);

    // Tambahkan aktivitas ke database
    db.Activities.Add(activity);

    // Simpan perubahan ke database
    await db.SaveChangesAsync();

    // Kembalikan mission yang dibuat dengan kode status dan header lokasi yang sesuai
    return Results.Created($"/missions/{mission.Id}", mission);
});


// Endpoint to update a mission
app.MapPut("/missions/{id}", async (int id, Mission updatedMission, FullStackContext db) =>
{
    var mission = await db.Missions.FindAsync(id);
    if (mission is null) return Results.NotFound();

    mission.Name = updatedMission.Name;
    mission.Robot = updatedMission.Robot;
    mission.Site = updatedMission.Site;
    mission.Group = updatedMission.Group;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Endpoint to delete a mission
app.MapDelete("/missions/{id}", async (int id, FullStackContext db) =>
{
    if (await db.Missions.FindAsync(id) is Mission mission)
    {
        db.Missions.Remove(mission);
        await db.SaveChangesAsync();
        return Results.Ok(mission);
    }

    return Results.NotFound();
});

// Endpoint to get all paths
app.MapGet("/paths", async (FullStackContext db) =>
    await db.Paths.ToListAsync());

// Endpoint to get a single path by id
app.MapGet("/paths/{id}", async (int id, FullStackContext db) =>
    await db.Paths.FindAsync(id) is Path path ? Results.Ok(path) : Results.NotFound());

// Endpoint to create a new path
app.MapPost("/paths", async (Path path, FullStackContext db) =>
{
    // Add the path to the database
    db.Paths.Add(path);

    // Save changes to the database
    await db.SaveChangesAsync();

    // Return the created path with the appropriate status code and location header
    return Results.Created($"/paths/{path.Id}", path);
});


// Endpoint to update a path
app.MapPut("/paths/{id}", async (int id, Path updatedPath, FullStackContext db) =>
{
    var path = await db.Paths.FindAsync(id);
    if (path is null) return Results.NotFound();

    path.Name = updatedPath.Name;
    path.Map = updatedPath.Map;
    path.Start = updatedPath.Start;
    path.Goal = updatedPath.Goal;
    path.Distance = updatedPath.Distance;

    await db.SaveChangesAsync();
    return Results.NoContent();
});
// Endpoint to delete all paths
app.MapDelete("/paths/delete-all", async (FullStackContext db) =>
{
    // Ambil semua path dari database
    var allPaths = await db.Paths.ToListAsync();

    // Hapus setiap path dari database
    db.Paths.RemoveRange(allPaths);

    // Simpan perubahan ke database
    await db.SaveChangesAsync();

    // Mengembalikan respons dengan status OK
    return Results.Ok("All paths deleted successfully");
});

// Endpoint to delete a path
app.MapDelete("/paths/{id}", async (int id, FullStackContext db) =>
{
    if (await db.Paths.FindAsync(id) is Path path)
    {
        db.Paths.Remove(path);
        await db.SaveChangesAsync();
        return Results.Ok(path);
    }

    return Results.NotFound();
});

// Endpoint to get all footprints
app.MapGet("/footprints", async (FullStackContext db) =>
    await db.Footprints.ToListAsync());



app.MapGet("/footprints/{id}", async (int id, FullStackContext db) =>
    await db.Footprints.FindAsync(id) is Footprint footprint ? Results.Ok(footprint) : Results.NotFound());

// Endpoint to create a new footprint
app.MapPost("/footprints", async (HttpContext context, FullStackContext db) =>
{
    try
    {
        var form = await context.Request.ReadFormAsync();

        var name = form["Name"];
        var robotname = form["Robotname"];

        // Process the image file
        var file = form.Files["file"];

        // Convert the image file to binary data
        byte[] imageData;
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            imageData = memoryStream.ToArray();
        }

        // Add the footprint to the database
        var newFootprint = new Footprint
        {
            Name = name,
            Robotname = robotname,
            ImageData = imageData
        };

        db.Footprints.Add(newFootprint);

        // Save changes to the database
        await db.SaveChangesAsync();

        // Return the created footprint with the appropriate status code and location header
        context.Response.Headers["Location"] = $"/footprints/{newFootprint.Id}";
        context.Response.StatusCode = 201; // Created

        // Optionally, you can return a response body with the created footprint
        await context.Response.WriteAsync(JsonSerializer.Serialize(newFootprint));
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500; // Internal Server Error
        await context.Response.WriteAsync($"Error: {ex.Message}");
    }
});



// Endpoint to update a footprint
app.MapPut("/footprints/{id}", async (int id, Footprint updatedFootprint, FullStackContext db) =>
{
    var footprint = await db.Footprints.FindAsync(id);
    if (footprint is null) return Results.NotFound();

    footprint.Name = updatedFootprint.Name;
    footprint.Robotname = updatedFootprint.Robotname;
    footprint.ImageData = updatedFootprint.ImageData;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Endpoint to delete a footprint
app.MapDelete("/footprints/{id}", async (int id, FullStackContext db) =>
{
    if (await db.Footprints.FindAsync(id) is Footprint footprint)
    {
        db.Footprints.Remove(footprint);
        await db.SaveChangesAsync();
        return Results.Ok(footprint);
    }

    return Results.NotFound();
});
// Endpoint to get all moduls
app.MapGet("/moduls", async (FullStackContext db) =>
    await db.Moduls.ToListAsync());

// Endpoint to get a single modul by id
app.MapGet("/moduls/{id}", async (int id, FullStackContext db) =>
    await db.Moduls.FindAsync(id) is Modul modul ? Results.Ok(modul) : Results.NotFound());

// Endpoint to create a new modul
app.MapPost("/moduls", async (Modul modul, FullStackContext db) =>
{
    // Add the modul to the database
    db.Moduls.Add(modul);

    // Save changes to the database
    await db.SaveChangesAsync();

    // Return the created modul with the appropriate status code and location header
    return Results.Created($"/moduls/{modul.Id}", modul);
});

// Endpoint to update a modul
app.MapPut("/moduls/{id}", async (int id, Modul updatedModul, FullStackContext db) =>
{
    var modul = await db.Moduls.FindAsync(id);
    if (modul is null) return Results.NotFound();

    modul.Name = updatedModul.Name;
    modul.Date = updatedModul.Date;
    modul.PortIn = updatedModul.PortIn;
    modul.PortOut = updatedModul.PortOut;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Endpoint to delete a modul
app.MapDelete("/moduls/{id}", async (int id, FullStackContext db) =>
{
    if (await db.Moduls.FindAsync(id) is Modul modul)
    {
        db.Moduls.Remove(modul);
        await db.SaveChangesAsync();
        return Results.Ok(modul);
    }

    return Results.NotFound();
});
// Endpoint to get all errors
app.MapGet("/errors", async (FullStackContext db) =>
    await db.Errors.ToListAsync());

// Endpoint to get a single error by id
app.MapGet("/errors/{id}", async (int id, FullStackContext db) =>
    await db.Errors.FindAsync(id) is Error error ? Results.Ok(error) : Results.NotFound());

app.MapPost("/errors", async (Error error, FullStackContext db) =>
{
    // Add the error to the database
    db.Errors.Add(error);

    // Create a new activity entry based on the error information
    var newActivity = new Activitie
    {
        Robotname = error.Robotname,
        Date = DateTime.Now, // Gunakan tanggal dan waktu saat ini
        Activity = $"Error occurred: {error.Explained}"
    };

    // Set expiry date 24 jam setelah waktu input
    newActivity.ExpiryTime = newActivity.Date.AddHours(24);


    db.Activities.Add(newActivity);

    // Save changes to the database
    await db.SaveChangesAsync();

    // Return the created error with the appropriate status code and location header
    return Results.Created($"/errors/{error.Id}", error);
});


// Endpoint to update an error
app.MapPut("/errors/{id}", async (int id, Error updatedError, FullStackContext db) =>
{
    var error = await db.Errors.FindAsync(id);
    if (error is null) return Results.NotFound();

    error.Robotname = updatedError.Robotname;
    error.Date = updatedError.Date;
    error.Explained = updatedError.Explained;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Endpoint to delete an error
app.MapDelete("/errors/{id}", async (int id, FullStackContext db) =>
{
    if (await db.Errors.FindAsync(id) is Error error)
    {
        db.Errors.Remove(error);
        await db.SaveChangesAsync();
        return Results.Ok(error);
    }

    return Results.NotFound();
});
// Endpoint to get activities by ID
app.MapGet("/activities/{id}", async (int id, FullStackContext db) =>
    await db.Activities.FindAsync(id) is Activitie activitie ? Results.Ok(activitie) : Results.NotFound());

// Endpoint to get activities by time range and robot name
// Endpoint untuk mendapatkan aktivitas berdasarkan rentang waktu dan nama robot
app.MapGet("/activities/robot/{robotName}", async (string robotName, DateTimeOffset? startTime, DateTimeOffset? endTime, FullStackContext db) =>
{
    IQueryable<Activitie> query = db.Activities.Where(a => a.Robotname == robotName);

    if (startTime.HasValue)
    {
        DateTime startTimeValue = startTime.Value.LocalDateTime; // Assign the local time value to a local variable
        query = query.Where(a => a.Date >= startTimeValue);
    }

    if (endTime.HasValue)
    {
        DateTime endTimeValue = endTime.Value.LocalDateTime; // Assign the local time value to a local variable
        query = query.Where(a => a.Date <= endTimeValue);
    }

    return Results.Ok(await query.ToListAsync());
});


// Endpoint to create a new activity
app.MapPost("/activities", async (Activitie activitie, FullStackContext db) =>
{
    // Add the activity to the database
    db.Activities.Add(activitie);

    // Save changes to the database
    await db.SaveChangesAsync();

    // Return the created activity with appropriate status code and location header
    return Results.Created($"/activities/{activitie.Id}", activitie);
});

// Endpoint to update an activity
app.MapPut("/activities/{id}", async (int id, Activitie updatedActivitie, FullStackContext db) =>
{
    var activitie = await db.Activities.FindAsync(id);
    if (activitie is null) return Results.NotFound();

    // Update activity properties with the updated ones
    activitie.Robotname = updatedActivitie.Robotname;
    activitie.Date = updatedActivitie.Date;
    activitie.Activity = updatedActivitie.Activity;

    // Save changes to the database
    await db.SaveChangesAsync();

    // Return response with no content
    return Results.NoContent();
});

// Endpoint to delete an activity
app.MapDelete("/activities/{id}", async (int id, FullStackContext db) =>
{
    var activitie = await db.Activities.FindAsync(id);
    if (activitie is null) return Results.NotFound();

    // Remove the activity from the database
    db.Activities.Remove(activitie);

    // Save changes to the database
    await db.SaveChangesAsync();

    // Return response with the deleted activity
    return Results.Ok(activitie);
});



app.UseCors();
app.Run();


public class FullStackContext : DbContext
{
    public DbSet<User> Users { get; set; } //Table User
    public DbSet<Robot> Robots { get; set; } //Table Robot
    public DbSet<Map> Maps { get; set; } //Table Map
    public DbSet<Path> Paths { get; set; } //Table Path
    public DbSet<Mission> Missions { get; set; } //Table Mission
    public DbSet<Footprint> Footprints { get; set; } //Table Footprnt
    public DbSet<Modul> Moduls { get; set; } //Table Modul
    public DbSet<Error> Errors {get; set;}//table error
    public DbSet<Activitie> Activities {get; set;}//tabel activities
   
    public FullStackContext(DbContextOptions<FullStackContext> options) : base(options) { }

}

//Table Name
public class User
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Phone { get; set; }
}
public class Robot
{
    public int Id {get; set;}
    public string? Name {get; set;}
    public string? Serialnumber {get; set;}
    public string? Ip {get; set;}
    public string? DomainId {get; set;}
}
public class Map
{
    public int Id {get; set;}
    public string? Name {get; set;}
    public string? SiteName {get; set;}
    public string? Creator {get; set;}
}
public class Path
{
    public int Id {get; set;}
    public string? Name {get; set;}
    public string? Map {get; set;}
    public string? Start {get; set;}
    public string? Goal {get; set;}
    public string? Distance {get; set;}
}
public class Mission
{
    public int Id {get; set;}
    public string? Name {get; set;}
    public string? Robot {get; set;}
    public string? Site {get; set;}
    public string? Group {get; set;}
}
public class Footprint
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Robotname { get; set; }
    public byte[]? ImageData { get; set; } // This column will store the binary image data
}
public class Modul
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Date { get; set; }
    public string? PortIn { get; set; } 
    public string? PortOut { get; set; }// This column will store the binary image data
}
public class Error
{
    public int Id { get; set; }
    public string? Robotname { get; set; }
    public string? Date { get; set; }
    public string? Explained { get; set; } 
   
}
public class Activitie
{
    public int Id { get; set; }
    public string? Robotname { get; set; }
    public DateTime Date { get; set; } // Menggunakan DateTime untuk tanggal dan waktu
    public string? Activity { get; set; } // Mengubah Activitiy menjadi Activity
    public DateTime ExpiryTime { get; set; } // Add ExpiryTime property
}


