using Microsoft.AspNetCore.ResponseCompression;
using RickAndMorty.Net.Api.Factory;
using RickAndMorty.Net.Api.Service;
using RickAndMortyKahoot.Hubs.Kahoot;
using RickAndMortyKahoot.Models.Questions;
using RickAndMortyKahoot.Services.Question;
using RickAndMortyKahoot.Services.RickAndMortyApi;
using RickAndMortyKahoot.Services.Score;
using RickAndMortyKahoot.Stores;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register stores
builder.Services.AddSingleton<ProjectStore>();

// Register Services
builder.Services.AddSingleton(_ => RickAndMortyApiFactory.Create());
builder.Services.AddSingleton(provider =>
{
  var ramApiService = provider.GetService<IRickAndMortyService>();
  if (ramApiService is null) throw new NullReferenceException(nameof(ramApiService));
  return new RickAndMortyKahootService(ramApiService);
});
builder.Services.AddSingleton(provider =>
{
  var kahootService = provider.GetService<RickAndMortyKahootService>();
  if (kahootService is null) throw new NullReferenceException(nameof(kahootService));

  List<Question> questions = QuestionService.DefineAllQuestions(kahootService).Result;
  return new QuestionService(questions);
});
builder.Services.AddSingleton(provider =>
{
  var questionService = provider.GetService<QuestionService>();
  if (questionService is null) throw new NullReferenceException(nameof(questionService));
  return new ScoreService(questionService);
});

// Register SignalR
builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
  opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(["application/octet-stream"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// SignalR
app.UseResponseCompression();
app.MapHub<KahootHub>("/kahoothub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
