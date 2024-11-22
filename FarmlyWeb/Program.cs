using FarmlyWeb.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao contêiner
builder.Services.AddControllersWithViews();

// Configura o contexto do banco de dados
builder.Services.AddDbContext<Contexto>(options =>
    options.UseSqlServer("Data Source=DESKTOP-MKPA54J;Initial Catalog=PIM_FAZENDA_URBANA;Integrated Security=True;Encrypt=False"));

// Adiciona suporte a controladoras de API
builder.Services.AddControllers();

// Configuração do Swagger (para documentação da API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Habilitar a sessão
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tempo de expiração da sessão
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Adicionar suporte ao HttpContextAccessor
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configuração do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Habilitar a sessão
app.UseSession();

app.UseAuthorization();

// Mapear rotas de controladoras MVC e API
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers(); // Adiciona suporte para controladoras de API

app.Run();
