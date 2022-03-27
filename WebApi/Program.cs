using FluentValidation.AspNetCore;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";  //�޸�ȫ�ֵ�ʱ���ʽ
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    //ע��
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //�Ƿ���ʾ������ע��
    option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), true);
}
    );

//������֤FluentValidation������ע��
builder.Services.AddFluentValidation(opt =>
{
    opt.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
});

//����
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
