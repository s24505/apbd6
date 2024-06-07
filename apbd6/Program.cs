using apbd6.Context;
using apbd6.Service;
using Microsoft.EntityFrameworkCore;

namespace apbd5
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApbdContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddScoped<IPrescriptionsService, PrescriptionsService>();
            builder.Services.AddScoped<IPatientsService, PatientsService>();
            

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
        }
    }
}