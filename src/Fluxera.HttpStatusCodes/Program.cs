using Markdig;
using Westwind.AspNetCore.Markdown;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMarkdown(options =>
{
	options.ConfigureMarkdigPipeline = pipeline =>
	{
		pipeline.UseYamlFrontMatter();
	};
});
builder.Services.AddRazorPages();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}
else
{
	app.UseExceptionHandler("/errors/{0}");
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseStaticFiles();

app.UseMarkdown();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
