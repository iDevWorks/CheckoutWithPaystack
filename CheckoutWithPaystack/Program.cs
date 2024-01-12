using CheckoutWithPaystack.Services;
using iDevWorks.Paystack;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddHttpClient();


var paystack = new PaystackClient("sk_test_56ad4dcb51028c01db37201b5d74f69032c4b9eb");

builder.Services.AddSingleton(paystack);
builder.Services.AddSingleton<Cache>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

app.Run();
