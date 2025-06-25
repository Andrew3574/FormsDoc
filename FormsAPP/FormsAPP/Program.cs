using FormsAPP.Hubs;
using FormsAPP.Profiles;
using FormsAPP.Services;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddSession();
builder.Services.AddSignalR();
builder.Services.AddAutoMapper(typeof(AccountProfile));
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<HttpClientService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseSession();
app.MapHub<FormsHub>("/formshub");
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=UserProfile}/{id?}");

app.Run();
