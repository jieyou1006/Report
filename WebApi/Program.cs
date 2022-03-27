using FluentValidation.AspNetCore;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";  //修改全局的时间格式
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    //注释
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //是否显示控制器注释
    option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), true);
}
    );

//数据验证FluentValidation的依赖注入
builder.Services.AddFluentValidation(opt =>
{
    opt.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
});

//跨域
builder.Services.AddCors(c =>
{
    c.AddPolicy("Cors", policy =>
     {
         policy.AllowAnyOrigin()
         .AllowAnyHeader()
         .AllowAnyMethod();
     });
}
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Cors");

app.UseAuthorization();

app.MapControllers();

app.Run();
