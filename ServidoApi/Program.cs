using Microsoft.EntityFrameworkCore;
using ServidoApi.Contenido;
using ServidoApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(op => op.UseSqlite(builder.Configuration.GetConnectionString("MiConexionLocalSQLite")));
var app = builder.Build();
app.MapGet("api/plato", async (AppDbContext contexto) => {
    var elementos = await contexto.Platos.ToListAsync();
    return Results.Ok(elementos);
});
app.MapPost("api/plato", async (AppDbContext contexto, Plato plato) => {
    var elementos = await contexto.Platos.AddAsync(plato);
    await contexto.SaveChangesAsync();
    return Results.Created($"api/plato/{plato.Id}", plato);// 201, URI y el objeto
});
app.MapPut("api/plato/{codigo}", async (AppDbContext contexto, int codigo, Plato plato) => {
    var platoModelo = await contexto.Platos.FirstOrDefaultAsync(p => p.Id == codigo);// SELECT * FROM plato WHERE ID = codigo LIMIT 1;
    if (platoModelo == null)
        return Results.NotFound();
    platoModelo.Nombre = plato.Nombre;
    platoModelo.Costo = plato.Costo;
    platoModelo.Ingredientes = plato.Ingredientes;
    await contexto.SaveChangesAsync();
    return Results.NoContent();//204
});
app.MapDelete("api/plato/{codigo}", async (AppDbContext contexto, int codigo) => {
    var platoModelo = await contexto.Platos.FirstOrDefaultAsync(p => p.Id == codigo);// SELECT * FROM plato WHERE ID = codigo LIMIT 1;
    if (platoModelo == null)
        return Results.NotFound();
    contexto.Platos.Remove(platoModelo);
    await contexto.SaveChangesAsync();
    return Results.NoContent();//204
});
app.Run();
