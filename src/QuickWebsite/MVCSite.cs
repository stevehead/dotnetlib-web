using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Stevehead.Web.QuickWebsite;

/// <summary>
/// Represents a MVC website with standard default setup.
/// </summary>
public sealed class MVCSite
{
    private readonly string[] _args;
    private readonly IMVCSiteConfiguration _configuration;
    private readonly WebApplication _webApplication;

    /// <summary>
    /// Initializes a new <see cref="MVCSite"/> instance.
    /// </summary>
    /// 
    /// <param name="args">The program arguments to be passed.</param>
    /// <param name="configuration">
    ///     The configuration to be used. A default configuration will be used if <c>null</c>.
    /// </param>
    public MVCSite(ReadOnlySpan<string> args, IMVCSiteConfiguration? configuration = null)
    {
        _args = args.ToArray();

        if (configuration != null && configuration is not DefaultMVCSiteConfiguration)
        {
            // We copy the provided configuration to prevent changes for this running instance.
            _configuration = new MvcSiteConfiguration(configuration);
        }
        else
        {
            _configuration = DefaultMVCSiteConfiguration.Instance;
        }

        _webApplication = BuildWebApplication();
    }

    /// <summary>
    /// Runs the website.
    /// </summary>
    public void Run()
    {
        _webApplication.Run();
    }

    // Helper method to build web application.
    private WebApplication BuildWebApplication()
    {
        var builder = WebApplication.CreateBuilder(_args);

        TimeSpan? sessionTimeout = _configuration.SessionTimeout;

        // Add services to the container.
        if (sessionTimeout.HasValue)
        {
            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = sessionTimeout.Value;
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }

        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        if (_configuration.UseForwardedHeaders)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler(_configuration.ExceptionHandler);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseStaticFiles();

        if (_configuration.RewriteUrlsToForceTrailingSlash)
        {
            app.UseRewriter(new RewriteOptions()
                .AddRedirect("(.*[^/])$", "$1/")
            );
        }

        app.UseRouting();

        if (sessionTimeout.HasValue)
        {
            app.UseSession();
        }

        app.MapControllerRoute(name: "default", pattern: _configuration.DefaultControllerRoutePattern);

        return app;
    }
}
