using Microsoft.EntityFrameworkCore;
using Tree.Services;
using Tree.Services.Contracts;
using Trees;
using Trees.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetValue<string>("ConnectionStrings:Default");
builder.Services.AddDbContextFactory<TreesContext>(opt => opt.UseNpgsql(connectionString));
builder.Services.AddScoped<ITreesService, TreesService>();
builder.Services.AddControllers(options => { options.Filters.Add<CustomExceptionFilter>(); });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
