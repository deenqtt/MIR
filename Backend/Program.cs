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

// Endpoint to delete a robot
app.MapDelete("/robots/{id}", async (int id, FullStackContext db) =>
{
    if (await db.Robots.FindAsync(id) is Robot robot)
    {
        db.Robots.Remove(robot);
        await db.SaveChangesAsync();
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

// Endpoint to create a new map
app.MapPost("/maps", async (Map map, FullStackContext db) =>
{
    // Add the map to the database
    db.Maps.Add(map);

    // Save changes to the database
    await db.SaveChangesAsync();

    // Return the created map with the appropriate status code and location header
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

// Endpoint to create a new mission
app.MapPost("/missions", async (Mission mission, FullStackContext db) =>
{
    // Add the mission to the database
    db.Missions.Add(mission);

    // Save changes to the database
    await db.SaveChangesAsync();

    // Return the created mission with the appropriate status code and location header
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


app.UseCors();
app.Run();


public class FullStackContext : DbContext
{
    public DbSet<User> Users { get; set; } //Table User
    public DbSet<Robot> Robots { get; set; } //Table User
    public DbSet<Map> Maps { get; set; } //Table User
    public DbSet<Path> Paths { get; set; } //Table User
    public DbSet<Mission> Missions { get; set; } //Table User
    public DbSet<Footprint> Footprints { get; set; } //Table User
    public DbSet<Modul> Moduls { get; set; } //Table User
    
   
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

