﻿using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using NgSoftware.Specular.Common.Mvc.Implementations;
using NgSoftware.Specular.Common.Mvc.Models;
using Serilog;

namespace NgSoftware.Specular.Common.Mvc.Extensions;

/// <summary>
/// Методы расширения для <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавляет аутентификацию
    /// </summary>
    public static void AddAuth(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var authSetting = configuration.GetSection(JwtSettingsModel.Key).Get<JwtSettingsModel>();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = authSetting.Audience,
                    ValidateIssuer = true,
                    ValidIssuer = authSetting.Issuer,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(authSetting.ClockSkewSec),
                    ValidateIssuerSigningKey = true,
                    TokenDecryptionKey = new SymmetricSecurityKey(
                        Base64UrlEncoder.DecodeBytes(authSetting.SecretKeyBase64)),
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Base64UrlEncoder.DecodeBytes(authSetting.SignKeyBase64))
                };
            });
    }

    /// <summary>
    /// Регистрирует все интерфейсы указанного типа
    /// </summary>
    /// <param name="services"><inheritdoc cref="IServiceCollection"/></param>
    /// <param name="lifetime"><inheritdoc cref="ServiceLifetime"/></param>
    /// <typeparam name="TService">Тип, для которого осуществляется регистрация</typeparam>
    public static void RegisterAsImplementedInterfaces<TService>(this IServiceCollection services, ServiceLifetime lifetime)
    {
        services.TryAdd(new ServiceDescriptor(typeof(TService), typeof(TService), lifetime));
        var interfaces = typeof(TService).GetTypeInfo().ImplementedInterfaces
            .Where(i => i != typeof(IDisposable) && (i.IsPublic));

        foreach (Type interfaceType in interfaces)
        {
            services.TryAdd(new ServiceDescriptor(interfaceType,
                provider => provider.GetRequiredService(typeof(TService)),
                lifetime));
        }
    }

    /// <summary>
    /// Регистрирует все интерфейсы инстансов в указанной сборке для указанного маркерного интерфейса
    /// </summary>
    /// <param name="services"><inheritdoc cref="IServiceCollection"/></param>
    /// <param name="lifetime"><inheritdoc cref="ServiceLifetime"/></param>
    /// <typeparam name="TInterface">Тип, для которого осуществляется регистрация</typeparam>
    public static void RegisterAssemblyInterfacesAssignableTo<TInterface>(this IServiceCollection services, ServiceLifetime lifetime)
    {
        var serviceType = typeof(TInterface);
        var types = serviceType.Assembly.GetTypes()
            .Where(p => serviceType.IsAssignableFrom(p) &&
                        !(p.IsAbstract ||
                          p.IsInterface));
        foreach (var type in types)
        {
            services.TryAdd(new ServiceDescriptor(type, type, lifetime));
            var interfaces = type.GetTypeInfo().ImplementedInterfaces
                .Where(i => i != typeof(IDisposable) &&
                            i.IsPublic &&
                            i != serviceType);

            foreach (Type interfaceType in interfaces)
            {
                services.TryAdd(new ServiceDescriptor(interfaceType,
                    provider => provider.GetRequiredService(type),
                    lifetime));
            }
        }
    }

    /// <summary>
    /// Регистрирует <see cref="Profile"/> автомапера
    /// </summary>
    public static void RegisterAutoMapperProfile<TProfile>(this IServiceCollection services) where TProfile : Profile
        => services.AddSingleton<Profile, TProfile>();

    /// <summary>
    /// Регистрация модуля в <see cref="IServiceCollection"/>
    /// </summary>
    public static void RegisterModule<TModule>(this IServiceCollection services)
    {
        if (Activator.CreateInstance(typeof(TModule)) is Module module)
        {
            module.Configure(services);
        }
    }

    /// <summary>
    /// Добавляет логгер
    /// </summary>
    public static void AddSpecularLogger(this IServiceCollection serviceCollection, IConfiguration configuration, string applicationName)
    {
        var serilogLogger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger()
            .ForContext("Application-entry", applicationName);
        serviceCollection.AddSingleton(serilogLogger);
        serviceCollection.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(serilogLogger));
        serviceCollection.RegisterAsImplementedInterfaces<SpecularLogManager>(ServiceLifetime.Transient);
    }
}
