using Common;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using WebApi;

var builder = WebApplication.CreateBuilder(args);
var MyCors = builder.Configuration["App:CorsOrigins"].Split(",", StringSplitOptions.RemoveEmptyEntries).ToArray();
// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";  //修改全局的时间格式
    });
builder.Services.AddMemoryCache();
#region Swagger
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer Token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });

    //注释
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //是否显示控制器注释
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), true);


}
    );
#endregion

//数据验证FluentValidation的依赖注入
builder.Services.AddFluentValidation(opt =>
{
    opt.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
});

//跨域
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Cors", 
        builder =>
        {
            builder.WithOrigins(MyCors)
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();

            //允许所有域
            //builder.AllowAnyMethod()
            //.SetIsOriginAllowed(_ => true)
            //.AllowAnyHeader()
            //.AllowCredentials();
        });
});
//builder.Services.AddCors(c =>
//{
//    c.AddPolicy("Cors", policy =>
//     {
//         policy.AllowAnyOrigin()
//         .AllowAnyHeader()
//         .AllowAnyMethod();
//     });
//}
//    );

#region Jwt
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var tokenModel = builder.Configuration.GetSection("Jwt").Get<TokenModelJwt>();
        var secretByte = Encoding.UTF8.GetBytes(tokenModel.Secret);
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = tokenModel.Issuer,
            ValidateAudience = true,
            ValidAudience = tokenModel.Audience,
            ValidateLifetime = true,  //是否验证失效时间
            ClockSkew = TimeSpan.FromMinutes(60),  //设置失效时间
            //ValidateIssuerSigningKey = true,  //是否验证SecurityKey
            IssuerSigningKey = new SymmetricSecurityKey(secretByte)
        };
        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                //此处代码为终止.Net Core默认的返回类型和数据结果，这个很重要哦，必须
                //context.HandleResponse();

                //自定义自己想要返回的数据结果，我这里要返回的是Json对象，通过引用Newtonsoft.Json库进行转换

                //自定义返回的数据类型
                //context.Response.ContentType = "text/plain";
                ////自定义返回状态码，默认为401 我这里改成 200
                ////context.Response.StatusCode = StatusCodes.Status200OK;
                //context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                ////输出Json数据结果
                //context.Response.WriteAsync("expired");
                return Task.FromResult(0);
            },
            //403
            OnForbidden = context =>
            {
                //context.Response.ContentType = "text/plain";
                ////自定义返回状态码，默认为401 我这里改成 200
                ////context.Response.StatusCode = StatusCodes.Status200OK;
                //context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                ////输出Json数据结果
                //context.Response.WriteAsync("expired");
                return Task.FromResult(0);
            }

        };
    });
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Cors");  //启用跨域
app.UseMiddleware<CorsMiddleware>();  //添加跨域中间件处理跨域问题
#region 鉴权授权
app.UseAuthentication();
app.UseAuthorization();
#endregion

app.MapControllers();

app.Run();
